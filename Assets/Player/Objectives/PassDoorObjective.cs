using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using static AgentInterface;

public class PassDoorObjective : Objective {
	public int targetDirection;
	public bool[,] attempts = {{false, false}, {false, false}};
	// attempts[0][x] -> before jumping over
	// attempts[1][x] -> after jumping over
	// attempts[x][0] -> involves dropping a box on button
	// attempts[x][1] -> simply pressing button
	// TODO: Optimizations:
	// - only searching for buttons on the appropriate side of the door
	// - only searching for boxes on the appropriate side of the door

	public PassDoorObjective(AgentInterface agent, GameObject target, Vector2 currentPosition) : base(agent, target) {
		Vector2 doorPosition = target.transform.position;
		if (currentPosition.x < doorPosition.x) {
			this.targetDirection = +1;
		} else {
			this.targetDirection = -1;
		}
		if (GameObject.FindGameObjectsWithTag("Box").Length <= 0) {
			attempts[0, 0] = true;
			attempts[1, 0] = true;
		}
	}

	public override bool isExclusive() {
		// If 
		return true;
	}

	public override bool isCompleted() {
		if (targetDirection == +1) {
			return agentInterface.getPosition().x > target.transform.position.x;
		}
		else {
			return agentInterface.getPosition().x < target.transform.position.x;
		}
	}

	public override bool isFailed() {
		// Doors always have at least one reachable button so this object can never fail
		return false;
	}

	public bool jumpedOver() {
		if (targetDirection == +1)
            return agentInterface.getPosition().x >= target.transform.position.x;
		else if (targetDirection == -1)
            return agentInterface.getPosition().x <= target.transform.position.x;
		return false;
	}

	public override AgentAction chooseAction() {
		Debug.Log(agentInterface.gameObject.name + ": Trying to pass the door!");
		if (target.GetComponent<Door>().isOpen()) {
			return agentInterface.getActionWalkTowards(target.transform.position + new Vector3(targetDirection, 0, 0));
		}
		else return AgentAction.STAY;
	}

	public override Objective updateObjective() {
		Agent partner = base.agentInterface.getPartner();
		if (partner.getCurrentObjective() is PassDoorObjective && this.isOlderThan(partner.getCurrentObjective())) {
			PassDoorObjective partnerObjective = (PassDoorObjective)partner.getCurrentObjective();
			if (!partnerObjective.attempts[0, 0]) {
				partnerObjective.attempts[0, 0] = true;
				return new PressButtonWithBoxObjective(base.agentInterface, partnerObjective);
			}
			else if (!partnerObjective.attempts[0, 1]) {
				partnerObjective.attempts[0, 1] = true;
				return new PressButtonObjective(base.agentInterface, partnerObjective);
			}
			else if (!jumpedOver()) {
				return new JumpOverObjective(base.agentInterface, partnerObjective);
			}
			else if (jumpedOver() && !partnerObjective.attempts[1, 0]) {
				partnerObjective.attempts[1, 0] = true;
				return new PressButtonWithBoxObjective(base.agentInterface, partnerObjective);
			}
			else if (jumpedOver() && !partnerObjective.attempts[1, 1]) {
				partnerObjective.attempts[1, 1] = true;
				return new PressButtonObjective(base.agentInterface, partnerObjective);
			}
		}
		else if (partner.getCurrentObjective() is JumpOverObjective) {
			//fixme should also check if JumpOverObjective target door is same as current current passdoorobjective target door
			JumpOverObjective partnerObjective = (JumpOverObjective)partner.getCurrentObjective();
			HelpJumpOverObjective newObjective = new HelpJumpOverObjective(agentInterface, partnerObjective);
			partnerObjective.supportingObjective = newObjective;
			return newObjective;
		}
		return null;
	}
}
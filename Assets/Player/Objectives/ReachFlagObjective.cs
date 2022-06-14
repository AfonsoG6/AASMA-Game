using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using static AgentInterface;

public class ReachFlagObjective : Objective {

	public ReachFlagObjective(AgentInterface agentInterface) : base(agentInterface, GameObject.Find("Flag")) {
	}

	public override bool isExclusive() {
		return false;
	}

	public override bool isCompleted() {
		// Special Objective that must never be counted as completed as long as the level is active.
		return false;
	}

	public override bool isFailed() {
		// Special Objective that can never fail, assuming the level is clearable.
		return false;
	}

	//fixme duplicated code
	public bool jumpedOver(GameObject target, int targetDirection) {
		if (targetDirection == +1)
            return agentInterface.getPosition().x >= target.transform.position.x;
		else if (targetDirection == -1)
            return agentInterface.getPosition().x <= target.transform.position.x;
		return false;
	}

	public override AgentAction chooseAction() {
		Debug.Log(agentInterface.gameObject.name + ": Walking towards the flag!");
		if (Vector2.Distance(target.transform.position, agentInterface.getPosition()) < 0.1) {
			return AgentAction.STAY;
		}
		else return agentInterface.getActionWalkTowards(target.transform.position);
	}

	public override Objective updateObjective() {
		Agent partner = agentInterface.getPartner();
		if (partner.getCurrentObjective() is PassDoorObjective) {
			PassDoorObjective partnerObjective = (PassDoorObjective)partner.getCurrentObjective();
			if (!partnerObjective.attempts[0, 0]) {
				partnerObjective.attempts[0, 0] = true;
				return new PressButtonWithBoxObjective(agentInterface, partnerObjective);
			}
			else if (!partnerObjective.attempts[0, 1]) {
				partnerObjective.attempts[0, 1] = true;
				return new PressButtonObjective(agentInterface, partnerObjective);
			}
			else if (!jumpedOver(partnerObjective.target, partnerObjective.targetDirection)) {
				return new JumpOverObjective(agentInterface, partnerObjective);
			}
			else if (jumpedOver(partnerObjective.target, partnerObjective.targetDirection) && !partnerObjective.attempts[1, 0]) {
				partnerObjective.attempts[1, 0] = true;
				return new PressButtonWithBoxObjective(agentInterface, partnerObjective);
			}
			else if (jumpedOver(partnerObjective.target, partnerObjective.targetDirection) && !partnerObjective.attempts[1, 1]) {
				partnerObjective.attempts[1, 1] = true;
				return new PressButtonObjective(agentInterface, partnerObjective);
			}
			return new PressButtonObjective(agentInterface, partnerObjective);
		}
		// probably useless
		else if (partner.getCurrentObjective() is JumpOverObjective) {
			//fixme should also check if JumpOverObjective target door is same as current current passdoorobjective target door
			JumpOverObjective partnerObjective = (JumpOverObjective)partner.getCurrentObjective();
			HelpJumpOverObjective newObjective = new HelpJumpOverObjective(agentInterface, partnerObjective);
			partnerObjective.supportingObjective = newObjective;
			return newObjective;
		}
		else if (!agentInterface.wasActionSuccessful()) {
			if (agentInterface.getLastAction() == AgentAction.WALK_RIGHT && agentInterface.isDoorAt(Vector2.right)) {
				return new PassDoorObjective(agentInterface, agentInterface.getDoorAt(Vector2.right), agentInterface.getPosition());
			}
			else if (agentInterface.getLastAction() == AgentAction.WALK_LEFT && agentInterface.isDoorAt(Vector2.left)) {
				return new PassDoorObjective(agentInterface, agentInterface.getDoorAt(Vector2.left), agentInterface.getPosition());
			}
		}
		
		return null;
	}
}
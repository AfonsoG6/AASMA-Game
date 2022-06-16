using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using static AgentInterface;

public class PassDoorObjective : Objective {
	public int targetDirection;
	public bool failed = false;

	public PassDoorObjective(AgentInterface agent, GameObject target, Vector2 currentPosition) : base(agent, target) {
		Vector2 doorPosition = target.transform.position;
		if (currentPosition.x < doorPosition.x) {
			this.targetDirection = +1;
		} else {
			this.targetDirection = -1;
		}
	}

	public override bool isExclusive() {
		// If 
		return true;
	}

	public override bool isCompleted() {
		return agentInterface.hasGonePastDoor(target.transform.position, targetDirection);
	}

	public override bool isFailed() {
		return failed;
	}

	public override AgentAction chooseAction() {
		Debug.Log(agentInterface.gameObject.name + ": Trying to pass the door!");
		if (target.GetComponent<Door>().isOpen()) {
			return agentInterface.getActionWalkTowards(target.transform.position + new Vector3(targetDirection, 0, 0));
		}
		else return AgentAction.STAY;
	}

	public override Objective updateObjective() {
		Agent partner = agentInterface.getPartner();
		if (partner.getCurrentObjective() is PassDoorObjective && this.isOlderThan(partner.getCurrentObjective())) {
			failed = true;
		}
		else if (partner.getCurrentObjective() is JumpOverObjective && !partner.getCurrentObjective().isCompleted()) {
			// FIXME: should also check if JumpOverObjective target door is same as current current passdoorobjective target door
			JumpOverObjective partnerObjective = (JumpOverObjective)partner.getCurrentObjective();
			HelpJumpOverObjective newObjective = new HelpJumpOverObjective(agentInterface, partnerObjective);
			partnerObjective.supportingObjective = newObjective;
			return newObjective;
		}

		if (target.GetComponent<Door>().getReachableButtons(partner.gameObject.transform.position).Count <= 0 &&
				target.GetComponent<Door>().getReachableButtons(agentInterface.getPosition()).Count > 0 &&
				agentInterface.hasBox()) {
			return new PressButtonWithBoxObjective(agentInterface, this);
		}
		return null;
	}
}
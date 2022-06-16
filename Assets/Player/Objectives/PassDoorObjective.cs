using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using static AgentInterface;

public class PassDoorObjective : Objective {
	public int targetDirection;

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
		return agentInterface.hasGonePastPosition(target.transform.position, targetDirection);
	}

	public override bool isFailed() {
		// Doors should always have at least one reachable button so this object can never fail
		return false;
	}

	public override AgentAction chooseAction() {
		Debug.Log(agentInterface.gameObject.name + ": Trying to pass the door!");
		if (agentInterface.dropBox()) return AgentAction.GRAB_OR_DROP;
		if (target.GetComponent<Door>().isOpen()) {
			return agentInterface.getActionWalkTowards(target.transform.position + new Vector3(targetDirection, 0, 0));
		}
		else return AgentAction.STAY;
	}

	public override Objective updateObjective() {
		Objective newObjective;

		newObjective = agentInterface.helpPassDoorObjective();
		if (newObjective != null) return newObjective;
		
		newObjective = agentInterface.helpJumpOverObjective();
		if (newObjective != null) return newObjective;

		// Hack, correct behavior should be in FindBoxObjective.updateObjective()
		// However, that behaviour is too complex to implement, as described on that function's comments
		if (target.GetComponent<Door>().getReachableButtons(agentInterface.getPartner().gameObject.transform.position).Count <= 0 &&
			target.GetComponent<Door>().getReachableButtons(agentInterface.getPosition()).Count > 0 && agentInterface.hasBox()) {
			return new PressButtonWithBoxObjective(base.agentInterface, this);
		}

		return null;
	}
}
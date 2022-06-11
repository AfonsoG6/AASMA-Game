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

	public override bool isRetryable() {
		return false;
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

	public override AgentAction chooseAction() {
		Debug.Log(agentInterface.gameObject.name + ": Trying to pass the door!");
		if (target.GetComponent<Door>().isOpen()) {
			return agentInterface.getActionWalkTowards(target.transform.position + new Vector3(targetDirection, 0, 0));
		}
		else return AgentAction.STAY;
	}

}
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

	public override AgentAction chooseAction() {
		Debug.Log(agentInterface.gameObject.name + ": Walking towards the flag!");
		if (agentInterface.dropBox()) return AgentAction.GRAB_OR_DROP;
		if (Math.Abs(Vector2.Distance(target.transform.position, agentInterface.getPosition())) < 0.1 ||
			target.GetComponent<Flag>().checkPlayerWin(agentInterface.gameObject.name)) {
			return AgentAction.STAY;
		}
		else return agentInterface.getActionWalkTowards(target.transform.position);
	}

	public override Objective updateObjective() {
		Objective newObjective;

		newObjective = agentInterface.helpPassDoorObjective();
		if (newObjective != null) return newObjective;
		
		newObjective = agentInterface.helpJumpOverObjective();
		if (newObjective != null) return newObjective;
		
		if (!agentInterface.wasActionSuccessful()) {
			newObjective = agentInterface.getPassDoorObjective();
			if (newObjective != null) return newObjective;
		}
		
		return null;
	}
}
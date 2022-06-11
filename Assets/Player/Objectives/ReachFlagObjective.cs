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
			return new PressButtonObjective(agentInterface, (PassDoorObjective)partner.getCurrentObjective());
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
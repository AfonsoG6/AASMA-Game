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
		if (Vector2.Distance(target.transform.position, agentInterface.getPosition()) < 0.1) {
			return AgentAction.STAY;
		}
		else return agentInterface.getActionWalkTowards(target.transform.position);
	}

	public override Objective updateObjective() {
		// FIXME: duplicated code
		Agent partner = base.agentInterface.getPartner();
		if (partner.getCurrentObjective() is PassDoorObjective) {
			PassDoorObjective partnerObjective = (PassDoorObjective)partner.getCurrentObjective();
			if (partnerObjective.target.GetComponent<Door>().getReachableButtons(agentInterface.getPosition()).Count <= 0)
				return new JumpOverObjective(base.agentInterface, partnerObjective);
			else if (agentInterface.reachableBoxExists(partnerObjective.target.transform.position))
				return new PressButtonWithBoxObjective(base.agentInterface, partnerObjective);
			else
				return new PressButtonObjective(base.agentInterface, partnerObjective);
		}
		else if (partner.getCurrentObjective() is JumpOverObjective && !partner.getCurrentObjective().isCompleted()) {
			// FIXME: should also check if JumpOverObjective target door is same as current current passdoorobjective target door
			JumpOverObjective partnerObjective = (JumpOverObjective)partner.getCurrentObjective();
			HelpJumpOverObjective newObjective = new HelpJumpOverObjective(agentInterface, partnerObjective);
			partnerObjective.supportingObjective = newObjective;
			return newObjective;
		}
		else if (!agentInterface.wasActionSuccessful()) {
			PassDoorObjective newObjective = agentInterface.getPassDoorObjective();
			if (newObjective != null) {
				return newObjective;
			}
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
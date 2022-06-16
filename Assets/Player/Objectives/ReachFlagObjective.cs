using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using static AgentInterface;

public class ReachFlagObjective : Objective {

	public Objective lastSupportedObjective = null;
	public bool triedSimpleHelp = false;
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
		if (Math.Abs(target.transform.position.x - agentInterface.getPosition().x) < 0.5) {
			return AgentAction.STAY;
		}
		else return agentInterface.getActionWalkTowards(target.transform.position);
	}

	public override Objective updateObjective() {
		Agent partner = agentInterface.getPartner();
		if (partner.getCurrentObjective() is PassDoorObjective) {
			PassDoorObjective partnerObjective = (PassDoorObjective) partner.getCurrentObjective();
			if (partnerObjective != lastSupportedObjective) {
				lastSupportedObjective = partnerObjective;
				triedSimpleHelp = false;
			}

			if (!triedSimpleHelp) {
				triedSimpleHelp = true;
				return new PressButtonObjective(agentInterface, partnerObjective);
			}
			else if (agentInterface.reachableBoxExists(partnerObjective.target.transform.position) ||
					agentInterface.hasBox()) {
				return new PressButtonWithBoxObjective(agentInterface, partnerObjective);
			}
			else {
				return new PressButtonObjective(agentInterface, partnerObjective, true);
			}
		}
		else if (partner.getCurrentObjective() is JumpOverObjective && !partner.getCurrentObjective().isCompleted()) {
			JumpOverObjective partnerObjective = (JumpOverObjective) partner.getCurrentObjective();
			HelpJumpOverObjective newObjective = new HelpJumpOverObjective(agentInterface, partnerObjective);
			partnerObjective.supportingObjective = newObjective;
			return newObjective;
		}
		else if (!agentInterface.wasActionSuccessful()) {
			Objective newObjective = agentInterface.getObjectiveAfterActionUnsuccessful(this);
			if (newObjective != null) {
				return newObjective;
			}
		}

		return null;
	}
}
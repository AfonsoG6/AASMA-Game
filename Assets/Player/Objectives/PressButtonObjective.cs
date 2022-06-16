using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using static AgentInterface;

public class PressButtonObjective : Objective {
	public PassDoorObjective supportedObjective;
	public List<Button> allButtons;
	public int buttonIdx;
	public bool triedAllButtonsOnce = false;
	public bool noButtonsLeft = false;
	
	public PressButtonObjective(AgentInterface agent, PassDoorObjective supportedObjective) :
				base(agent, supportedObjective.target.GetComponent<Door>().getButtons()[0].gameObject) {
		this.supportedObjective = supportedObjective;
		this.allButtons = new List<Button>();
		allButtons.AddRange(supportedObjective.target.GetComponent<Door>().getReachableButtons(agent.getPosition()));
		this.buttonIdx = 0;
		if (allButtons.Count > 0)
			target = allButtons[buttonIdx].gameObject;
		else {
			triedAllButtonsOnce = true;
			noButtonsLeft = true;
		}
	}

	public override bool isExclusive() {
		// As it is a SUPPORT objective, it should not be the objective of both agents at the same time.
		return true;
	}

	public override bool isCompleted() {
		return supportedObjective.isCompleted();
	}

	public override bool isFailed() {
		// Fails if no buttons left 
		return noButtonsLeft;
	}

	public override AgentAction chooseAction() {
		Debug.Log(agentInterface.gameObject.name + ": Pressing Button!");
		if (agentInterface.dropBox()) return AgentAction.GRAB_OR_DROP;
		if (target.GetComponent<Button>().pressedByPlayer()) return AgentAction.STAY;
		else return agentInterface.getActionWalkTowards(target.transform.position);
	}

	public override Objective updateObjective() {
		if (agentInterface.wasActionSuccessful()) return null;

		// If the last action failed, we try another button if it exists.
		if (!triedAllButtonsOnce) {
			if (buttonIdx+1 < allButtons.Count) {
				buttonIdx++;
				target = allButtons[buttonIdx].gameObject;
				return null;
			}
			else {
				buttonIdx = 0;
				triedAllButtonsOnce = true;
				return null;
			}
		}
		
		// If we already tried all buttons that were easily accessible
		PassDoorObjective newObjective = agentInterface.getPassDoorObjective();
		if (newObjective != null && !newObjective.equalsTo(supportedObjective)) {
			return newObjective;
		}

		if (buttonIdx+1 < allButtons.Count) {
			buttonIdx++;
			target = allButtons[buttonIdx].gameObject;
			return null;
		}
		else {
			noButtonsLeft = true;
			return null;
		}
	}
}
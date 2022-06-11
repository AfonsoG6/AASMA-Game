using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using static AgentInterface;

public class PressButtonObjective : Objective {
	public PassDoorObjective supportedObjective;
	public List<Button> allButtons;
	public int buttonIdx;
	
	public PressButtonObjective(AgentInterface agent, PassDoorObjective supportedObjective) :
				base(agent, supportedObjective.target.GetComponent<Door>().getButtons()[0].gameObject) {
		this.supportedObjective = supportedObjective;
		this.allButtons = new List<Button>();
		allButtons.AddRange(supportedObjective.target.GetComponent<Door>().getButtons());
		this.buttonIdx = 0;
	}

	public override bool isExclusive() {
		// As it is a SUPPORT objective, it should not be the objective of both agents at the same time.
		return true;
	}

	public override bool isCompleted() {
		return supportedObjective.isCompleted();
	}

	public override AgentAction chooseAction() {
		Debug.Log(agentInterface.gameObject.name + ": Pressing Button!");
		if (target.GetComponent<Button>().pressed()) {
			return AgentAction.STAY;
		}
		else return agentInterface.getActionWalkTowards(target.transform.position);
	}

	public override Objective updateObjective() {
		if (agentInterface.wasActionSuccessful()) return null;

		if (buttonIdx+1 < allButtons.Count) {
			buttonIdx++;
			target = allButtons[buttonIdx].gameObject;
			return null;
		}

		if (agentInterface.getLastAction() == AgentAction.WALK_RIGHT && agentInterface.isDoorAt(Vector2.right)) {
			PassDoorObjective objective = new PassDoorObjective(agentInterface, agentInterface.getDoorAt(Vector2.right), agentInterface.getPosition());
			if (!objective.equalsTo(agentInterface.getPartner().getCurrentObjective())) return objective;
		}
		if (agentInterface.getLastAction() == AgentAction.WALK_LEFT && agentInterface.isDoorAt(Vector2.left)) {
			PassDoorObjective objective = new PassDoorObjective(agentInterface, agentInterface.getDoorAt(Vector2.left), agentInterface.getPosition());
			if (!objective.equalsTo(agentInterface.getPartner().getCurrentObjective())) return objective;
		}



		return null;
	}
}
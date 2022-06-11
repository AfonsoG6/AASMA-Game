using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using static AgentInterface;

public class PressButtonObjective : Objective {
	public PassDoorObjective supportedObjective;
	public List<Button> buttonsLeft;
	
	public PressButtonObjective(AgentInterface agent, PassDoorObjective supportedObjective) :
				base(agent, supportedObjective.target.GetComponent<Door>().getButtons()[0].gameObject) {
		this.supportedObjective = supportedObjective;
		this.buttonsLeft = new List<Button>();
		buttonsLeft.AddRange(supportedObjective.target.GetComponent<Door>().getButtons());
		buttonsLeft.RemoveAt(0);
	}

	public override bool isRetryable() {
		return buttonsLeft.Count > 0;
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
}
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using static AgentInterface;

public class PressButtonWithBoxObjective : PressButtonObjective {

    public bool lookedForBox = false;
    public bool droppedBox = false;
	
	public PressButtonWithBoxObjective(AgentInterface agent, PassDoorObjective supportedObjective) :
				base(agent, supportedObjective) { }

    public override bool isCompleted() {
		return base.isCompleted();
	}

    public override bool isFailed() {
        return base.isFailed() || (!agentInterface.hasBox() && lookedForBox && !droppedBox);
    }

	public override AgentAction chooseAction() {
		Debug.Log(agentInterface.gameObject.name + ": Dropping off box on button!");
        if (target.GetComponent<Button>().pressed()) {
			if (agentInterface.hasBox()) {
                droppedBox = true;
				return AgentAction.GRAB_OR_DROP;
            }
			else
				return AgentAction.STAY;
		}
		else return agentInterface.getActionWalkTowards(target.transform.position);
	}

	public override Objective updateObjective() {

		// Look for box if doesn't have box
		if (!agentInterface.hasBox() && !lookedForBox) {
			GameObject[] boxesInLevel = GameObject.FindGameObjectsWithTag("Box");
			if (boxesInLevel.Length > 0) {
				FindBoxObjective objective = new FindBoxObjective(agentInterface, boxesInLevel);
				if (!objective.equalsTo(agentInterface.getPartner().getCurrentObjective())) {
                    lookedForBox = true;
                    return objective;
                }
			}
		}

		return base.updateObjective();
	}
}
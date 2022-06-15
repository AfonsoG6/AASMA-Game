using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using static AgentInterface;

public class HelpJumpOverObjective : Objective {
	public JumpOverObjective supportedObjective;
    public int targetDirection;
    public bool lookedForBox = false;
    public bool readyToHelp = false;
	
	public HelpJumpOverObjective(AgentInterface agent, JumpOverObjective supportedObjective) :
				base(agent, supportedObjective.target) {
		this.supportedObjective = supportedObjective;
        this.targetDirection = supportedObjective.targetDirection;
	}

	public override bool isExclusive() {
		// JumpOver and HelpJumpOver complement each other so they are exclusive
		return true;
	}

	public override bool isCompleted() {
		return supportedObjective.isCompleted();
	}

    public override bool isFailed() {
        return !agentInterface.hasBox() && lookedForBox;
    }

	public override AgentAction chooseAction() {
        // If agent is holding box, go towards target (door)
        // If not, updateObjective will push FindBox as an objective before this
		Debug.Log(agentInterface.gameObject.name + ": Helping partner jump over!");
		if (isFailed())
			return AgentAction.GRAB_OR_DROP;
		if (agentInterface.hasBox()) {
			if (agentInterface.isDoorAt(new Vector3(targetDirection, 0, 0))) {
                readyToHelp = true;
			    return AgentAction.STAY;
            }
            else {
                readyToHelp = false; // useless
                return agentInterface.getActionWalkTowards(target.transform.position);
            }
		}
		else {
            lookedForBox = false;
            return AgentAction.STAY; // Should never get here
        }
	}

	public override Objective updateObjective() {

		if (!agentInterface.hasBox()) {
            GameObject[] boxesInLevel = GameObject.FindGameObjectsWithTag("Box");
			if (boxesInLevel.Length > 0) {
                lookedForBox = true;
                return new FindBoxObjective(agentInterface, target.transform.position);
            }
        }
		if (agentInterface.wasActionSuccessful()) return null; //FIXME useless?

		return null;
	}
}
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using static AgentInterface;

public class HelpJumpOverObjective : Objective {
	public JumpOverObjective supportedObjective;
    public int targetDirection;
	public Vector3 helpPosition;
    public bool lookedForBox = false;
    public bool readyToHelp = false;
	
	public HelpJumpOverObjective(AgentInterface agent, JumpOverObjective supportedObjective) :
				base(agent, supportedObjective.target) {
		this.supportedObjective = supportedObjective;
        this.targetDirection = supportedObjective.targetDirection;
		this.helpPosition = new Vector3(supportedObjective.jumpPosition.x, supportedObjective.jumpPosition.y, supportedObjective.jumpPosition.z);
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
			if (Math.Abs(Vector2.Distance(helpPosition, agentInterface.getPosition())) < 0.5f) {
                readyToHelp = true;
			    return AgentAction.STAY;
            }
            else {
                readyToHelp = false; // Useless
                return agentInterface.getActionWalkTowards(helpPosition);
            }
		}
		else {
            lookedForBox = false;
            return AgentAction.STAY; // Should never get here
        }
	}

	public override Objective updateObjective() {

		// Look for box if doesn't have box
		if (!agentInterface.hasBox() && !lookedForBox) {
            GameObject[] boxesInLevel = GameObject.FindGameObjectsWithTag("Box");
			if (boxesInLevel.Length > 0) {
				FindBoxObjective newObjective = new FindBoxObjective(agentInterface, target.transform.position);
				if (!newObjective.equalsTo(agentInterface.getPartner().getCurrentObjective())) {
                    lookedForBox = true;
                    return newObjective;
                }
            }
        }

		return null;
	}
}
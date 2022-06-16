using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using static AgentInterface;

public class JumpOverObjective : Objective {
	public HelpJumpOverObjective supportingObjective;
    public int targetDirection;
    public Vector3 targetPosition;
    public Vector3 jumpPosition;
    public bool droppedBox = false;
    public bool ready = false;
	
	public JumpOverObjective(AgentInterface agent, PassDoorObjective originalSupportedObjective, Vector3 realTargetPosition) :
				base(agent, originalSupportedObjective.target) {
        this.supportingObjective = null;
        this.targetDirection = originalSupportedObjective.targetDirection;
        this.targetPosition = realTargetPosition;
        this.jumpPosition = new Vector3(realTargetPosition.x - 1f*targetDirection, realTargetPosition.y - 2, realTargetPosition.z);
	}

	public override bool isExclusive() {
		// JumpOver and HelpJumpOver complement each other so they are exclusive
		return true;
	}

	public override bool isCompleted() {
        // Is completed when agent has effectively gone past the door by jumping over
        return agentInterface.hasGonePastPosition(target.transform.position, targetDirection);
	}

    public override bool isFailed() {
        if (agentInterface.getPartner().getCurrentObjective() is HelpJumpOverObjective && supportingObjective != null){
            return supportingObjective.isFailed();
        }
        return false;
	}

    public bool readyToJump() {
        return Math.Abs(Vector2.Distance(jumpPosition, agentInterface.getPosition())) < 0.5f;
    }

	public override AgentAction chooseAction() {
        Debug.Log(agentInterface.gameObject.name + ": I want to jump over!");
        if ((agentInterface.hasBox() && !droppedBox) || (agentInterface.dropBox())) {
            droppedBox = true;
            return AgentAction.GRAB_OR_DROP;
        }
		if (supportingObjective != null && supportingObjective.readyToHelp) {
            if (readyToJump())
                ready = true;
            if (!readyToJump() && !ready) {
                return agentInterface.getActionWalkTowards(jumpPosition);
            }
            else if (ready) {
                if (targetDirection == +1) {
                    return AgentAction.JUMP_RIGHT;
                }
                else if (targetDirection == -1) {
                    return AgentAction.JUMP_LEFT;
                }
                return AgentAction.STAY;
            }
            else {
                // Should never get here
                return AgentAction.STAY;
            }
        }
		else return AgentAction.STAY;
	}

	public override Objective updateObjective() {
		// No updates needed here as:
        // - JumpOverObjective is usually a "last resort" attempt
        // - isFailed() should automatically remove this if it leads to nowhere
        
		return null;
	}
}
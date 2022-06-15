using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using static AgentInterface;

public class JumpOverObjective : Objective {
	public HelpJumpOverObjective supportingObjective;
    public int targetDirection;
    public Vector3 jumpPosition;
    public bool droppedBox = false;
    public bool ready = false;
	
	public JumpOverObjective(AgentInterface agent, PassDoorObjective originalSupportedObjective) :
				base(agent, originalSupportedObjective.target) {
        this.supportingObjective = null;
        this.targetDirection = originalSupportedObjective.targetDirection;
        this.jumpPosition = new Vector3(target.transform.position.x - 1.5f*targetDirection, target.transform.position.y, target.transform.position.z);
        //fixme bad position
	}

	public override bool isExclusive() {
		// JumpOver and HelpJumpOver complement each other so they are exclusive
		return true;
	}

	public override bool isCompleted() {
        // Is completed when agent has effectively gone past the door by jumping over
        if (targetDirection == +1)
            return agentInterface.getPosition().x >= target.transform.position.x;
		else
            return agentInterface.getPosition().x <= target.transform.position.x;
	}

    public override bool isFailed() {
        if (agentInterface.getPartner().getCurrentObjective() is HelpJumpOverObjective && supportingObjective != null){
            return supportingObjective.isFailed();
        }
        return false;
	}

    public bool readyToJump() {
        return Math.Abs(Vector2.Distance(agentInterface.getPosition(), jumpPosition)) < 0.8f; //fixme 0.2f
    }

	public override AgentAction chooseAction() {
        Debug.Log(agentInterface.gameObject.name + ": I want to jump over!");
        if (agentInterface.hasBox() && !droppedBox){
            droppedBox = true;
            return AgentAction.GRAB_OR_DROP;
        }
		if (supportingObjective != null && supportingObjective.readyToHelp) {
            jumpPosition = new Vector3(agentInterface.getPartner().transform.position.x - 1f*targetDirection, agentInterface.getPartner().transform.position.y, agentInterface.getPartner().transform.position.z);
            if(readyToJump())
                ready = true;
            if (!readyToJump() && !ready) {
                return agentInterface.getActionWalkTowards(jumpPosition);
            }
            else if(ready){
                if (targetDirection == +1){
                    return AgentAction.JUMP_RIGHT;
                }
                else if (targetDirection == -1){
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
		if (agentInterface.wasActionSuccessful()) return null;

        // Probably nothing needed here as isFailed() will automatically remove this if needed
        
		return null;
	}
}
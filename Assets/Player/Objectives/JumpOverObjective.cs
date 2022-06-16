using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using static AgentInterface;

public class JumpOverObjective : Objective {
	public HelpJumpOverObjective supportingObjective;
    public int targetDirection;
    public Vector3 jumpPosition;
	
	public JumpOverObjective(AgentInterface agent, Vector3 targetPosition) :
				base(agent, GameObject.Instantiate(Resources.Load<GameObject>("Target"), targetPosition, Quaternion.identity)) {
        this.supportingObjective = null;
        this.targetDirection = agentInterface.getTargetDirection(targetPosition);
        this.jumpPosition = new Vector3(target.transform.position.x - 1*targetDirection, target.transform.position.y - 2, target.transform.position.z);
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
        Objective partnerObjective = agentInterface.getPartner().getCurrentObjective();
        if (partnerObjective is HelpJumpOverObjective && supportingObjective != null){
            return supportingObjective.isFailed();
        }
        return false;
	}

    public bool readyToJump() {
        return Math.Abs(Vector2.Distance(agentInterface.getPosition(), jumpPosition)) < 0.2f;
    }

	public override AgentAction chooseAction() {
        Debug.Log(agentInterface.gameObject.name + ": I want to jump over!");
        
        if (agentInterface.hasBox()){
            return AgentAction.GRAB_OR_DROP;
        }
		else if (supportingObjective != null && supportingObjective.readyToHelp) {
            if (readyToJump()) {
                if (targetDirection == +1) {
                    return AgentAction.JUMP_RIGHT;
                }
                else {
                    return AgentAction.JUMP_LEFT;
                }
            }
        }
		
        return agentInterface.getActionWalkTowards(target.transform.position);
	}

	public override Objective updateObjective() {
		return null;
	}
}
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using static AgentInterface;

public class JumpOverObjective : Objective {
	public PassDoorObjective supportedObjective;
    public int targetDirection;
	
	public JumpOverObjective(AgentInterface agent, PassDoorObjective supportedObjective) :
				base(agent, supportedObjective.target) {
		this.supportedObjective = supportedObjective;
        this.targetDirection = supportedObjective.targetDirection; 
	}

	public override bool isExclusive() {
		// JumpOver and HelpJumpOver complement each other so they are exclusive
		return true;
	}

	public override bool isCompleted() {
        // TODO: should be completed when agent has managed to get to the other side of the door by jumping over
		/*
		if (targetDirection == +1) {
			return agentInterface.getPosition().x > target.transform.position.x;
		}
		else {
			return agentInterface.getPosition().x < target.transform.position.x;
		}
		*/
		return supportedObjective.isCompleted();
	}

	public override AgentAction chooseAction() {
        // Await one position unit away from opposite direction of door until other agent has fulfilled HelpJumpOver
        // return AgentAction.STAY if HelpJumpOver is not yet completed
        // When other agent has fulfilled HelpJumpOver and is thus waiting by the door holding a box, jump in direction of door
        // Then move towards target button by jumping or moving in direction of it
		Debug.Log(agentInterface.gameObject.name + ": I want to jump over!");
        return AgentAction.STAY;
	}

	public override Objective updateObjective() {
		if (agentInterface.wasActionSuccessful()) return null;

        // TODO
        
		return null;
	}
}
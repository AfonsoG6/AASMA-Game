using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using static AgentInterface;

public class Agent : MonoBehaviour {
	private AgentInterface i;
	private Stack<Objective> objectives;

	void Awake()
    {	
        i = GetComponent<AgentInterface>();
		objectives = new Stack<Objective>();
        objectives.Push(new ReachFlagObjective(i));
    }

	void FixedUpdate()
	{
		if (i.isActing()) return;
		
		updateObjectives();
		i.act(getCurrentObjective().chooseAction());
	}

	private void updateObjectives() {
		
		if (getCurrentObjective().isCompleted()) {
			objectives.Pop();
		}

		Objective currentObjective = getCurrentObjective();
		Objective newObjective = currentObjective.updateObjective();
		if (newObjective != null) {
			objectives.Push(newObjective);
		}
	}

	public Objective getCurrentObjective() {
		return objectives.Peek();
	}
}
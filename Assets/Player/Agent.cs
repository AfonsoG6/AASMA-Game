using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using static AgentInterface;

public class Agent : MonoBehaviour {
	private AgentInterface i;
	private Stack<Objective> objectives;
	private List<AgentInterface.AgentAction> solution;

	void Awake()
    {	
        i = GetComponent<AgentInterface>();
		objectives = new Stack<Objective>();
        objectives.Push(new ReachFlagObjective(i));
		LevelManager levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
		if (levelManager.isScriptedRun()) {
			solution = levelManager.getCurrentLevelSolution((gameObject.name == "Player1")? 0 : 1);
		}
    }

	void FixedUpdate()
	{
		if (i.isActing()) return;
		
		if (solution != null) {
			if (solution.Count > 0) {
				i.act(solution[0]);
				solution.RemoveAt(0);
			}
		}
		else {
			updateObjectives();
			i.act(getCurrentObjective().chooseAction());
		}
	}

	private void updateObjectives() {
		
		if (getCurrentObjective().isCompleted() || getCurrentObjective().isFailed()) {
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
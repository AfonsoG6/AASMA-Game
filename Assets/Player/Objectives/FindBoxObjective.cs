using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using static AgentInterface;

public class FindBoxObjective : Objective {
	public List<Box> allBoxes;
	public int boxIdx;
	
	public FindBoxObjective(AgentInterface agent, GameObject[] boxesInLevel) :
				base(agent, boxesInLevel[0]) {
		this.allBoxes = new List<Box>();
		foreach (GameObject box in boxesInLevel)
			allBoxes.Add(box.GetComponent<Box>());
		this.boxIdx = 0;
	}

	public override bool isExclusive() {
		// Finding a box follows from a SUPPORT objective: to press button or to help player get over wall
		return true;
	}

	public override bool isCompleted() {
		return agentInterface.hasBox();
	}

	public override AgentAction chooseAction() {
		Debug.Log(agentInterface.gameObject.name + ": Looking for Box!");
		if (Math.Abs(agentInterface.gameObject.transform.position.x - target.transform.position.x) < 0.5f)
			return AgentAction.GRAB_OR_DROP;
		else
			return agentInterface.getActionWalkTowards(target.transform.position);
	}

	public override Objective updateObjective() {
		if (agentInterface.wasActionSuccessful()) return null;

		// If the last action failed, we try another box if it exists.
		if (boxIdx+1 < allBoxes.Count) {
			boxIdx++;
			target = allBoxes[boxIdx].gameObject;
			return null;
		}

		return null;
	}
}
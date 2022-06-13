using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using static AgentInterface;

public class FindBoxObjective : Objective {
	public List<Box> allBoxes;
	public int boxIdx;
	public bool failed = false;
	
	public FindBoxObjective(AgentInterface agent, GameObject[] boxesInLevel) :
				base(agent, boxesInLevel[0]) {
		this.allBoxes = new List<Box>();
		foreach (GameObject box in boxesInLevel)
			allBoxes.Add(box.GetComponent<Box>());
		this.boxIdx = 0;
	}

	public override bool isExclusive() {
		// Both agents can look for different boxes at any given time
		return false;
	}

	public override bool isCompleted() {
		return agentInterface.hasBox();
	}

	public override bool isFailed() {
		return !agentInterface.hasBox() && this.failed;
	}

	public override AgentAction chooseAction() {
		Debug.Log(agentInterface.gameObject.name + ": Looking for Box!");
		//FIXME isBoxAt(targetDirection) && boxAt(targetDirection) == allBoxes[boxIdx] would be more correct perhaps
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
		// If couldn't grab any box, should remove this objective (mark as "completed"/failed)
		else failed = true;

		return null;
	}
}
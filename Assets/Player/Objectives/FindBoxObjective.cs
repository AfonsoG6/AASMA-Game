using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using static AgentInterface;

public class FindBoxObjective : Objective {
	public int boxId = 0;
	public bool failed = false;
	
	public FindBoxObjective(AgentInterface agent) :
				base(agent, findTarget()) {}

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
		if (Math.Abs(agentInterface.gameObject.transform.position.x - target.transform.position.x) < 0.5f){
			return AgentAction.GRAB_OR_DROP;
		}
		else
			return agentInterface.getActionWalkTowards(target.transform.position);
	}

	public override Objective updateObjective() {
		if (agentInterface.wasActionSuccessful()) return null;

		// If the last action failed, we try another box if it exists.
		boxId++;
		updateTarget();

		return null;
	}

	private static GameObject findTarget() {
		GameObject[] boxesInLevel = GameObject.FindGameObjectsWithTag("Box");
		Dictionary<int, Box> allBoxes = new Dictionary<int, Box>();
		foreach (GameObject box in boxesInLevel) {
			allBoxes.Add(box.GetComponent<Box>().getID(), box.GetComponent<Box>());
		}
		return allBoxes[0].gameObject;
	}

	private void updateTarget() {
		GameObject[] boxesInLevel = GameObject.FindGameObjectsWithTag("Box");
		Dictionary<int, Box> allBoxes = new Dictionary<int, Box>();
		foreach (GameObject box in boxesInLevel) {
			allBoxes.Add(box.GetComponent<Box>().getID(), box.GetComponent<Box>());
		}
		if (boxId < allBoxes.Count) {
			target = allBoxes[boxId].gameObject;
		}
		else {
			failed = true;
		}
	}
}
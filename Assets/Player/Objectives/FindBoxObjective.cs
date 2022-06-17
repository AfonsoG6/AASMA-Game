using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using static AgentInterface;

public class FindBoxObjective : Objective {
	public int boxId = 0;
	public bool failed = false;
	
	public FindBoxObjective(AgentInterface agent, Vector3 doorPosition) :
				base(agent, findTarget(agent.getPosition(), doorPosition)) {}

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
		// Something along the lines of this would probably be more correct:
		// - agentInterface.isBoxAt(targetDirection) && agentInterface.getBoxAt(targetDirection) == target
		// But checking the distance to the target box also works fine
		Debug.Log(agentInterface.gameObject.name + ": Looking for Box!");
		if (Math.Abs(Vector2.Distance(target.transform.position, agentInterface.getPosition())) <= 0.5f)
			return AgentAction.GRAB_OR_DROP;
		else
			return agentInterface.getActionWalkTowards(target.transform.position);
	}

	public override Objective updateObjective() {

		// If the last action failed, we try another box if it exists.
		if (!agentInterface.wasActionSuccessful()) {
			boxId++;
			updateTarget();
		}

		return null;
	}

	private static GameObject findTarget(Vector3 agentPosition, Vector3 doorPosition) {
		GameObject[] boxesInLevel = GameObject.FindGameObjectsWithTag("Box");
		Dictionary<int, Box> allBoxes = new Dictionary<int, Box>();
		foreach (GameObject box in boxesInLevel) {
			// Optimization: only look for viable boxes
			Vector3 boxPosition = box.transform.position;
			if ((agentPosition.x <= doorPosition.x && boxPosition.x <= doorPosition.x) ||
                (agentPosition.x >= doorPosition.x && boxPosition.x >= doorPosition.x)) {
                allBoxes.Add(box.GetComponent<Box>().getID(), box.GetComponent<Box>());
            }
		}

		Dictionary<int, Box>.KeyCollection keys = allBoxes.Keys;
		foreach (int key in keys) {
			if (allBoxes[key] != null) return allBoxes[key].gameObject;
		}
		return null;
	}

	private void updateTarget() {
		GameObject[] boxesInLevel = GameObject.FindGameObjectsWithTag("Box");
		Dictionary<int, Box> allBoxes = new Dictionary<int, Box>();
		foreach (GameObject box in boxesInLevel) {
			allBoxes.Add(box.GetComponent<Box>().getID(), box.GetComponent<Box>());
		}
		if (allBoxes.ContainsKey(boxId)) {
			target = allBoxes[boxId].gameObject;
		}
		else {
			failed = true;
		}
	}
}
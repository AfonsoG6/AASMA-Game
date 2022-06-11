using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using static AgentInterface;

public class Agent : MonoBehaviour {
	private AgentInterface i;
	private Agent partner;
	private Stack<Objective> objectives;

	void Awake()
    {	
        i = GetComponent<AgentInterface>();
		GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
		if (players[0] != this.gameObject) partner = players[0].GetComponent<Agent>();
		else partner = players[1].GetComponent<Agent>();
		objectives = new Stack<Objective>();
        objectives.Push(new ReachFlagObjective(i));
    }

	void FixedUpdate()
	{
		if (!i.isActing()) {
			i.act(getCurrentObjective().chooseAction());
			updateObjectives();
		}
	}

	private void updateObjectives() {
		if (objectives.Peek().isCompleted()) {
			objectives.Pop();
		}
		
		if (!i.wasActionSuccessful()) {
			if (i.getLastAction() == AgentAction.WALK_RIGHT && i.isDoorAt(Vector2.right)) {
				GameObject door = i.getDoorAt(Vector2.right);
				if (!((objectives.Peek() is PassDoorObjective) && ((PassDoorObjective)objectives.Peek()).target == door)) {
					objectives.Push(new PassDoorObjective(i, i.getDoorAt(Vector2.right), i.getPosition()));
				}
			}
			else if (i.getLastAction() == AgentAction.WALK_LEFT && i.isDoorAt(Vector2.left)) {
				GameObject door = i.getDoorAt(Vector2.left);
				if (!((objectives.Peek() is PassDoorObjective) && ((PassDoorObjective)objectives.Peek()).target == door)) {
					objectives.Push(new PassDoorObjective(i, i.getDoorAt(Vector2.left), i.getPosition()));
				}
			}
		}

		if (getCurrentObjective().isExclusive() &&
				getCurrentObjective().GetType() == partner.getCurrentObjective().GetType()) {
			// Give up on the current objective and support the other agent.
			objectives.Pop();
			if (partner.getCurrentObjective() is PassDoorObjective) {
				objectives.Push(new PressButtonObjective(i, (PassDoorObjective) partner.getCurrentObjective()));
			}
		}
	}

	public Objective getCurrentObjective() {
		return objectives.Peek();
	}
}
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using static AgentInterface;

public class Agent : MonoBehaviour {

	private AgentInterface i;
	private Agent partner;

	void Awake()
    {
        i = GetComponent<AgentInterface>();
		GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
		if (players[0] != this.gameObject) partner = players[0].GetComponent<Agent>();
		else partner = players[1].GetComponent<Agent>();
    }

	void FixedUpdate()
	{
		if (!i.isActing()) {
			i.act(chooseAction());
		}
	}

	private AgentAction chooseAction()
	{
		if (i.whereIsFlag().x - i.whereAmI().x > 0) {
			if (Array.IndexOf(i.whatIsAt(Direction.RIGHT), "Ground") > -1) return AgentAction.JUMP_RIGHT;
			else return AgentAction.WALK_RIGHT;
		}
		else if (i.whereIsFlag().x - i.whereAmI().x < 0) {
			if (Array.IndexOf(i.whatIsAt(Direction.LEFT), "Ground") > -1) return AgentAction.JUMP_LEFT;
			else return AgentAction.WALK_LEFT;
		}
		else {
			return AgentAction.STAY;
		}
	}

}
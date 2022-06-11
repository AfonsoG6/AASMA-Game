using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using static AgentInterface;

public abstract class Objective {
	public AgentInterface agentInterface;
	public GameObject target;

	public Objective(AgentInterface agent, GameObject target) {
		this.agentInterface = agent;
		this.target = target;
	}

	public abstract bool isRetryable();
	public abstract bool isExclusive();
	public abstract bool isCompleted();
	public abstract AgentAction chooseAction();
	//public abstract void retry();
}
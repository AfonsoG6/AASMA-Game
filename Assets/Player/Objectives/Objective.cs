using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using static AgentInterface;

public abstract class Objective {
	public float timeStarted;
	public AgentInterface agentInterface;
	public GameObject target;

	public Objective(AgentInterface agent, GameObject target) {
		this.timeStarted = Time.time;
		this.agentInterface = agent;
		this.target = target;
	}

	protected bool isOlderThan(Objective other) {
		return this.timeStarted < other.timeStarted;
	}

	public virtual bool equalsTo(Objective other) {
		return this.target == other.target;
	}

	public abstract bool isExclusive();
	public abstract bool isCompleted();
	public abstract AgentAction chooseAction();
	public abstract Objective updateObjective();
}
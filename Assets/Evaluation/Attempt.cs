using System.Collections;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Attempt {
	public double time;
	public int[] actions;

	public Attempt() {
		this.time = 0;
		this.actions = new int[2]{0, 0};
	}

	public Attempt(double time, int[] actions) {
		this.time = time;
		this.actions = actions;
	}

	public Attempt(Attempt attempt) {
		this.time = attempt.time;
		this.actions = new int[2]{attempt.actions[0], attempt.actions[1]};
	}

	public void incrementTime(double time) {
		this.time += time;
	}

	public void incrementActions(int[] actions) {
		this.actions[0] += actions[0];
		this.actions[1] += actions[1];
	}
}
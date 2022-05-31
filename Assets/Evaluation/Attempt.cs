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
		this.time = double.MaxValue;
		this.actions = new int[2];
		this.actions[0] = int.MaxValue;
		this.actions[1] = int.MaxValue;
	}

	public Attempt(double time, int[] actions) {
		this.time = time;
		this.actions = actions;
	}
}
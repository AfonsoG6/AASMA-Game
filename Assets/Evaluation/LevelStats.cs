using System.Collections;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelStats {
	public Attempt fastestAttempt;
	public Attempt simplestAttempt;
	public List<Attempt> attemptHistory;

	public LevelStats() {
		fastestAttempt = new Attempt();
		simplestAttempt = new Attempt();
		attemptHistory = new List<Attempt>();
	}

	public void addAttempt(Attempt attempt) {
		if (attempt.time < fastestAttempt.time) {
			fastestAttempt = attempt;
		}
		if (attempt.time > simplestAttempt.time) {
			simplestAttempt = attempt;
		}
		attemptHistory.Add(attempt);
	}
}
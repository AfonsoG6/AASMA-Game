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
		fastestAttempt = new Attempt(-1, new int[2]{-1, -1});
		simplestAttempt = new Attempt(-1, new int[2]{-1, -1});
		attemptHistory = new List<Attempt>();
	}

	public void addAttempt(Attempt attempt) {
		if (fastestAttempt.time == -1 || fastestAttempt.actions[0] == -1 || fastestAttempt.actions[1] == -1) {
			fastestAttempt = attempt;
		}
		else if (attempt.time < fastestAttempt.time) {
			fastestAttempt = attempt;
		}

		if (simplestAttempt.time == -1 || simplestAttempt.actions[0] == -1 || simplestAttempt.actions[1] == -1) {
			simplestAttempt = attempt;
		}
		else if (attempt.actions[0] + attempt.actions[1] < simplestAttempt.actions[0] + simplestAttempt.actions[1]) {
			simplestAttempt = attempt;
		}

		attemptHistory.Add(attempt);
	}
}
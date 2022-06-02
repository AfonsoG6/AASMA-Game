using System.Collections;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalStats {
	public Attempt fastestFullAttempt;
	public Attempt simplestFullAttempt;
	public Dictionary<string, LevelStats> levels;
	public List<Attempt> fullAttemptHistory;

	public GlobalStats() {
		fastestFullAttempt = new Attempt(-1, new int[2]{-1, -1});
		simplestFullAttempt = new Attempt(-1, new int[2]{-1, -1});
		levels = new Dictionary<string, LevelStats>();
		fullAttemptHistory = new List<Attempt>();
	}

	public void AddFullAttempt(Attempt attempt) {
		if (fastestFullAttempt.time == -1 || fastestFullAttempt.actions[0] == -1 || fastestFullAttempt.actions[1] == -1) {
			fastestFullAttempt = attempt;
		}
		else if (attempt.time < fastestFullAttempt.time) {
			fastestFullAttempt = attempt;
		}

		if (simplestFullAttempt.time == -1 || simplestFullAttempt.actions[0] == -1 || simplestFullAttempt.actions[1] == -1) {
			simplestFullAttempt = attempt;
		}
		else if (attempt.actions[0] + attempt.actions[1] < simplestFullAttempt.actions[0] + simplestFullAttempt.actions[1]) {
			simplestFullAttempt = attempt;
		}

		fullAttemptHistory.Add(attempt);
	}

	public void AddLevelAttempt(string levelName, Attempt attempt) {
		if (!levels.ContainsKey(levelName)) {
			levels.Add(levelName, new LevelStats());
		}
		LevelStats levelStats = levels[levelName];
		levelStats.addAttempt(attempt);
	}

	public string toJson() {
		return JsonConvert.SerializeObject(this);
	}
}
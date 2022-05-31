using System.Collections;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalStats {
	private Attempt fastestFullAttempt;
	private Attempt simplestFullAttempt;
	private Dictionary<string, LevelStats> levels;
	private List<Attempt> fullAttemptHistory;

	public GlobalStats() {
		fastestFullAttempt = new Attempt(double.MaxValue, new int[2]{int.MaxValue, int.MaxValue});
		simplestFullAttempt = new Attempt(double.MaxValue, new int[2]{int.MaxValue, int.MaxValue});
		levels = new Dictionary<string, LevelStats>();
		fullAttemptHistory = new List<Attempt>();
	}

	public void AddFullAttempt(Attempt attempt) {
		if (attempt.time < fastestFullAttempt.time) {
			fastestFullAttempt = attempt;
		}
		if (attempt.time > simplestFullAttempt.time) {
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
}
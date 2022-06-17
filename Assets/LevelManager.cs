using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class LevelManager : MonoBehaviour {
    private const string FILENAME_GLOBAL = "results.json";
    private GlobalStats globalStats;
    private GameObject winCanvasPrefab;
    private List<string> levelNames = new List<string>();
    private bool isFullAttempt = false;
    private bool isPerfectRun = false;
    private int currentLevelID;
    private Attempt fullAttempt = null;
    private Attempt levelAttempt = null;

    void Awake()
    {
        winCanvasPrefab = Resources.Load("UI/WinCanvas") as GameObject;
        DontDestroyOnLoad(this.gameObject);
        levelNames.Add("Basic_Level1");
        levelNames.Add("Basic_Level2");
        levelNames.Add("Basic_Level3");
        levelNames.Add("Intermediate_Level1");
        levelNames.Add("Intermediate_Level2");
        levelNames.Add("Intermediate_Level3");
        levelNames.Add("Advanced_Level1");
        levelNames.Add("Advanced_Level2");
        levelNames.Add("Advanced_Level3");
        loadGlobalStats();
    }

    void FixedUpdate()
    {
        if (levelAttempt != null) levelAttempt.time += Time.deltaTime;
    }

    public void incrActions(string playerName) {
        if (playerName == "Player1") {
            levelAttempt.actions[0]++;
        } else {
            levelAttempt.actions[1]++;
        }
    }

    public void win() {
        showWinScreen();
        Time.timeScale = 0;

        globalStats.AddLevelAttempt(SceneManager.GetActiveScene().name, new Attempt(levelAttempt));
        
        if (isFullAttempt) {
            fullAttempt.incrementTime(levelAttempt.time);
            fullAttempt.incrementActions(levelAttempt.actions);
            if (currentLevelID+1 >= levelNames.Count) {
                globalStats.AddFullAttempt(new Attempt(fullAttempt));
                saveGlobalStats();
            }
            else {
                saveGlobalStats();
                GoToNextLevel();
            }
        }
        else {
            saveGlobalStats();
        }
    }

    private void showWinScreen() {
        GameObject winCanvas = Instantiate(winCanvasPrefab);
        TMPro.TMP_Text winText = winCanvas.transform.Find("WinText").GetComponent<TMPro.TMP_Text>();
        winText.gameObject.SetActive(true);
        winText.text = "Time: " + levelAttempt.time + "\n" +
                    "Actions of Player 1: " + levelAttempt.actions[0] + "\n" +
                    "Actions of Player 2: " + levelAttempt.actions[1];
    }

    private void loadGlobalStats() {
        string path = Path.Combine(Application.dataPath, FILENAME_GLOBAL);
        if (File.Exists(path)) {
            string jsonString = File.ReadAllText(path);
            globalStats = JsonConvert.DeserializeObject<GlobalStats>(jsonString);
        }
        else {
            globalStats = new GlobalStats();
        }
    }

    private void saveGlobalStats() {
        string path = Path.Combine(Application.dataPath, FILENAME_GLOBAL);
        string jsonString = JsonConvert.SerializeObject(globalStats);
        File.WriteAllText(path, jsonString);
    }

    public void StartAttempt(int buttonID) {
        if (buttonID == -2) {
            isFullAttempt = true;
            isPerfectRun = true;
            fullAttempt = new Attempt();
            ChangeLevel(8);
        }
        else if (buttonID == -1) {
            // Full Attempt (All Levels)
            isFullAttempt = true;
            fullAttempt = new Attempt();
            ChangeLevel(0);
        }
        else {
            // Single Attempt (Single Level)
            isFullAttempt = false;
            ChangeLevel(buttonID);
        }
    }

    private void ChangeLevel(int levelID) {
        currentLevelID = levelID;
        levelAttempt = new Attempt();
        Time.timeScale = 1;
        
        SceneManager.LoadScene(levelNames[levelID]);
    }

    public void GoToNextLevel() {
        currentLevelID++;
        ChangeLevel(currentLevelID);
    }

    public bool isScriptedRun() {
        return isPerfectRun;
    }

    public List<AgentInterface.AgentAction> getCurrentLevelSolution(int agentNum) {
        List<AgentInterface.AgentAction> solution = new List<AgentInterface.AgentAction>();
        if (agentNum == 0) {
            switch(currentLevelID) {
                case 0:
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.JUMP_RIGHT);
                    solution.Add(AgentInterface.AgentAction.JUMP_RIGHT);
                    solution.Add(AgentInterface.AgentAction.JUMP_RIGHT);
                    solution.Add(AgentInterface.AgentAction.JUMP_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    break;
                case 1:
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.STAY);
                    solution.Add(AgentInterface.AgentAction.STAY);
                    solution.Add(AgentInterface.AgentAction.STAY);
                    solution.Add(AgentInterface.AgentAction.STAY);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    break;
                case 2:
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    break;
                case 3:
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.STAY);
                    solution.Add(AgentInterface.AgentAction.STAY);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.JUMP_RIGHT);
                    solution.Add(AgentInterface.AgentAction.JUMP_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.STAY);
                    solution.Add(AgentInterface.AgentAction.STAY);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    break;
                case 4:
                    solution.Add(AgentInterface.AgentAction.STAY);
                    solution.Add(AgentInterface.AgentAction.GRAB_OR_DROP);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.STAY);
                    solution.Add(AgentInterface.AgentAction.STAY);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.GRAB_OR_DROP);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    break;
                case 5:
                    solution.Add(AgentInterface.AgentAction.STAY);
                    solution.Add(AgentInterface.AgentAction.GRAB_OR_DROP);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.STAY);
                    solution.Add(AgentInterface.AgentAction.STAY);
                    solution.Add(AgentInterface.AgentAction.STAY);
                    solution.Add(AgentInterface.AgentAction.STAY);
                    solution.Add(AgentInterface.AgentAction.STAY);
                    solution.Add(AgentInterface.AgentAction.STAY);
                    solution.Add(AgentInterface.AgentAction.STAY);
                    solution.Add(AgentInterface.AgentAction.STAY);
                    solution.Add(AgentInterface.AgentAction.STAY);
                    solution.Add(AgentInterface.AgentAction.STAY);
                    solution.Add(AgentInterface.AgentAction.STAY);
                    solution.Add(AgentInterface.AgentAction.STAY);
                    solution.Add(AgentInterface.AgentAction.STAY);
                    solution.Add(AgentInterface.AgentAction.STAY);
                    solution.Add(AgentInterface.AgentAction.STAY);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.GRAB_OR_DROP);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    break;
                case 6:
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.STAY);
                    solution.Add(AgentInterface.AgentAction.STAY);
                    solution.Add(AgentInterface.AgentAction.STAY);
                    solution.Add(AgentInterface.AgentAction.STAY);
                    solution.Add(AgentInterface.AgentAction.STAY);
                    solution.Add(AgentInterface.AgentAction.STAY);
                    solution.Add(AgentInterface.AgentAction.STAY);
                    solution.Add(AgentInterface.AgentAction.STAY);
                    solution.Add(AgentInterface.AgentAction.STAY);
                    solution.Add(AgentInterface.AgentAction.STAY);
                    solution.Add(AgentInterface.AgentAction.STAY);
                    solution.Add(AgentInterface.AgentAction.STAY);
                    solution.Add(AgentInterface.AgentAction.STAY);
                    solution.Add(AgentInterface.AgentAction.STAY);
                    solution.Add(AgentInterface.AgentAction.STAY);
                    solution.Add(AgentInterface.AgentAction.STAY);
                    solution.Add(AgentInterface.AgentAction.STAY);
                    solution.Add(AgentInterface.AgentAction.STAY);
                    solution.Add(AgentInterface.AgentAction.STAY);
                    solution.Add(AgentInterface.AgentAction.STAY);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    break;
                case 7:
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.JUMP_RIGHT);
                    solution.Add(AgentInterface.AgentAction.JUMP_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.STAY);
                    solution.Add(AgentInterface.AgentAction.STAY);
                    solution.Add(AgentInterface.AgentAction.STAY);
                    solution.Add(AgentInterface.AgentAction.STAY);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    break;
                case 8:
                    solution.Add(AgentInterface.AgentAction.WALK_LEFT);
                    solution.Add(AgentInterface.AgentAction.WALK_LEFT);
                    solution.Add(AgentInterface.AgentAction.STAY);
                    solution.Add(AgentInterface.AgentAction.STAY);
                    solution.Add(AgentInterface.AgentAction.STAY);
                    solution.Add(AgentInterface.AgentAction.STAY);
                    solution.Add(AgentInterface.AgentAction.JUMP_LEFT);
                    solution.Add(AgentInterface.AgentAction.JUMP_LEFT);
                    solution.Add(AgentInterface.AgentAction.GRAB_OR_DROP);
                    solution.Add(AgentInterface.AgentAction.WALK_LEFT);
                    solution.Add(AgentInterface.AgentAction.WALK_LEFT);
                    solution.Add(AgentInterface.AgentAction.WALK_LEFT);
                    solution.Add(AgentInterface.AgentAction.GRAB_OR_DROP);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.JUMP_RIGHT);
                    solution.Add(AgentInterface.AgentAction.JUMP_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    break;
            }
        }
        else {
            switch (currentLevelID) {
                case 0:
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.JUMP_RIGHT);
                    solution.Add(AgentInterface.AgentAction.JUMP_RIGHT);
                    solution.Add(AgentInterface.AgentAction.JUMP_RIGHT);
                    solution.Add(AgentInterface.AgentAction.JUMP_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    break;
                case 1:
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.STAY);
                    solution.Add(AgentInterface.AgentAction.STAY);
                    solution.Add(AgentInterface.AgentAction.STAY);
                    solution.Add(AgentInterface.AgentAction.STAY);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    break;
                case 2:
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.GRAB_OR_DROP);
                    solution.Add(AgentInterface.AgentAction.WALK_LEFT);
                    solution.Add(AgentInterface.AgentAction.WALK_LEFT);
                    solution.Add(AgentInterface.AgentAction.GRAB_OR_DROP);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    break;
                case 3:
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.STAY);
                    solution.Add(AgentInterface.AgentAction.STAY);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.GRAB_OR_DROP);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.STAY);
                    solution.Add(AgentInterface.AgentAction.STAY);
                    solution.Add(AgentInterface.AgentAction.STAY);
                    solution.Add(AgentInterface.AgentAction.STAY);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    break;
                case 4:
                    solution.Add(AgentInterface.AgentAction.STAY);
                    solution.Add(AgentInterface.AgentAction.STAY);
                    solution.Add(AgentInterface.AgentAction.JUMP_RIGHT);
                    solution.Add(AgentInterface.AgentAction.JUMP_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.STAY);
                    solution.Add(AgentInterface.AgentAction.STAY);
                    solution.Add(AgentInterface.AgentAction.STAY);
                    solution.Add(AgentInterface.AgentAction.STAY);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    break;
                case 5:
                    solution.Add(AgentInterface.AgentAction.STAY);
                    solution.Add(AgentInterface.AgentAction.STAY);
                    solution.Add(AgentInterface.AgentAction.JUMP_RIGHT);
                    solution.Add(AgentInterface.AgentAction.JUMP_RIGHT);
                    solution.Add(AgentInterface.AgentAction.JUMP_RIGHT);
                    solution.Add(AgentInterface.AgentAction.JUMP_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.STAY);
                    solution.Add(AgentInterface.AgentAction.STAY);
                    solution.Add(AgentInterface.AgentAction.STAY);
                    solution.Add(AgentInterface.AgentAction.STAY);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    break;
                case 6:
                    solution.Add(AgentInterface.AgentAction.WALK_LEFT);
                    solution.Add(AgentInterface.AgentAction.WALK_LEFT);
                    solution.Add(AgentInterface.AgentAction.WALK_LEFT);
                    solution.Add(AgentInterface.AgentAction.WALK_LEFT);
                    solution.Add(AgentInterface.AgentAction.WALK_LEFT);
                    solution.Add(AgentInterface.AgentAction.WALK_LEFT);
                    solution.Add(AgentInterface.AgentAction.WALK_LEFT);
                    solution.Add(AgentInterface.AgentAction.WALK_LEFT);
                    solution.Add(AgentInterface.AgentAction.GRAB_OR_DROP);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.GRAB_OR_DROP);
                    solution.Add(AgentInterface.AgentAction.WALK_LEFT);
                    solution.Add(AgentInterface.AgentAction.WALK_LEFT);
                    solution.Add(AgentInterface.AgentAction.WALK_LEFT);
                    solution.Add(AgentInterface.AgentAction.WALK_LEFT);
                    solution.Add(AgentInterface.AgentAction.WALK_LEFT);
                    break;
                case 7:
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.GRAB_OR_DROP);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.STAY);
                    solution.Add(AgentInterface.AgentAction.STAY);
                    solution.Add(AgentInterface.AgentAction.STAY);
                    solution.Add(AgentInterface.AgentAction.STAY);
                    solution.Add(AgentInterface.AgentAction.STAY);
                    solution.Add(AgentInterface.AgentAction.STAY);
                    solution.Add(AgentInterface.AgentAction.STAY);
                    solution.Add(AgentInterface.AgentAction.STAY);
                    solution.Add(AgentInterface.AgentAction.JUMP_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    break;
                case 8:
                    solution.Add(AgentInterface.AgentAction.WALK_LEFT);
                    solution.Add(AgentInterface.AgentAction.GRAB_OR_DROP);
                    solution.Add(AgentInterface.AgentAction.WALK_LEFT);
                    solution.Add(AgentInterface.AgentAction.WALK_LEFT);
                    solution.Add(AgentInterface.AgentAction.WALK_LEFT);
                    solution.Add(AgentInterface.AgentAction.WALK_LEFT);
                    solution.Add(AgentInterface.AgentAction.STAY);
                    solution.Add(AgentInterface.AgentAction.STAY);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.STAY);
                    solution.Add(AgentInterface.AgentAction.STAY);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    solution.Add(AgentInterface.AgentAction.WALK_RIGHT);
                    break;
            }
        }
        return solution;
    }
}

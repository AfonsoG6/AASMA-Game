using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{

    private const string FILENAME_GLOBAL = "global.json";
    private GlobalStats globalStats;
    private bool isFullAttempt = false;
    private int currentLevelID;
    private Attempt fullAttempt;
    private Attempt levelAttempt;
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    enum Level {
        Level1
    }

    void Start()
    {
        loadGlobalStats();
    }

    void FixedUpdate()
    {
        levelAttempt.time += Time.deltaTime;
    }

    public void incrActions(string playerName) {
        if (playerName == "Player1") {
            levelAttempt.actions[0]++;
        } else {
            levelAttempt.actions[1]++;
        }
    }

    public void win() {
        globalStats.AddLevelAttempt(SceneManager.GetActiveScene().name, new Attempt(levelAttempt));
        
        if (isFullAttempt) {
            fullAttempt.incrementTime(levelAttempt.time);
            fullAttempt.incrementActions(levelAttempt.actions);
            if (currentLevelID+1 >= System.Enum.GetNames(typeof(Level)).Length) {
                globalStats.AddFullAttempt(new Attempt(fullAttempt));
            }
        }
        
        
        showWinScreen();
    }

    private void showWinScreen() {
        TMPro.TMP_Text winText = GameObject.Find("WinText").GetComponent<TMPro.TMP_Text>();
        winText.gameObject.SetActive(true);
        winText.text = "Time: " + levelAttempt.time + "\n" +
                    "Actions of Player 1: " + levelAttempt.actions[0] + "\n" +
                    "Actions of Player 2: " + levelAttempt.actions[1];
    }

    private void loadGlobalStats() {
        string path = Path.Combine(Application.dataPath, FILENAME_GLOBAL);
        if (File.Exists(path)) {
            string jsonString = File.ReadAllText(path);
            globalStats = JsonUtility.FromJson<GlobalStats>(jsonString);
        }
        else {
            globalStats = new GlobalStats();
        }
    }

    private void saveGlobalStats() {
        string path = Path.Combine(Application.dataPath, FILENAME_GLOBAL);
        string jsonString = JsonUtility.ToJson(globalStats);
        File.WriteAllText(path, jsonString);
    }

    public void StartAttempt(int buttonID) {
        if (buttonID == -1) {
            // Full Attempt (All Levels)
            isFullAttempt = true;
            fullAttempt = new Attempt();
            ChangeLevel((Level) 0);
        }
        else {
            // Single Attempt (Single Level)
            isFullAttempt = false;
            ChangeLevel((Level) buttonID);
        }
    }

    private void ChangeLevel(Level level) {
        currentLevelID = (int) level;
        SceneManager.LoadScene(System.Enum.GetName(typeof(Level), level));
    }

    public void GoToNextLevel() {
        currentLevelID++;
        ChangeLevel((Level) currentLevelID);
    }
}

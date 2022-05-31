using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Evaluator : MonoBehaviour
{
    private const string FILENAME_GLOBAL = "global.json";
    private const string FILENAME_SESSION = "session.json";
//--------------------------------JSON Keys--------------------------------
    private const string FASTEST_FULL_RUN = "fastest_full_run";
    private const string SIMPLEST_FULL_RUN = "simplest_full_run";
    private const string FASTEST_RUN = "fastest_run";
    private const string SIMPLEST_RUN = "simplest_run";
    private const string LEVELS = "levels";
    private const string HISTORY = "history";
    private const string TIME = "time";
    private const string ACTIONS = "actions";
//--------------------------------------------------------------------------

    [SerializeField] private TMPro.TMP_Text winText;
    private GlobalStats globalStats;
    private string currentLevel;
    private int playersInRange = 0;
    private float timer = 0;
    private int[] actions = {0, 0};
    void Start()
    {
        currentLevel = SceneManager.GetActiveScene().name;
        winText.gameObject.SetActive(false);
        loadGlobalStats();
    }

    void FixedUpdate()
    {
        timer += Time.deltaTime;
    }

    public void incrActions(string playerName) {
        if (playerName == "Player1") {
            actions[0]++;
        } else {
            actions[1]++;
        }
    }

    private bool checkWin() {
        return playersInRange == 2;
    }

    private void win() {
        globalStats.AddLevelAttempt(currentLevel, new Attempt(timer, actions));
        /*
        if (is a full attempt AND no level left) {
            globalStats.AddFullAttempt(new Attempt(timer, actions));
        }
        */
        showWinScreen();
    }

    private void showWinScreen() {
        winText.gameObject.SetActive(true);
        winText.text = "Time: " + timer + "\n" +
                    "Actions of Player 1: " + actions[0] + "\n" +
                    "Actions of Player 2: " + actions[1];
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playersInRange++;
            if (checkWin()) win();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playersInRange--;
        }
    }
}

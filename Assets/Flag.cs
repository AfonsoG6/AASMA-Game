using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Evaluator : MonoBehaviour
{
    private int playersInRange = 0;

    private bool checkWin() {
        return playersInRange == 2;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playersInRange++;
            if (checkWin()) {
                LevelManager levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
                levelManager.win();
            }
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

using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Flag : MonoBehaviour
{
    private int playersInRange = 0;

    private bool checkWin() {
        return playersInRange == 2;
    }

    void OnTriggerEnter2D(Collider2D other)
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

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playersInRange--;
        }
    }
}

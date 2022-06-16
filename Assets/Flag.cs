using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Flag : MonoBehaviour
{   
    private int playersInRange = 0;
    private bool player1inRange = false;
    private bool player2inRange = false;

    private bool checkWin() {
        return playersInRange == 2;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playersInRange++;
            if (other.name == "Player1") {
			    player1inRange = true;
		    }
		    else if (other.name == "Player2") {
			    player2inRange = true;
		    }
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
            if (other.name == "Player1") {
			    player1inRange = false;
		    }
		    else if (other.name == "Player2") {
			    player2inRange = false;
		    }
        }
    }

    public bool checkPlayerWin(string name) {
        if (name == "Player1") {
			return player1inRange;
		}
		else if (name == "Player2") {
			return player2inRange;
		}
        return false;
    }
}

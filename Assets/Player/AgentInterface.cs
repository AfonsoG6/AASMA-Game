using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentInterface : MonoBehaviour
{
    private const float MOVE_UNIT_TIME = 0.21f;
    private const float JUMP_TIME = 0.1f;
    private LevelManager levelManager;
    private PlayerControlsManager playerControls;

    void Awake()
    {
        playerControls = GetComponent<PlayerControlsManager>();
        levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.U)) {
            playerControls.toggleControlsEnabled();
        }
        else if (Input.GetKeyDown(KeyCode.I)) {
            act(Action.JUMP_LEFT);
        }
        else if (Input.GetKeyDown(KeyCode.O)) {
            act(Action.JUMP_RIGHT);
        }
        else if (Input.GetKeyDown(KeyCode.K)) {
            act(Action.WALK_LEFT);
        }
        else if (Input.GetKeyDown(KeyCode.L)) {
            act(Action.WALK_RIGHT);
        }
        else if (Input.GetKeyDown(KeyCode.P)) {
            act(Action.GRAB_OR_DROP);
        }

    }

    public IEnumerator jumpRight() {
		playerControls.setJumping(true);
		yield return new WaitForSeconds(JUMP_TIME);
		playerControls.setJumping(false);
		playerControls.setMoving(1);
		yield return new WaitForSeconds(MOVE_UNIT_TIME);
		playerControls.setMoving(0);
	}

	public IEnumerator jumpLeft() {
		playerControls.setJumping(true);
		yield return new WaitForSeconds(JUMP_TIME);
		playerControls.setJumping(false);
		playerControls.setMoving(-1);
		yield return new WaitForSeconds(MOVE_UNIT_TIME);
		playerControls.setMoving(0);
	}

	public IEnumerator walkRight() {
		playerControls.setMoving(1);
		yield return new WaitForSeconds(MOVE_UNIT_TIME);
		playerControls.setMoving(0);
	}

	public IEnumerator walkLeft() {
		playerControls.setMoving(-1);
		yield return new WaitForSeconds(MOVE_UNIT_TIME);
		playerControls.setMoving(0);
	}

    public IEnumerator grabOrDrop() {
        playerControls.setInteracting(true);
        yield return new WaitForSeconds(0.1f);
        playerControls.setInteracting(false);
    }

    public void act(Action action) {
        levelManager.incrActions(this.gameObject.name);
        switch (action) {
            case Action.JUMP_RIGHT:
                StartCoroutine(jumpRight());
                break;
            case Action.JUMP_LEFT:
                StartCoroutine(jumpLeft());
                break;
            case Action.WALK_RIGHT:
                StartCoroutine(walkRight());
                break;
            case Action.WALK_LEFT:
                StartCoroutine(walkLeft());
                break;
            case Action.GRAB_OR_DROP:
                StartCoroutine(grabOrDrop());
                break;
            default:
                break;
        }
    }

    public enum Action {
		JUMP_RIGHT,
		JUMP_LEFT,
		WALK_RIGHT,
		WALK_LEFT,
		GRAB_OR_DROP,
		STAY
	}
}

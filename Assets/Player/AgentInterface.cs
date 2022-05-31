using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentInterface : MonoBehaviour
{
    private const float MOVE_UNIT_TIME = 0.21f;
    private const float JUMP_TIME = 0.1f;
    private PlayerControlsManager playerControls;

    void Awake()
    {
        playerControls = GetComponent<PlayerControlsManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.U)) {
            playerControls.toggleControls();
        }
        if (Input.GetKeyDown(KeyCode.I)) {
            StartCoroutine(jumpLeft());
        }
        if (Input.GetKeyDown(KeyCode.O)) {
            StartCoroutine(jumpRight());
        }
        if (Input.GetKeyDown(KeyCode.K)) {
            StartCoroutine(walkLeft());
        }
        if (Input.GetKeyDown(KeyCode.L)) {
            StartCoroutine(walkRight());
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

    public IEnumerator interact() {
        playerControls.setInteracting(true);
        yield return new WaitForSeconds(0.1f);
        playerControls.setInteracting(false);
    }

    public enum Action {
		JUMP_RIGHT,
		JUMP_LEFT,
		WALK_RIGHT,
		WALK_LEFT,
		GRAB_DROP,
		STAY
	}
}

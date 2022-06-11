using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentInterface : MonoBehaviour
{
    private const float MOVE_UNIT_TIME = 0.21f;
    private const float JUMP_TIME = 0.1f;
    private LevelManager levelManager;
    private PlayerControlsManager playerControls;
    private bool acting = false;

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
            act(AgentAction.JUMP_LEFT);
        }
        else if (Input.GetKeyDown(KeyCode.O)) {
            act(AgentAction.JUMP_RIGHT);
        }
        else if (Input.GetKeyDown(KeyCode.K)) {
            act(AgentAction.WALK_LEFT);
        }
        else if (Input.GetKeyDown(KeyCode.L)) {
            act(AgentAction.WALK_RIGHT);
        }
        else if (Input.GetKeyDown(KeyCode.P)) {
            act(AgentAction.GRAB_OR_DROP);
        }
    }

    public bool isActing() {
        return acting;
    }

    public IEnumerator jumpRight() {
        Vector2 targetPos = new Vector2(Mathf.Floor(transform.position.x + 1) + 0.5f, transform.position.y + 1);

        acting = true;
		playerControls.setJumping(true);
		while (transform.position.y < targetPos.y) {
            yield return new WaitForFixedUpdate();
        }
		playerControls.setJumping(false);
		playerControls.setMoving(1);
		while (transform.position.x > targetPos.x) {
            yield return new WaitForFixedUpdate();
        }
		playerControls.setMoving(0);
        acting = false;
    }

	public IEnumerator jumpLeft() {
        Vector2 targetPos = new Vector2(Mathf.Floor(transform.position.x - 1) + 0.5f, transform.position.y + 1);

        acting = true;
		playerControls.setJumping(true);
		while (transform.position.y < targetPos.y) {
            yield return new WaitForFixedUpdate();
        }
		playerControls.setJumping(false);
		playerControls.setMoving(-1);
		while (transform.position.x < targetPos.x) {
            yield return new WaitForFixedUpdate();
        }
		playerControls.setMoving(0);
        acting = false;
	}

	public IEnumerator walkRight() {
        Vector2 targetPos = new Vector2(Mathf.Floor(transform.position.x + 1) + 0.5f, transform.position.y);
        acting = true;
		playerControls.setMoving(1);
        while (transform.position.x < targetPos.x) {
            yield return new WaitForFixedUpdate();
        }
		playerControls.setMoving(0);
        acting = false;
	}

	public IEnumerator walkLeft() {
        Vector2 targetPos = new Vector2(Mathf.Floor(transform.position.x - 1) + 0.5f, transform.position.y);
        acting = true;
		playerControls.setMoving(-1);
		while (transform.position.x > targetPos.x) {
            yield return new WaitForFixedUpdate();
        }
		playerControls.setMoving(0);
        acting = false;
	}

    public IEnumerator grabOrDrop() {
        acting = true;
        playerControls.setInteracting(true);
        yield return new WaitForSeconds(0.1f);
        playerControls.setInteracting(false);
        acting = false;
    }

    public void act(AgentAction action) {
        levelManager.incrActions(this.gameObject.name);
        switch (action) {
            case AgentAction.JUMP_RIGHT:
                StartCoroutine(jumpRight());
                break;
            case AgentAction.JUMP_LEFT:
                StartCoroutine(jumpLeft());
                break;
            case AgentAction.WALK_RIGHT:
                StartCoroutine(walkRight());
                break;
            case AgentAction.WALK_LEFT:
                StartCoroutine(walkLeft());
                break;
            case AgentAction.GRAB_OR_DROP:
                StartCoroutine(grabOrDrop());
                break;
            default:
                break;
        }
    }

    public Vector2 whereAmI() {
        return transform.position;
    }

    public Vector2 whereIsFlag() {
        return GameObject.Find("Flag").transform.position;
    }

    public string[] whatIsAt(Direction direction) {
        Vector3 dirVector;
        switch (direction) {
            case Direction.LEFT:
                dirVector = Vector3.left;
                break;
            case Direction.RIGHT:
                dirVector = Vector3.right;
                break;
            case Direction.UP:
                dirVector = Vector3.up;
                break;
            default:
                dirVector = Vector3.down;
                break;
        }
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, dirVector, 1f);
        string[] tags = new string[hits.Length];
        for (int i = 0; i < hits.Length; i++) {
            tags[i] = hits[i].collider.tag;
        }
        return tags;
    }

    public enum Direction {
        LEFT,
        RIGHT,
        UP
    }

    public enum AgentAction {
		JUMP_RIGHT,
		JUMP_LEFT,
		WALK_RIGHT,
		WALK_LEFT,
		GRAB_OR_DROP,
		STAY
	}
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentInterface : MonoBehaviour
{
    private const float MOVE_UNIT_TIME = 0.21f;
    private const float JUMP_TIME = 0.1f;
    private const float TIMEOUT = 0.5f;
    private LevelManager levelManager;
    private PlayerControlsManager playerControls;
    private GameObject attachedBox;
    private bool acting = false;
    private AgentAction lastAction = AgentAction.STAY;
    private bool actionSuccessful = true;

    void Awake()
    {
        playerControls = GetComponent<PlayerControlsManager>();
        levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        Transform[] children = GetComponentsInChildren<Transform>();
        foreach (Transform child in children) {
            if (child.gameObject.name == "AttachedBox") {
                attachedBox = child.gameObject;
            }
        }
    }

    public bool isActing() {
        return acting;
    }
    
    public bool wasActionSuccessful() {
        return actionSuccessful;
    }

    public AgentAction getLastAction() {
        return lastAction;
    }

    public IEnumerator jumpRight() {
        Vector2 targetPos = new Vector2(Mathf.Floor(transform.position.x + 1) + 0.5f, transform.position.y + 1);
        float timer = 0;

        acting = true;
		playerControls.setJumping(true);
		while (transform.position.y < targetPos.y) {
            yield return new WaitForFixedUpdate();
            timer += Time.fixedDeltaTime;
            if (timer > TIMEOUT) {
                playerControls.setJumping(false);
                acting = false;
                actionSuccessful = false;
                yield return null;
            }
        }
		playerControls.setJumping(false);
		playerControls.setMoving(1);
		while (transform.position.x > targetPos.x) {
            yield return new WaitForFixedUpdate();
            timer += Time.fixedDeltaTime;
            if (timer > TIMEOUT) {
                playerControls.setMoving(0);
                acting = false;
                actionSuccessful = false;
                yield return null;
            }
        }
		playerControls.setMoving(0);
        acting = false;
        actionSuccessful = true;
    }

	public IEnumerator jumpLeft() {
        Vector2 targetPos = new Vector2(Mathf.Floor(transform.position.x - 1) + 0.5f, transform.position.y + 1);
        float timer = 0;

        acting = true;
		playerControls.setJumping(true);
		while (transform.position.y < targetPos.y) {
            yield return new WaitForFixedUpdate();
            timer += Time.fixedDeltaTime;
            if (timer > TIMEOUT) {
                playerControls.setJumping(false);
                acting = false;
                actionSuccessful = false;
                yield return null;
            }
        }
		playerControls.setJumping(false);
		playerControls.setMoving(-1);
		while (transform.position.x < targetPos.x) {
            yield return new WaitForFixedUpdate();
            timer += Time.fixedDeltaTime;
            if (timer > TIMEOUT) {
                playerControls.setMoving(0);
                acting = false;
                actionSuccessful = false;
                yield return null;
            }
        }
		playerControls.setMoving(0);
        acting = false;
        actionSuccessful = true;
	}

	public IEnumerator walkRight() {
        Vector2 targetPos = new Vector2(Mathf.Floor(transform.position.x + 1) + 0.5f, transform.position.y);
        float timer = 0;

        acting = true;
		playerControls.setMoving(1);
        while (transform.position.x < targetPos.x) {
            yield return new WaitForFixedUpdate();
            timer += Time.fixedDeltaTime;
            if (timer > TIMEOUT) {
                playerControls.setMoving(0);
                acting = false;
                actionSuccessful = false;
                yield return null;
            }
        }
		playerControls.setMoving(0);
        acting = false;
        actionSuccessful = true;
	}

	public IEnumerator walkLeft() {
        Vector2 targetPos = new Vector2(Mathf.Floor(transform.position.x - 1) + 0.5f, transform.position.y);
        float timer = 0;

        acting = true;
		playerControls.setMoving(-1);
		while (transform.position.x > targetPos.x) {
            yield return new WaitForFixedUpdate();
            timer += Time.fixedDeltaTime;
            if (timer > TIMEOUT) {
                playerControls.setMoving(0);
                acting = false;
                actionSuccessful = false;
                yield return null;
            }
        }
		playerControls.setMoving(0);
        acting = false;
        actionSuccessful = true;
	}

    public IEnumerator grabOrDrop() {
        bool startingState = attachedBox.activeSelf;

        acting = true;
        playerControls.setInteracting(true);
        yield return new WaitForSeconds(0.1f);
        playerControls.setInteracting(false);
        acting = false;

        actionSuccessful = attachedBox.activeSelf != startingState;
    }

    public void act(AgentAction action) {
        levelManager.incrActions(this.gameObject.name);
        lastAction = action;
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
                // STAY
                actionSuccessful = true;
                break;
        }
    }

    public Vector2 getPosition() {
        return transform.position;
    }

    public Vector2 getFlagPosition() {
        return GameObject.Find("Flag").transform.position;
    }

    public AgentAction getActionWalkTowards(Vector2 target) {
        if (target.x > transform.position.x) {
            if (isGroundAt(Vector2.right)) return AgentAction.JUMP_RIGHT;
            else return AgentAction.WALK_RIGHT;
        }
        else if (target.x < transform.position.x) {
            if (isGroundAt(Vector2.right)) return AgentAction.JUMP_LEFT;
            else return AgentAction.WALK_LEFT;
        }
        else {
            return AgentAction.STAY;
        }
    }

    public string[] getTagsOfEverythingAt(Vector3 direction) {
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, direction, 1f);
        string[] tags = new string[hits.Length];
        for (int i = 0; i < hits.Length; i++) {
            tags[i] = hits[i].collider.tag;
        }
        return tags;
    }

    public bool isGroundAt(Vector3 direction) {
        return Array.IndexOf(getTagsOfEverythingAt(direction), "Ground") > -1;
    }

    public bool isDoorAt(Vector3 direction) {
        return Array.IndexOf(getTagsOfEverythingAt(direction), "Door") > -1;
    }

    public GameObject getDoorAt(Vector3 direction) {
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, direction, 1f);
        foreach (RaycastHit2D hit in hits) {
            if (hit.collider.tag == "Door") {
                return hit.collider.gameObject;
            }
        }
        return null;
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

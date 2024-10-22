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
    private Agent partner;
    private GameObject attachedBox;
    private bool acting = false;
    private AgentAction lastAction = AgentAction.STAY;
    private bool actionSuccessful = true;

    void Awake()
    {
        playerControls = GetComponent<PlayerControlsManager>();
        levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
		if (players[0] != this.gameObject) partner = players[0].GetComponent<Agent>();
		else partner = players[1].GetComponent<Agent>();
        Transform[] children = GetComponentsInChildren<Transform>();
        foreach (Transform child in children) {
            if (child.gameObject.name == "AttachedBox") {
                attachedBox = child.gameObject;
            }
        }
    }

    public Agent getPartner() {
        return partner;
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
                yield break;
            }
        }
		playerControls.setJumping(false);
		playerControls.setMoving(1);
		while (transform.position.x < targetPos.x) {
            yield return new WaitForFixedUpdate();
            timer += Time.fixedDeltaTime;
            if (timer > TIMEOUT) {
                playerControls.setMoving(0);
                acting = false;
                actionSuccessful = false;
                yield break;
            }
        }
		playerControls.setMoving(0);
        actionSuccessful = true;
        acting = false;
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
                yield break;
            }
        }
		playerControls.setJumping(false);
		playerControls.setMoving(-1);
		while (transform.position.x > targetPos.x) {
            yield return new WaitForFixedUpdate();
            timer += Time.fixedDeltaTime;
            if (timer > TIMEOUT) {
                playerControls.setMoving(0);
                acting = false;
                actionSuccessful = false;
                yield break;
            }
        }
		playerControls.setMoving(0);
        actionSuccessful = true;
        acting = false;
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
                yield break;
            }
        }
		playerControls.setMoving(0);
        actionSuccessful = true;
        acting = false;
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
                yield break;
            }
        }
		playerControls.setMoving(0);
        actionSuccessful = true;
        acting = false;
	}

    public IEnumerator grabOrDrop() {
        bool startingState = attachedBox.activeSelf;
        acting = true;
        playerControls.setInteracting(true);
        yield return new WaitForSeconds(0.1f);
        playerControls.setInteracting(false);
        actionSuccessful = attachedBox.activeSelf != startingState;
        acting = false;
    }

    public IEnumerator stay() {
        acting = true;
        yield return new WaitForSeconds(0.1f);
        actionSuccessful = true;
        acting = false;
    }

    public void act(AgentAction action) {
        if (action != AgentAction.STAY) {
            levelManager.incrActions(this.gameObject.name);
        }
        lastAction = action;
        actionSuccessful = true;
        //Debug.Log("Agent " + this.gameObject.name + " is acting: " + action);
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
                StartCoroutine(stay());
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
            if (isPathBlocked(getPosition(), Vector2.right) && !isPathBlocked(getPosition()+Vector2.up, Vector2.right)) return AgentAction.JUMP_RIGHT;
            else return AgentAction.WALK_RIGHT;
        }
        else if (target.x < transform.position.x) {
            if (isPathBlocked(getPosition(), Vector2.left) && !isPathBlocked(getPosition()+Vector2.up, Vector2.left)) return AgentAction.JUMP_LEFT;
            else return AgentAction.WALK_LEFT;
        }
        else {
            return AgentAction.STAY;
        }
    }

    public PassDoorObjective getPassDoorObjective() {
        if (wasActionSuccessful()) return null;
        if (getLastAction() == AgentAction.WALK_RIGHT) {
            if (isDoorAt(Vector2.right)) {
                return new PassDoorObjective(this, getDoorAt(Vector2.right), getPosition());
            }
            else if (isDoorAt(Vector2.right, true)) {
                return new PassDoorObjective(this, getDoorAt(Vector2.right, true), getPosition());
            }
        }
        else if (getLastAction() == AgentAction.WALK_LEFT) {
            if (isDoorAt(Vector2.left)) {
                return new PassDoorObjective(this, getDoorAt(Vector2.left), getPosition());
            }
            else if (isDoorAt(Vector2.left, true)) {
                return new PassDoorObjective(this, getDoorAt(Vector2.left, true), getPosition());
            }
        }
        return null;
    }

    // Returns the most suitable objective to help a partner pass through a door
    public Objective helpPassDoorObjective() {
		if (partner.getCurrentObjective() is PassDoorObjective) {
			PassDoorObjective partnerObjective = (PassDoorObjective)partner.getCurrentObjective();
			if (partnerObjective.target.GetComponent<Door>().getReachableButtons(getPosition()).Count <= 0 && canJumpOverWithHelp(getPosition(), partnerObjective.targetDirection))
				return new JumpOverObjective(this, partnerObjective, partnerObjective.targetDirection, getJumpTargetPosition(partnerObjective.target.transform.position));
			else if (reachableBoxExists(partnerObjective.target.transform.position))
				return new PressButtonWithBoxObjective(this, partnerObjective);
			else
				return new PressButtonObjective(this, partnerObjective);
		}
        return null;
    }

    // Returns HelpJumpOverObjective if partner agent is trying to jump over something
    public HelpJumpOverObjective helpJumpOverObjective() {
        if (partner.getCurrentObjective() is JumpOverObjective && !partner.getCurrentObjective().isCompleted()) {
			JumpOverObjective partnerObjective = (JumpOverObjective)partner.getCurrentObjective();
			HelpJumpOverObjective newObjective = new HelpJumpOverObjective(this, partnerObjective);
			partnerObjective.supportingObjective = newObjective;
			return newObjective;
		}
        return null;
    }

    public GameObject[] getEverythingAt(Vector3 sourcePos, Vector3 direction) {
        RaycastHit2D[] hits = Physics2D.RaycastAll(sourcePos, direction, 1f);
        GameObject[] everything = new GameObject[hits.Length];
        for (int i = 0; i < hits.Length; i++) {
            everything[i] = hits[i].collider.gameObject;
        }
        return everything;
    }

    public bool arrayContains(GameObject[] array, string tag) {
        foreach (GameObject obj in array) {
            if (obj.tag == tag) return true;
        }
        return false;
    }

    public GameObject findTagInArray(GameObject[] array, string tag) {
        foreach (GameObject obj in array) {
            if (obj.tag == tag) return obj;
        }
        return null;
    }

    public bool isGroundAt(Vector3 direction, bool upOne = false) {
        Vector3 origin = getPosition();
        if (upOne) origin.y += 1;
        return arrayContains(getEverythingAt(origin, direction), "Ground");
    }

    public bool isDoorAt(Vector3 direction, bool upOne = false) {
        Vector3 origin = getPosition();
        if (upOne) origin.y += 1;
        return arrayContains(getEverythingAt(origin, direction), "Door");
    }

    public GameObject getDoorAt(Vector3 direction, bool upOne = false) {
        Vector3 origin = getPosition();
        if (upOne) origin.y += 1;
        RaycastHit2D[] hits = Physics2D.RaycastAll(origin, direction, 1f);
        foreach (RaycastHit2D hit in hits) {
            if (hit.collider.tag == "Door") {
                return hit.collider.gameObject;
            }
        }
        return null;
    }

    public bool hasBox() {
        return attachedBox.activeSelf;
    }

    public bool dropBox() {
        return getPartner().getCurrentObjective() is FindBoxObjective && hasBox();
    }

    public bool boxExists() {
        return GameObject.FindGameObjectsWithTag("Box").Length > 0 || this.hasBox();
    }

    // Optimization: check if a reachable box exists
    public bool reachableBoxExists(Vector3 doorPosition) {
        if (hasBox()) return true;
        GameObject[] boxesInLevel = GameObject.FindGameObjectsWithTag("Box");
		foreach (GameObject box in boxesInLevel) {
			Vector3 boxPosition = box.transform.position;
			if ((getPosition().x <= doorPosition.x && boxPosition.x <= doorPosition.x) ||
                (getPosition().x >= doorPosition.x && boxPosition.x >= doorPosition.x)) {
                return true;
            }
		}
        return false;
    }

    public bool isPathBlocked(Vector3 origin, Vector3 direction) {
        GameObject[] everything = getEverythingAt(origin, direction);
        if (arrayContains(everything, "Ground")) return true;
        else if (arrayContains(everything, "Door")) {
            Door door = findTagInArray(everything, "Door").GetComponent<Door>();
            return !door.isOpen();
        }
        else return false;
    }

    public bool canJumpOverWithHelp(Vector3 position, int direction) {
        Vector3 sourceDirection = new Vector3(direction, 0, 0);
        Vector3 sourcePosition = new Vector3(position.x, position.y + 2, position.z);
        GameObject[] everything = getEverythingAt(sourcePosition, sourceDirection);
        return !arrayContains(everything, "Ground");
    }

    public int lastActionDirection() {
        if (lastAction == AgentAction.WALK_RIGHT || lastAction == AgentAction.JUMP_RIGHT)
            return +1;
        else if (lastAction == AgentAction.WALK_LEFT || lastAction == AgentAction.JUMP_LEFT)
            return -1;
        else return 0;
    }

    public JumpOverObjective getButtonJumpOverObjective(PassDoorObjective supportedObjective) {
        int direction = lastActionDirection();
        if (partner.getCurrentObjective() is PassDoorObjective) {
            PassDoorObjective partnerObjective = (PassDoorObjective)partner.getCurrentObjective();
            if (supportedObjective != partnerObjective) return null;
        }
        if (canJumpOverWithHelp(getPosition(), direction)) {
            Vector3 tilePosition = new Vector3(getPosition().x + direction, getPosition().y, transform.position.z);
            return new JumpOverObjective(this, supportedObjective, direction, getJumpTargetPosition(tilePosition));
        }
        else return null;
    }

    public bool hasGonePastPosition(Vector3 position, int direction) {
        if (direction == +1)
			return getPosition().x > position.x;
		else
			return getPosition().x < position.x;
    }

    public Vector3 getJumpTargetPosition(Vector3 sourcePosition) {
        GameObject[] everything = getEverythingAt(sourcePosition, Vector2.up);
        if (arrayContains(everything, "Ground"))
            return new Vector3(sourcePosition.x, sourcePosition.y + 2, sourcePosition.z);
        else
            return new Vector3(sourcePosition.x, sourcePosition.y + 1, sourcePosition.z);
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

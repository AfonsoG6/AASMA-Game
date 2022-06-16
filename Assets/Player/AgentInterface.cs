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

    public JumpOverObjective getJumpOverObjective() {
        if (wasActionSuccessful()) return null;
        if ((getLastAction() == AgentAction.WALK_RIGHT || getLastAction() == AgentAction.JUMP_RIGHT) &&
                isPathBlocked(getPosition(),                Vector2.right) && 
                isPathBlocked(getPosition() +   Vector2.up, Vector2.right) &&
                !isPathBlocked(getPosition() + 2*Vector2.up, Vector2.right)) {
			return new JumpOverObjective(this, roundPosition(getPosition()+2*Vector2.up+Vector2.right));
        }
        else if ((getLastAction() == AgentAction.WALK_LEFT || getLastAction() == AgentAction.JUMP_LEFT) &&
                isPathBlocked(getPosition(),                Vector2.left) && 
                isPathBlocked(getPosition() +   Vector2.up, Vector2.left) &&
                !isPathBlocked(getPosition() + 2*Vector2.up, Vector2.left)) {
            return new JumpOverObjective(this, roundPosition(getPosition()+2*Vector2.up+Vector2.left));
        }
        else return null;
    }

    public Objective getObjectiveAfterActionUnsuccessful(Objective currentObjective) {
        if (!wasActionSuccessful()) {
            PassDoorObjective pdo = getPassDoorObjective();
            if (pdo != null) {
                Door targetDoor = pdo.target.GetComponent<Door>();
                if (!(currentObjective is PressButtonWithBoxObjective) &&
                    targetDoor.getReachableButtons(getPosition()).Count > 0 &&
                        (hasBox() ||
                        reachableBoxExists(targetDoor.transform.position))) {
                    Objective partnerObjective = getPartner().getCurrentObjective();
                    PressButtonWithBoxObjective pbwbo = new PressButtonWithBoxObjective(this, pdo);
                    if (!(partnerObjective is PressButtonWithBoxObjective && pbwbo.target == partnerObjective.target ||
                            partnerObjective is FindBoxObjective && pbwbo.target == ((FindBoxObjective)partnerObjective).supportedObjective.target)) {
                        return pbwbo;
                    }
                }
                if (targetDoor.getReachableButtons(getPosition()).Count <= 0 &&
                        targetDoor.areBothPlayersOnSameSide() &&
                        reachableBoxExists(targetDoor.transform.position)) {
                    JumpOverObjective joo = getJumpOverObjective();
                    if (joo != null) {
                        return joo;
                    }
                }
                return pdo;
            }
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

    public int getTargetDirection(Vector3 targetPosition) {
        if (targetPosition.x > transform.position.x) return 1;
        else if (targetPosition.x < transform.position.x) return -1;
        else return 0;
    }

    public bool isDoorAt(Vector3 direction, bool upOne = false) {
        Vector3 origin = getPosition();
        if (upOne) origin.y += 1;
        return arrayContains(getEverythingAt(origin, direction), "Door");
    }

    public bool isClosedDoorAt(Vector3 direction, bool upOne = false) {
        return isDoorAt(direction, upOne) && !getDoorAt(direction, upOne).GetComponent<Door>().isOpen();
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

    public bool boxExists() {
        return GameObject.FindGameObjectsWithTag("Box").Length > 0 || this.hasBox();
    }

    // Optimization
    public bool reachableBoxExists(Vector3 doorPosition) {
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

    public bool canJumpOverDoor(Vector3 doorPosition, int direction) {
        Vector3 sourceDirection = new Vector3(direction, 0, 0);
        Vector3 sourcePosition = new Vector3(doorPosition.x - direction, doorPosition.y + 2, doorPosition.z);
        return !arrayContains(getEverythingAt(sourcePosition, sourceDirection), "Ground");
    }

    public bool hasGonePastDoor(Vector3 doorPosition, int direction) {
        if (direction == +1)
			return getPosition().x > doorPosition.x;
		else
			return getPosition().x < doorPosition.x;
    }

    public Vector2 roundPosition(Vector2 position) {
        return new Vector2(Mathf.Floor(position.x)+0.5f, Mathf.Floor(position.y)+0.5f);
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

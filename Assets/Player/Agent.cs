using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using static AgentInterface;

public class Agent : MonoBehaviour {

	[SerializeField] private int role;

	private AgentInterface i;
	private Agent partner;
	private List<GameObject> objectives;
    private List<Objective> objectiveTypes;
	private bool onFlag = false;

	void Awake()
    {	
        i = GetComponent<AgentInterface>();
		GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
		if (players[0] != this.gameObject) partner = players[0].GetComponent<Agent>();
		else partner = players[1].GetComponent<Agent>();
		objectives = new List<GameObject>();
        objectiveTypes = new List<Objective>();
        addObjective(GameObject.Find("Flag"), Objective.FLAG);
    }

	void FixedUpdate()
	{
		if (!i.isActing()) {
			i.act(chooseAction());
		}
	}

	void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Flag"))
        {
            onFlag = true;
        }
    }

	void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Flag"))
        {
            onFlag = false;
        }
    }

	private bool currentObjectiveFulfilled() {
		if (currentObjectiveType() == Objective.FLAG)
			return onFlag;
		if (nextObjective() == null)
			return false;
		if (currentObjectiveType() == Objective.PRESS_BUTTON_0 || currentObjectiveType() == Objective.PRESS_BUTTON_1) {
			float doorX = currentObjective().transform.position.x;
			float nextX = nextObjective().transform.position.x;
			float agentX = i.whereAmI().x;
			if ((distance(doorX, nextX) < 0 && Math.Abs(distance(doorX, agentX)) > 2 && objectiveDirection(doorX) == Direction.LEFT && objectiveDirection(nextX) == Direction.RIGHT) ||
				(distance(doorX, nextX) > 0 && Math.Abs(distance(doorX, agentX)) < 2 && objectiveDirection(doorX) == Direction.RIGHT && objectiveDirection(nextX) == Direction.LEFT)) {
				return true; // Made it past door and towards flag (or next objective)
			}
		}
		return false;
	}

	private AgentAction chooseAction()
	{
		if (currentObjectiveFulfilled() && partner.currentObjectiveFulfilled()) {
			removeCurrentObjective();
			partner.removeCurrentObjective(); //TODO
		} else if (!currentObjectiveFulfilled() && partner.currentObjectiveFulfilled() && nextObjective() != null)
			return moveTowardsObjective(nextObjective().transform.position.x);
		if (atCurrentObjective()) {
			return AgentAction.STAY;
		}
		else if (Array.IndexOf(i.whatIsAt(objectiveDirection(currentObjectivePos().x)), "Door") > -1) {
			GameObject[] doors = GameObject.FindGameObjectsWithTag("Door");
			// Only working for one door with two buttons separated at the moment
			// Also need to check for different doors (objectives)
			if (!hasObjective(doors[0])) {
				Objective subObjective = (role == 0 ? Objective.PRESS_BUTTON_0 : Objective.PRESS_BUTTON_1);
				addObjective(doors[0], subObjective);
			}
			return moveTowardsObjective(currentObjectivePos().x);
		}
		else return moveTowardsObjective(currentObjectivePos().x);
	}

	private AgentAction moveTowardsObjective(float objectiveX)
	{
		if (Array.IndexOf(i.whatIsAt(objectiveDirection(objectiveX)), "Ground") > -1) return jumpDirection(objectiveX);
		else return walkDirection(objectiveX);
	}

	public bool atCurrentObjective() {
        switch (currentObjectiveType()) {
            case Objective.FLAG:
                return onFlag;
                break;
            case Objective.PRESS_BUTTON_0:
			case Objective.PRESS_BUTTON_1:
                List<Button> buttons = currentObjective().GetComponent<Door>().getButtons();
				return buttons[role].pressed();
                break;
            default:
                return objectiveDistance(currentObjectivePos().x) != 0;
                break;
        }
    }

	public float distance(float x1, float x2) {
		return x1 - x2;
	}

	public float objectiveDistance(float objectiveX) {
		return distance(objectiveX, i.whereAmI().x);
	}

    public Direction objectiveDirection(float objectiveX) {
        if (objectiveDistance(objectiveX) > 0)
			return Direction.RIGHT;
		else
			return Direction.LEFT;
    }

    public AgentAction walkDirection(float objectiveX) {
		if (objectiveDistance(objectiveX) > 0)
			return AgentAction.WALK_RIGHT;
		else if (objectiveDistance(objectiveX) < 0)
			return AgentAction.WALK_LEFT;
		else
			return AgentAction.STAY;
	}

	public AgentAction jumpDirection(float objectiveX) {
		if (objectiveDistance(objectiveX) > 0)
			return AgentAction.JUMP_RIGHT;
		else if (objectiveDistance(objectiveX) < 0)
			return AgentAction.JUMP_LEFT;
		else
			return AgentAction.STAY;
	}

    public void addObjective(GameObject objective, Objective objectiveType) {
        objectives.Insert(0, objective);
        objectiveTypes.Insert(0, objectiveType);
    }

    public void removeCurrentObjective() {
        objectives.RemoveAt(0);
        objectiveTypes.RemoveAt(0);
    }
    
    public GameObject currentObjective() {
        return objectives[0];
    }

    public Objective currentObjectiveType() {
        return objectiveTypes[0];
    }

    public Vector2 currentObjectivePos() {
        if (currentObjectiveType() == Objective.PRESS_BUTTON_0 || currentObjectiveType() == Objective.PRESS_BUTTON_1) {
            List<Button> buttons = currentObjective().GetComponent<Door>().getButtons();
            int b = (currentObjectiveType() == Objective.PRESS_BUTTON_0 ? 0 : 1);
            return buttons[b].transform.position;
        }
        return objectives[0].transform.position;
    }

    public bool hasObjective(GameObject objective) {
        return objectives.Contains(objective);
    }

	public GameObject nextObjective() {
		if (objectives.Count > 1)
			return objectives[1];
		return null;
	}

	public int getRole() {
		return role;
	}

	public enum Objective {
        FLAG,
        PRESS_BUTTON_0,
        PRESS_BUTTON_1,
        GRAB_BOX,
        NONE
    }

}
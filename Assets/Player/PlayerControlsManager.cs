using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
using UnityEngine.Rendering.PostProcessing;

public class PlayerControlsManager : MonoBehaviour
{
/* -------------------------------- CONSTANTS ------------------------------- */

	private const float PICKUP_COOLDOWN = 0.5f;

/* ------------------------------- ATTRIBUTES ------------------------------- */
	private PostProcessVolume postProcessVolume;
	private Tilemap tilemap;
	private TextMesh toolTip;
	private GameObject attachedBox;
	private GameObject boxPrefab;
	private int grabbedBoxID = -1;
	private bool interacting = false;
	private float moving = 0f;
	private bool jumping = false;
	private float toolTipTimer = -2000f;
	private float pickupCooldown = 0f;
	private bool controlsDisabled = false;

/* ------------------------------ INPUT METHODS ----------------------------- */

	public void OnMovement(InputAction.CallbackContext context) {
		if (controlsDisabled) return;
		moving = context.ReadValue<float>();
	}

	public void OnJump(InputAction.CallbackContext context) {
		if (controlsDisabled) return;
		if (context.ReadValue<float>() > 0) jumping = true;
		else jumping = false;
	}

    public void OnInteract(InputAction.CallbackContext context) {
		if (controlsDisabled) return;
		if (context.ReadValue<float>() > 0) interacting = true;
		else interacting = false;
	}

/* ------------------------------ UNITY METHODS ----------------------------- */
	void Awake()
	{
		boxPrefab = Resources.Load<GameObject>("Puzzles/Box");
		postProcessVolume = GameObject.Find("MainCamera").GetComponent<PostProcessVolume>();
		tilemap = GameObject.Find("Tilemap").GetComponent<Tilemap>();
		toolTip = findTooltipInChildren();
		attachedBox = findAttachedBoxInChildren();
		attachedBox.SetActive(false);
	}

	void FixedUpdate() {
		if (toolTip.text != "" && toolTipTimer > 0) {
			toolTipTimer -= Time.deltaTime;
		}
		else if (toolTipTimer > -1000f) {
			clearTooltipText();
			toolTipTimer = -2000f;
		}

		if (pickupCooldown > 0) {
			pickupCooldown -= Time.deltaTime;
		}
		else if (hasBox() && isInteracting()) {
			// Spawn Box
			GameObject newBox = Instantiate(boxPrefab, attachedBox.transform.position, Quaternion.identity);
			attachedBox.SetActive(false);
			newBox.GetComponentInChildren<Box>().setID(grabbedBoxID);
		}
	}

/* ----------------------------- NORMAL METHODS ----------------------------- */

	private TextMesh findTooltipInChildren() {
		Transform[] transforms = GetComponentsInChildren<Transform>();
		foreach (Transform t in transforms) {
			if (t.tag == "Tooltip") {
				return t.GetComponent<TextMesh>();
			}
		}
		return null;
	}

	private GameObject findAttachedBoxInChildren() {
		Transform[] transforms = GetComponentsInChildren<Transform>();
		foreach (Transform t in transforms) {
			if (t.name == "AttachedBox") {
				return t.gameObject;
			}
		}
		return null;
	}

	public void setTooltipText(string text) {
		toolTip.text = text;
	}

	public void setTooltipText(string text, int durationInSeconds) {
		setTooltipText(text);
		toolTipTimer = durationInSeconds;
	}

	public void clearTooltipText() {
		toolTip.text = "";
	}

	public void pickBox(int id) {
		attachedBox.SetActive(true);
		grabbedBoxID = id;
		pickupCooldown = PICKUP_COOLDOWN;
	}

	public bool hasBox() {
		return attachedBox.activeSelf;
	}

	public bool isJumping() {
		return jumping;
	}

	public void setJumping(bool jumping) {
		this.jumping = jumping;
	}

	public float getMoving() {
		return moving;
	}

	public void setMoving(float moving) {
		this.moving = moving;
	}

	public bool isInteracting() {
		return interacting;
	}

	public void setInteracting(bool interacting) {
		this.interacting = interacting;
	}

	public void setControlsEnabled(bool enabled) {
		controlsDisabled = enabled;
	}

	public void toggleControlsEnabled() {
		controlsDisabled = !controlsDisabled;
	}

}
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
using UnityEngine.Rendering.PostProcessing;

public class PlayerControlsManager : MonoBehaviour
{
/* ------------------------------- ATTRIBUTES ------------------------------- */
	private PostProcessVolume postProcessVolume;
	private Tilemap tilemap;
	private TextMesh toolTip;
	private bool interacting = false;
	private float toolTipTimer = -2000f;
/* ------------------------------ INPUT METHODS ----------------------------- */

    public void OnInteract(InputAction.CallbackContext context) {
		if (context.ReadValue<float>() > 0) interacting = true;
		else interacting = false;
	}

/* ------------------------------ UNITY METHODS ----------------------------- */
	void Awake()
	{
		postProcessVolume = GameObject.Find("MainCamera").GetComponent<PostProcessVolume>();
		tilemap = GameObject.Find("Tilemap").GetComponent<Tilemap>();
		toolTip = GameObject.Find("Tooltip").GetComponent<TextMesh>();
		toolTip.GetComponent<Renderer>().sortingLayerName = "Tooltip";
	}

	void FixedUpdate() {
		if (toolTip.text != "" && toolTipTimer > 0) {
			toolTipTimer -= Time.deltaTime;
		}
		else if (toolTipTimer > -1000f) {
			clearTooltipText();
			toolTipTimer = -2000f;
		}
	}

/* ----------------------------- NORMAL METHODS ----------------------------- */
	public bool isInteracting() {
		return interacting;
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
}

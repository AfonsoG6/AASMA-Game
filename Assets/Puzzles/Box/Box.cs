using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Box : MonoBehaviour
{
	protected PlayerControlsManager[] playerControlsManager = new PlayerControlsManager[2];
	private bool[] waitingForInteract = {false, false};
	private Vector3 originalPosition;

	protected void pick(int i) {
		playerControlsManager[i].pickBox();
		Destroy(transform.parent.gameObject);
	}

	protected virtual void Awake() {
		playerControlsManager[0] = GameObject.Find("Player1").GetComponent<PlayerControlsManager>();
		playerControlsManager[1] = GameObject.Find("Player2").GetComponent<PlayerControlsManager>();
		originalPosition = transform.position;
	}

    void OnTriggerStay2D(Collider2D other) {
		if (!other.CompareTag("Player")) return;

		if (other.name == "Player1") checkInteractions(0);
		else if (other.name == "Player2") checkInteractions(1);
	}

	private void checkInteractions(int i) {
		if (!waitingForInteract[i]) {
			waitingForInteract[i] = true;
			playerControlsManager[i].setTooltipText("Press E to pick up");
		}
		if (waitingForInteract[i] && playerControlsManager[i].isInteracting()) {
			if (playerControlsManager[i].hasBox()) return;
			waitingForInteract[i] = false;
			playerControlsManager[i].clearTooltipText();
			pick(i);
		}
	}

	void OnTriggerExit2D(Collider2D other) {
		if (!other.CompareTag("Player")) return;

		if (other.name == "Player1") {
			waitingForInteract[0] = false;
			playerControlsManager[0].clearTooltipText();
		}
		else if (other.name == "Player2") {
			waitingForInteract[1] = false;
			playerControlsManager[1].clearTooltipText();
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
/* -------------------------------- CONSTANTS ------------------------------- */
	private const float RUN_SPEED = 300f;
	private const float CLIMB_SPEED = 4f;
	private const float MOV_SMOOTHING = .05f;
	private const float FALL_GRAVITY = 3f;
	[SerializeField] private Transform[] GROUND_CHECKS;
	private const float GROUNDED_RADIUS = .1f; // Radius of the overlap circle to determine if grounded
	private const float JUMP_FORCE = 425f;
/* ------------------------------- ATTRIBUTES ------------------------------- */
	private Rigidbody2D rb2D;
	private SpriteRenderer spriteRend;
	private Animator animator;
	private float timeStill = 0f;
	private bool grounded;
	private Vector3 velocity = Vector3.zero;
	private bool isFrozen = false;
	private Vector3 prevVelocity;
	private PlayerControlsManager playerControls;
/* ------------------------------ UNITY METHODS ----------------------------- */
	void Awake() {
		rb2D = GetComponent<Rigidbody2D>();
		spriteRend = GetComponent<SpriteRenderer>();
		animator = GetComponent<Animator>();
		playerControls = GetComponent<PlayerControlsManager>();
	}

	void FixedUpdate() {
		grounded = checkIsGrounded();

		updateMovement(playerControls.getMoving() * Time.fixedDeltaTime, playerControls.isJumping());
		updateAnimations();
	}
/* ----------------------------- NORMAL METHODS ----------------------------- */

	private bool isMoving() {
		return playerControls.getMoving() != 0;
	}

	private bool isGrounded() {
		return grounded && (-0.01f <= rb2D.velocity.y && rb2D.velocity.y <= 0.01f);
	}

	private void updateAnimations() {
		// Update Animator for Walking animation
		animator.SetBool("Moving", isMoving());
		// Update Animator for Jumping animation
		animator.SetFloat("VelocityY", rb2D.velocity.y);
		// Update Animator for Idle animation
		if (isMoving() || playerControls.isJumping()) timeStill = 0;
		else if (timeStill < 1000) {	// To prevent overflow
			timeStill = (timeStill + Time.deltaTime);
		}
		animator.SetFloat("TimeStill", timeStill);
	}

	private bool checkIsGrounded() {
		foreach (Transform check in GROUND_CHECKS) {
			Collider2D[] colliders = Physics2D.OverlapCircleAll(check.position, GROUNDED_RADIUS, LayerMask.GetMask("Ground"));
			for (int i = 0; i < colliders.Length; i++) {
				if (colliders[i].gameObject != gameObject) {	// Ignore self (Kinda redundant)
					return true;
				}
			}
		}
		return false;
	}

	public void updateMovement(float move, bool jump) {
		if (isFrozen) return;

		// Check if the character should be flipped
		updateOrientation(move);

		updateJump(jump);
		// Calculating the target velocity for the character
		Vector3 targetVelocity = new Vector2(move * RUN_SPEED, rb2D.velocity.y);
		// And then smoothing it out and applying it to the character
		rb2D.velocity = Vector3.SmoothDamp(rb2D.velocity, targetVelocity, ref velocity, MOV_SMOOTHING);
	}

	public void knock(Vector2 knockback) {
		rb2D.velocity = new Vector2(rb2D.velocity.x, 0);
		rb2D.AddForce(knockback);
	}

	private void updateOrientation(float movingSide) {
		if ((movingSide > 0 && spriteRend.flipX)        // If moving right and sprite facing left
		|| (movingSide < 0 && !spriteRend.flipX)) {     // If moving left and sprite facing right
			spriteRend.flipX = !spriteRend.flipX;
		}
	}

	private void updateJump(bool jump) {
		if (jump && isGrounded()) {
			// Make the character jump
			grounded = false;
			rb2D.AddForce(new Vector2(0f, JUMP_FORCE));
		}
	}

	public void setIdle() {
		timeStill = 1000;
		animator.SetFloat("TimeStill", timeStill);
	}

	public void resetIdle() {
		timeStill = 0;
	}

	public void teleportTo(Vector3 position) {
		transform.position = position;
		GameObject.Find("CursedMask").transform.position = position;
	}


// --------------------------------- ACTIONS --------------------------------- //
}

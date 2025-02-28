using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Podoboo : MonoBehaviour
{
	public float jumpHeight = 9f; // Height of Podoboo's jump
	private float maxJumpDelay = 1f; // Maximum time between jumps
	public float maxYPosition = 20f; // Maximum Y position Podoboo can reach
	public float minYPosition = -20f; // Minimum Y position Podoboo can reach

	private Rigidbody2D rb;
	private bool isJumping = false;
	private float jumpDelayTimer; // Timer for tracking the time until the next jump

	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		jumpDelayTimer = 10f; // Initialize the jump delay timer
	}

	void Update()
	{
		// Update the jump delay timer
		jumpDelayTimer -= Time.deltaTime;
		if (jumpDelayTimer <= 0f)
		{
			Jump();
			jumpDelayTimer = maxJumpDelay; // Reset the jump delay timer
		}

		// Keep Podoboo's Y position within the specified range
		transform.position = new Vector2(transform.position.x, Mathf.Clamp(transform.position.y, minYPosition, maxYPosition));
	}

	void Jump()
	{
		// Ensure Podoboo is not already jumping
		if (!isJumping)
		{
			isJumping = true;
			// Set random X velocity to add variety to jumps
			rb.velocity = new Vector2(Random.Range(-1f, 1f), 0f);
			// Apply upward force for jump
			rb.AddForce(Vector2.up * Mathf.Sqrt(jumpHeight * -2f * Physics2D.gravity.y), ForceMode2D.Impulse);
		}
	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		if (!collision.gameObject.CompareTag("Player"))
		{
			isJumping = false;
			jumpDelayTimer = maxJumpDelay; // Reset the jump delay timer
		}

		if (collision.gameObject.CompareTag("Player"))
		{
			Player player = collision.gameObject.GetComponent<Player>();
			player.Hit();
		}
	}
}

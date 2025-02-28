using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Blooper : MonoBehaviour
{
	public float moveSpeed = 2f;
	public float moveDuration = 1f; // Duration of movement
	public float stopDuration = 1f; // Duration of stop
	public Transform player;
	public float verticalDownSpeed = 2f; // Speed for vertical downward movement
	public AnimatedSprite idleSpriteAnim;
	public AnimatedSprite startAct;

	private bool isMoving = true;
	private Vector3 direction;
	private float moveTimer = 0f;
	private float stopTimer = 0f;
	private bool isStopping = false;

	private new Rigidbody2D rigidbody;
	private Vector2 velocity;

	private void Awake()
	{
		rigidbody = GetComponent<Rigidbody2D>();
		enabled = false;
	}

	private void OnBecameVisible()
	{
#if UNITY_EDITOR
		enabled = !EditorApplication.isPaused;
#else
        enabled = true;
#endif
	}

	private void OnBecameInvisible()
	{
		enabled = false;
	}

	private void OnEnable()
	{
		rigidbody.WakeUp();
	}

	private void OnDisable()
	{
		rigidbody.velocity = Vector2.zero;
		rigidbody.Sleep();
	}

	private void FixedUpdate()
	{
		if (isMoving)
		{
			// If not stopping, move in the current direction
			if (!isStopping)
			{
				transform.Translate(direction * moveSpeed * Time.deltaTime);
				moveTimer += Time.deltaTime;

				// Check if it's time to stop
				if (moveTimer >= moveDuration)
				{
					isStopping = true;
					moveTimer = 0f;
				}
			}
			else
			{
				// Increment stop timer
				stopTimer += Time.deltaTime;

				// Check if stop duration has elapsed
				if (stopTimer >= stopDuration)
				{
					// Start moving again
					StartMoving();
				}
				else
				{
					// Move vertically downwards
					transform.Translate(Vector3.down * verticalDownSpeed * Time.deltaTime);
				}
			}
		}

		// Update direction towards the player
		if (player != null)
		{
			direction = (player.position - transform.position).normalized;
		}

		idleSpriteAnim.enabled = isMoving && isStopping;
		startAct.enabled= isMoving && !isStopping;
	}



	void StartMoving()
	{
		// Reset timers
		stopTimer = 0f;
		moveTimer = 0f;

		// Start moving
		isMoving = true;
		isStopping = false;
	}

	void StopMoving()
	{
		// Stop moving
		isMoving = false;
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("Player"))
		{
			StopMoving();

			rigidbody.velocity = Vector2.zero;
			Player player = collision.gameObject.GetComponent<Player>();

			player.Hit();
		}
	}
}


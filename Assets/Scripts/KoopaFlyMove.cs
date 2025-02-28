using System.Collections;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class KoopaFlyMove : MonoBehaviour
{
	public float speed = 1f;
	public float jumpForce = 10f; // Adjust this value as needed
	public Vector2 direction = Vector2.left;
	public float jumpHeight = 10f;
	private bool facingLeft = true;
	public bool canJump = true;

	private new Rigidbody2D rigidbody;
	private Vector2 velocity;

	private Coroutine collisionExitCoroutine;

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
		if (!canJump)
		{
			velocity.x = speed * (facingLeft ? -1 : 1);
			velocity.y += Physics2D.gravity.y * Time.fixedDeltaTime;

			rigidbody.velocity = velocity;
			rigidbody.MovePosition(rigidbody.position + velocity * Time.fixedDeltaTime);
		}
		
		if (rigidbody.Raycast(direction))
		{
			direction = -direction;
		}

		if (rigidbody.Raycast(Vector2.down))
		{
			velocity.y = Mathf.Max(velocity.y, 0f);
		}

		if (direction.x > 0f)
		{
			transform.localEulerAngles = new Vector3(0f, 180f, 0f);
		}
		else if (direction.x < 0f)
		{
			transform.localEulerAngles = Vector3.zero;
		}
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.layer == LayerMask.NameToLayer("EnemyBarrier"))
		{
			// Change direction if collision with EnemyBarrier
			direction = -direction;
		}

		if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
		{
			if (collision.transform.DotTest(transform, Vector2.right) || collision.transform.DotTest(transform, Vector2.left))
			{
				facingLeft = !facingLeft;
				// Flip the sprite horizontally
				transform.localScale = new Vector3(facingLeft ? -1 : 1, 1, 1);
			}
		}

		if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
		{
			// Ignore collision with Player
			Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>());
		}
	}
	
	private void OnCollisionStay2D(Collision2D collision)
	{
		if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
		{
			// Ignore collision with Player
			Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>());
		}

		if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
		{
			if (IsTopCollision(collision.contacts[0].normal))
			{
				if (canJump)
				{
					Jump();
				}
			}
		}
	}

	private void OnCollisionExit2D(Collision2D collision)
	{
		if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
		{
			// Stop ignoring collision with Player after 1 second
			if (collisionExitCoroutine != null)
			{
				StopCoroutine(collisionExitCoroutine);
			}
			collisionExitCoroutine = StartCoroutine(ResetCollisionAfterDelay(collision.collider));
		}
	}

	public void Jump()
	{
		float jumpVelocity = Mathf.Sqrt(2f * jumpHeight * -Physics2D.gravity.y);

		// Apply the jump force gradually over time
		rigidbody.velocity = new Vector2(-speed, jumpVelocity); // Set negative horizontal velocity for jumping left
	}

	private bool IsTopCollision(Vector2 normal)
	{
		// Check if the collision is from the top of the ground
		return normal.y > 0.9f; // Adjust the threshold as needed
	}

	private IEnumerator ResetCollisionAfterDelay(Collider2D collider)
	{
		yield return new WaitForSeconds(0.5f);
		Physics2D.IgnoreCollision(collider, GetComponent<Collider2D>(), false);
	}
}


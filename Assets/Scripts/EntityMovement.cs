using System.Collections;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EntityMovement : MonoBehaviour
{
	public float speed = 1f;
	public Vector2 direction = Vector2.left;

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
		velocity.x = direction.x * speed;
		velocity.y += Physics2D.gravity.y * Time.fixedDeltaTime;

		rigidbody.MovePosition(rigidbody.position + velocity * Time.fixedDeltaTime);

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
	}
	private void OnCollisionStay2D(Collision2D collision)
	{
		if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
		{
			// Ignore collision with Player
			Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>());
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
	private IEnumerator ResetCollisionAfterDelay(Collider2D collider)
	{
		yield return new WaitForSeconds(0.5f);
		Physics2D.IgnoreCollision(collider, GetComponent<Collider2D>(), false);
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeBowser : MonoBehaviour
{
	public float moveSpeed = 1f; // Speed of Fake Bowser movement
	public float jumpForce = 8f; // Force applied when Fake Bowser jumps
	public float detectionRange = 5f; // Range to detect the player
	public float attackRange = 5f; // Range to attack the player
	public GameObject fireballPrefab; // Prefab of the fireball projectile
	public Transform firePoint; // Point where fireballs are spawned
	public Transform Fireout;
	public float speed = 5;

	private Rigidbody2D rb;
	private bool isFacingRight = true; // Flag to indicate direction
	private bool isAttacking = false; // Flag to indicate if Fake Bowser is attacking
	private Transform playerTF; // Reference to the player's transform

	//private Player player;

	bool grounded = true;

	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		playerTF = GameObject.FindGameObjectWithTag("Player").transform;

		isFacingRight = false;
	}

	void Update()
	{
		// Check if player is within detection range
		if (Vector2.Distance(transform.position, playerTF.position) <= detectionRange)
		{
			// Face the player
			if (playerTF.position.x < transform.position.x && isFacingRight)
			{
				Flip();
			}
			else if (playerTF.position.x > transform.position.x && !isFacingRight)
			{
				Flip();
			}

			// Check if player is within attack range
			if (Vector2.Distance(transform.position, playerTF.position) <= attackRange)
			{
				// Attack the player
				Attack();
			}
		}

		Vector3 firePointPos = firePoint.position;
		firePointPos.y = playerTF.position.y;
		firePoint.position = firePointPos;

		Move();

		// Jump occasionally
		if (Random.Range(0, 100) < 1 && grounded == true)
		{
			Jump();
			grounded = false;
		}
	}

	void Move()
	{
		if (isFacingRight)
		{
			rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
		}
		else
		{
			rb.velocity = new Vector2(-moveSpeed, rb.velocity.y);
		}

		if (Random.Range(0, 100) < 1) // Adjust the probability as needed
		{
			StartCoroutine(WalkBack());
		}
	}

	void Jump()
	{
		rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
	}

	void Attack()
	{
		if (!isAttacking)
		{
			isAttacking = true;
			// Spawn fireball at Fireout position
			GameObject fireball = Instantiate(fireballPrefab, Fireout.position, Quaternion.identity);
			// Move the fireball to firePoint position
			StartCoroutine(MoveFireball(fireball.transform, firePoint.position));
			// Set direction of fireball
			Rigidbody2D fireballRB = fireball.GetComponent<Rigidbody2D>();
			fireballRB.velocity = isFacingRight ? Vector2.right * moveSpeed * 3f : Vector2.left * moveSpeed * 3f;
			// Reset attack flag after delay
			StartCoroutine(ResetAttack());
		}
	}

	IEnumerator ResetAttack()
	{
		float attackDelay = Random.Range(1.5f, 3f);
		yield return new WaitForSeconds(attackDelay); // Adjust attack delay as needed
		isAttacking = false;
	}

	void Flip()
	{
		isFacingRight = !isFacingRight;
		Vector3 scale = transform.localScale;
		scale.x *= -1;
		transform.localScale = scale;
	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		Player player = collision.gameObject.GetComponent<Player>();

		if (collision.gameObject.CompareTag("Ground"))
		{
			isFacingRight = !isFacingRight;
			transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);

			grounded = true;
		}

		if (collision.gameObject.layer == LayerMask.NameToLayer("EnemyBarrier"))
		{
			isFacingRight = !isFacingRight; // Change facing direction
		}

		if (collision.gameObject.CompareTag("Player"))
		{
			player.Hit();
		}
	}

	IEnumerator MoveFireball(Transform fireballTransform, Vector3 targetPosition)
	{
		while (Vector3.Distance(fireballTransform.position, targetPosition) > 0.125f)
		{
			fireballTransform.position = Vector3.MoveTowards(fireballTransform.position, targetPosition, speed * Time.deltaTime);
			yield return null;
		}

		fireballTransform.position = targetPosition;
	}

	IEnumerator WalkBack()
	{
		float walkBackDistance = Random.Range(0.5f, 1.5f); // Adjust the distance as needed
		float timer = 0f;

		while (timer < walkBackDistance)
		{
			if (isFacingRight)
			{
				rb.velocity = new Vector2(-moveSpeed, rb.velocity.y);
			}
			else
			{
				rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
			}

			timer += Time.deltaTime;
			yield return null;
		}
	}
}

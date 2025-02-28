using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerBrother : MonoBehaviour
{
	public float moveSpeed = 2f; // Speed of Hammer Brother movement
	public float jumpForce = 5f; // Force applied when jumping
	public float throwInterval = 2f; // Interval between throws
	public GameObject hammerPrefab; // Prefab of the hammer projectile

	private Rigidbody2D rb;
	private bool isFacingRight = true; // Flag to indicate direction
	private float throwTimer = 0f; // Timer for throws
	private float waitTimer = 0f;

	private int scoreValue = 1000;
	public GameObject scorePopupPrefab;

	private GameObject scoreCanvas;
	private GameManager gameManager;

	bool grounded = true;

	private void Awake()
	{
		gameManager = FindObjectOfType<GameManager>();
		scoreCanvas = GameObject.Find("ScoreCanvas");

		rb = GetComponent<Rigidbody2D>();
	}

	void Update()
	{
		// Move horizontally
		Move();

		// Jump occasionally
		if (Random.Range(0, 100) < 1 && grounded == true)
		{
			Jump();
			grounded = false;
		}

		// Throw hammers periodically
		throwTimer += Time.deltaTime;
		waitTimer += Time.deltaTime;

		if (throwTimer >= throwInterval)
		{
			// Reset throw timer
			throwTimer = 0f;

			// Check if it's time to wait
			if (waitTimer >= 3f)
			{
				// Reset wait timer
				waitTimer = 0f;
				StartCoroutine(StopThrowingCoroutine());
			}
			else
			{
				// Throw the hammer
				ThrowHammer();
			}
		}
	}

	// Method to move the Hammer Brother
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
	}

	// Method to make the Hammer Brother jump
	void Jump()
	{
		rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
	}

	// Method to throw a hammer
	void ThrowHammer()
	{
		GameObject hammer = Instantiate(hammerPrefab, transform.position, Quaternion.identity);
		Rigidbody2D hammerRb = hammer.GetComponent<Rigidbody2D>();
		hammerRb.velocity = new Vector2(-5f, 5f);
	}

	IEnumerator StopThrowingCoroutine()
	{
		yield return new WaitForSeconds(2f);
	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("Ground"))
		{
			isFacingRight = !isFacingRight;
			transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);

			grounded= true;
		}

		if (collision.gameObject.layer == LayerMask.NameToLayer("EnemyBarrier"))
		{
			isFacingRight = !isFacingRight; 
		}

		if (collision.gameObject.CompareTag("Player"))
		{
			Player player = collision.gameObject.GetComponent<Player>();

			if (player.starpower)
			{
				Hit();
			}
			else
			{
				player.Hit();
			}
		}

		if (collision.gameObject.CompareTag("FireBall"))
		{
			Addscore();
		}
	}

	private void Hit()
	{
		GetComponent<AnimatedSprite>().enabled = false;
		GetComponent<DeathAnimation>().enabled = true;

		Addscore();
		Destroy(gameObject, 3f);
	}

	private void ShowScorePopup(int scoreValue)
	{
		if (scoreCanvas != null && gameObject != null)
		{
			Vector3 positionDifference = transform.position - scoreCanvas.transform.position;

			Vector3 popupPosition = scoreCanvas.transform.position + positionDifference + Vector3.up * 2f;

			GameObject scorePopup = Instantiate(scorePopupPrefab, popupPosition, Quaternion.identity, scoreCanvas.transform);

			ScorePopup popupScript = scorePopup.GetComponent<ScorePopup>();

			if (popupScript != null)
			{
				popupScript.ShowScore(scoreValue);
			}
		}
	}

	private void Addscore()
	{
		ShowScorePopup(scoreValue);
		gameManager.score += scoreValue;
	}
}

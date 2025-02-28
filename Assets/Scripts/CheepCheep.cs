using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheepCheep : MonoBehaviour
{
	public float moveSpeed = 2f; // Speed of Cheep Cheep movement
	public float jumpForce = 50f; // Force applied when jumping out of water
	public bool underWater = true; // Y position of the water surface
	public Transform player; // Reference to the player's transform
	public Sprite die;

	private Rigidbody2D rb;
	private float jumpTimer = 0f; // Timer to track time between jumps

	//private bool isFrozen = false;
	private float freezeDuration = 1f;
	private bool hasJump = false;

	private int scoreValue = 100;
	public GameObject scorePopupPrefab;

	private GameObject scoreCanvas;
	private GameManager gameManager;

	void Start()
	{
		gameManager = FindObjectOfType<GameManager>();
		scoreCanvas = GameObject.Find("ScoreCanvas");

		rb = GetComponent<Rigidbody2D>();
	}

	void FixedUpdate()
	{
		bool playerOnRight = (player.position.x > transform.position.x);

		if (underWater)
		{
			transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);
		}

		if (!underWater && !hasJump)
		{
			if (jumpTimer >= 0.57f)
			{
				JumpTowardsPlayer();
				jumpTimer = 0f;
			}
		}

		if (!underWater && IsOnEnemyBarrier() && playerOnRight)
		{
			jumpTimer += Time.deltaTime;
		}
	}

	// Method to make the Cheep Cheep jump towards the player's position
	private void JumpTowardsPlayer()
	{
		if (player == null)
		{
			return;
		}

		hasJump = true;

		Vector2 direction = (player.position - transform.position).normalized;
		rb.velocity = Vector2.zero;
		rb.AddForce(direction * jumpForce, ForceMode2D.Impulse);
	}


	void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.layer == LayerMask.NameToLayer("EnemyBarrier"))
		{
			rb.velocity = Vector2.zero; // Stop movement
		}

		if (collision.gameObject.CompareTag("Player"))
		{
			Player player = collision.gameObject.GetComponent<Player>();

			if (underWater)
			{
				
				player.Hit();
			
			}
			else
			{
				if (collision.transform.DotTest(transform, Vector2.down))
				{
					GetComponent<SpriteRenderer>().sprite = die;

					StartCoroutine(FreezePositionForDuration(freezeDuration));

					Addscore();
				}
				else
				{
					player.Hit();
				}
			}
		}
	}

	private bool IsOnEnemyBarrier()
	{
		Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, transform.localScale, 0f);
		foreach (Collider2D collider in colliders)
		{
			if (collider.gameObject.layer == LayerMask.NameToLayer("EnemyBarrier"))
			{
				return true;
			}
		}
		return false;
	}

	private IEnumerator FreezePositionForDuration(float duration)
	{
		yield return new WaitForSeconds(duration);
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


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KoopaFly : MonoBehaviour
{
	private KoopaFlyMove koopaFlyMove;

	public Sprite shellSprite;
	public AnimatedSprite walkSpriteAnim;
	public AnimatedSprite shellSpriteAnim;

	public float shellSpeed = 12f;
	public GameObject scorePopupPrefab;
	
	private GameObject scoreCanvas;

	private GameManager gameManager;
	private int scoreValue = 100;
	private int koopaDieTwice = 200;

	private bool shelled = false;
	private bool pushed = false;
	private bool walked = true;

	private int HP = 2;

	private void Awake()
	{
		gameManager = FindObjectOfType<GameManager>();
		koopaFlyMove = GetComponent<KoopaFlyMove>();
		scoreCanvas = GameObject.Find("ScoreCanvas");

		walked = true;
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (!shelled && collision.gameObject.CompareTag("Player"))
		{
			Player player = collision.gameObject.GetComponent<Player>();
			if (collision.transform.DotTest(transform, Vector2.down))
			{
				koopaFlyMove.canJump = false;
				HP--;

				Addscore();
			}
			else
			{
				player.Hit();
			}

			Addscore();

			if (HP <=0)
			{
				if (player.starpower)
				{
					koopaFlyMove.canJump = false;
					Hit();
				}
				else if (collision.transform.DotTest(transform, Vector2.down))
				{
					walked = false;
					koopaFlyMove.canJump = false;
					EnterShell();
				}
				else
				{
					player.Hit();
					Addscore();
				}
			}
		}

		if (collision.gameObject.CompareTag("FireBall"))
		{
			KoopaDieTwice();
		}
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (shelled && other.CompareTag("Player"))
		{
			if (!pushed)
			{
				Vector2 direction = new Vector2(transform.position.x - other.transform.position.x, 0f);
				PushShell(direction);
				walked = false;
			}
			else
			{
				Player player = other.GetComponent<Player>();

				if (player.starpower)
				{
					Hit();
				}
				else
				{
					player.Hit();
				}
			}
		}
		else if (!shelled && other.gameObject.layer == LayerMask.NameToLayer("Shell"))
		{
			Hit();
		}
	}

	private void EnterShell()
	{
		if (!walked)
		{
			shelled = true;
		}

		ShowScorePopup(scoreValue);
		Addscore();

		GetComponent<SpriteRenderer>().sprite = shellSprite;
		GetComponent<AnimatedSprite>().enabled = false;
		GetComponent<KoopaFlyMove>().enabled = false;
	}

	private void PushShell(Vector2 direction)
	{
		pushed = true;

		GetComponent<AnimatedSprite>().enabled = true;
		shellSpriteAnim.enabled = shelled && pushed && !walked;
		walkSpriteAnim.enabled = !shelled && !pushed && walked;

		GetComponent<Rigidbody2D>().isKinematic = false;

		KoopaFlyMove movement = GetComponent<KoopaFlyMove>();
		movement.direction = direction.normalized;
		movement.speed = shellSpeed;
		movement.enabled = true;

		gameObject.layer = LayerMask.NameToLayer("Shell");
	}

	private void Hit()
	{
		GetComponent<AnimatedSprite>().enabled = false;
		GetComponent<DeathAnimation>().enabled = true;
		Addscore();
		Destroy(gameObject, 3f);
	}

	private void OnBecameInvisible()
	{
		if (pushed)
		{
			Destroy(gameObject);
		}
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

	private void KoopaDieTwice()
	{
		ShowScorePopup(koopaDieTwice);
		gameManager.score += koopaDieTwice;
	}
}

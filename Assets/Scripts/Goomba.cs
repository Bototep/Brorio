using UnityEngine;

public class Goomba : MonoBehaviour
{
	public Sprite flatSprite;
	public GameObject scorePopupPrefab;
	
	private GameObject scoreCanvas;

	private GameManager gameManager;
	private int scoreValue = 100;

	private void Awake()
	{
		gameManager = FindObjectOfType<GameManager>();

		scoreCanvas = GameObject.Find("ScoreCanvas");
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("Player"))
		{
			Player player = collision.gameObject.GetComponent<Player>();

			if (player.starpower)
			{
				Hit();
			}
			else if (collision.transform.DotTest(transform, Vector2.down))
			{
				Flatten();
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

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.layer == LayerMask.NameToLayer("Shell"))
		{
			Hit();
		}
	}

	private void Flatten()
	{
		GetComponent<Collider2D>().enabled = false;
		GetComponent<EntityMovement>().enabled = false;
		GetComponent<AnimatedSprite>().enabled = false;
		GetComponent<SpriteRenderer>().sprite = flatSprite;

		Addscore();
		Destroy(gameObject, 0.5f);
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

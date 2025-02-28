using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiranhaPlant : MonoBehaviour
{
	public float moveDistance = 1f; 
	public float moveSpeed = 0.6f; 
	public float maxStayTime = 0.5f; 

	private bool movingUp = true; 
	private Vector3 initialPosition; 

	private GameManager gameManager;

	public GameObject scorePopupPrefab;
	private GameObject scoreCanvas;
	private int scoreValue = 200;

	void Start()
	{
		gameManager = FindObjectOfType<GameManager>();

		initialPosition = transform.position;

		scoreCanvas = GameObject.Find("ScoreCanvas");

		StartCoroutine(MovePiranhaPlant());
	}

	IEnumerator MovePiranhaPlant()
	{
		Vector3 upPosition = initialPosition + Vector3.up * moveDistance;
		Vector3 downPosition = initialPosition - Vector3.up * moveDistance;

		while (transform.position != upPosition)
		{
			transform.position = Vector3.MoveTowards(transform.position, upPosition, moveSpeed * Time.deltaTime);
			yield return null;
		}

		while (true)
		{
			if (movingUp)
			{
				transform.position = Vector3.MoveTowards(transform.position, upPosition, moveSpeed * Time.deltaTime);
				if (transform.position == upPosition)
				{
					movingUp = false;
					yield return new WaitForSeconds(maxStayTime);
				}
			}
			else
			{
				transform.position = Vector3.MoveTowards(transform.position, downPosition, moveSpeed * Time.deltaTime);
				if (transform.position == downPosition)
				{
					movingUp = true;
				}
			}
			yield return null;
		}
	}

	private void Hit()
	{
		GetComponent<AnimatedSprite>().enabled = false;
		GetComponent<DeathAnimation>().enabled = true;

		Addscore();
		Destroy(gameObject);
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

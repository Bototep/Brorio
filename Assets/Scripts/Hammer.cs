using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hammer : MonoBehaviour
{
	private int scoreValue = 200;
	public GameObject scorePopupPrefab;

	private GameObject scoreCanvas;
	private GameManager gameManager;
	private Rigidbody2D rb;
	private float rotationSpeed = 1000f;

	private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
		gameManager = FindObjectOfType<GameManager>();
		scoreCanvas = GameObject.Find("ScoreCanvas");

		StartRotating();
	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("Player"))
		{
			Player player = collision.gameObject.GetComponent<Player>();

			if (player.starpower)
			{
				Addscore();
				Destroy(this.gameObject);
			}
			else
			{
				player.Hit();
			}
		}
		
		if (collision.gameObject.layer == LayerMask.NameToLayer("DeathBarrier"))
		{
			Destroy(this.gameObject);
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

	private void StartRotating()
	{
		rb.angularVelocity = rotationSpeed;
	}
}

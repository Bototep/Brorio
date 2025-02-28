using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToadScript : MonoBehaviour
{
	public GameObject toadSpeakScreen;
	public AnimatedSprite idleAnim;
	public Sprite happyAnim;

	private GameObject Canvas;
	private PlayerMovement playerMovement;
	private bool Happy = false;
	private SFXPlaying sFXPlaying;

	private void Awake()
	{
		Canvas = GameObject.Find("Canvas");

		sFXPlaying = GetComponent<SFXPlaying>();
		sFXPlaying = FindObjectOfType<SFXPlaying>();

		Happy = false;
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			Happy = true;

			GetComponent<SpriteRenderer>().sprite = happyAnim;
			GetComponent<AnimatedSprite>().enabled = false;


			sFXPlaying.ToadSFX();

			Canvas = GameObject.Find("Canvas");
			CreateToadSpeakScreen();

			playerMovement = other.GetComponent<PlayerMovement>();
			
			StartCoroutine(WaitAndLoadLevel());
		}
	}

	private IEnumerator WaitAndLoadLevel()
	{
		playerMovement.enabled = false;

		yield return new WaitForSeconds(7f);

		GameManager.Instance.NextLevel();

		playerMovement.enabled = true;
	}

	private void CreateToadSpeakScreen()
	{
		if (toadSpeakScreen != null && Canvas != null)
		{
			GameObject gameOverUI = Instantiate(toadSpeakScreen, Canvas.transform);
			// You can modify the instantiated UI here, such as setting the score text
		}
		else
		{
			Debug.LogWarning("Toad or Canvas not set in GameManager.");
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeScript : MonoBehaviour
{
	public GameObject wall;
	public GameObject bridge;

	private bool collected = false;

	private SFXPlaying sFXPlaying;

	private void Awake()
	{
		sFXPlaying = GetComponent<SFXPlaying>();
		sFXPlaying = FindObjectOfType<SFXPlaying>();
		wall.SetActive(false);
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (!collected && other.CompareTag("Player"))
		{
			sFXPlaying.BossWinSFX();

			StartCoroutine(Collect());
		}

		wall.SetActive(true);

		Destroy(bridge);
	}

	private IEnumerator Collect()
	{
		Rigidbody2D[] allRigidbodies = FindObjectsOfType<Rigidbody2D>();
		foreach (Rigidbody2D rb in allRigidbodies)
		{
			if (rb.gameObject != bridge)
			{
				rb.simulated = false;
			}
		}

		Time.timeScale = 0f; // Freeze time
		collected = true;

		// Optionally, you can disable the collider of this object to prevent repeated collection
		GetComponent<Collider2D>().enabled = false;

		// Wait for 2 seconds before destroying the object and unfreezing time
		yield return new WaitForSecondsRealtime(2f);

		// Enable physics for all objects except the bridge
		foreach (Rigidbody2D rb in allRigidbodies)
		{
			if (rb.gameObject != bridge)
			{
				rb.simulated = true;
			}
		}

		// Destroy the object and unfreeze time
		Destroy(gameObject);
		//Destroy(bridge);
		Time.timeScale = 1f; // Restore normal time scale
	}
}

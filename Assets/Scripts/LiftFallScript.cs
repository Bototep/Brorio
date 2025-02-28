using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class LiftFallScript : MonoBehaviour
{
	public float fallSpeed = 10f;
	public float delayBeforeFall = 2f;
	public float deadY = -5f;

	private Vector3 startPos;
	private bool shouldFall = false;
	private Player player;

	void Start()
	{
		startPos = transform.position;
	}

	void Update()
	{
		if (transform.position.y < deadY)
		{
			Destroy(gameObject);
		}

		if (shouldFall)
		{
			transform.position += Vector3.down * fallSpeed * Time.deltaTime;
		}
	}

	void StartFalling()
	{
		shouldFall = true;
	}

	void StopFalling() 
	{
		shouldFall = false;
	}

	void ResetPlatform()
	{
		transform.position = startPos;
		shouldFall = false;
	}
	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("Player"))
		{
			StartFalling();
		}
	}

	private void OnCollisionExit2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("Player"))
		{
			StopFalling();
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.layer == LayerMask.NameToLayer("DeathBarrier"))
		{
			Player player = collision.gameObject.GetComponent<Player>();

			if (player != null)
			{
				player.Hit();
			}

			Destroy(gameObject);
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyEnter : MonoBehaviour
{
	public Transform connection;
	public KeyCode enterKeyCode = KeyCode.S;
	public float enterDistance = 10f;
	public Vector3 exitDirection = Vector3.zero;
	public bool climb;

	private void Awake()
	{
		
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (connection != null && other.CompareTag("Player"))
		{
			climb = true;
			StartCoroutine(Enter(other.transform));
		}
	}

	private IEnumerator Enter(Transform player)
	{
		player.GetComponent<PlayerMovement>().enabled = false;

		Vector3 enteredPosition = new Vector3(player.position.x, player.position.y + enterDistance, player.position.z);
		Vector3 enteredScale = Vector3.one * 1f;

		climb = true;
		yield return Move(player, enteredPosition, enteredScale);
		climb = false;
		yield return new WaitForSeconds(1f);

		var sideSrolling = Camera.main.GetComponent<SideScrolling>();
		sideSrolling.SetSuperskyHeight(connection.position.y > sideSrolling.height);

		if (exitDirection != Vector3.zero)
		{
			player.position = connection.position - exitDirection;
			yield return Move(player, connection.position + exitDirection, Vector3.one);
		}
		else
		{
			player.position = connection.position;
			player.localScale = Vector3.one;
		}

		player.GetComponent<PlayerMovement>().enabled = true;
	}

	private IEnumerator Move(Transform player, Vector3 endPosition, Vector3 endScale)
	{
		float elapsed = 0f;
		float duration = 4f;

		Vector3 startPosition = player.position;
		Vector3 startScale = player.localScale;

		while (elapsed < duration)
		{
			float t = elapsed / duration;

			player.position = Vector3.Lerp(startPosition, endPosition, t);
			player.localScale = Vector3.Lerp(startScale, endScale, t);
			elapsed += Time.deltaTime;

			yield return null;
		}

		player.position = endPosition;
		player.localScale = endScale;
	}
}

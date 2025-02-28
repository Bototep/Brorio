using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TwinLiftScript : MonoBehaviour
{
	public float fallSpeed = 10f;
	public float delayBeforeFall = 2f;
	public float referenceY = 2f;
	public float deadY = -5f;
	public float upY = 10f;

	public GameObject lift2;
	public GameObject rope;

	private Vector3 startPos;
	private Vector3 startPos2;
	private bool shouldFall = false;
	private Player player;
	private bool FunctionActivate = false;

	void Start()
	{
		FunctionActivate = true;

		startPos = transform.position;
		if (lift2 != null)
		{
			startPos2 = lift2.transform.position;
		}
	}

	void Update()
	{
		if (transform.position.y < deadY)
		{
			StartCoroutine(DelayedDestroy(1f));

			Destroy(gameObject);
		}

		if (shouldFall && FunctionActivate)
        {
            transform.position += Vector3.down * fallSpeed * Time.deltaTime;
			if (lift2 != null)
			{
				lift2.transform.position -= Vector3.down * fallSpeed * Time.deltaTime;
			}
        }

		if (transform.position.y < referenceY || transform.position.y > upY)
		{
			FunctionActivateSys();

			FunctionActivate = false;
			shouldFall = true;

			rope.transform.SetParent(null);
		}
	}

	void StartFalling()
	{
		shouldFall = true;
	}

	void StopFalling()
	{
		if (FunctionActivate)
		{
			shouldFall = false;
		}
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
			if (collision.transform.DotTest(transform, Vector2.down))
			{
				StartFalling();
			}
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

	public void FunctionActivateSys()
	{
		if (FunctionActivate == false)
		{
			transform.position += Vector3.down * fallSpeed * Time.deltaTime;

			if (lift2 != null)
			{
				lift2.transform.position += Vector3.down * fallSpeed * Time.deltaTime;
			}
		}
	}

	private void OnDestroy()
	{
		if (lift2 != null)
		{
			lift2.transform.SetParent(null); // Detach lift2 from its parent
		}
	}

	private IEnumerator DelayedDestroy(float delay)
	{
		yield return new WaitForSeconds(delay);
		Destroy(gameObject);
	}
}

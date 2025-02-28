using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyLeftRight : MonoBehaviour
{
	public float speed = 2f; // Speed at which the platform moves
	public float moveDistance = 5f; // Distance the platform moves
	public bool moveHorizontally = true; // Whether the platform moves horizontally or vertically

	private Vector3 originalPosition;
	private bool movingRight = true;
	private bool MoveEnable = false;

	void Start()
	{
		originalPosition = transform.position;

		MoveEnable = false;
	}

	private void FixedUpdate()
	{
		if (MoveEnable)
		{
			Startmove();
		}
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("Player"))
		{
			MoveEnable = true;
		}
	}

	private void Startmove()
	{
		if (moveHorizontally)
		{
			transform.Translate(Vector3.right * speed * Time.deltaTime);

			// If the platform reaches the minimum distance, switch direction
			if (transform.position.x <= originalPosition.x - moveDistance)
			{
				movingRight = true;
			}
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftLeftRight : MonoBehaviour
{
    public float speed = 2f; // Speed at which the platform moves
	public float moveDistance = 5f; // Distance the platform moves
	public bool moveHorizontally = true; // Whether the platform moves horizontally or vertically

	private Vector3 originalPosition;
	private bool movingRight = true;

	void Start()
	{
		// Store the original position of the platform
		originalPosition = transform.position;
	}

	void Update()
	{
		// If the platform moves horizontally
		if (moveHorizontally)
		{
			// Move the platform horizontally
			if (movingRight)
			{
				transform.Translate(Vector3.right * speed * Time.deltaTime);

				// If the platform reaches the maximum distance, switch direction
				if (transform.position.x >= originalPosition.x + moveDistance)
				{
					movingRight = false;
				}
			}
			else
			{
				transform.Translate(Vector3.left * speed * Time.deltaTime);

				// If the platform reaches the minimum distance, switch direction
				if (transform.position.x <= originalPosition.x - moveDistance)
				{
					movingRight = true;
				}
			}
		}
		// If the platform moves vertically
		else
		{
			// Move the platform vertically
			if (movingRight)
			{
				transform.Translate(Vector3.up * speed * Time.deltaTime);

				// If the platform reaches the maximum distance, switch direction
				if (transform.position.y >= originalPosition.y + moveDistance)
				{
					movingRight = false;
				}
			}
			else
			{
				transform.Translate(Vector3.down * speed * Time.deltaTime);

				// If the platform reaches the minimum distance, switch direction
				if (transform.position.y <= originalPosition.y - moveDistance)
				{
					movingRight = true;
				}
			}
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftUpDown : MonoBehaviour
{
	public float speed = 2f; // Speed at which the platform moves
	public float ascendHeight = 10f; // Height at which the platform ascends
	public float descendHeight = -7f; // Height at which the platform descends

	private Vector3 originalPosition;
	private bool ascending = true;

	void Start()
	{
		// Store the original position of the platform
		originalPosition = transform.position;
	}

	void Update()
	{
		// If the platform is ascending and below the ascend height, move it up
		if (ascending && transform.position.y < ascendHeight)
		{
			transform.Translate(Vector3.up * speed * Time.deltaTime);

			// If the platform reaches the ascend height, switch to descending
			if (transform.position.y >= ascendHeight)
			{
				ascending = false;
			}
		}
		// If the platform is descending and above the descend height, move it down
		else if (!ascending && transform.position.y > descendHeight)
		{
			transform.Translate(Vector3.down * speed * Time.deltaTime);

			// If the platform reaches the descend height, switch to ascending
			if (transform.position.y <= descendHeight)
			{
				ascending = true;
			}
		}
	}
}

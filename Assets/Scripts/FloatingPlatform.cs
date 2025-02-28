using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingPlatform : MonoBehaviour
{
	public float upwardSpeed = 2f; // Speed at which the platform moves upwards
	public float downwardSpeed = -2f; // Speed at which the platform moves downwards
	public float respawnHeight = -7f; // Height at which the platform respawns
	public float respawnPositionHeight = -25f; // Height at which the platform respawns after reaching respawnHeight

	private Vector3 originalPosition;
	private bool respawned = false;

	void Start()
	{
		// Store the original position of the platform
		originalPosition = transform.position;
	}

	void Update()
	{
		// If the platform has not respawned and is below the respawn height, move it up
		if (!respawned && transform.position.y < respawnHeight)
		{
			MovePlatform(upwardSpeed);

			// If the platform reaches the respawn height, respawn it at the specified position
			if (transform.position.y >= respawnHeight)
			{
				RespawnPlatform();
			}
		}
		// If the platform has respawned, continue floating upwards
		else if (respawned)
		{
			MovePlatform(upwardSpeed);

			// If the platform reaches the respawn position height, reset respawned flag
			if (transform.position.y >= respawnPositionHeight)
			{
				respawned = false;
			}
		}
		// If the platform is above the respawn height, move it down
		else if (!respawned && transform.position.y > respawnHeight)
		{
			MovePlatform(downwardSpeed);

			// If the platform reaches the respawn height, respawn it at the specified position
			if (transform.position.y <= respawnHeight)
			{
				RespawnPlatform();
			}
		}
		// If the platform has respawned, continue floating downwards
		else if (respawned)
		{
			MovePlatform(downwardSpeed);

			// If the platform reaches the respawn position height, reset respawned flag
			if (transform.position.y <= respawnPositionHeight)
			{
				respawned = false;
			}
		}
	}

	void MovePlatform(float speed)
	{
		transform.Translate(Vector3.up * speed * Time.deltaTime);
	}

	void RespawnPlatform()
	{
		// Respawn the platform at the specified height
		transform.position = new Vector3(originalPosition.x, respawnPositionHeight, originalPosition.z);
		respawned = true;
	}

}

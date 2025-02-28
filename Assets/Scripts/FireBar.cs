using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBar : MonoBehaviour
{
	public float rotationSpeed = 110f; // Speed at which the Fire Bar rotates
	public float maxRotation = 360f; // Maximum rotation angle of the Fire Bar

	private void Update()
	{
		// Rotate the Fire Bar around the Z-axis
		transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);

		// Clamp rotation to avoid overflow
		if (transform.localEulerAngles.z > maxRotation)
		{
			Vector3 newRotation = transform.localEulerAngles;
			newRotation.z = maxRotation;
			transform.localEulerAngles = newRotation;
		}
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("Player"))
		{
			Player player = collision.gameObject.GetComponent<Player>();

			player.Hit();
		}
	}
}

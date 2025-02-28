using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftDownFix : MonoBehaviour
{
	private PlayerMovement movement;

	//public bool drown = false;

	private void Awake()
	{
		movement = GetComponentInParent<PlayerMovement>();
	}

	void OnTriggerEnter2D(Collider2D Collision)
	{
		if (Collision.CompareTag("Water"))
		{
			movement.drown = true;
			Debug.Log("In");
		}
	}

	void OnTriggerExit2D(Collider2D Collision)
	{
		if (Collision.CompareTag("Water"))
		{
			movement.drown = false;
			Debug.Log("Out");
		}
	}
}

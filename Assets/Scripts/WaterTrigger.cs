using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;

public class WaterTrigger : MonoBehaviour
{
	private PlayerMovement movement;

	private void Awake()
	{
		movement = GetComponentInParent<PlayerMovement>();
	}

	void OnTriggerEnter2D(Collider2D Collision)
	{
		if (Collision.CompareTag("Water"))
		{
			movement.drown = true;
		}

		if (Collision.CompareTag("LiftDownFix"))
        {
            movement.lifted = true;
        }
	}
	
	void OnTriggerExit2D(Collider2D Collision)
	{
		if (Collision.CompareTag("Water"))
		{
			movement.drown = false;
		}

		if (Collision.CompareTag("LiftDownFix"))
		{
			movement.lifted = false;
		}
	}
}

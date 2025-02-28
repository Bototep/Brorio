using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spring : MonoBehaviour
{
	private PlayerMovement movement;
	public Animator animator;
	
	private void Awake()
	{
		movement = GetComponentInParent<PlayerMovement>();
		animator = GetComponent<Animator>();
	}

	private void OnCollisionExit2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("Player"))
		{
			animator.SetTrigger("SpringWork");
		}
	}
}

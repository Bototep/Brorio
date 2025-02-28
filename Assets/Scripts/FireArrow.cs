using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireArrow : MonoBehaviour
{
	void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("Player"))
		{
			Player player = collision.gameObject.GetComponent<Player>();
			player.Hit();
		}

		if (collision.gameObject.layer == LayerMask.NameToLayer("DeathBarrier"))
		{
			Destroy(this.gameObject);
		}

		if (collision.gameObject.layer == LayerMask.NameToLayer("EnemyBarrier"))
		{
			Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>());
		}
	}
}

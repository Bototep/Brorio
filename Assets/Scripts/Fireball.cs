using UnityEngine;
using System.Collections;

public class Fireball : MonoBehaviour
{
	public GameObject blastParticle;
    public float bounceForce = 10f;
    public float destructionDelay = 3f;

	private float rotationSpeed = -1000f;
	private Rigidbody2D rb;
    private PlayerMovement movement;

	private SFXPlaying sFXPlaying;

	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		movement = FindObjectOfType<PlayerMovement>();

		Invoke("DestroySelf", destructionDelay);

		sFXPlaying = GetComponent<SFXPlaying>();
		sFXPlaying = FindObjectOfType<SFXPlaying>();
		sFXPlaying.FireBallSFX();

		StartRotating();
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("Ground"))
		{
			if (IsTopCollision(collision.contacts[0].normal))
			{
				Bounce();
			}
			else
			{
				Instantiate(blastParticle, transform.position, Quaternion.identity);
				Destroy(gameObject);
				movement.activeProjectiles = Mathf.Max(0, movement.activeProjectiles - 1);
			}
		}
		else if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
		{
			Instantiate(blastParticle, transform.position, Quaternion.identity);
			Destroy(collision.gameObject);
			DestroySelf();
		}
	}

	private void Bounce()
	{
		rb.velocity = new Vector2(rb.velocity.x, Mathf.Abs(bounceForce));
	}

	private bool IsTopCollision(Vector2 normal)
	{
		return normal.y > 0.9f;
	}

	private void StartRotating()
	{
		rb.angularVelocity = rotationSpeed;
	}

	private void DestroySelf()
	{
		Destroy(gameObject);
		if (movement != null)
		{
			movement.activeProjectiles = Mathf.Max(0, movement.activeProjectiles - 1);
		}
	}
}
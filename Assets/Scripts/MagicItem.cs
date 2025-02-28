using System.Collections;
using UnityEngine;
using static UnityEngine.ParticleSystem;
using UnityEngine.Rendering.Universal;

public class MagicItem : MonoBehaviour
{
	private Light2D QLight;
	private Player player;

	public GameObject Magic;
	public GameObject Flower;
	public Sprite emptyBlock;
	public int maxHits = -1;
	private bool animating;
	public GameObject Normal;

	public AnimatedSprite glow;

	private void Start()
	{
		// Find the Player script in the scene and assign it to the 'player' variable
		player = FindObjectOfType<Player>();
		QLight = GetComponentInChildren<Light2D>();
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (!animating && maxHits != 0 && collision.gameObject.CompareTag("Player"))
		{
			if (collision.transform.DotTest(transform, Vector2.up))
			{
				Hit();
			}
		}
	}

	private void Hit()
	{
		SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
		spriteRenderer.enabled = true; // show if hidden

		if (glow != null)
		{
			glow.enabled = false;

			if (QLight != null)
			{
				QLight.enabled = false; // Disable the Light2D component
			}
		}
			
		maxHits--;

		if (maxHits == 0)
		{
			spriteRenderer.sprite = emptyBlock;
		}

		if (player != null && player.big && Magic != null && Magic.CompareTag("MagicMushroom") || player != null && player.white && Magic != null && Magic.CompareTag("MagicMushroom"))
		{
			Instantiate(Flower, transform.position, Quaternion.identity);
		}

		if (Magic != null && !player.big && !player.white)
		{
			Instantiate(Magic, transform.position, Quaternion.identity);
			//item.SetActive(false);
		}

		StartCoroutine(Animate());
	}

	private IEnumerator Animate()
	{
		animating = true;

		Vector3 restingPosition = transform.localPosition;
		Vector3 animatedPosition = restingPosition + Vector3.up * 0.5f;

		yield return Move(restingPosition, animatedPosition);
		yield return Move(animatedPosition, restingPosition);

		animating = false;
	}

	private IEnumerator Move(Vector3 from, Vector3 to)
	{
		float elapsed = 0f;
		float duration = 0.125f;

		while (elapsed < duration)
		{
			float t = elapsed / duration;

			transform.localPosition = Vector3.Lerp(from, to, t);
			elapsed += Time.deltaTime;

			yield return null;
		}

		transform.localPosition = to;
	}
}

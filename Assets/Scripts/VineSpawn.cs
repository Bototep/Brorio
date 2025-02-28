using System.Collections;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class VineSpawn : MonoBehaviour
{
	private ParticleSystem particle;
	private Light2D QLight;

	private Player player;
	public GameObject item;
	public Sprite emptyBlock;
	public int maxHits = -1;
	private bool animating;
	public GameObject vine;

	public AnimatedSprite glow;

	private void Start()
	{
		// Find the Player script in the scene and assign it to the 'player' variable
		player = FindObjectOfType<Player>();
		particle = GetComponentInChildren<ParticleSystem>();
		QLight = GetComponentInChildren<Light2D>();
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (!animating && maxHits != 0 && collision.gameObject.CompareTag("Player"))
		{
			if (collision.transform.DotTest(transform, Vector2.up))
			{
				Hit();

				vine.SetActive(true);
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

		if (item != null)
		{
			Instantiate(item, transform.position, Quaternion.identity);
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

using System.Collections;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class BlockHit : MonoBehaviour
{
    private ParticleSystem particle;
    private Light2D QLight;

    private Player player;
    public GameObject item;
    public Sprite emptyBlock;
    public int maxHits = -1;
    private bool animating;
    public GameObject Normal;

	public AnimatedSprite glow;

	private GameManager gameManager;
	private int scoreValue = 50;

	private void Start()
	{
		gameManager = FindObjectOfType<GameManager>();
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

        if (maxHits == 0) {
            spriteRenderer.sprite = emptyBlock;
        }
		
		if (item != null) {
            Instantiate(item, transform.position, Quaternion.identity);
		}

        StartCoroutine(Animate());

		if (player != null && player.big && item == null || player != null && player.white && item == null)
		{

			gameManager.score += scoreValue;
			StartCoroutine(Break());
		}
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

    private IEnumerator Break()
    {
		particle.Play();
		Collider2D collider = GetComponent<Collider2D>();
		if (collider != null)
		{
			collider.enabled = false;
		}

		if (Normal != null && Normal.GetComponent<SpriteRenderer>() != null)
		{
			Normal.GetComponent<SpriteRenderer>().enabled = false; // Disable the SpriteRenderer
		}

		yield return new WaitForSeconds(particle.main.startLifetime.constantMax);

		if (Normal != null)
		{
			Destroy(Normal.gameObject);
		}
	}
}

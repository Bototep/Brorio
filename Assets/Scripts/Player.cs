using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    public PlayerSpriteRenderer smallRenderer;
    public PlayerSpriteRenderer bigRenderer;
	public PlayerSpriteRenderer whiteRenderer;
	private PlayerSpriteRenderer activeRenderer;

    public CapsuleCollider2D capsuleCollider { get; private set; }
    public DeathAnimation deathAnimation { get; private set; }
	private PlayerMovement movement;

	private GameManager gameManager;

	public GameObject scorePopupPrefab;

	private GameObject scoreCanvas;
	private int PowerUpScoreValue = 1000;

	public bool white => whiteRenderer.enabled;
	public bool big => bigRenderer.enabled;
    public bool dead => deathAnimation.enabled;
    public bool starpower { get; private set; }

    public bool shrinkAnim = false;
    public bool growAnim = false;
    public int henshinTime = 2;

    private SFXPlaying sFXPlaying;

	private void Awake()
    {
		movement = FindObjectOfType<PlayerMovement>();
		gameManager = FindObjectOfType<GameManager>();
		capsuleCollider = GetComponent<CapsuleCollider2D>();
        deathAnimation = GetComponent<DeathAnimation>();
        sFXPlaying= GetComponent<SFXPlaying>();
        sFXPlaying= FindObjectOfType<SFXPlaying>();
        activeRenderer = smallRenderer;

		scoreCanvas = GameObject.Find("ScoreCanvas");
	}

	public void Hit()
    {
        if (!dead && !starpower)
        {
            if (big || white) {
                Shrink();
            } else {
                Death();
            }
        }
    }

    public void Death()
    {
        smallRenderer.enabled = false;
        bigRenderer.enabled = false;
        deathAnimation.enabled = true;

        sFXPlaying.DieSFX();

        GameManager.Instance.ResetLevel(3f);
    }

    public void Grow()
    {
        sFXPlaying.PowerSFX();

        smallRenderer.enabled = false;
        bigRenderer.enabled = true;
        activeRenderer = bigRenderer;

        capsuleCollider.size = new Vector2(0.6f, 2f);
        capsuleCollider.offset = new Vector2(0f, 0.4f);

		ShowScorePopup(PowerUpScoreValue);
		gameManager.score += PowerUpScoreValue;

		growAnim = true;

		FreezeTime();

		//StartCoroutine(HenshinTimer(henshinTime));
		StartCoroutine(ScaleAnimation());

	}

	private IEnumerator HenshinTimer(int duration)
	{
		yield return new WaitForSecondsRealtime(duration); // Wait for henshinTime seconds

		growAnim = false;

		shrinkAnim = false;// Turn off grow animation after henshinTime seconds

		// If you want to do something else after the duration, you can add it here
	}

	public void FreezeTime()
	{
		StartCoroutine(FreezeTimeCoroutine());
	}

	private IEnumerator FreezeTimeCoroutine()
	{
		float previousTimeScale = Time.timeScale;
		Time.timeScale = 0f; // Freeze time for this object

		yield return new WaitForSecondsRealtime(0.5f); // Freeze time for 2 seconds

        shrinkAnim = false;
		Time.timeScale = previousTimeScale; // Restore previous time scale
	}

	public void Shrink()
    {
        smallRenderer.enabled = true;
        bigRenderer.enabled = false;
		whiteRenderer.enabled = false;
		activeRenderer = smallRenderer;

        capsuleCollider.size = new Vector2(0.6f, 1f);
        capsuleCollider.offset = new Vector2(-0.003798574f, -0.135f);

        shrinkAnim = true;

		FreezeTime();

		StartCoroutine(ScaleAnimation());
    }

    private IEnumerator ScaleAnimation()
    {
        float elapsed = 0f;
        float duration = 0.1f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            if (Time.frameCount % 4 == 0)
            {
                smallRenderer.enabled = !smallRenderer.enabled;
                bigRenderer.enabled = !smallRenderer.enabled;
            }

            yield return null;
        }

        smallRenderer.enabled = false;
        bigRenderer.enabled = false;
        activeRenderer.enabled = true;
    }

    public void Starpower()
    {
        StartCoroutine(StarpowerAnimation());
	}

    private IEnumerator StarpowerAnimation()
    {
		starpower = true;

        float elapsed = 0f;
        float duration = 10f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            if (Time.frameCount % 10 == 0) {
                activeRenderer.spriteRenderer.color = Random.ColorHSV(0f, 1f, 1f, 1f, 1f, 1f);
            }

            yield return null;
        }

        activeRenderer.spriteRenderer.color = Color.white;
        starpower = false;
    }

	public void FireFlower()
	{
		sFXPlaying.PowerSFX();

		ShowScorePopup(PowerUpScoreValue);
		gameManager.score += PowerUpScoreValue;

		movement.canShoot = true;

		bigRenderer.enabled = false;
        whiteRenderer.enabled = true;

        activeRenderer = whiteRenderer;
	}

	private void ShowScorePopup(int scoreValue)
	{
		if (scoreCanvas != null && gameObject != null)
		{
			Vector3 positionDifference = transform.position - scoreCanvas.transform.position;

			Vector3 popupPosition = scoreCanvas.transform.position + positionDifference + Vector3.up * 2f;

			GameObject scorePopup = Instantiate(scorePopupPrefab, popupPosition, Quaternion.identity, scoreCanvas.transform);

			ScorePopup popupScript = scorePopup.GetComponent<ScorePopup>();

			if (popupScript != null)
			{
				popupScript.ShowScore(scoreValue);
			}
		}
	}
}

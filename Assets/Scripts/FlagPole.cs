using System.Collections;
using UnityEngine;

public class FlagPole : MonoBehaviour
{
    public Transform flag;
    public Transform poleBottom;
    public Transform castle;
    public float speed = 6f;
    public int nextWorld = 1;
    public int nextStage = 1;
    public bool slideDown = false;

    private PlayerSpriteRenderer playerSpriteRenderer;
	private SFXPlaying sFXPlaying;

	private void Awake()
	{
		playerSpriteRenderer= GetComponent<PlayerSpriteRenderer>();
		sFXPlaying = GetComponent<SFXPlaying>();
		sFXPlaying = FindObjectOfType<SFXPlaying>();
	}

	private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            sFXPlaying.WinSFX();
            StartCoroutine(MoveTo(flag, poleBottom.position));
            StartCoroutine(LevelCompleteSequence(other.transform));
        }
    }

    private IEnumerator LevelCompleteSequence(Transform player)
    {
        player.GetComponent<PlayerMovement>().enabled = false;
		slideDown = true;
		yield return MoveTo(player, poleBottom.position);
        slideDown = false;
		yield return MoveTo(player, player.position + Vector3.right);
		yield return MoveTo(player, player.position + Vector3.right + Vector3.down);
        yield return MoveTo(player, castle.position);

        player.gameObject.SetActive(false);

        yield return new WaitForSeconds(4f);

		GameManager.Instance.NextLevel();
	}

    private IEnumerator MoveTo(Transform subject, Vector3 position)
    {
        while (Vector3.Distance(subject.position, position) > 0.125f)
        {
            subject.position = Vector3.MoveTowards(subject.position, position, speed * Time.deltaTime);
            yield return null;
        }

        subject.position = position;
    }

}

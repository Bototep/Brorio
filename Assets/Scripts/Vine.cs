using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vine : MonoBehaviour
{
	private void Start()
	{

		StartCoroutine(Animate());
	}

	private IEnumerator Animate()
	{
		Vector3 restingPosition = transform.localPosition;
		Vector3 animatedPosition = restingPosition + Vector3.up * 13f;

		yield return Move(restingPosition, animatedPosition);
	}

	private IEnumerator Move(Vector3 from, Vector3 to)
	{
		float elapsed = 0f;
		float duration = 5f;

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

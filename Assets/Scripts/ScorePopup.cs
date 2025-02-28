using System.Collections;
using TMPro;
using UnityEngine;

public class ScorePopup : MonoBehaviour
{
	public TextMeshProUGUI textMesh; 

	private void Awake()
	{
		textMesh = GetComponent<TextMeshProUGUI>();
	}

	public void ShowScore(int scoreValue)
	{
		if (textMesh != null)
		{
			textMesh.text = scoreValue.ToString(); 
			StartCoroutine(ShowPopupCoroutine());
		}
	}

	private IEnumerator ShowPopupCoroutine()
	{
		gameObject.SetActive(true);

		yield return new WaitForSeconds(1f);

		if (gameObject != null && gameObject.activeSelf)
		{
			Destroy(gameObject); 
		}
	}
}
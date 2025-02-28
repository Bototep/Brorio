using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
	public TMP_Text timeTextField;
	public TMP_Text liveTextField;
	public TMP_Text coinsTextField;
	public TMP_Text scoreTextField;
	public TMP_Text worldTextField;
	public int WorldTime = 400;
	public bool isTimeDecreasing = true;

	private float CurrentTime;

	private Player player;
	private GameManager gameManager;

	private void Awake()
	{
		player = FindObjectOfType<Player>();
		gameManager = FindObjectOfType<GameManager>();
	}

	public void Start()
	{
		ResetTime();
	}

	private void Update()
	{
		if (CurrentTime > 0)
		{

			float timeScaleFactor = 2f; // Change this factor as needed
			if (isTimeDecreasing)
			{
				CurrentTime -= Time.deltaTime * timeScaleFactor;
			}

			timeTextField.text = "TIME " + "\n" + Mathf.FloorToInt(CurrentTime);
			liveTextField.text = "LIVES " + "\n" + gameManager.lives;
			coinsTextField.text = "COINS " + "\n" + gameManager.coins;
			scoreTextField.text = "SCORE " + "\n" +  gameManager.score;
			worldTextField.text = "WORLD " + "\n" + SceneManager.GetActiveScene().name;


			if (CurrentTime <= 0)
			{
				player.Hit();
				timeTextField.text = "TIME 0";
			}
		}
	}

	public void ResetTime()
	{
		CurrentTime = WorldTime;
		isTimeDecreasing = true;
	}

	public void FreezeTime()
	{
		isTimeDecreasing = false; // Stop the time countdown
	}

	public void ResumeTime()
	{
		isTimeDecreasing = true; // Resume the time countdown
	}
}

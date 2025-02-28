using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;
using UnityEditor.Rendering;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance { get; private set; }

	public int lives { get; private set; }
	public int coins { get; private set; }
	public int score;

	public GameObject gameOverScreen;

	private int CoinScoreValue = 200;
	private GameObject Canvas;
	private UIManager uiManager;

	private SFXPlaying sFXPlaying;

	private void Awake()
	{
		Canvas = GameObject.Find("Canvas");
		uiManager = FindObjectOfType<UIManager>();
		sFXPlaying = GetComponent<SFXPlaying>();
		sFXPlaying = FindObjectOfType<SFXPlaying>();

		if (Instance != null)
		{
			DestroyImmediate(gameObject);
		}
		else
		{
			Instance = this;
			DontDestroyOnLoad(gameObject);
		}
	}

	private void OnDestroy()
	{
		if (Instance == this)
		{
			Instance = null;
		}
	}

	private void Start()
	{
		Application.targetFrameRate = 60;

		NewGame();
	}

	public void NewGame()
	{
		lives = 3;
		coins = 0;
		score = 0;

		LoadLevel(1);
	}

	public void GameOver()
	{
		Canvas = GameObject.Find("Canvas");
		uiManager = FindObjectOfType<UIManager>();

		if (uiManager != null)
		{
			uiManager.isTimeDecreasing = false;
		}

		CreateGameOverUI(score);

		sFXPlaying.GameOverSFX();

		StartCoroutine(DelayBeforeNewGame(5f));
	}

	public void LoadLevel(int buildIndex)
	{
		SceneManager.LoadScene(buildIndex);

		Debug.Log("Loaded scene: " + SceneManager.GetSceneByBuildIndex(buildIndex).name);
	}

	public void NextLevel()
	{
		int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

		// Load the next scene by incrementing the current build index
		LoadLevel(currentSceneIndex + 1);
	}

	public void ResetLevel(float delay)
	{
		Invoke(nameof(ResetLevel), delay);
	}

	public void ResetLevel()
	{
		lives--;

		if (lives > 0)
		{
			int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

			// Reload the current scene
			LoadLevel(currentSceneIndex);
		}
		else
		{
			GameOver();
		}
	}

	public void AddCoin()
	{
		coins++;

		sFXPlaying.CoinSFX();

		if (coins == 100)
		{
			coins = 0;
			AddLife();
		}

		score += CoinScoreValue;
	}

	public void AddScore()
	{
		score++;
	}

	public void AddLife()
	{
		sFXPlaying.OneUpSFX();
		lives++;
	}

	private void CreateGameOverUI(int score)
	{
		if (gameOverScreen != null && Canvas != null)
		{
			GameObject gameOverUI = Instantiate(gameOverScreen, Canvas.transform);
			// You can modify the instantiated UI here, such as setting the score text
		}
		else
		{
			Debug.LogWarning("Game Over UI or Canvas not set in GameManager.");
		}
	}

	private IEnumerator DelayBeforeNewGame(float delay)
	{
		yield return new WaitForSeconds(delay);

		NewGame();
	}
}

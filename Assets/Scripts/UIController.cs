using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using Dan.Main;

public class UIController : MonoBehaviour
{
	public GameObject blackSquare;
	public GameObject lossText;
	public Button continueButtonLose;
	public Button continueButtonWin;
	public TMP_Text upgradeMessageText;
	public GameObject materialBar;
	public GameObject linearHolder;
	public GameObject coinText;
	public PlayerMovement playerMovement;
	public TimerController timerController;
	public TMP_Text highScoreText;

	public GameObject upgradeUI;

	private string currentUserName;


	void Start()
	{
		continueButtonLose.onClick.AddListener(OnClickLose);
		continueButtonWin.onClick.AddListener(OnClickWin);

		// Load the username from PlayerPrefs
		currentUserName = PlayerPrefs.GetString("PlayerUsername", "Guest");
		Debug.Log("Loaded username in Game Scene: " + currentUserName);
	}

	public void UploadEntry()
	{
		string url = Dan.ConstantVariables.GetServerURL(Dan.Enums.Routes.Upload);
		Debug.Log("Uploading score to: " + url);
		Debug.Log("Username: " + currentUserName + ", Score: " + playerMovement.score);

		// Instantiate LeaderboardReference
		LeaderboardReference leaderboard = new LeaderboardReference("geggsend_go");

		leaderboard.UploadNewEntry(currentUserName, playerMovement.score, isSuccessful =>
		{
			if (isSuccessful)
				Debug.Log("✅ Upload succeeded!");
			else
				Debug.LogError("❌ Upload failed!");
		});

	}


	void OnClickLose()
	{
		Destroy(PlayerMovement.Instance.gameObject);
		SceneManager.LoadScene("Menu");
	}

	public void OnClickWin()
	{
		Debug.Log("win");
		continueButtonWin.gameObject.SetActive(false);
		playerMovement.HideDeathInfoUI();
		upgradeUI.SetActive(true);

	}


	public void Continue()
	{
		upgradeUI.SetActive(false);
		timerController.max_time = playerMovement.time;
		timerController.time_remaining = timerController.max_time;
		timerController.timerPaused = false;
		playerMovement.Unfreeze();
		SceneManager.LoadScene("Level");
		materialBar.SetActive(true);
		linearHolder.SetActive(true);
		//coinText.SetActive(true);
		StartCoroutine(FadeBlackOutSquare(false));

	}
	public void PlayerLoss()
	{
		highScoreText.gameObject.SetActive(true);
		highScoreText.text = "Score: " + playerMovement.score.ToString();
		playerMovement.depositText.gameObject.SetActive(false);
		timerController.timerPaused = true;
		materialBar.SetActive(false);
		linearHolder.SetActive(false);
		//coinText.SetActive(false);
		StartCoroutine(FadeBlackOutSquare());
		StartCoroutine(WaitTextLoss());



	}

	public void PlayerWin()
	{
		string username = PlayerPrefs.GetString("PlayerUsername", "Guest");
		int score = playerMovement.score;

		UploadEntry();

		StartCoroutine(FadeBlackOutSquare());
		StartCoroutine(WaitTextWin());
		timerController.timerPaused = true;
		materialBar.SetActive(false);
		linearHolder.SetActive(false);
	}

	public void SetUsername(string username)
	{
		currentUserName = username;
		PlayerPrefs.SetString("PlayerUsername", currentUserName);
		PlayerPrefs.Save();

		Debug.Log("Username saved in UIController: " + currentUserName);
	}


	IEnumerator WaitTextLoss()
	{
		yield return new WaitForSeconds(2);
		lossText.SetActive(true);
		continueButtonLose.gameObject.SetActive(true);
		yield return new WaitForSeconds(1);
		continueButtonLose.gameObject.SetActive(true);
		playerMovement.WipeMaterials();

		playerMovement.Reposition();
	}


	void PlayerWinFunc()
	{
		playerMovement.DisplayDeathInfoUI();
	}
	IEnumerator WaitTextWin()
	{
		yield return new WaitForSeconds(2);
		playerMovement.DisplayDeathInfoUI();
		continueButtonWin.gameObject.SetActive(true);

	}

	public IEnumerator FadeBlackOutSquare(bool fadeToBlack = true, int fadeSpeed = 1)
	{
//		Debug.Log("Fade");
		Color objectColor = blackSquare.GetComponent<Image>().color;
		float fadeAmount;

		if (fadeToBlack)
		{
			while (blackSquare.GetComponent<Image>().color.a < 1)
			{
				fadeAmount = objectColor.a + (fadeSpeed * Time.deltaTime);

				objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
				blackSquare.GetComponent<Image>().color = objectColor;
				yield return null;
			}
		}
		else
		{
			while (blackSquare.GetComponent<Image>().color.a > 0)
			{
				fadeAmount = objectColor.a - (fadeSpeed * Time.deltaTime);

				objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
				blackSquare.GetComponent<Image>().color = objectColor;
				yield return null;
			}
		}
	}

	public void UpgradeTimeTextReset()
	{
		
		upgradeMessageText.text = "";
	}


}

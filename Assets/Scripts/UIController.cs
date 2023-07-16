using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
	public GameObject blackSquare;
	public GameObject lossText;
	public Button continueButtonLose;
	public Button continueButtonWin;

	public GameObject materialBar;
	public GameObject linearHolder;
	public GameObject coinText;
	public PlayerMovement playerMovement;
	public TimerController timerController;
	
public GameObject upgradeUI;


	void Start()
	{
		continueButtonLose.onClick.AddListener(OnClickLose);
		continueButtonWin.onClick.AddListener(OnClickWin);
	}

	void OnClickLose()
	{
		lossText.SetActive(false);
		continueButtonLose.gameObject.SetActive(false);
		playerMovement.DisplayDeathInfoUI();
	}

	public void OnClickWin()
	{
		//continueButtonWin.gameObject.SetActive(false);
		playerMovement.HideDeathInfoUI();
		upgradeUI.SetActive(true);

	}

	public void Continue(){
		    upgradeUI.SetActive(false);
timerController.max_time = playerMovement.time;
		timerController.time_remaining = timerController.max_time;
		timerController.timerPaused = false;
		playerMovement.Unfreeze();
		SceneManager.LoadScene(0);
			materialBar.SetActive(true);
		linearHolder.SetActive(true);
		coinText.SetActive(true);
				StartCoroutine(FadeBlackOutSquare(false));

	}
	public void PlayerLoss()
	{
		timerController.timerPaused = true;
		materialBar.SetActive(false);
		linearHolder.SetActive(false);
		coinText.SetActive(false);
		timerController.
		StartCoroutine(FadeBlackOutSquare());
		StartCoroutine(WaitTextLoss());
		
		

	}

	public void PlayerWin()
	{
		StartCoroutine(FadeBlackOutSquare());
		StartCoroutine(WaitTextWin());
		timerController.timerPaused = true;
		materialBar.SetActive(false);
		linearHolder.SetActive(false);
		coinText.SetActive(false);


	}


	IEnumerator WaitTextLoss()
	{
		yield return new WaitForSeconds(2);
		lossText.SetActive(true);
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

	}

	public IEnumerator FadeBlackOutSquare(bool fadeToBlack = true, int fadeSpeed = 1)
	{
		Debug.Log("Fade");
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




}

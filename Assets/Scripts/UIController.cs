using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
	public GameObject blackSquare;
	public GameObject lossText;
	public Button continueButton;

	public GameObject materialBar;
	public GameObject linearHolder;
	public PlayerMovement playerMovement;
	public TimerController timerController;


	void Start()
	{
		continueButton.onClick.AddListener(OnClick);
	}

	void OnClick()
	{
		Debug.Log("Button Clicked");
		lossText.SetActive(false);
		continueButton.gameObject.SetActive(false);
		playerMovement.DisplayDeathInfoUI();
	}
	

	public void PlayerLoss()
	{
		Debug.Log("Fade Called");
		timerController.timerPaused = true;
		materialBar.SetActive(false);
		linearHolder.SetActive(false);
		timerController.
		StartCoroutine(FadeBlackOutSquare());
		StartCoroutine(WaitText());
		
	}

	IEnumerator WaitText()
	{
		yield return new WaitForSeconds(2);
		lossText.SetActive(true);
		yield return new WaitForSeconds(1);
		continueButton.gameObject.SetActive(true);
	}

	public IEnumerator FadeBlackOutSquare(bool fadeToBlack = true, int fadeSpeed = 1)
	{
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
	public GameObject blackSquare;
	public GameObject lossText;

	// Update is called once per frame
	/*void Update()
	{
		if (Input.GetKeyDown(KeyCode.A))
		{
			StartCoroutine(FadeBlackOutSquare());
		}
		if (Input.GetKeyDown(KeyCode.S))
		{
			StartCoroutine(FadeBlackOutSquare(false));
		}
	}
	*/

	public void Fade()
	{
		Debug.Log("Fade Called");
		StartCoroutine(FadeBlackOutSquare());
		StartCoroutine(WaitText());
		
	}

	IEnumerator WaitText()
	{
		yield return new WaitForSeconds(2);
		lossText.SetActive(true);
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

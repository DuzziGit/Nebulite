using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerController : MonoBehaviour
{

    public Image timer_foreground;

    public float time_remaining;

    public float max_time;

    public float playerTime;

    public GameObject player;

	public PlayerMovement playerMovement;

    public bool timerPaused = false;




	void Start()
    {
		playerTime = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().time;

	

		max_time = playerTime;

		time_remaining = max_time;

        
	}

   void Update()
{
    		max_time = playerTime;

    if (!timerPaused)
    {
        // Reflect the playerMovement's time directly
        time_remaining = playerMovement.time;

        // Update the fill amount based on the current time_remaining
        timer_foreground.fillAmount = time_remaining / max_time;

        if (time_remaining <= 0)
        {
            playerMovement.TimeHasRunOut();
        }
    }
}
public void SetMaxTime(float newMaxTime)
{
    max_time = newMaxTime;
    time_remaining = max_time;
}

}

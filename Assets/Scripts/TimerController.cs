using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerController : MonoBehaviour
{

    public Image timer_foreground;

    float time_remaining;

    public float max_time;

    public float playerTime;

    public GameObject player;

	public PlayerMovement playerMovement;




	void Start()
    {
		playerTime = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().time;

		
		//playerTime = 

		max_time = playerTime;

		time_remaining = max_time;

        
	}

    void Update()
    {
      //  Debug.Log(playerTime);
		
        
        if(time_remaining > 0)
		{
            time_remaining -= Time.deltaTime;
            timer_foreground.fillAmount = time_remaining / max_time;

		}

        if (time_remaining <= 0)
        {
			// Debug.Log("Time Remaining: " + time_remaining);
			playerMovement.TimeHasRunOut();
		}
    }

}

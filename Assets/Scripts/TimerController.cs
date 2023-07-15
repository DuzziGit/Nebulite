using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerController : MonoBehaviour
{

    public Image timer_foreground;

    float time_remaining;

    public float max_time = 5.0f;




    void Start()
    {
        time_remaining = max_time;
    }

    void Update()
    {

		
        
        if(time_remaining > 0)
		{
            time_remaining -= Time.deltaTime;
            timer_foreground.fillAmount = time_remaining / max_time;

		}

        if (time_remaining <= 0)
        {
            Debug.Log("Time Remaining: " + time_remaining);
            TimeHasRunOut();
        }
    }


    static void TimeHasRunOut()
    {
        // Called when time is equal to, or less then 0
    }
}

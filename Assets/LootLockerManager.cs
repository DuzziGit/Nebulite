using UnityEngine;
using LootLocker.Requests;

public class LootLockerManager : MonoBehaviour
{
    public static bool isSessionStarted = false;

    void Start()
    {
        StartSession();
    }

    void StartSession()
    {
        LootLockerSDKManager.StartGuestSession((response) =>
        {
            if (response.success)
            {
                Debug.Log("Session started");
                isSessionStarted = true;
            }
            else
            {
                Debug.LogError("Failed to start session");
            }
        });
    }
}

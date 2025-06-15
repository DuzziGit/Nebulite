using UnityEngine;
using Dan.Main;

public class LeaderboardTester : MonoBehaviour
{
    void Start()
    {
        Debug.Log("Testing connection...");
        var leaderboard = new LeaderboardReference("PublicKey1");

        leaderboard.TestConnection(
            isOnline => Debug.Log($"Server is online: {isOnline}"),
            error => Debug.LogError($"Error testing connection: {error}")
        );
    }
}

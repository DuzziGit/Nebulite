using UnityEngine;
using UnityEngine.UI;
using LootLocker.Requests;
using TMPro;

public class PlayerNameChanger : MonoBehaviour
{
    public TMP_InputField playerNameInput;

    public void SetPlayerNameFromInput()
    {
        string newName = playerNameInput.text;

        LootLockerSDKManager.SetPlayerName(newName, (response) =>
        {
            if (response.success)
            {
                Debug.Log("Player name successfully set to: " + newName);
            }
            else
            {
                Debug.LogError("Failed to set player name: " + response.errorData.message);
            }
        });
    }
}

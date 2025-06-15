using UnityEngine;
using TMPro;

public class PlayerNameChanger : MonoBehaviour
{
    public TMP_InputField playerNameInput;

    public void SetPlayerNameFromInput()
    {
        string newName = playerNameInput.text;

        // Save it to PlayerPrefs
        PlayerPrefs.SetString("PlayerUsername", newName);
        PlayerPrefs.Save();

        Debug.Log("Username set to: " + newName);
    }
}

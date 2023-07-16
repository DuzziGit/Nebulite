using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class playButton : MonoBehaviour
{

public void PlayGame(){
SceneManager.LoadScene("Level");
}
}

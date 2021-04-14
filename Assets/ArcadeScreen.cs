using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ArcadeScreen : MonoBehaviour
{
    public void GoToArcadeScreen(int level)
    {
        PlayerPrefs.SetInt("ArcadeLevel", level);
        SceneManager.LoadScene("Excavator");
    }
}

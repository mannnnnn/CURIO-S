using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour
{

    public GameObject VolumeMenu;
    bool VolumeMenuShown = false;

    void Start()
    {

        //Setup player prefs for volume if this is teh first time playing
        if(!PlayerPrefs.HasKey("Volume"))
        {
            PlayerPrefs.SetFloat("Volume", 0.5f);
        }
        if (!PlayerPrefs.HasKey("Muted"))
        {
            PlayerPrefs.SetInt("Muted", 0);
        }
    }


    public void ToggleVolumeMenu()
    {
        if (VolumeMenuShown)
        {
            VolumeMenu.GetComponent<Animator>().SetTrigger("Hide");
        } else
        {
            VolumeMenu.GetComponent<Animator>().SetTrigger("Show");
        }
        VolumeMenuShown = !VolumeMenuShown;
    }

    public void IncreaseVolume()
    {
        if (PlayerPrefs.GetFloat("Volume") < 1)
        {
            PlayerPrefs.SetFloat("Volume", PlayerPrefs.GetFloat("Volume") + 0.1f);
        }
           
    }

    public void DecreaseVolume()
    {
        if (PlayerPrefs.GetFloat("Volume") > 0)
        {
            PlayerPrefs.SetFloat("Volume", PlayerPrefs.GetFloat("Volume") - 0.1f);
        }
    }

    public void ToggleMute()
    {
        if (PlayerPrefs.GetInt("Mute") > 0)
        {
            PlayerPrefs.SetInt("Mute", 0);
        } else
        {
            PlayerPrefs.SetInt("Mute", 1);
        }
    }

    public void GoToArcadeScreen()
    {
        SceneManager.LoadScene("ArcadeScreen");
    }
}

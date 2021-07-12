using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class Excavator : MonoBehaviour
{
    float refWidth = 2160;
    float refHeight = 1080;
    private List<MineableObject> treasures = new List<MineableObject>();
    private List<MineableObject> junk = new List<MineableObject>();
    public ControlMode controlMode = ControlMode.HAND;
    public GameObject scanner;
    public Text currProgressLabel;
    public Text currQualityLabel;
    public Image progressFill;
    public Image damageFill;

    private MineableObject currentObject;

    public Color minedColor = Color.red;

    public GameObject MarkerCursor;
    public GameObject DrillCursor;
    public GameObject HandCursor;

    Animator drillAnim;

    public List<SFXSet> sfx = new List<SFXSet>();
    public AudioSource audioSource;
    public string currSoundClip = "";

    public GameObject resultTreasure;
    public GameObject resultScreen;
    public GameObject resultBox;

    private bool gameEnded = false;
    public List<TreasureBook.MinedFossil> SessionCollectedFossils = new List<TreasureBook.MinedFossil>();

    [System.Serializable]
    public class SFXSet
    {
        public string sfxName = "";
        public AudioClip audio;
        public bool loopUntilCancelled = false; //sfx will play until the event comes in to stop it
    }

    public enum ControlMode //what the mouse/finger is controlling
    {
        MARKER,
        DRILL,
        HAND,
    }

    public static Excavator GetInstance()
    {
        return GameObject.Find("ChimeraController").GetComponent<Excavator>();
    }
    public void BackButtonPressed()
    {
        SceneManager.LoadScene("Arcade");
    }

    public MineableObject[] GetAllTreasures()
    {
        //TODO: Let's optimize 
        return FindObjectsOfType<MineableObject>();
    }

    void Start()
    {
        currProgressLabel.text = "0%";
        currQualityLabel.text = "0%";
        progressFill.fillAmount = 0;
        damageFill.fillAmount = 0;
        if (PlayerPrefs.GetInt("ArcadeLevel") > 0)
        {
            LoadLevel(PlayerPrefs.GetInt("ArcadeLevel"));
        }

        drillAnim = DrillCursor.GetComponentInChildren<Animator>();
        Cursor.visible = false; //custom cursors are setup
        audioSource = GetComponent<AudioSource>();
        //Load in references to all the mineable objects, and sort them out
        foreach (MineableObject obj in FindObjectsOfType<MineableObject>())
        {
            switch (obj.classification)
            {
                case MineableObject.Classification.TREASURE: treasures.Add(obj); break;
                case MineableObject.Classification.TRASH:
                default: junk.Add(obj); break;
            }

        }
    }

    public void PickTool(int mode)
    {
        PickTool((ControlMode)mode);
    }

    public void Update()
    {

        if (GetAllTreasures().Length == 0 && !gameEnded)
        {
            //end the game
            gameEnded = true;
            EndGameResults();
        }


        /*
        if (drillAnim != null && controlMode == ControlMode.DRILL && ((Input.GetMouseButton(0) && !drillAnim.GetBool("Drilling")) || (!Input.GetMouseButton(0) && drillAnim.GetBool("Drilling"))))
        {
            RunDrill();
        } */
    }

    public void PickTool(ControlMode mode)
    {
        controlMode = mode;
        PlaySFX("pick tool");
        switch (controlMode)
        {
            case ControlMode.MARKER:
                MarkerCursor.SetActive(true);
                DrillCursor.SetActive(false);
                HandCursor.SetActive(false);
                break;
            case ControlMode.DRILL:
                MarkerCursor.SetActive(false);
                DrillCursor.SetActive(true);
                HandCursor.SetActive(false);
                break;
            case ControlMode.HAND:
                MarkerCursor.SetActive(false);
                DrillCursor.SetActive(false);
                HandCursor.SetActive(true);
                break;
            default: break;
        }

    }

    public void ToggleScanner()
    {
        scanner.SetActive(!scanner.active);
    }

    public void RunDrill()
    {
        Animator drillAnim = DrillCursor.GetComponentInChildren<Animator>();
        drillAnim.SetBool("Drilling", !drillAnim.GetBool("Drilling"));
    }

    public void PlaySFX(string sfxEvent)
    {
        foreach (SFXSet set in sfx)
        {
            if (set.sfxName == sfxEvent)
            {
                audioSource.clip = set.audio;
                audioSource.loop = set.loopUntilCancelled;
            }
        }
        if (audioSource.clip != null)
        {
            currSoundClip = audioSource.loop ? sfxEvent : "";
            audioSource.Play();

        }
    }

    public void StopSFX()
    {
        audioSource.Stop();
        currSoundClip = "";
    }

    public bool LoadLevel(int level)
    {
        Vector3 screenDim = Camera.main.ViewportToScreenPoint(Vector3.one);
        float scale = Mathf.Min(refWidth / screenDim.x, refHeight / screenDim.y);



        foreach (MineableObject fossil in FindObjectsOfType<MineableObject>())
        {
            DestroyImmediate(fossil.gameObject);
        }


        TextAsset file = Resources.Load(level.ToString()) as TextAsset;
        string testRead = file.ToString();
        LevelSetupSaveFile save = JsonUtility.FromJson<LevelSetupSaveFile>(testRead);

        foreach (TreasureBook.Fossil fossil in save.fossils)
        {
            GameObject loadedFossil = Instantiate(PlayerInfo.GetInstance().fossilBook.fossilPrefabs[fossil.prefabIndex]);
            loadedFossil.transform.localPosition = new Vector3(fossil.xy[0] * scale, fossil.xy[1] * scale, -1);
            loadedFossil.transform.localScale = new Vector3(fossil.scale[0] * scale, fossil.scale[1] * scale, 1);
            loadedFossil.transform.eulerAngles = new Vector3(fossil.rotation[0], fossil.rotation[1], fossil.rotation[2]);
        }

        return true;
    }



    public void EndGameResults()
    {
        resultScreen.SetActive(true);
        foreach(TreasureBook.MinedFossil fossil in SessionCollectedFossils)
        {
            GameObject treasure = Instantiate(resultTreasure, resultBox.transform);
            Image treasureImageShadow = treasure.gameObject.transform.GetChild(1).GetComponent<Image>();
            Image treasureImage = treasure.gameObject.transform.GetChild(2).GetComponent<Image>();
            Text treasureName = treasure.gameObject.transform.GetChild(3).GetComponent<Text>();
            Text treasurePercent = treasure.gameObject.transform.GetChild(5).GetComponent<Text>();
            

            treasureImageShadow.sprite = PlayerInfo.GetInstance().fossilBook.GetFossilSprite(fossil.type);
            treasureImage.sprite = PlayerInfo.GetInstance().fossilBook.GetFossilSprite(fossil.type);
            treasureImage.color = new Color(treasureImage.color.r, treasureImage.color.g*(fossil.topQuality), treasureImage.color.b, treasureImage.color.a);
            treasureName.text = PlayerInfo.GetInstance().fossilBook.GetFossilInfo(fossil.type).name;
            treasurePercent.text = String.Format("{0:P2}", fossil.topQuality);
        }
    }
}

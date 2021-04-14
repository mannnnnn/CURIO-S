﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class Excavator : MonoBehaviour
{

    private List<MineableObject> treasures = new List<MineableObject>();
    private List<MineableObject> junk = new List<MineableObject>();
    public ControlMode controlMode = ControlMode.HAND;
    public GameObject scanner;

    [Header("DebugLabels")]
    public Text currNameLabel;
    public Text currProgressLabel;
    public Text currQualityLabel;
    private MineableObject currentObject;

    public Color minedColor = Color.red;

    public GameObject MarkerCursor;
    public GameObject DrillCursor;
    public GameObject HandCursor;

    Animator drillAnim;

    public List<SFXSet> sfx = new List<SFXSet>();
    public AudioSource audioSource;
    public string currSoundClip = "";

    [System.Serializable]
    public class SFXSet{
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

    void Start()
    {

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
            switch (obj.classification) {
                case MineableObject.Classification.TREASURE:  treasures.Add(obj); break;
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
        if (controlMode == ControlMode.DRILL && ((Input.GetMouseButton(0) && !drillAnim.GetBool("Drilling")) || (!Input.GetMouseButton(0) && drillAnim.GetBool("Drilling"))))
        {
            RunDrill();
        } 
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

    public void PlaySFX(string sfxEvent){
       foreach(SFXSet set in sfx)
        {
            if(set.sfxName == sfxEvent)
            {
                audioSource.clip = set.audio;
                audioSource.loop = set.loopUntilCancelled;
            }
        }
       if(audioSource.clip != null)
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
        foreach (MineableObject fossil in FindObjectsOfType<MineableObject>())
        {
            DestroyImmediate(fossil.gameObject);
        }


        if (File.Exists(Application.persistentDataPath + "/" + level + ".nonsense"))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/" + level + ".nonsense", FileMode.Open);
            LevelLoader.LevelSetupSaveFile save = (LevelLoader.LevelSetupSaveFile)formatter.Deserialize(file);
            file.Close();

            foreach (TreasureBook.Fossil fossil in save.fossils)
            {
                GameObject loadedFossil = Instantiate(PlayerInfo.GetInstance().fossilBook.fossilPrefabs[fossil.prefabIndex]);
                loadedFossil.transform.position = new Vector3(fossil.xy[0], fossil.xy[1], -1);
            }

            return true;
        }
        return false;
    }

}

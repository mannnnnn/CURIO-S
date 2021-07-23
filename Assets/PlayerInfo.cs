using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    public List<TreasureBook.MinedFossil> collectedFossils = new List<TreasureBook.MinedFossil>();
    public int currentStage = 0;

    string playerName = "lightmoon";
    public TreasureBook fossilBook; //used for looking up fossil information

    void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Player");

        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }

    public static PlayerInfo GetInstance()
    {
        return GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<PlayerInfo>();
    }

    void Start()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("GameController");

        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        } else
        {
           // DontDestroyOnLoad(this.gameObject);
        }

        if (LoadGame())
        {
            UnityEngine.Debug.Log("Loaded last save for " + playerName.ToUpper());
        }
    }

    public void CollectFossil(TreasureBook.MinedFossil fossil)
    {
        bool prevRecorded = false;
       foreach(TreasureBook.MinedFossil recordedFind in collectedFossils)
        {
            if(recordedFind.type == fossil.type)
            {
                recordedFind.quantity++;
                if(fossil.topQuality > recordedFind.topQuality)
                {
                    recordedFind.topQuality = fossil.topQuality;
                }
                prevRecorded = true;
            }
        }

        if (!prevRecorded)
        {
            //TODO: new fossil!
            collectedFossils.Add(fossil);
        }
        Excavator.GetInstance().SessionCollectedFossils.Add(fossil);
        SaveGame();
    }

    public void DestroyFossil(TreasureBook.MinedFossil fossil)
    {
        Excavator.GetInstance().SessionCollectedFossils.Add(fossil);
        SaveGame();
    }

    //////////////////// SAVE LOAD SYSTEM
    ///
    [System.Serializable]
    public class SaveFile
    {
        public List<TreasureBook.MinedFossil> fossils = new List<TreasureBook.MinedFossil>();
        public int stage = 0;
    }

    public void SaveGame()
    {
        SaveFile save = new SaveFile();
        save.stage = currentStage;
        save.fossils = collectedFossils;
        string fileLocation = Path.Combine(Application.persistentDataPath, playerName + ".json");
        string jsonData = JsonUtility.ToJson(save, true);
        File.WriteAllText(fileLocation, jsonData);
    }

    public bool LoadGame()
    {
        string fileLocation = Path.Combine(Application.persistentDataPath, playerName + ".json");
        if (File.Exists(fileLocation))
        {
            TextAsset file = Resources.Load(playerName) as TextAsset;
            string testRead = file.ToString();
            SaveFile save = JsonUtility.FromJson<SaveFile>(testRead);
            currentStage = save.stage;
            collectedFossils = save.fossils;
            return true;
        }
        return false;
    }



}

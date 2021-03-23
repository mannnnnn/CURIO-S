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

    

    public static PlayerInfo GetInstance()
    {
        return GameObject.Find("ImmortalChimera").GetComponent<PlayerInfo>();
    }

    void Start()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("GameController");

        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        } else
        {
            DontDestroyOnLoad(this.gameObject);
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
                    //TODO: new record
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

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/" + playerName + ".nonsense");
        formatter.Serialize(file, save);
        file.Close();
    }

    private bool LoadGame()
    {
        if (File.Exists(Application.persistentDataPath + "/" + playerName + ".nonsense"))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/" + playerName + ".nonsense", FileMode.Open);
            SaveFile save = (SaveFile)formatter.Deserialize(file);
            file.Close();


            currentStage = save.stage;
            collectedFossils = save.fossils;
            return true;
        }
        return false;
    }



}

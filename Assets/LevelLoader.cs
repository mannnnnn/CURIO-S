using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Excavator))]
public class LevelLoader : Editor
{
    int level = 0;

    public override void OnInspectorGUI()
    {
       
        level = EditorGUILayout.IntField("Level", level);

        if (GUILayout.Button("Save Level"))
        {
            SaveLevel(level);
        }

        if (GUILayout.Button("Load Level"))
        {
            LoadLevel(level);
        }

        EditorGUILayout.Space(10);

        DrawDefaultInspector();

    }

    [System.Serializable]
    public class LevelSetupSaveFile
    {
        public List<TreasureBook.Fossil> fossils = new List<TreasureBook.Fossil>();
    }

    public void SaveLevel(int saveSlot){
        LevelSetupSaveFile save = new LevelSetupSaveFile();

        foreach (MineableObject fossil in FindObjectsOfType<MineableObject>())
        {
            TreasureBook.Fossil savingFossil = new TreasureBook.Fossil();
            savingFossil.prefabIndex = (int)fossil.type;
            savingFossil.xy = new float[2] { fossil.gameObject.transform.position.x, fossil.gameObject.transform.position.y };
            save.fossils.Add(savingFossil);
        }

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/" + level + ".nonsense");
        formatter.Serialize(file, save);
        file.Close();
    }

    private bool LoadLevel(int level)
    {
        foreach (MineableObject fossil in FindObjectsOfType<MineableObject>())
        {
            DestroyImmediate(fossil.gameObject);
        }


        if (File.Exists(Application.persistentDataPath + "/" + level + ".nonsense"))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/" + level + ".nonsense", FileMode.Open);
            LevelSetupSaveFile save = (LevelSetupSaveFile)formatter.Deserialize(file);
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

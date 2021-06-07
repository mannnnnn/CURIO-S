using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;


[CustomEditor(typeof(Excavator))]
public class LevelLoader : Editor
{
    int level = 0;
    private string fileLocation;

    public override void OnInspectorGUI()
    {
         string fileLocation = Path.Combine(Application.persistentDataPath, level + ".json");
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

    public void SaveLevel(int saveSlot){
        LevelSetupSaveFile save = new LevelSetupSaveFile();
         string fileLocation = Path.Combine(Application.persistentDataPath, level + ".json");
        foreach (MineableObject fossil in FindObjectsOfType<MineableObject>())
        {
            TreasureBook.Fossil savingFossil = new TreasureBook.Fossil();
            savingFossil.prefabIndex = (int)fossil.type;
            savingFossil.xy = new float[2] { fossil.gameObject.transform.position.x, fossil.gameObject.transform.position.y };
            save.fossils.Add(savingFossil);
        }

        string jsonData = JsonUtility.ToJson( save , true );
        File.WriteAllText( fileLocation + level + ".json" , jsonData );

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream file = File.Create(fileLocation + level + ".nonsense");
        formatter.Serialize(file, save);
        file.Close();
    }

    public bool LoadLevel(int level)
    {
         string fileLocation = Path.Combine(Application.persistentDataPath, level + ".json");
        foreach (MineableObject fossil in FindObjectsOfType<MineableObject>())
        {
            DestroyImmediate(fossil.gameObject);
        }

        if (File.Exists(fileLocation + level + ".json")){
            LevelSetupSaveFile save = JsonUtility.FromJson<LevelSetupSaveFile>( File.ReadAllText(fileLocation + level + ".json") );
            
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
#endif

[System.Serializable]
public class LevelSetupSaveFile
{
    public List<TreasureBook.Fossil> fossils = new List<TreasureBook.Fossil>();
}

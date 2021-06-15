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
    float refWidth = 2160  ;
    float refHeight = 1080;
    float widthScalar = 1;
    float heightScalar = 1;

    public Transform WorldParent;
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
         string fileLocation = Application.dataPath + "/Resources/";
        foreach (MineableObject fossil in FindObjectsOfType<MineableObject>())
        {
            TreasureBook.Fossil savingFossil = new TreasureBook.Fossil();
            savingFossil.prefabIndex = (int)fossil.type;
            savingFossil.xy = new float[2] { fossil.gameObject.transform.position.x, fossil.gameObject.transform.position.y };
            savingFossil.scale = new float[3] { fossil.gameObject.transform.localScale.x, fossil.gameObject.transform.localScale.y, fossil.gameObject.transform.localScale.z };
            savingFossil.rotation = new float[3] { fossil.gameObject.transform.eulerAngles.x, fossil.gameObject.transform.eulerAngles.y, fossil.gameObject.transform.eulerAngles.z };
            save.fossils.Add(savingFossil);
        }
        string jsonData = JsonUtility.ToJson( save , true );
        File.WriteAllText( fileLocation + level + ".json" , jsonData );
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
            loadedFossil.transform.localPosition = new Vector3(fossil.xy[0]*scale, fossil.xy[1] * scale, -1);
            loadedFossil.transform.localScale = new Vector3(fossil.scale[0] * scale, fossil.scale[1] * scale, fossil.scale[2] * scale);
            loadedFossil.transform.eulerAngles = new Vector3(fossil.rotation[0], fossil.rotation[1], fossil.rotation[2]);
        }

        return true;
    }
}
#endif

[System.Serializable]
public class LevelSetupSaveFile
{
    public List<TreasureBook.Fossil> fossils = new List<TreasureBook.Fossil>();
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TreasureBook", menuName = "ScriptableObjects/TreasureBook", order = 1)]
public class TreasureBook : ScriptableObject
{

    public List<Fossil> fossils = new List<Fossil>();
    public List<GameObject> fossilPrefabs = new List<GameObject>();

    [System.Serializable]
    public class MinedFossil
    {
        public FossilType type;
        public float topQuality = 0;
        public int quantity = 0;

        public MinedFossil(FossilType type, float quality, int quantity)
        {
            this.type = type;
            this.topQuality = quality;
            this.quantity = quantity;
        }
    }

    [System.Serializable]
    public class Fossil
    {
        public FossilType type = FossilType.STEGALOMANIAC;
        public string name = "";
        public string desc = "Doo do daat dat-";
        public MineableObject.Classification classification = MineableObject.Classification.TREASURE;
        public int prefabIndex = -1;
        public float[] xy;
    }

    [System.Serializable]
    public enum FossilType{
        DEFAULT,
        STEGALOMANIAC,
        DATRIRAPTOR,
        MESODIPUS,
        ORNEROPTYRIX,
        PHELOPTHESAURUS,
        TERAGNATHUS,
        VILACTASAURUS
    }
}

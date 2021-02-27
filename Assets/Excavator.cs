using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Excavator : MonoBehaviour
{

    private List<MineableObject> treasures = new List<MineableObject>();
    private List<MineableObject> junk = new List<MineableObject>();
    public ControlMode controlMode = ControlMode.HAND;
    public GameObject scanner;

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
        //Load in references to all the mineable objects, and sort them out
        foreach(MineableObject obj in FindObjectsOfType<MineableObject>())
        {
            switch (obj.type) {
                case MineableObject.Type.TREASURE:  treasures.Add(obj); break;
                case MineableObject.Type.TRASH:
                default: junk.Add(obj); break;
            }
            
        }
    }

    public void PickTool(int mode)
    {
        controlMode = (ControlMode)mode;
    }

    public void ToggleScanner()
    {
        scanner.SetActive(!scanner.active);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

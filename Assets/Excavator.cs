using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
        Cursor.visible = false; //custom cursors are setup
        //Load in references to all the mineable objects, and sort them out
        foreach (MineableObject obj in FindObjectsOfType<MineableObject>())
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
        PickTool((ControlMode)mode);
    }

    public void PickTool(ControlMode mode)
    {
        controlMode = mode;

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

    // Update is called once per frame
    void Update()
    {
        
    }
}

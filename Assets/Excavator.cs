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
    public GameObject drawLayer;


    [Header("DebugLabels")]
    public Text currNameLabel;
    public Text currProgressLabel;
    public Text currQualityLabel;
    private MineableObject currentObject;

    public Color minedColor = Color.red;

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
        drawLayer.transform.localPosition = new Vector3(drawLayer.transform.localPosition.x, drawLayer.transform.localPosition.y, controlMode == ControlMode.MARKER ? -1 : -0.1f);  
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

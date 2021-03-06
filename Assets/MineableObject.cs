using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MineableObject : MonoBehaviour
{

    public enum Type
    {
        DEFAULT,
        TREASURE,
        TRASH,
        HAZARD
    }

    public Type type = Type.DEFAULT;
    private float quality = 1; //100%  //the score for this piece
    private float progress = 0; //0%  //how far into the successful excavation you are
    private float completionThreshold = 0.80f; //80% //when this piece can be removed
    private float failureThreshold = 0f; //0% //when this piece becomes broken
    private bool removeable = false;
    private bool broken = false;
    private float fragility = 0.01f; // how quickly an object breaks

    //interaction logic
    private bool interacting = false;
    private Excavator.ControlMode cursorTool = Excavator.ControlMode.HAND;
    private Excavator chimera;
    void Start()
    {
        chimera = Excavator.GetInstance();
    }

    public void Remove()
    {
        //TODO: play remove animation
        //TODO: play some sfx
        Destroy(this.gameObject);
    }

    public void OnMouseDown()
    {
        UnityEngine.Debug.Log("Hello there!");
        cursorTool = Excavator.GetInstance().controlMode;

        if (cursorTool == Excavator.ControlMode.HAND)
        {
            if(progress >= completionThreshold && !broken)
            {
                Remove();
            } else
            {
                //TODO: error sound fx
                //jiggle animation
            }
        }
        interacting = true;
        UpdateDebugLabels();
    }

    public void OnMouseUp()
    {
        interacting = false;
    }

    void OnMouseOver()
    {
        UpdateDebugLabels();
    }

    void UpdateDebugLabels()
    {
        chimera.currNameLabel.text = gameObject.name;
        chimera.currProgressLabel.text = broken ? "BROKEN" : "Progress: " + progress + "%";
        chimera.currQualityLabel.text = "Quality: " + quality + "%";
        chimera.currNameLabel.color = broken ? Color.red : Color.white;
        chimera.currProgressLabel.color = broken ? Color.red : Color.white;
        chimera.currQualityLabel.color = broken ? Color.red : Color.white;
    }

    void FixedUpdate()
    {
        if(cursorTool == Excavator.ControlMode.DRILL && interacting && !broken)
        {
            UnityEngine.Debug.Log("Drilling!");
            //damage has occurred!
            //TODO: bad vfx+sfx
            quality -= fragility;
            if(quality <= failureThreshold)
            {
                broken = true;
                quality = failureThreshold;
            }
            UpdateDebugLabels();
        }
    }
}

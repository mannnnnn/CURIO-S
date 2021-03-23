﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MineableObject : MonoBehaviour
{

    public enum Classification
    {
        DEFAULT,
        TREASURE,
        TRASH,
        HAZARD
    }

    public TreasureBook.FossilType type = TreasureBook.FossilType.DEFAULT;
    public Classification classification = Classification.DEFAULT;
    private float quality = 1; //100%  //the score for this piece
    public float progress = 0; //0%  //how far into the successful excavation you are
    public float completionThreshold = 0.80f; //80% //when this piece can be removed
    private float failureThreshold = 0f; //0% //when this piece becomes broken
    public bool removeable = false;
    public bool broken = false;
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
        TreasureBook.MinedFossil fossil = new TreasureBook.MinedFossil(type, quality, 1);
        PlayerInfo.GetInstance().CollectFossil(fossil);
        //TODO: play remove animation
        //TODO: play some sfx
        Destroy(this.gameObject);
    }

    public void OnMouseDown()
    {
        cursorTool = Excavator.GetInstance().controlMode;
        if (cursorTool == Excavator.ControlMode.HAND)
        {
            if(removeable && !broken)
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

    void OnMouseExit()
    {
        interacting = false;
    }

    void OnMouseOver()
    {
        
        cursorTool = Excavator.GetInstance().controlMode;
        if (!interacting && Input.GetMouseButton(0) && cursorTool == Excavator.ControlMode.DRILL)
        {
            interacting = true;
            UpdateDebugLabels();
        }
    }

    public void UpdateDebugLabels()
    {
        chimera.currNameLabel.text = gameObject.name;
        chimera.currProgressLabel.text = broken ? "BROKEN" : "Progress: " + (progress/completionThreshold*100).ToString("#.##") + "%";
        chimera.currQualityLabel.text = "Quality: " + (quality*100).ToString("#.##") + "%";
        chimera.currNameLabel.color = broken ? Color.red : Color.white;
        chimera.currProgressLabel.color = broken ? Color.red : Color.white;
        chimera.currQualityLabel.color = broken ? Color.red : Color.white;
    }

    void FixedUpdate()
    {
        if(cursorTool == Excavator.ControlMode.DRILL && interacting && !broken)
        {
            //damage has occurred!
            //TODO: bad vfx+sfx
            quality -= fragility;
            if(quality <= failureThreshold)
            {
                broken = true;
                quality = failureThreshold;
            }
            if (!removeable)
            {
                removeable = progress >= completionThreshold;
            }
           
            UpdateDebugLabels();
        }
    }
}

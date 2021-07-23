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
    public SpriteRenderer animatedFossil;
    public GameObject dirtMound;
    //interaction logic
    private bool interacting = false;
    private Excavator.ControlMode cursorTool = Excavator.ControlMode.HAND;
    private Excavator chimera;
    private Animator animator;

    private float fallbackCooldownForRemoval = 2000f;

    void Start()
    {
        chimera = Excavator.GetInstance();
        animator = GetComponent<Animator>();
        animatedFossil.sprite = transform.GetChild(0).GetComponent<SpriteRenderer>().sprite;
        dirtMound.transform.localScale = dirtMound.transform.localScale / transform.localScale.x;
    }

    public void RemoveFossil()
    {

        TreasureBook.MinedFossil fossil = new TreasureBook.MinedFossil(type, quality, 1);
        if (quality > failureThreshold)
        {
            if (PlayerInfo.GetInstance() != null)
            {
                chimera.currProgressLabel.text = "COLLECT1";
                PlayerInfo.GetInstance().CollectFossil(fossil);
                chimera.currProgressLabel.text = "COLLECT2";
            } else
            {
                chimera.currProgressLabel.text = "ERROR1";
            }
          
           
        } else
        {
            if (PlayerInfo.GetInstance() != null)
            {
                PlayerInfo.GetInstance().DestroyFossil(fossil);
            } else
            {
                chimera.currProgressLabel.text = "ERROR2";
            }
               
           
        }
       
        if(chimera != null)
        {
            chimera.currProgressLabel.text = "0%";
            chimera.currQualityLabel.text = "0%";

            chimera.progressFill.fillAmount = 0;
            chimera.damageFill.fillAmount = 0;
            foreach (SpriteRenderer sprite in GetComponentsInChildren<SpriteRenderer>())
            {
                sprite.color = Color.red;
            }
        }

        Destroy(this.gameObject);
        
    }

    public void OnMouseDown()
    {
        cursorTool = Excavator.GetInstance().controlMode;
        if (cursorTool == Excavator.ControlMode.HAND)
        {
            if(removeable && !broken)
            {
                GetComponent<Animator>().SetTrigger("Collect");
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

        float progressFillPercent = progress / completionThreshold ;
        float failureFillPercent = ((1 - quality));

        if(completionThreshold > 0 && !float.IsNaN(progressFillPercent) && progressFillPercent >= 0)
        {
            chimera.currProgressLabel.text = broken ? "0%" : (Mathf.Clamp(progressFillPercent, 0, 1) * 100).ToString("#.##") + "%";
        } else
        {
            chimera.currProgressLabel.text = "0%";
            chimera.progressFill.fillAmount = 0;
        }

        if (!float.IsNaN(failureFillPercent) && failureFillPercent > 0)
        {
            chimera.currQualityLabel.text = (Mathf.Clamp(failureFillPercent, 0, 1) * 100).ToString("#.##") + "%";
        }
        else
        {
            chimera.currQualityLabel.text = "0%";
            chimera.damageFill.fillAmount = 0;
        }
      
        chimera.progressFill.fillAmount = progressFillPercent;
        chimera.damageFill.fillAmount = failureFillPercent;
        chimera.currProgressLabel.color = broken ? brokenText : Color.white;
        chimera.currQualityLabel.color = broken ? brokenText : Color.white;

        if (broken)
        {
            chimera.progressFill.color = Color.magenta;
        }
    }

    Color brokenText = new Color(1, 0.2f, 0.2f, 1f);

    void FixedUpdate()
    {
        if(cursorTool == Excavator.ControlMode.DRILL && interacting)
        {
            //damage has occurred!
            if (chimera.currSoundClip != "bone crunch")
            {
                //chimera.PlaySFX("bone crunch");
            }
               

            if (!broken)
            {
                quality -= fragility;
                if (quality <= failureThreshold)
                {
                    broken = true;
                    quality = failureThreshold;
                    animator.SetTrigger("Break");
                }
                if (!removeable)
                {
                    removeable = progress >= completionThreshold;
                }

                UpdateDebugLabels();
            }
        } else
        {
            if(chimera.currSoundClip == "bone crunch")
            {
                //chimera.StopSFX();
            }
        }
    }
}

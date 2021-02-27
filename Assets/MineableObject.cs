using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MineableObject : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
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
    private bool removeable = false;


    //interaction logic
    private bool interacting = false;
    private Excavator.ControlMode cursorTool = Excavator.ControlMode.HAND;
    
    void Start()
    {
        
    }

    public void Remove()
    {
        //TODO: play remove animation
        //TODO: play some sfx
        Destroy(this.gameObject);
    }

    public void OnPointerDown(PointerEventData pointerEventData)
    {
        cursorTool = Excavator.GetInstance().controlMode;

        if (cursorTool == Excavator.ControlMode.HAND)
        {
            if(progress >= completionThreshold)
            {
                Remove();
            } else
            {
                //TODO: error sound fx
                //jiggle animation
            }
        }
       interacting = true;
    }

    public void OnPointerUp(PointerEventData pointerEventData)
    {
        interacting = false;
    }

    void FixedUpdate()
    {
        if(cursorTool == Excavator.ControlMode.DRILL)
        {
            //damage has occurred!
            //TODO: bad vfx+sfx
            quality -= 0.01f;
        }
    }
}

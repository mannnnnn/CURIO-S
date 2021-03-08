using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineLine : MonoBehaviour
{
    //interaction logic
    private MineableObject parent;
    public bool interacting = false;
    private Excavator.ControlMode cursorTool = Excavator.ControlMode.HAND;
    void Start()
    {
        parent = transform.GetComponentInParent<MineableObject>();
        if(parent == null)
        {
            UnityEngine.Debug.Log("MineLine isn't attached to a mineable object.");
        }
    }


    public void OnMouseDown()
    {
        cursorTool = Excavator.GetInstance().controlMode;
        interacting = true;
        parent.UpdateDebugLabels();
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
        if (Input.GetMouseButton(0))
        {
            cursorTool = Excavator.GetInstance().controlMode;
            interacting = true;
        }
        parent.UpdateDebugLabels();
    }

   

    //called by Drawable to notify when a pixel has been colored
    public void ProgressUpdate(float progress)
    {
        cursorTool = Excavator.GetInstance().controlMode;
        if (cursorTool == Excavator.ControlMode.DRILL && !parent.broken)
        {
            //progress!
            parent.progress = progress;
            if (parent.progress >= parent.completionThreshold)
            {
                parent.removeable = true;
            }
            parent.UpdateDebugLabels();
        }
    }


    void FixedUpdate()
    {
        //N/A
    }
}

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
        UnityEngine.Debug.Log("Hello!");
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
        UnityEngine.Debug.Log("Oh!");
        if (Input.GetMouseButton(0))
        {
            cursorTool = Excavator.GetInstance().controlMode;
            interacting = true;
        }
        parent.UpdateDebugLabels();
    }

    void FixedUpdate()
    {
        if (cursorTool == Excavator.ControlMode.DRILL && interacting && !parent.broken)
        {
            UnityEngine.Debug.Log("Brrrr!");
            //progress!
            parent.progress += 0.01f;
            if (parent.progress >= parent.completionThreshold)
            {
                parent.removeable = true;
            }
            parent.UpdateDebugLabels();
        }
    }
}

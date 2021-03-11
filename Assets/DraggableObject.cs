using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableObject : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    //interaction logic
    private bool interacting = false;
    Vector2 clickPos;
    Vector3 myOrigPos;
    Excavator.ControlMode prevControl; 

    public void OnPointerDown(PointerEventData pointerEventData)
    {
        UnityEngine.Debug.Log("Down");
        myOrigPos = gameObject.transform.localPosition;
        clickPos = Input.mousePosition;
        prevControl = Excavator.GetInstance().controlMode;
        Excavator.GetInstance().controlMode = Excavator.ControlMode.HAND;
        interacting = true;
    }

    public void OnPointerUp(PointerEventData pointerEventData)
    {
        UnityEngine.Debug.Log("Up");
        interacting = false;
        Excavator.GetInstance().controlMode = prevControl;
        prevControl = Excavator.ControlMode.HAND;
    }

    void OnMouseExit()
    {
        UnityEngine.Debug.Log("Exit");
        Excavator.GetInstance().controlMode = prevControl;
        prevControl = Excavator.ControlMode.HAND;
    }

    void OnMouseOver()
    {
        UnityEngine.Debug.Log("Over");
        prevControl = Excavator.GetInstance().controlMode;
        Excavator.GetInstance().controlMode = Excavator.ControlMode.HAND;
        //prevent mining/drawing above or below scanner
    }

    void FixedUpdate()
    {
        if (interacting)
        {
            float deltaX = clickPos.x - Input.mousePosition.x;
            float deltaY = clickPos.y - Input.mousePosition.y;
            gameObject.transform.localPosition = new Vector3(myOrigPos.x - deltaX, myOrigPos.y - deltaY, myOrigPos.z);
        }
    }
}

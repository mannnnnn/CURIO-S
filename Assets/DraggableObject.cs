using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableObject : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    //interaction logic
    private bool interacting = false;
    Vector2 clickPos;
    Vector3 myOrigPos;
    public Excavator.ControlMode prevControl; 

    public void OnPointerDown(PointerEventData pointerEventData)
    {
        myOrigPos = gameObject.transform.localPosition;
        clickPos = Input.mousePosition;
        if (Excavator.GetInstance().controlMode != Excavator.ControlMode.HAND)
        {
            prevControl = Excavator.GetInstance().controlMode;
        }
        Excavator.GetInstance().controlMode = Excavator.ControlMode.HAND;
        interacting = true;
    }

    public void OnPointerUp(PointerEventData pointerEventData)
    {
        UnityEngine.Debug.Log("Pointer up!");
        if (interacting)
        {
            Excavator.GetInstance().controlMode = prevControl;
        }
        interacting = false;
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        Excavator.GetInstance().controlMode = prevControl;
    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        if (Excavator.GetInstance().controlMode != Excavator.ControlMode.HAND)
        {
            prevControl = Excavator.GetInstance().controlMode;
        }
       
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

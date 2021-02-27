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

    public void OnPointerDown(PointerEventData pointerEventData)
    {
        myOrigPos = gameObject.transform.localPosition;
        clickPos = Input.mousePosition;
        interacting = true;
    }

    public void OnPointerUp(PointerEventData pointerEventData)
    {
        interacting = false;
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

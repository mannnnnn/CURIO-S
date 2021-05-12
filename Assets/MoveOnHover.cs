using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MoveOnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject origPosition;
    public GameObject altPosition;

    bool inAltPosition = false;
    bool hovering = false;

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        hovering = true;
        if (!inAltPosition && origPosition.transform.childCount > 0)
        {
            origPosition.transform.GetChild(0).transform.SetParent(altPosition.transform);
            altPosition.transform.GetChild(0).transform.localPosition = Vector3.zero;
        }
        inAltPosition = true;
    }

    public void Update()
    {
        if (!hovering && altPosition.transform.childCount > 0)
        {
            altPosition.transform.GetChild(0).transform.SetParent(origPosition.transform);
            origPosition.transform.GetChild(0).transform.localPosition = Vector3.zero;
            inAltPosition = !inAltPosition;
        }
       
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        hovering = false;
    }
}

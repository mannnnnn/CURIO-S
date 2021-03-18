using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldableObject : MonoBehaviour
{
    List<LineRenderer> cords = new List<LineRenderer>();
    void Start()
    {
        cords.AddRange(GetComponentsInChildren<LineRenderer>());
    }

    float fudgedWind = 300;

   void FixedUpdate()
    {
            Vector2 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y));
            transform.position = worldPosition;

        fudgedWind -= 1;
        if(fudgedWind <= 0)
        {
            fudgedWind = 300;
        }

        foreach(LineRenderer cord in cords)
        {
            Vector3 cordPosition = new Vector3(cord.gameObject.transform.position.x, cord.gameObject.transform.position.y, cord.GetPosition(cord.positionCount - 1).z);
            cord.SetPosition(0, cordPosition);

            for(int i = 1; i<cord.positionCount-1; i++)
            {
                float x = cordPosition.x + (cord.GetPosition(cord.positionCount - 1).x - cordPosition.x) / cord.positionCount * i;
                float y = cordPosition.y + (cord.GetPosition(cord.positionCount - 1).y - cordPosition.y) / cord.positionCount * i;
                y -= 2 * Mathf.Sin(1.75f*Mathf.PI / cord.positionCount * i);

                //sway cord
               y += 0.2f * Mathf.Sin(2 * Mathf.PI * (fudgedWind / 300));
               x += 0.2f * Mathf.Cos(2 * Mathf.PI * (fudgedWind / 150));
                cord.SetPosition(i, new Vector3(x, y, (cord.GetPosition(cord.positionCount - 1).z)));
            }

            
        }
    }
}

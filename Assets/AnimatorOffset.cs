using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorOffset : MonoBehaviour
{
    public float offset = 0;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Animator>().SetFloat("Offset", offset);
    }
}

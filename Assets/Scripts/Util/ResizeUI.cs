using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResizeUI : MonoBehaviour
{
    public Vector2 Size16_9, Size2_1;
    // Start is called before the first frame update
    void Start()
    {
        float raito = Screen.height / Screen.width;
        if (raito >= 2)
        {
            GetComponent<RectTransform>().sizeDelta = Size2_1;
        }
        else
        {
            GetComponent<RectTransform>().sizeDelta = Size16_9;
        }
    }



}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AutoPositionScroll : MonoBehaviour
{
    public Vector3 PosTop_Right, PosBot_Left;
    public bool isVertical;
    // Start is called before the first frame update
    public RectTransform RectTransform;
    ScrollRect ScrollRect;
    private void Start()
    {
        ScrollRect = gameObject.GetComponent<ScrollRect>();
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (isVertical)
        {
            if (RectTransform.anchoredPosition.y > PosTop_Right.y)
            {
                RectTransform.anchoredPosition = PosTop_Right;
                ScrollRect.velocity = Vector2.zero;
            }
            if (RectTransform.anchoredPosition.y < PosBot_Left.y)
            {
                RectTransform.anchoredPosition = PosBot_Left;
                ScrollRect.velocity = Vector2.zero;
            }
        }
        else
        {
            if (RectTransform.anchoredPosition.x > PosTop_Right.x)
            {
                RectTransform.anchoredPosition = PosTop_Right;
                ScrollRect.velocity = Vector2.zero;
            }
            if (RectTransform.anchoredPosition.x < PosBot_Left.x)
            {
                RectTransform.anchoredPosition = PosBot_Left;
                ScrollRect.velocity = Vector2.zero;
            }
        }
    }
}

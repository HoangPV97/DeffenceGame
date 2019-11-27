using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EffectButton : MonoBehaviour
{
    public RectTransform rectTransform;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnMouseDown()
    {
        rectTransform.localScale = new Vector3(0.9f, 0.9f, 0f);
    }
    private void OnMouseUp()
    {
        rectTransform.localScale = new Vector3(1f, 1f, 0f);
    }
}

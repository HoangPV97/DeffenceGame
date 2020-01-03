using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class LabelLanguagesTMP : LabelLanguages
{
    private TextMeshProUGUI current_labelTMP;

    void Awake()
    {
        current_labelTMP = this.gameObject.GetComponent<TextMeshProUGUI>();
        Language.AddLableLanguage(this);
    }

    void Start()
    {
        SetText();
    }

    //Hàm gán lại text cho label
    public virtual void SetText()
    {
        if (current_labelTMP == null)
        {
            Debug.LogError("Null " + this.gameObject.name);
        }
        else
        {
            string str = Language.GetKey(KEY);
            current_labelTMP.text = str.Trim();
        }
    }
    //Lúc chuyển scene sẽ remove hết list
    void OnDestroy()
    {
        Language.RemoveLaleLanguage(this);
    }


}

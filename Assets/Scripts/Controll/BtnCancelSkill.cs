using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class BtnCancelSkill : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{

    public Image imgBtn;


    public void Start()
    {
        // imgBtn.sprite = DataManager.Instance.GamePlayResource.ImageCancelNomarl;
        imgBtn = gameObject.GetComponent<Image>();
        imgBtn.color = Color.white;
        GameplayController.Instance.CancelSkill = false;
    }
    public void OnPointerEnter(PointerEventData Point)
    {
        //  imgBtn.sprite = DataManager.Instance.GamePlayResource.ImageCancelHighLight;
        imgBtn.color = Color.red;
        GameplayController.Instance.OnCancelSkill(true);
    }

    public void OnPointerExit(PointerEventData Point)
    {
        Debug.Log("UIGamePlayController.Instance.CancelSkill = false;");
        // imgBtn.sprite = DataManager.Instance.GamePlayResource.ImageCancelNomarl;
        imgBtn.color = Color.white;
        GameplayController.Instance.OnCancelSkill(false);
    }

    public void OnDrop(PointerEventData Point)
    {
        Debug.Log("OnDrop(PointerEventData Point)");
        imgBtn.color = Color.white;
        GameplayController.Instance.OnCalcelSkillClick();
    }
}

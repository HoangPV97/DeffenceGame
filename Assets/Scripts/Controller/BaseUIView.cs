using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;
using UnityEngine.UI;
public class BaseUIView : MonoBehaviour, IBaseUI
{
    public UITYPE UITYPE;
    public virtual void OnHide()
    {
        //  SoundManager.Instance.PlaySound("u12");
    }

    public virtual void SetUpCoinIcon()
    {

    }

    public virtual void OnHideLeft(UnityAction unityAction = null)
    {
        transform.DOLocalMoveX(-1111f, 0.3f, true).OnComplete(
           () =>
           {
               gameObject.SetActive(false);
               if (unityAction != null)
                   unityAction();
           }
            );
    }

    public virtual void OnHideRight(UnityAction unityAction = null)
    {
        transform.DOLocalMoveX(1111f, 0.3f, true).OnComplete(
           () =>
           {
               gameObject.SetActive(false);
               if (unityAction != null)
                   unityAction();
           }
            );
    }

    public virtual void OnShow()
    {
    }

    public virtual void OnShowFromLeft(UnityAction unityAction = null)
    {
        gameObject.SetActive(true);
        transform.localPosition = new Vector3(-1111f, transform.localPosition.y, 0);
        transform.DOLocalMoveX(0f, 0.3f, true).OnComplete(() =>
        {
            if (unityAction != null)
                unityAction();
        });
    }

    public virtual void OnShowFromRight(UnityAction unityAction = null)
    {
        gameObject.SetActive(true);
        transform.localPosition = new Vector3(1111f, transform.localPosition.y, 0);
        transform.DOLocalMoveX(0f, 0.3f, true).OnComplete(() =>
        {
            if (unityAction != null)
                unityAction();
        });
    }

    public void ReSetUI()
    {
        throw new System.NotImplementedException();
    }

    public void UpDateData()
    {
        throw new System.NotImplementedException();
    }
}

public enum UITYPE
{
    none = 0,
    shop,
    inventory,
    selectLevel,
    heroAlliance,
    fortress
}
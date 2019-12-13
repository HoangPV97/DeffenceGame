using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public interface IBaseUI
{
    void ReSetUI();
    void UpDateData();
    void OnHide();
    void OnShow();

    void OnHideLeft(UnityAction unityAction = null);
    void OnHideRight(UnityAction unityAction = null);
    void OnShowFromRight(UnityAction unityAction = null);
    void OnShowFromLeft(UnityAction unityAction = null);

}

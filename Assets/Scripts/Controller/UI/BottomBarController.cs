using InviGiant.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class BottomBarController : Singleton<BottomBarController>
{
    [Header("Bottom")]
    public BotUIButton[] BotUIButton;
    public GameObject EmptyBtn;
    private void Awake()
    {
        BotUIButton = GetComponentsInChildren<BotUIButton>();
    }
    IEnumerator Start()
    {
        yield return new WaitForSeconds(0.01f);
        BotUIButton[2].OnClick();
    }
    public void RefreshAllButton(BotUIButton current)
    {
        foreach (var bub in BotUIButton)
        {
            if (bub != current)
            {
                bub.SetActiveSize(false);
            }
        }
    }

    public void RefreshAllButton()
    {
        foreach (var bub in BotUIButton)
        {
            bub.SetDefaultSize();
        }
    }
    public void SuspendAllBotButton()
    {
        foreach (var bub in BotUIButton)
        {
            bub.SuspendClick(true);
        }
        DOVirtual.DelayedCall(0.34f, () =>
        {
            foreach (var bub in BotUIButton)
            {
                bub.SuspendClick(false);
            }
        }, false);
    }
}

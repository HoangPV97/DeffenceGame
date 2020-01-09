using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TabItemWithIcon : TabItem
{
    public Sprite IconOn, IconOff;
    public Image Icon;
    public override void OnDisableTab()
    {
        base.OnDisableTab();
        Icon.sprite = IconOff;
    }
    public override void OnEnableTab()
    {
        base.OnEnableTab();
        Icon.sprite = IconOn;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class TabController : MonoBehaviour
{
    // Start is called before the first frame update
    TabItem[] TabItems;
    TabItem CurrentTabItems;
    public List<UnityEvent> unityActions = new List<UnityEvent>();
    public UnityEvent unityEvent;
    void Awake()
    {
        TabItems = GetComponentsInChildren<TabItem>();
    }

    private void Start()
    {
        TabItems[0].OnTabClick();
        for (int i = 1; i < TabItems.Length; i++)
            TabItems[i].OnDisableTab();
    }
    // Update is called once per frame

    public void OnTabClick(TabItem tabItem)
    {
        if (CurrentTabItems != tabItem)
        {
            if (CurrentTabItems != null)
                CurrentTabItems.OnDisableTab();
            tabItem.OnEnableTab();
            CurrentTabItems = tabItem;
            if (tabItem.Tab == 0)
            {
                MenuController.Instance.UIPanelHeroAlliance.SetUpData(true);
            }
            else
                MenuController.Instance.UIPanelHeroAlliance.SetUpData(false);
        }
        else
        {

        }
    }
}

using UnityEngine;
public class TabController : MonoBehaviour
{
    // Start is called before the first frame update
    TabItem[] TabItems;
    TabItem CurrentTabItems;
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
            CurrentTabItems.unityEvent.Invoke();
        }
        else
        {

        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetElementalImage : MonoBehaviour
{
    [SerializeField]
    Image elematalImage;
    // Start is called before the first frame update
    void Start()
    {
        SetImage(this.GetComponent<EnemyController>().enemy.elemental);
    }

    // Update is called once per frame
    public void SetImage(Elemental elemental)
    {
        switch (elemental)
        {
            case Elemental.Fire:
                elematalImage.sprite = loadImageResource("icon_element_fire");
                break;
            case Elemental.Ice:
                elematalImage.sprite = loadImageResource("icon_element_ice");
                break;
            case Elemental.Wind:
                elematalImage.sprite = loadImageResource("icon_element_wind");
                break;
            case Elemental.None:
                elematalImage.sprite = loadImageResource("icon_element_none");
                break;
        }
    }
    Sprite loadImageResource(string pathFolder)
    {
        Sprite prefab = Resources.Load<Sprite>(pathFolder);
        return prefab;
    }
}

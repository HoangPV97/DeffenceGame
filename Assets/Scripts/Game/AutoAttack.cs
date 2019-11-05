using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AutoAttack : MonoBehaviour
{
    public Sprite autoImage, disableImage;
    public Button btnAuto;
    public Player player;
    public bool Status;
    int Count = 0;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    public void ChangeStatus()
    {
        Count++;
        if (Count % 2 != 0)
        {
            player.currentMode = Player.AutoMode.TurnOn;
            btnAuto.image.sprite = autoImage;
        }
        else
        {
            player.currentMode = Player.AutoMode.TurnOff;
            btnAuto.image.sprite = disableImage;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AutoAttack : MonoBehaviour
{
    public Sprite autoImage, disableImage;
    public TextMeshProUGUI textMesh;
    public Button btnAuto;
    public PlayerController player;
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
            player.currentMode = PlayerController.AutoMode.TurnOn;
            btnAuto.image.sprite = autoImage;
            //textMesh.text = "Auto\nOn";
        }
        else
        {
            player.currentMode = PlayerController.AutoMode.TurnOff;
            btnAuto.image.sprite = disableImage;
            //textMesh.text = "Auto\nOff";
        }
    }
}

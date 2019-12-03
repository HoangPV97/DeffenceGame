using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    public void OnBtnPlayClick()
    {
        DataController.Instance.CurrentSelected = 1;
        DataController.Instance.LoadIngameStage();
        SceneManager.LoadScene(2);
    }

}

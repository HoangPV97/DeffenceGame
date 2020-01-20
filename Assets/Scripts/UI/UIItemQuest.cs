using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class UIItemQuest : MonoBehaviour
{
    public TextMeshProUGUI txtQuestName, txtQuestDes, txtWuestNumber;
    public GameObject pfItem;
    public Transform Contain;
    public Image ProgressBar;
    public UIButton btnClaim;
    public GameObject Completed;
    GameDataQuest gameDataQuest;

    // Start is called before the first frame update
    private void Start()
    {
        btnClaim.SetUpEvent(OnbtnClaimClick);
    }
    void OnbtnClaimClick()
    {
        DataController.Instance.ClaimDailyQuestReward(gameDataQuest.QUEST_TYPE);
        SetUpData(gameDataQuest);
    }
    public void SetUpData(GameDataQuest gdq)
    {
        gameDataQuest = gdq;
        txtQuestName.text = Language.GetKey("NAME_" + gdq.QUEST_TYPE);
        txtQuestDes.text = string.Format(Language.GetKey("DES_" + gdq.QUEST_TYPE), gdq.Target);
        txtWuestNumber.text = string.Format("{0}/{1}", gdq.Current, gdq.Target);
        ProgressBar.fillAmount = gdq.Current * 1.0f / gdq.Target;
        if (gdq.Status == 0)
        {
            ProgressBar.transform.parent.gameObject.SetActive(true);
            btnClaim.gameObject.SetActive(false);
            Completed.SetActive(false);
        }
        else if (gdq.Status == 1)
        {
            ProgressBar.transform.parent.gameObject.SetActive(false);
            btnClaim.gameObject.SetActive(true);
            Completed.SetActive(false);
        }
        else if (gdq.Status == 2)
        {
            ProgressBar.transform.parent.gameObject.SetActive(false);
            btnClaim.gameObject.SetActive(false);
            Completed.SetActive(true);
        }
        foreach (Transform child in Contain)
        {
            Destroy(child.gameObject);
        }
        for (int i = 0; i < gdq.Rewards.Count; i++)
        {
            var it = Instantiate(pfItem.gameObject, Vector3.zero, Quaternion.identity);
            it.SetActive(true);
            it.transform.SetParent(Contain);
            it.transform.SetDefaultTransform();
            it.GetComponent<UiItem>().SetUpData(gdq.Rewards[i], 1);
        }
        GetComponent<Image>().sprite = DataController.Instance.DefaultData.LoadSprite("BACKGROUND_" + gdq.QUEST_TYPE);
    }
}

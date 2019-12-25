using UnityEngine;
using UnityEngine.UI;

public class LabelLanguages : MonoBehaviour
{
	public string KEY;

	private Text current_label;

	void Awake ()
	{
		current_label = this.gameObject.GetComponent<Text> ();
		Language.AddLableLanguage (this);
	}

	void Start(){
		SetText ();
	}

//Hàm gán lại text cho label
	public virtual void SetText ()
	{
		if (current_label == null) {
			Debug.LogError ("Null " + this.gameObject.name);
		}
        else
        {
            string str = Language.GetKey(KEY);  
            current_label.text = str.Trim(); 
        }
	}
//Lúc chuyển scene sẽ remove hết list
	void OnDestroy ()
	{
		Language.RemoveLaleLanguage (this);
	}
 
}

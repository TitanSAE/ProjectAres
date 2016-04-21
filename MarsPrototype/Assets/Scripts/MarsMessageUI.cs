using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MarsMessageUI : MonoBehaviour {

	private Text txtMessages;
	private MarsMessageManager messages;
    private Color colRegular = new Color(0, 0, 0, 255);
    private Color colTransparent = new Color(255, 255, 255, 0);

    void Start() {
		txtMessages = this.GetComponent<Text>();
		messages = GameObject.FindGameObjectWithTag("SceneManager").GetComponent<MarsMessageManager>();
	}

	void Update() {
		txtMessages.text = messages.iUnread.ToString();

        if(txtMessages.text == "0")
        {
            txtMessages.color = colTransparent;
        }
        else
        {
            txtMessages.color = colRegular;
        }
	}
}

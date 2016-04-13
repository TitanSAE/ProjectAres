using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MarsMessageUI : MonoBehaviour {

	private Text txtMessages;
	private MarsMessageManager messages;

	void Start() {
		txtMessages = this.GetComponent<Text>();
		messages = GameObject.FindGameObjectWithTag("SceneManager").GetComponent<MarsMessageManager>();
	}

	void Update() {
		txtMessages.text = messages.iUnread.ToString() + " unread";
	}
}

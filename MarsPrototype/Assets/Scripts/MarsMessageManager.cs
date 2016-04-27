using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class MarsMessageManager : MonoBehaviour {

	public List<MarsMessage> l_messages = new List<MarsMessage>();
    public int iCurrentMessage = 0;
    public bool bIsPanelOpen;

	public GameObject goMessageScreen;
	public Text txtHeader;
	public Text txtFooter;
	public Image imgAvatar;
	public Text txtBody;

	private MarsPlayerSettings mps;

    public int iUnread {
		get {
			if (l_messages.Count == 0) {
				return 0;
			}
			else {
				int i = 0;
				foreach (MarsMessage msg in l_messages) {
					if (!msg.bRead) {
						i++;
					}
				}

				return i;
			}
		}
	}

	void Start() {
		mps = GameObject.FindGameObjectWithTag("SceneManager").GetComponent<MarsPlayerSettings>();

		AddMessage("Main Objective", "Build all base structures", "Mineral", true);
		AddMessage("Misc Objective", "Gather minerals from rocks", "Mineral", true);
	}

	public void LoadMessageAvatar() {
		if (l_messages.Count != 0) {
			if (l_messages[iCurrentMessage].texAvatar != null) {
				imgAvatar.sprite = Sprite.Create(l_messages[iCurrentMessage].texAvatar, 
					new Rect(0, 0, l_messages[iCurrentMessage].texAvatar.width, 
						l_messages[iCurrentMessage].texAvatar.height), 
					imgAvatar.rectTransform.pivot);
			}
			else {
				imgAvatar.sprite = null;
				Debug.Log("There was no avatar for the current message. Intentional?");
			}
		}
		else {
			imgAvatar.sprite = null;
		}
	}

	void Update() {
    	if (Input.GetButtonDown("OpenMessages")) {
			bIsPanelOpen = !bIsPanelOpen;

			goMessageScreen.SetActive(bIsPanelOpen);

            if (bIsPanelOpen) {
				LoadMessageAvatar();

				if (l_messages.Count == 0) {
					txtHeader.text = "No messages";
					txtFooter.text = "0/0 (0 unread)";
					txtBody.text = "You have no messages.";
				}
            }
        }

		//Navigate
		if (bIsPanelOpen) {
			if (Input.GetButtonDown("NextMessage")) {
				iCurrentMessage = Mathf.Clamp(iCurrentMessage + 1, 0, l_messages.Count - 1);

				LoadMessageAvatar();
			}

			if (Input.GetButtonDown("PrevMessage")) {
				iCurrentMessage = Mathf.Clamp(iCurrentMessage - 1, 0, l_messages.Count - 1);

				LoadMessageAvatar();
			}
		}

		//Render
		if (bIsPanelOpen && l_messages.Count > 0) {
			l_messages[iCurrentMessage].bRead = true;

			txtHeader.text = l_messages[iCurrentMessage].sTitle;
			txtFooter.text = (iCurrentMessage + 1).ToString() + "/" + (l_messages.Count).ToString() + "(" + iUnread.ToString() + " unread)";
			txtBody.text = l_messages[iCurrentMessage].sBody;
			if (l_messages[iCurrentMessage].iDayReceived >= 0) {
				txtBody.text += "\n\nSent on Sol " + l_messages[iCurrentMessage].iDayReceived.ToString();
			}
		}
    }

	public void AddMessage(string sTitle, string sBody, string avatar = "", bool preread = false) {
		MarsMessage newMessage = new MarsMessage();
        newMessage.sTitle = sTitle;
        newMessage.sBody = sBody;
		if (avatar != "") {
			if (avatar == "sponsor") {
				newMessage.texAvatar = mps.texSponsorAvatar;
			}
			else {
				newMessage.LoadAvatar(avatar);
			}
		}
		newMessage.iDayReceived = GameObject.FindGameObjectWithTag("DayNight").GetComponent<DayNightCycle>().iDayCount;
		newMessage.bRead = preread;

        l_messages.Add(newMessage);
    }
}

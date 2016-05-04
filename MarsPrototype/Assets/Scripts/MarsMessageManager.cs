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

	[Header("Trigger Targets")]
	public MarsPlayer ply;

	[Header("Triggers")]
	public bool bFirstAdvice;
	public bool bFirstPackageFind;
	public bool bFirstLowEnergy;
	public bool bFirstMineral;
	public bool bFirstWater;
	public bool bFirstLowHealth;
	public bool bFirstMessage;
	public bool bFirstBuilt;
	public bool bFirstPackageCollect;

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

	private void PollMessages() {
		//Initial How-to-play help
		if (!bFirstAdvice && Time.timeSinceLevelLoad > 5) {
			bFirstAdvice = true;
			if (ply.IsJoystickConnected()) {
				AddMessage("Information", "Use the left stick to control the rover. " +
					"Press the left stick to toggle your torch. " +
					"Press A to toggle the map size. " +
					"Press B to open and close this message panel.", "Info");
			}
			else {
				AddMessage("Information", "Use the WASD keys to control the rover. " +
					"Press the F key to toggle your torch. " + 
					"Press the M key to toggle the map size. " +
					"Press the O key to open and close this message panel.", "Info");
			}
			bIsPanelOpen = true;
			goMessageScreen.SetActive(bIsPanelOpen);
			LoadMessageAvatar();
		}

		//Low energy
		if (!bFirstLowEnergy && ply.fEnergy < (ply.fMaxEnergy * 0.75f)) {
			bFirstLowEnergy = true;
			AddMessage("Information", "Your energy appears to be running out. Energy is used to move, especially climbing hills." +
				"It is recharged back at the recharge pad at home base. If it runs out, your rover will be stranded.", "Info");
		}

		//Low health
		if (!bFirstLowHealth && ply.fHealth < (ply.fMaxHealth * 0.75f)) {
			bFirstLowHealth = true;
			AddMessage("Information", "Your chassis integrity appears to be running out. As it becomes damaged, it will become harder to climb hills and go fast" +
				"It is repaired back at the recharge pad at home base. If it runs out, your rover will be stranded.", "Info");
		}
	}

	void Start() {
		mps = GameObject.FindGameObjectWithTag("SceneManager").GetComponent<MarsPlayerSettings>();
		ply = GameObject.FindGameObjectWithTag("Player").GetComponent<MarsPlayer>();


		AddMessage("Misc Objective", "Gather minerals from certain rocks to further production.", "Mineral", true);
		AddMessage("Misc Objective", "Gather water from excavation sites for future colonists.", "Water", true);
		AddMessage("Main Objective", "Build all base structures so that future astronauts can colonise Mars.", "Trophy", true);
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
		PollMessages();

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

	public void AddMessage(string sTitle, string sBody, string avatar = "Info", bool preread = false) {
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
		if (newMessage.iDayReceived == 0) {
			newMessage.iDayReceived = 1;
		}
		newMessage.bRead = preread;

		List<MarsMessage> bup = new List<MarsMessage>();
		bup.AddRange(l_messages);
		l_messages.Clear();
		l_messages.Add(newMessage);
		l_messages.AddRange(bup);
        //l_messages.Add(newMessage);
    }
}

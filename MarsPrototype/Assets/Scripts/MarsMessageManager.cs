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
	[SerializePrivateVariables]
	private bool bFirstAdvice;
	private bool bFirstMessage;

	private bool bFirstTimeTouchRepairPad;
	public bool bExternalFlagTouchRepairPad;

	private bool bFirstLowEnergy75;
	private bool bFirstLowEnergy35;
	private bool bFirstLowEnergy10;

	private bool bFirstMineral;
	public bool bExternalFlagFirstMineral;
	private bool bFirstWater;

	private bool bFirstLowHealth75;
	private bool bFirstLowHealth35;
	private bool bFirstLowHealth10;

	private bool bFirstBuilt;
	public bool bExternalFlagFirstPackageDelivery;
	private bool bFirstPackageDelivery;
	private bool bFirstPickupDelivery;
	public bool bExternalFlagFirstPickupDelivery;
	private bool bFirstAttemptToCarryTooMuch;
	public bool bExternalFlagAttemptToCarryTooMuch;

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

		//First package delivery
		if (!bFirstPackageDelivery && bExternalFlagFirstPackageDelivery) {
			bFirstPackageDelivery = true;
			AddMessage("Information", "We've sent a delivery of construction materials from Earth. It should be landing somewhere near you." +
				"All you need to do is collect it from the drop site, acquire some additional Mars minerals, then bring the package back to base." +
				"The beacon that highlights the location of the package will only last a limited time. Once you collect the package, you'll recieve further instructions.", "Earth");
		}

		//Low energy
		if (!bFirstLowEnergy75 && ply.fEnergy < (ply.fMaxEnergy * 0.75f)) {
			bFirstLowEnergy75 = true;
			AddMessage("Information", "Your energy appears to be running out. Energy is used to move, especially climbing hills." +
				"It is recharged back at the recharge pad at home base. If it runs out, your rover will be stranded.", "Info");
		}

		if (!bFirstLowEnergy35 && ply.fEnergy < (ply.fMaxEnergy * 0.35f)) {
			bFirstLowEnergy35 = true;
			AddMessage("Information", "Your energy is nearly depleted. You should head to base and recharge.", "Info");
		}

		if (!bFirstLowEnergy10 && ply.fEnergy < (ply.fMaxEnergy * 0.10f)) {
			bFirstLowEnergy10 = true;
			AddMessage("Information", "Your energy levels are critical. Return to base and recharge immediately!", "Caution");
		}

		//Low health
		if (!bFirstLowHealth75 && ply.fHealth < (ply.fMaxHealth * 0.75f)) {
			bFirstLowHealth75 = true;
			AddMessage("Information", "Your chassis integrity appears to be running out. As it becomes damaged, it will become harder to climb hills and go fast" +
				"It is repaired back at the recharge pad at home base. If it runs out, your rover will be stranded.", "Info");
		}

		if (!bFirstLowHealth35 && ply.fHealth < (ply.fMaxHealth * 0.35f)) {
			bFirstLowHealth35 = true;
			AddMessage("Information", "Your chassis appears to be in bad shape. You should consider returning to base for repairs.", "Info");
		}

		if (!bFirstLowHealth10 && ply.fHealth < (ply.fMaxHealth * 0.10f)) {
			bFirstLowHealth10 = true;
			AddMessage("Information", "Your chassis is badly damaged. You need to return to base and repair!", "Caution");
		}

		//Attempt to carry two packages
		if (bExternalFlagAttemptToCarryTooMuch && !bFirstAttemptToCarryTooMuch) {
			bFirstAttemptToCarryTooMuch = true;
			AddMessage("Error", "You can only carry one Earth delivery at a time!", "Caution");
		}

		//Run in with first mineral
		if (bExternalFlagFirstMineral && !bFirstMineral) {
			bFirstMineral = true;
			if (ply.IsJoystickConnected()) {
				AddMessage("Information", "Press the A button to harvest the minerals from this rock. This process takes 1 Sol.", "Info");
			}
			else {
				AddMessage("Information", "Press C to harvest the minerals from this rock. This process takes 1 Sol.", "Info");
			}
		}

		//Pickup first package
		if (bExternalFlagFirstPickupDelivery && !bFirstPickupDelivery) {
			bFirstPickupDelivery = true;
			AddMessage("Information", "This Earth delivery of important building components can be taken back to base and used to build a new base struture." +
				"However, you will also need at least 1 harvested Mars mineral to complete the construction." +
				"Look for rock symbols on your minimap to guide you to them.", "Earth");
		}

		//Help message for repair pad
		if (bExternalFlagTouchRepairPad && !bFirstTimeTouchRepairPad) {
			bFirstTimeTouchRepairPad = true;
			if (ply.IsJoystickConnected()) {
				AddMessage("Information", "Press the A button to repair while on the repair pad. This process takes 1 Sol.", "Info");
			}
			else {
				AddMessage("Information", "Press C to repair while on the repair pad. This process takes 1 Sol.", "Info");
			}
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

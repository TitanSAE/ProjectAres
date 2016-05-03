using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIButtonSwitcher : MonoBehaviour {

	private Image imgXbox;
	private GameObject goKey;

	void Start () {
		imgXbox = this.gameObject.GetComponent<Image>();
		goKey = this.transform.GetChild(0).gameObject;
	}

	void Update () {
		if (IsJoystickConnected()) {
			imgXbox.enabled = true;
			goKey.SetActive(false);
		}
		else {
			imgXbox.enabled = false;
			goKey.SetActive(true);
		}
	}

	//Find the first non-virtual (fake) joystick/gamepad
	public bool IsJoystickConnected() {
		bool joy = false;

		if (Input.GetJoystickNames().Length != 0) {
			for (int i = 0; i < Input.GetJoystickNames().Length; i++) {
				if (!Input.GetJoystickNames()[i].ToUpper().Contains("VIRTUAL") && Input.GetJoystickNames()[i].Length > 3) {
					joy = true;
				}
			}
		}

		return joy;
	}
}

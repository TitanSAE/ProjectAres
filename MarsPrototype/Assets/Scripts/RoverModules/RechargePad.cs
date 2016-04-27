using UnityEngine;
using System.Collections;

public class RechargePad : MonoBehaviour {
	
	public bool onPad = false;
	private DayNightCycle daynight;

	void Start() {
		daynight = GameObject.FindGameObjectWithTag("DayNight").GetComponent<DayNightCycle>();
	}

	void Update() {
		if (Input.GetButton("ConfirmRepair") && onPad) {
			daynight.SkipDay();
			GameObject.Find ("Player").GetComponent<MarsPlayer> ().Repair ();
		}
	}

	void OnTriggerEnter(Collider col) {
		if (col.gameObject.tag == "Player") {
			onPad = true;
		}
	}

	void OnTriggerExit(Collider col) {
		if (col.gameObject.tag == "Player") {
			onPad = false;
		}
	}
}

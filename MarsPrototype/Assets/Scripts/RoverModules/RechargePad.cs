using UnityEngine;
using System.Collections;

public class RechargePad : MonoBehaviour {
	bool onPad = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButton("ConfirmRepair") && onPad) {
			GameObject.Find ("SceneManager").GetComponent<Fading> ().ResetFade ();
			GameObject.Find ("SceneManager").GetComponent<Fading> ().BeginFade (-1);
			Debug.Log ("hit");
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

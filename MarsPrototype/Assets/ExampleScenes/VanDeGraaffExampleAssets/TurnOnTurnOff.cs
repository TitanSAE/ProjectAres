using UnityEngine;
using System.Collections;

public class TurnOnTurnOff : MonoBehaviour {
	public GameObject charge;
	public Light glow;

//	StaticChargeScript chargeScript;
	public int target = 0;
	public float current = 0.0f;
	public GameObject Arm;
	Animation anim;
	void Start() {
//		chargeScript = charge.GetComponent<StaticChargeScript>();
		anim = Arm.GetComponent<Animation> ();
	}

	void OnMouseUp() {
		
		if (target == 0) {
			target = 1;

			anim.Play ("Arm");
			Debug.Log ("Selected");
			GameObject.Find ("House").GetComponent<HouseMagnet> ().OnPad = false;
		}
		else {
			target = 0;
			anim.Play ("CloseArm");

		}
	}

	void FixedUpdate() {



//		if (target == 0) {
//			current *= 0.9f;
//
//		}
//		else {
//			current = 1.0f - 0.99f * (1.0f - current);
//		}
//
//		chargeScript.strength = 1e-4f * current;
//		glow.intensity = 8.0f * current;
	}
}

using UnityEngine;
using System.Collections;

public class Magnet : MonoBehaviour{// TurnOnTurnOff {

//	bool Pull = false;
//	StaticChargeScript chargeScript;
//	// Use this for initialization
//	void Start () {
//		chargeScript = charge.GetComponent<StaticChargeScript>();
//	}
//	
//	// Update is called once per frame
//
//
//
//	void Update(){
//		
//			if (Input.GetMouseButtonDown (0)) { 
//				RaycastHit hit; 
//				Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition); 
//				if (Physics.Raycast (ray, out hit, 100.0f)) {
//					Debug.Log ("You selected the " + hit.transform.name); // ensure you picked right object
//
//					if (hit.transform.name == "Trailor") {
//
//					if (target == 0) {
//						target = 1;
//
//					} else {
//						target = 0;
//					}
//						
//
//
//					}
//				}
//			}
//
//
//				if (target == 0) {
//					current *= 0.9f;
//		
//				}
//				else {
//					current = 1.0f - 0.99f * (1.0f - current);
//				}
//		
//				chargeScript.strength = 1e-4f * current;
//				glow.intensity = 8.0f * current;
//
//	}
}

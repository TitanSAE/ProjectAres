using UnityEngine;
using System.Collections;

public class Propellor : MonoBehaviour {
	int Speed;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if (GameObject.Find ("GameManager").GetComponent<KaceyGameManager> ().copterControl == true) {
			if (HelicopterMovement.HaveFuel == true) {
				if (Input.GetKey (KeyCode.Space) || Input.GetAxis ("Vertical") > 0 || Input.GetAxis ("Vertical") < 0) { //up
					Speed += 40;
					transform.Rotate (Vector3.up * Time.deltaTime * Speed);
				} else {
					Speed -= 70;
					if (Speed > 0) {
						transform.Rotate (Vector3.up * Time.deltaTime * Speed);
					} else if (Speed < 0) {
						Speed = 0;
						transform.Rotate (Vector3.zero);
					}
				}

			}
		}
	}
}

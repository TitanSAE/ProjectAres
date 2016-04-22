using UnityEngine;
using System.Collections;

public class MarsScoutDroneLandingCheck : MonoBehaviour {

	public MarsScoutDrone drone;

	void Start () {
		//drone = this.GetComponentInChildren<MarsScoutDrone>();
	}

	void Update () {
	
	}

	void OnCollisionEnter(Collision c) {
		if (c.gameObject.tag == "LandingPad" && !drone.bTakeoff) {
			drone.AttachToRover(c.gameObject);
		}
	}
}

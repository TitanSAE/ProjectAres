using UnityEngine;
using System.Collections;

public class OculusMoveBack : MonoBehaviour {

	// Use this for initialization
	void Start () {
		if (UnityEngine.VR.VRDevice.isPresent) {
			this.transform.position = new Vector3(0, 0, -9.5f);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

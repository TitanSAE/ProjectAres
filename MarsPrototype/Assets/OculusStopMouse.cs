using UnityEngine;
using System.Collections;

public class OculusStopMouse : MonoBehaviour {

	// Use this for initialization
	void Start () {
		if (UnityEngine.VR.VRDevice.isPresent) {
			this.GetComponent<UnityEngine.UI.Image>().color = new Color(0, 0, 0, 0);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

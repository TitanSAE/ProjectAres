using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public bool roverControl = true;
	public bool copterControl = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKey (KeyCode.O)) {
			roverControl = false;
			copterControl = true;
			Debug.Log ("Rover Controlled");
		
		}

		if (Input.GetKey (KeyCode.P)) {
			roverControl = true;
			copterControl = false;
			Debug.Log ("Copter Controlled");
		}
	
	}
}

using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public bool roverControl = true;
	public bool copterControl = false;
	public GameObject Tornado;
	bool enabled = false;
	public GameObject module;
	public GameObject Establishment;
	public GameObject Car;

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

		if (Input.GetKey (KeyCode.T)) {
			
			enabled = !enabled;
			Tornado.SetActive (enabled);
		}

		if (Input.GetKeyDown (KeyCode.U)) {
			Vector3 radius = new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10));
			Instantiate (module, Random.insideUnitSphere * 20f + transform.position, Random.rotation);
		}
		if (Input.GetKeyDown (KeyCode.Space)) {
			GameObject.Find ("GameManager").GetComponent<Fading> ().ResetFade ();
			GameObject.Find ("GameManager").GetComponent<Fading> ().BeginFade (-1);
			Instantiate(Establishment, Car.transform.position + (Car.transform.forward*1) + new Vector3(0,1.3f,0), transform.rotation);
		}
	
	}
}

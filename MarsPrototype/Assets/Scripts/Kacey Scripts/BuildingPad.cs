using UnityEngine;
using System.Collections;

public class BuildingPad : MonoBehaviour {
	
	public GameObject preBuilt;
	//public GameObject postBuilt;
	bool bBuilt = false;

	void Start() {
	
	}
	

	void Update() {
		try{
			float dist = Vector3.Distance (transform.position,	GameObject.Find("Module(Clone)").transform.position);
		if (dist < 2 && built == false) {
				Destroy(GameObject.Find("Module(Clone)").gameObject);
				GameObject.Find("Player").gameObject.GetComponent<RoverPlayerTowbar>().bTowing = false;
			GameObject.Find ("GameManager").GetComponent<Fading> ().ResetFade ();
			GameObject.Find ("GameManager").GetComponent<Fading> ().BeginFade (-1);
			preBuilt.gameObject.SetActive (false);
			postBuilt.gameObject.SetActive (true);
			built = true;
			GameObject.Find("Player").transform.position = transform.position + (transform.forward * 8);
			Destroy (GameObject.Find ("Module").gameObject);
		}
		}catch{

		}


			
	}

	void OnTriggerEnter(Collision col) {
		if (col.gameObject.tag == "Module") {
			preBuilt.gameObject.SetActive (false);
			postBuilt.gameObject.SetActive (true);
			Debug.Log ("Collided");
		}
	}
}

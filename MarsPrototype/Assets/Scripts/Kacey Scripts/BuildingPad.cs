using UnityEngine;
using System.Collections;

public class BuildingPad : MonoBehaviour {
	public GameObject preBuilt;
	public GameObject postBuilt;
	bool built = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		float dist = Vector3.Distance (transform.position,	GameObject.Find("Module").transform.position);
		if (dist < 2 && built == false) {
			GameObject.Find ("GameManager").GetComponent<Fading> ().ResetFade ();
			GameObject.Find ("GameManager").GetComponent<Fading> ().BeginFade (-1);
			preBuilt.gameObject.SetActive (false);
			postBuilt.gameObject.SetActive (true);
			built = true;
			GameObject.Find("Player").transform.position = transform.position + (transform.forward * 8);
			Destroy (GameObject.Find ("Module").gameObject);
		}



			
	}

	void OnTriggerEnter(Collision col){
		if (col.gameObject.tag == "Module") {
			preBuilt.gameObject.SetActive (false);
			postBuilt.gameObject.SetActive (true);
			Debug.Log ("Collided");
		}
	}
}

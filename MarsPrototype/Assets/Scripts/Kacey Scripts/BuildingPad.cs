using UnityEngine;
using System.Collections;

public class BuildingPad : MonoBehaviour {
	
	public MarsBuilding building;
	public BoxCollider buildcollider;
	bool bBuilt = false;

	void Start() {
	
	}
	

	void Update() {
//		try{
//			float dist = Vector3.Distance (transform.position,	GameObject.Find("Module(Clone)").transform.position);
//		if (dist < 2 && built == false) {
//				Destroy(GameObject.Find("Module(Clone)").gameObject);
//				GameObject.Find("Player").gameObject.GetComponent<RoverPlayerTowbar>().bTowing = false;
//			GameObject.Find ("GameManager").GetComponent<Fading> ().ResetFade ();
//			GameObject.Find ("GameManager").GetComponent<Fading> ().BeginFade (-1);
//			preBuilt.gameObject.SetActive (false);
//			postBuilt.gameObject.SetActive (true);
//			built = true;
//			GameObject.Find("Player").transform.position = transform.position + (transform.forward * 8);
//			Destroy (GameObject.Find ("Module").gameObject);
//		}
//		}catch{
//
//		}


			
	}

	void OnCollisionEnter(Collision c) {
		if (c.gameObject.tag == "MarsDelivery") {


			Debug.Log ("Built c!");
		}
	}

	void OnTriggerEnter(Collider col) {
		if (col.gameObject.tag == "MarsDelivery" && !bBuilt) {

			bBuilt = true;

			if (building.bGhosted && !building.bLerping) {
				building.bLerping = true;
			}

			buildcollider.isTrigger = false;
			MarsDelivery del = col.gameObject.GetComponent<MarsDelivery> ();
			del.bAttachedToPlayer = false;
			del.bDelivered = true;
			del.goPlayerTowbar.GetComponent<RoverPlayerTowbar>().bTowing = false;

			Debug.Log ("Built!");
			GameObject.Destroy (col.gameObject);
		}
	}
}

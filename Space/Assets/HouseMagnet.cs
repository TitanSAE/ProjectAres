using UnityEngine;
using System.Collections;

public class HouseMagnet : MonoBehaviour {



	Transform padPos;
	public GameObject landPad;

	public bool OnPad = false;

	// Update is called once per frame
	private void Update()
	{
		if (OnPad) {
			transform.position = new Vector3(padPos.position.x,transform.position.y,padPos.position.z);
			transform.GetComponent<Rigidbody> ().freezeRotation = true;

		}
		float dist = Vector3.Distance (transform.position, landPad.transform.position);
		if (dist > 1) {
			OnPad = false;
			transform.SetParent (null);
			LandingPad landingPad;
			landingPad = landPad.gameObject.GetComponent ("LandingPad") as LandingPad;
			landingPad.DeActivate ();
		}

	}


	private void OnCollisionEnter(Collision hitInfo)
	{
		if (hitInfo.gameObject.name == "Trailor") {
			Debug.Log ("Landed");
			LandingPad landingPad;
			landingPad = hitInfo.gameObject.GetComponent ("LandingPad") as LandingPad;
			landingPad.Activate ();
			padPos = hitInfo.transform;
			transform.SetParent (landPad.transform.parent);
			OnPad = true;
		}


	}



}

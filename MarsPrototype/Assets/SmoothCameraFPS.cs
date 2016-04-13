using UnityEngine;
using System.Collections;

public class SmoothCameraFPS : MonoBehaviour {

	public Transform tTarget;
	public float fSmoothTime = 0.3f;
	public Vector3 vVel;

	void Start() {
	
	}

	void Update() {
		
	}

	void LateUpdate() {
		Vector3 newPos = new Vector3();

		newPos.x = Mathf.SmoothDamp(this.transform.position.x, tTarget.position.x, ref vVel.x, fSmoothTime);
		newPos.y = Mathf.SmoothDamp(this.transform.position.y, tTarget.position.y, ref vVel.y, fSmoothTime);
		newPos.z = Mathf.SmoothDamp(this.transform.position.z, tTarget.position.z, ref vVel.z, fSmoothTime);

		transform.position = Vector3.Slerp(transform.position, newPos, Time.time);
	}
}

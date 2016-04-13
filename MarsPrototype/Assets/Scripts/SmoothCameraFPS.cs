using UnityEngine;
using UnityEngine.VR;
using System.Collections;

public class SmoothCameraFPS : MonoBehaviour {

	public Transform tPosTarget;
	public Transform tRotTarget;
	public float fSmoothTime = 0.3f;
	public Vector3 vVel;

	public Vector3 vNewPos;
	public Vector3 vPrevPos;
	public Vector3 vDif;

	void Start() {
		vNewPos = tPosTarget.position;
	}

	void Update() {
		if (!VRDevice.isPresent) {
			this.transform.rotation = tRotTarget.transform.rotation;
		}
	}

	void LateUpdate() {
		vDif = vNewPos - vPrevPos;

		if (vDif.magnitude < 5) {
			vNewPos = transform.position = Vector3.Slerp(vNewPos, vPrevPos, Time.time);
		}
		else {
			vNewPos = tPosTarget.position;
		}

//		newPos.x = Mathf.SmoothDamp(this.transform.position.x, tPosTarget.position.x, ref vVel.x, fSmoothTime);
//		newPos.y = Mathf.SmoothDamp(this.transform.position.y, tPosTarget.position.y, ref vVel.y, fSmoothTime);
//		newPos.z = Mathf.SmoothDamp(this.transform.position.z, tPosTarget.position.z, ref vVel.z, fSmoothTime);

		//transform.position = Vector3.Slerp(transform.position, newPos, Time.time);

		vPrevPos = tPosTarget.position;

		transform.position = vNewPos;
	}
}

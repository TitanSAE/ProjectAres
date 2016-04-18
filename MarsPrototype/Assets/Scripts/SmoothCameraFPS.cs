using UnityEngine;
using UnityEngine.VR;
using System.Collections;

public class SmoothCameraFPS : MonoBehaviour {

	public Transform tPosTarget;
	//public Transform tRotTarget;
	public float fSmoothTime = 0.3f;
	//public Vector3 vVel;

	public Vector3 vNewPos;
	public Vector3 vPrevPos;
	//public Vector3 vDif;

	public float fHeightOffset = 5.0f;
	public float fDistanceOffset = 10.0f;
	public float fSwingOffset = 0.0f;

	void Start() {
		//vNewPos = tPosTarget.position;
	}

	void Update() {
		//
	}

	void LateUpdate() {
		//float smoothProg = Mathf.SmoothStep(0, 1, prog);
		//transform.position = Vector3.Lerp(vOriginPos, tGoalTrans.position, smoothProg);

		Vector3.Lerp(vPrevPos, vNewPos, 0.5f);
		vNewPos = tPosTarget.position + new Vector3(fSwingOffset, fHeightOffset, -fDistanceOffset);

		transform.position = vNewPos;

//		vDif = vNewPos - vPrevPos;
//
//		if (vDif.magnitude < 5) {
//			vNewPos = transform.position = Vector3.Slerp(vNewPos, vPrevPos, Time.time);
//		}
//		else {
//			vNewPos = tPosTarget.position;
//		}
//
////		newPos.x = Mathf.SmoothDamp(this.transform.position.x, tPosTarget.position.x, ref vVel.x, fSmoothTime);
////		newPos.y = Mathf.SmoothDamp(this.transform.position.y, tPosTarget.position.y, ref vVel.y, fSmoothTime);
////		newPos.z = Mathf.SmoothDamp(this.transform.position.z, tPosTarget.position.z, ref vVel.z, fSmoothTime);
//
//		//transform.position = Vector3.Slerp(transform.position, newPos, Time.time);
//
//		vPrevPos = tPosTarget.position;
//
//		transform.position = vNewPos;
	}
}

using UnityEngine;
using System.Collections;

public class MarsMovement : MonoBehaviour {

	//public Rigidbody tSkiFrontLeft;
	//public Rigidbody tSkiFrontRight;
	//public Rigidbody tSkiBackLeft;
	//public Rigidbody tSkiBackRight;

	public Transform tCenterOfGravity;

	public float fTurnSpeed = 1.0f;
	//public float fThrustSpeed = 100.0f;

	public float fInitalAcceleration = 1.5f;

	public float fAcceleration = 0.0f;
	public float fAccelerationGrowth = 1.1f;
	public float fMaxAcceleration = 1000.0f;

	private Rigidbody rbBody;

	public float fVel;

	void Start() {
		rbBody = this.gameObject.GetComponent<Rigidbody>();
	}

	void LateUpdate() {
		//No gamepad, use keys
		if (!IsJoystickConnected()) {
			if (Input.GetAxis("Vertical") > 0) {
				rbBody.AddForce(tCenterOfGravity.forward * 1000);
				//rbBody.AddForce(tCenterOfGravity.forward * 100, ForceMode.Acceleration);
			}

			if (Input.GetAxis("Vertical") < 0) {
				rbBody.AddForce(tCenterOfGravity.forward * -100, ForceMode.Force);
			}

//			if (Input.GetAxis("Vertical") != 0) {
//				
//				//Forward
//				if (Input.GetAxis("Vertical") > 0) {
//					if (fAcceleration == 0) {
//						fAcceleration += fInitalAcceleration;
//					}
//
//					if (fAcceleration < fMaxAcceleration) {
//						fAcceleration *= fAccelerationGrowth;
//					}
//				}
//
//				//Backward
////				if (Input.GetAxis("Vertical") < 0) {
////					if (fAcceleration > -(fMaxAcceleration / 2)) {
////						fAcceleration -= 0.1f;
////						fAcceleration *= fAccelerationGrowth;
////					}
////				}
//
//				Mathf.Clamp(fAcceleration, -fMaxAcceleration, fMaxAcceleration);
//
//				if (Mathf.Abs(fAcceleration) > 1.0f) {
//					rbBody.AddForceAtPosition(rbBody.transform.forward * fAcceleration, tCenterOfGravity.position);
//				}
//			}
//			else {
//				if (fAcceleration > 1.0f) {
//					fAcceleration -= fAcceleration / 2;
//				}
//
//				if (fAcceleration < -1.0f) {
//					fAcceleration += fAcceleration / 2;
//				}
//
//				if (fAcceleration > -1.0f && fAcceleration < 1.0f) {
//					fAcceleration = 0;
//				}
//			}
		}
		else {
			for (int i = 0; i < Input.GetJoystickNames().Length; i++) {
				Debug.Log(Input.GetJoystickNames()[i]);
			}
		}

		if (Input.GetButton("Brake")) {
			Debug.Log("BRAKE");
			if (fAcceleration > 0.5f || fAcceleration < -0.5f) {
				fAcceleration *= 0.8f;
			}
			else {
				fAcceleration = 0;
			}
		}

		//fVel = rbBody.velocity.magnitude;
	}

	//Find the first non-virtual (fake) joystick/gamepad
	public bool IsJoystickConnected() {
		bool joy = false;

		if (Input.GetJoystickNames().Length != 0) {
			for (int i = 0; i < Input.GetJoystickNames().Length; i++) {
				if (!Input.GetJoystickNames()[i].ToUpper().Contains("VIRTUAL") && Input.GetJoystickNames()[i].Length > 3) {
					joy = true;
				}
			}
		}

		return joy;
	}
}

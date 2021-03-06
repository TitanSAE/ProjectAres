﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.VR;

public class RoverPlayerController : MonoBehaviour {

	public Vector3 vFinalMove;
	public Rigidbody body;

	public float fForwardMoveSpeed = 10;
	public float fSideMoveSpeed = 10;
	public float fReverseMoveSpeed = 5;
	public float fMaxSpeed = 10;
	public float fTurnSpeed = 10;
	public float fMaxTurnSpeed = 10;

	float fForewardPush = 0;
	float fTurning = 0;

	public bool bJetting = false;
	public bool bIdle = false;

	public Camera MouseCamera;
	public GameObject OVRCameraRig;
	public GameObject goTowbar;

	public bool bCopterActive;
	public GameObject goCopterStatic;
	public GameObject goCopterLive;

	// Use this for initialization
	void Start () {
		//Fallback for no Rift
		if (!VRDevice.isPresent) {
			Debug.LogWarning("Can't find Oculus. Activating mouse controls...");
			OVRCameraRig.SetActive(false);
			MouseCamera.gameObject.SetActive(true);
		}
	}
	
	// Update is called once per frame
	void Update () {
		vFinalMove = Vector3.zero;
		fForewardPush = 0;
		fTurning = 0;

		if (!bCopterActive) {
			if (Input.GetAxis("Vertical") > 0) {
				fForewardPush += fForwardMoveSpeed * Input.GetAxis("Vertical");
			}
			else if (Input.GetAxis("Vertical") < 0) {
				fForewardPush += fReverseMoveSpeed * Input.GetAxis("Vertical");
			}

			if (Input.GetAxis("XboxRightStickX") != 0) {
				fTurning += fTurnSpeed * Input.GetAxis("XboxRightStickX");
			}

			if (Input.GetKey(KeyCode.D)) {
				fTurning += fTurnSpeed;
			}
			else if (Input.GetKey(KeyCode.A)) {
				fTurning -= fTurnSpeed;
			}

			if (Input.GetAxis("Vertical") != 0 && Input.GetAxis("XboxRightStickX") != 0) {
				bIdle = false;
			}
			else {
				bIdle = true;
			}
		}

		if (Input.GetKeyDown(KeyCode.P)) {
			bCopterActive = !bCopterActive;

			if (bCopterActive) {
				goCopterLive.transform.position = goCopterStatic.transform.position;
				goCopterLive.transform.rotation = goCopterStatic.transform.rotation;

				OVRCameraRig.SetActive(false);
				goCopterStatic.SetActive(false);
				goCopterLive.SetActive(true);

				if (!VRDevice.isPresent) {
					MouseCamera.gameObject.SetActive(false);
				}
			}
			else {
				OVRCameraRig.SetActive(true);
				goCopterStatic.SetActive(true);
				goCopterLive.SetActive(false);

				if (!VRDevice.isPresent) {
					MouseCamera.gameObject.SetActive(true);
				}
			}
		}
	}

	void LateUpdate() {
		if (body.angularVelocity.magnitude < fMaxTurnSpeed) {
			body.AddRelativeTorque(new Vector3(0, fTurning, 0));
		}

		vFinalMove = body.transform.forward * fForewardPush;
		if (body.velocity.magnitude < fMaxSpeed) {
			body.AddForce(vFinalMove, ForceMode.Acceleration);
		}

		if (vFinalMove != Vector3.zero) {
			//Debug.Log(body.velocity.magnitude);
		}
	}
}

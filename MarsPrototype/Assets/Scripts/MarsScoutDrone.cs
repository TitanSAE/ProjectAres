﻿using UnityEngine;
using System.Collections;

public class MarsScoutDrone : MonoBehaviour {

	public float fMaxHeight = 40.0f;
	public float fTempMaxHeight;
	public float fVerticalThrustPower = 10.0f;
	public float fForwardThrustPower = 10.0f;
	public float fRotateSpeed = 0.1f;
	private Rigidbody rbBody;
	private bool bHolding;

	public GameObject goLocked;
	public GameObject goUnlocked;
	public UnityEngine.UI.Text txtHeightCap;

	private Vector3 vVel;

	void Start() {
		rbBody = this.GetComponent<Rigidbody>();
	}

	void Update() {
		if (Input.GetKeyDown(KeyCode.LeftShift)) {
			bHolding = !bHolding;

			if (bHolding) {
				fTempMaxHeight = Mathf.Ceil(this.transform.position.y);
			}
		}

		if (bHolding) {
			goLocked.SetActive(true);
			goUnlocked.SetActive(false);
			txtHeightCap.text = Mathf.Clamp(Mathf.RoundToInt(fTempMaxHeight - this.transform.position.y), 0, 999).ToString("D3") + "m";
		}
		else {
			goLocked.SetActive(false);
			goUnlocked.SetActive(true);
			txtHeightCap.text = Mathf.Clamp(Mathf.RoundToInt(fMaxHeight - this.transform.position.y), 0, 999).ToString("D3") + "m";
		}
	}

	void LateUpdate() {
		if (Input.GetAxis("Horizontal") != 0) {
			float sign = Mathf.Sign(Input.GetAxis("Horizontal"));

			rbBody.AddTorque(Vector3.up * sign * fRotateSpeed);
		}

		if (Input.GetAxis("Vertical") != 0) {
			float sign = Mathf.Sign(Input.GetAxis("Vertical"));

			rbBody.AddForce(this.transform.forward * fForwardThrustPower * sign, ForceMode.Acceleration);
		}

		if (Input.GetKey(KeyCode.Space)) {
			if (!bHolding) {
				if (this.transform.position.y < fMaxHeight) {
					rbBody.AddForce(Vector3.up * fVerticalThrustPower, ForceMode.Acceleration);
				}
				else {
					//float loss = fMaxHeight / this.transform.position.y;
					//float cur = Mathf.SmoothStep(fMaxHeight, this.transform.position.y, 0.6f);
					//rbBody.AddForce(Vector3.down * cur, ForceMode.Acceleration);
					this.transform.position = new Vector3(this.transform.position.x, fMaxHeight, this.transform.position.z);
				}
			}
			else {
				if (this.transform.position.y < fTempMaxHeight) {
					rbBody.AddForce(Vector3.up * fVerticalThrustPower, ForceMode.Acceleration);
				}
				else {
					this.transform.position = new Vector3(this.transform.position.x, fTempMaxHeight, this.transform.position.z);
				}
			}
		}
	}
}
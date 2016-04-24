﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MarsPlayer : MonoBehaviour {

	public float fHealth = 100;
	[SerializeField]
	private float fMaxHealth = 100;

	public float fEnergy = 100;
	[SerializeField]
	private float fMaxEnergy = 100;

	public Image imgHealthBar;
	public Image imgEnergyBar;
	public Text txtHealth;
	public Text txtEnergy;

	public bool bFullScreenMap = false;

	public Camera camSky;
	public Transform tMap;
	public Transform tMaxMap;

	private float fEnergyDrainTimer;
	public float fEnergyDrainSeconds = 10;

	public MarsTowbar towbar;

	public bool bControllingScout;
	public GameObject goRoverCam;
	public GameObject goScoutDrone;

//	public OVRCameraRig camrig;
//	public OVRCameraRig camrig_drone;
	public bool bLockTracking;

	void Start() {
		towbar = gameObject.GetComponent<MarsTowbar>();
//		camrig.UpdatedAnchors += ResetEyes;
//		camrig_drone.UpdatedAnchors += ResetEyes;
	}

	public void SetMaxHealth(float health) {
		fMaxHealth = health;
		fHealth = fMaxHealth;
	}

	public void SetMaxEnergy(float energy) {
		fMaxEnergy = energy;
		fEnergy = fMaxEnergy;
	}

//	void ResetEyes(Object obj) {
//		if (bLockTracking) {
//			camrig.trackingSpace.FromOVRPose(camrig.centerEyeAnchor.ToOVRPose(true).Inverse());
//			camrig_drone.trackingSpace.FromOVRPose(camrig_drone.centerEyeAnchor.ToOVRPose(true).Inverse());
//		}
//	}

	void Update() {
		//Bars
		imgHealthBar.fillAmount = fHealth / fMaxHealth;
		imgEnergyBar.fillAmount = fEnergy / fMaxEnergy;

		txtHealth.text = ((int)(imgHealthBar.fillAmount * 100)).ToString() + "%";
		txtEnergy.text = ((int)(imgEnergyBar.fillAmount * 100)).ToString() + "%";

		if (Input.GetButtonDown("ToggleCopter")) {
			bControllingScout = !bControllingScout;

			if (bControllingScout) {
				this.GetComponent<RoverController>().enabled = false;
				goRoverCam.SetActive(false);
				goScoutDrone.SetActive(true);
				//bLockTracking = true;
				//goRoverCam.transform.localPosition = Vector3.zero;

			}

			if (!bControllingScout) {
				this.GetComponent<RoverController>().enabled = true;
				goRoverCam.SetActive(true);
				goScoutDrone.SetActive(false);
				//bLockTracking = false;
			}
		}
		if (Input.GetAxis("Vertical") != 0) {
			if (fEnergy > 0) {
				fEnergyDrainTimer += Time.deltaTime;
			}

			if (fEnergyDrainTimer >= fEnergyDrainSeconds) {
				fEnergyDrainTimer = 0;
				fEnergy -= 1;
			}

			if (fHealth <= 0) {
				GameObject.Destroy (this.gameObject);
			}
		}

		if (bControllingScout) {
			//Energy

		}
		else {
			//Map
			if (Input.GetButtonDown("ToggleMapSize")) {
				bFullScreenMap = !bFullScreenMap;
			}

			if (bFullScreenMap) {
				camSky.fieldOfView = 90;
				tMap.gameObject.SetActive(false);
				tMaxMap.gameObject.SetActive(true);
			}
			else {
				camSky.fieldOfView = 30;
				tMap.gameObject.SetActive(true);
				tMaxMap.gameObject.SetActive(false);
			}
		}
	}


}

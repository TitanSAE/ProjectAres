using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MarsPlayer : MonoBehaviour {

	public float fHealth = 100;
	public float fMaxHealth = 100;

	public float fEnergy = 100;
	public float fMaxEnergy = 100;

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
	public GameObject goClock;

	public Canvas cnvPlyMarker;

	public int iRocksCarried;
	public Text txtRocks;

	public bool bCopterEnabled;

	public Light ligTorch;
	public Light ligNotify;

	public SkyCamera data_skycam;

//	public OVRCameraRig camrig;
//	public OVRCameraRig camrig_drone;
	//public bool bLockTracking;

	public MarsMessageManager mngMessages;

	public void Repair(){
		fEnergy = fMaxEnergy;
		fHealth = fMaxHealth;
	}

	void Start() {
		towbar = gameObject.GetComponent<MarsTowbar>();
//		camrig.UpdatedAnchors += ResetEyes;
//		camrig_drone.UpdatedAnchors += ResetEyes;

		if (!bCopterEnabled) {
			goScoutDrone.transform.parent.gameObject.SetActive(false);
		}
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
		if (Input.GetButtonDown("ToggleTorch")) {
			ligTorch.gameObject.SetActive(!ligTorch.gameObject.activeInHierarchy);
		}

		if (ligTorch.gameObject.activeInHierarchy) {
			fEnergyDrainTimer += Time.deltaTime;
		}

		if (mngMessages.iUnread > 0) {
			//
		}

		//Rocks
		txtRocks.text = "Minerals: " + iRocksCarried.ToString();

		//Bars
		imgHealthBar.fillAmount = fHealth / fMaxHealth;
		imgEnergyBar.fillAmount = fEnergy / fMaxEnergy;

		txtHealth.text = ((int)(imgHealthBar.fillAmount * 100)).ToString() + "%";
		txtEnergy.text = ((int)(imgEnergyBar.fillAmount * 100)).ToString() + "%";

		//Copter
		if (Input.GetButtonDown("ToggleCopter") && goScoutDrone.transform.parent.gameObject.activeInHierarchy) {
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
				if (fEnergy > 0) {
					fEnergy--;
				}
				else {
					fHealth -= 5;
				}
			}

			if (fHealth <= 0) {
				GameObject.Destroy (this.gameObject);
				//temp loss
				UnityEngine.SceneManagement.SceneManager.LoadScene("loss");
			}
		}

		if (bControllingScout) {
			//Energy

		}
		else {
			//Map
			if (Input.GetButtonDown("ToggleMapSize") && !mngMessages.bIsPanelOpen) {
				bFullScreenMap = !bFullScreenMap;
			}

			if (mngMessages.bIsPanelOpen) {
				bFullScreenMap = false;
			}

			if (bFullScreenMap) {
				camSky.fieldOfView = 55;
				tMap.gameObject.SetActive(false);
				tMaxMap.gameObject.SetActive(true);
				txtRocks.gameObject.SetActive(false);
				goClock.SetActive(false);
				mngMessages.bIsPanelOpen = false;
				data_skycam.fHoverHeight = 896.0f;
				data_skycam.bSpinToFace = false;
				data_skycam.bLockToCentre = true;
				//cnvPlyMarker.gameObject.transform.localScale = new Vector3(0.1f, 0.0425f, 0);
			}
			else {
				camSky.fieldOfView = 30;
				tMap.gameObject.SetActive(true);
				tMaxMap.gameObject.SetActive(false);
				txtRocks.gameObject.SetActive(true);
				goClock.SetActive(true);
				data_skycam.fHoverHeight = 72.1f;
				data_skycam.bSpinToFace = true;
				data_skycam.bLockToCentre = false;
				//cnvPlyMarker.gameObject.transform.localScale = new Vector3(0.023f, 0.0094f, 0);
			}
		}
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

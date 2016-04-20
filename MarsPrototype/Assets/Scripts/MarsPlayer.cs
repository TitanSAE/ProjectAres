using UnityEngine;
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

	void Start() {
		towbar = gameObject.GetComponent<MarsTowbar>();
	}

	void Update() {
		//Bars
		imgHealthBar.fillAmount = fHealth / fMaxHealth;
		imgEnergyBar.fillAmount = fEnergy / fMaxEnergy;

		txtHealth.text = ((int)(imgHealthBar.fillAmount * 100)).ToString() + "%";
		txtEnergy.text = ((int)(imgEnergyBar.fillAmount * 100)).ToString() + "%";

		if (Input.GetKeyDown(KeyCode.Alpha1)) {
			bControllingScout = !bControllingScout;

			if (bControllingScout) {
				this.GetComponent<MarsMovement>().enabled = false;
				goRoverCam.SetActive(false);
				goScoutDrone.SetActive(true);
			}

			if (!bControllingScout) {
				this.GetComponent<MarsMovement>().enabled = true;
				goRoverCam.SetActive(true);
				goScoutDrone.SetActive(false);
			}
		}

		if (bControllingScout) {
			//Energy
			if (fEnergy > 0) {
				fEnergyDrainTimer += Time.deltaTime;
			}

			if (fEnergyDrainTimer >= fEnergyDrainSeconds) {
				fEnergyDrainTimer = 0;
				fEnergy -= 1;
			}

			if (fHealth <= 0) {
				GameObject.Destroy(this.gameObject);
			}
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

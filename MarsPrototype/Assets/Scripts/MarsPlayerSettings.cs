using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Xml;
using System.Xml.Linq;
using System.Linq;

public class MarsPlayerSettings : MonoBehaviour {

	private string sRoverName = "";
	private string sSponsorName = "";

	private float fMaxTorque;
	private float fMaxSteeringAngle;
	private float fMass;
	private float fMaxHealth;
	private float fMaxEnergy;

	public Texture2D texSponsorAvatar;

	private RoverController player;

	void Start() {
		if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "main") {
			player = GameObject.FindGameObjectWithTag("Player").GetComponent<RoverController>();

			if (player == null) {
				Debug.LogError("MARS PLAYER SETTINGS COULDN'T FIND PLAYER ROVER CONTROLLER!");
				return;
			}

			string stemp = LoadAndGetSponsorName();
			LoadSponsorFromXML(Application.dataPath + "/Resources/Sponsors/" + stemp + ".xml");

			player.maxMotorTorque = fMaxTorque;
			player.maxSteeringAngle = fMaxSteeringAngle;
			player.GetComponent<Rigidbody>().mass = fMass;
			player.GetComponent<MarsPlayer>().SetMaxHealth(fMaxHealth);
			player.GetComponent<MarsPlayer>().SetMaxEnergy(fMaxEnergy);
		}
		else {
			Debug.Log("Scene is not main, skipping player find..");
		}
	}

	void Update() {
	
	}

//	public string LoadAndGetRoverName() {
//		if (PlayerPrefs.HasKey("RoverName")) {
//			sRoverName = PlayerPrefs.GetString("RoverName");
//		}
//
//		return sRoverName;
//	}
//
//	public void SetAndSaveRoverName(string rover) {
//		//if (PlayerPrefs.HasKey("RoverName")) {
//		PlayerPrefs.SetString("RoverName", rover);
//		PlayerPrefs.Save();
//	}
//
	public string LoadAndGetSponsorName() {
		if (PlayerPrefs.HasKey("SponsorName")) {
			sSponsorName = PlayerPrefs.GetString("SponsorName");
		}

		return sSponsorName;
	}

	public void SetAndSaveSponsorName(string sponsor) {
		PlayerPrefs.SetString("SponsorName", sponsor);
		PlayerPrefs.Save();
	}
//
//	public float LoadAndGetMaxTorque() {
//		if (PlayerPrefs.HasKey("MaxTorque")) {
//			fMaxTorque = PlayerPrefs.GetFloat("MaxTorque");
//		}
//
//		return fMaxTorque;
//	}
//
//	public void SetAndSaveMaxTorque(float torque) {
//		PlayerPrefs.SetFloat("MaxTorque", torque);
//		PlayerPrefs.Save();
//	}
//
//	public float LoadAndGetMaxSteeringAngle() {
//		if (PlayerPrefs.HasKey("MaxSteeringAngle")) {
//			fMaxSteeringAngle = PlayerPrefs.GetFloat("MaxSteeringAngle");
//		}
//
//		return fMaxSteeringAngle;
//	}
//
//	public void SetAndSaveMaxSteeringAngle(float angle) {
//		PlayerPrefs.SetFloat("MaxSteeringAngle", angle);
//		PlayerPrefs.Save();
//	}
//
//	public float LoadAndGetChassisMass() {
//		if (PlayerPrefs.HasKey("ChassisMass")) {
//			fMass = PlayerPrefs.GetFloat("ChassisMass");
//		}
//
//		return fMass;
//	}
//
//	public void SetAndSaveChassisMass(float mass) {
//		PlayerPrefs.SetFloat("ChassisMass", mass);
//		PlayerPrefs.Save();
//	}
//
//	public float LoadAndGetMaxHealth() {
//		if (PlayerPrefs.HasKey("MaxHealth")) {
//			fMaxHealth = PlayerPrefs.GetFloat("MaxHealth");
//		}
//
//		return fMaxHealth;
//	}
//
//	public void SetAndSaveMaxHealth(float health) {
//		PlayerPrefs.SetFloat("MaxHealth", health);
//		PlayerPrefs.Save();
//	}
//
//	public float LoadAndGetMaxEnergy() {
//		if (PlayerPrefs.HasKey("MaxEnergy")) {
//			fMaxEnergy = PlayerPrefs.GetFloat("MaxEnergy");
//		}
//
//		return fMaxEnergy;
//	}
//
//	public void SetAndSaveMaxEnergy(float energy) {
//		PlayerPrefs.SetFloat("MaxEnergy", energy);
//		PlayerPrefs.Save();
//	}

	public void LoadSponsorFromXML(string path) {
		XDocument s_xmlDoc = XDocument.Load(path);

		foreach (XElement xroot in s_xmlDoc.Elements()) {
			foreach (XElement xlayer1 in xroot.Elements()) {
				if (xlayer1.Name == "name") {
					sSponsorName = xlayer1.Value;
				}
//				else if (xlayer1.Name == "description") {
//					sSponsorDescription = xlayer1.Value;
//				}
				else if (xlayer1.Name == "avatar") {
					string stemp = xlayer1.Value;
					texSponsorAvatar = Resources.Load<Texture2D>("Sponsors/Images/" + stemp);
				}
//				else if (xlayer1.Name == "rover") {
//					string stemp = xlayer1.Value;
//					sCurRover = stemp;
//				}
				else if (xlayer1.Name == "torque") {
					float ftemp = float.Parse(xlayer1.Value);
					fMaxTorque = ftemp;
				}
				else if (xlayer1.Name == "steeringangle") {
					float ftemp = float.Parse(xlayer1.Value);
					fMaxSteeringAngle = ftemp;
				}
				else if (xlayer1.Name == "mass") {
					float ftemp = float.Parse(xlayer1.Value);
					fMass = ftemp;
				}
				else if (xlayer1.Name == "health") {
					float ftemp = float.Parse(xlayer1.Value);
					fMaxHealth = ftemp;
				}
				else if (xlayer1.Name == "energy") {
					float ftemp = float.Parse(xlayer1.Value);
					fMaxEnergy = ftemp;
				}
			}
		}
	}
}

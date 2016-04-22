using UnityEngine;
using System.Collections;

public class MarsPlayerSettings : MonoBehaviour {

	private string sRoverName = "";
	private string sSponsorName = "";

	void Start() {
	
	}

	void Update() {
	
	}

	public string LoadAndGetRoverName() {
		if (PlayerPrefs.HasKey("RoverName")) {
			sRoverName = PlayerPrefs.GetString("RoverName");
		}

		return sRoverName;
	}

	public void SetAndSaveRoverName(string rover) {
		//if (PlayerPrefs.HasKey("RoverName")) {
		PlayerPrefs.SetString("RoverName", rover);
		PlayerPrefs.Save();
	}

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
}

using UnityEngine;
using System.Collections;

public class SponsorSceneManager : MonoBehaviour {

	public MarsPlayerSettings mps;
	public UISceneManager uism;

	public UnityEngine.UI.Button confirm;

	void Start() {
		
	}

	void Update() {
		if (uism.sRoverName != "" && uism.sSponsorName != "") {
			confirm.interactable = true;
		}
		else {
			confirm.interactable = false;
		}
	}

	public void SaveData() {
		//mps.SetAndSaveRoverName(uism.sRoverName);
		mps.SetAndSaveSponsorName(uism.sSponsorName);
	}

	public void NextScene() {
		UnityEngine.SceneManagement.SceneManager.LoadScene("main");
	}
}

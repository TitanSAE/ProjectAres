using UnityEngine;
using System.Collections;

public class WinSceneManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void NextScene() {
		UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
	}
}

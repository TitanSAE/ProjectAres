using UnityEngine;
using System.Collections;

public class MainMenuSceneManager : MonoBehaviour {

	public AudioSource asHappy;
	private float fTime;
	public float fHappyTime = 60;
	private bool bHappy;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		fTime += Time.deltaTime;

		if (fTime >= fHappyTime && !bHappy) {
			asHappy.Play();
			bHappy = true;
		}
	}

	public void NextScene() {
		UnityEngine.SceneManagement.SceneManager.LoadScene("sponsor_select");
	}

	public void QuitGame() {
		if (!Application.isEditor) {
			UnityEngine.Application.Quit();
		}

		#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
		#endif
	}
}

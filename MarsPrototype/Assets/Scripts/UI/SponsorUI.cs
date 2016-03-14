using UnityEngine;
using System.Collections;

public class SponsorUI : MonoBehaviour {

	public Texture texBackdrop;

	void Start() {
		//
	}

	void Update() {
		//
	}

	void OnGUI() {
		Rect screen = new Rect(0, 0, Screen.width, Screen.height);

		GUILayout.BeginArea(screen);
		GUI.DrawTexture(screen, texBackdrop);
		//
		GUILayout.EndArea();
	}
}

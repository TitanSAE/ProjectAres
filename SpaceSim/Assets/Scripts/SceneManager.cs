using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SceneManager : MonoBehaviour {

	public SimpleSmoothMouseLook mouselook;
	public FPS_Player ply;

	public List<Camera> l_cams;
	public int iActiveCam;

	void Start() {
	
	}

	void Update() {
		if (Input.GetKeyDown(KeyCode.R)) {
			CycleCameras();
		}

		if (Input.GetKeyDown(KeyCode.Escape)) {
			if (Application.isEditor) {
				Debug.Break();
			}

			Application.Quit();
		}

		if (Input.GetMouseButtonDown(2)) {
			mouselook.bMouseLocked = !mouselook.bMouseLocked;
		}
	}

	public void CycleCameras() {
		l_cams[iActiveCam].gameObject.SetActive(false);

		iActiveCam++;
		if (iActiveCam > l_cams.Count - 1) {
			iActiveCam = 0;
		}

		l_cams[iActiveCam].gameObject.SetActive(true);

		if (iActiveCam == 0) {
			ply.bFrozen = false;
			mouselook.bMouseLocked = true;
		}
		else {
			ply.bFrozen = true;
			mouselook.bMouseLocked = false;
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
		}
	}
}

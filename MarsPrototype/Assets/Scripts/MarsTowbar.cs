using UnityEngine;
using System.Collections;

public class MarsTowbar : MonoBehaviour {

	public GameObject goTowbar;
	public GameObject goAttached;
	public bool bTowing = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		//
	}

	void LateUpdate() {
		if (goAttached == null) {
			bTowing = false;
		}
	}
}

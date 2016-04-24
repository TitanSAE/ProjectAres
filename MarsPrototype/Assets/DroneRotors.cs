using UnityEngine;
using System.Collections;

public class DroneRotors : MonoBehaviour {

	public Transform tRot1;
	public Transform tRot2;
	public GameObject controller;

	void Start () {
	
	}

	void Update () {
		if (controller.activeInHierarchy) {
			tRot1.Rotate(new Vector3(0, 32, 0));
			tRot2.Rotate(new Vector3(0, -32, 0));
		}
	}
}

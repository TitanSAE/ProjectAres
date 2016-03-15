using UnityEngine;
using System.Collections;

public class Gravity : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	Physics.gravity = new Vector3 (0f, 1f, 0f);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

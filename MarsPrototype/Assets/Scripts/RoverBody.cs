using UnityEngine;
using System.Collections;

public class RoverBody : MonoBehaviour {
	public GameObject Wheels;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		this.transform.position = Wheels.transform.position;
	}
}

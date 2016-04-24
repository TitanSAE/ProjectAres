using UnityEngine;
using System.Collections;

public class WorldCanvasFaceUp : MonoBehaviour {



	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		this.transform.position = this.transform.parent.transform.position + new Vector3(0, 1, 0);
		this.transform.rotation = Quaternion.Euler(new Vector3(90, 0, 0));
	}
}

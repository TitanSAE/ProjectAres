using UnityEngine;
using System.Collections;

public class WorldCanvasFaceUp : MonoBehaviour {

	GameObject ply;

	// Use this for initialization
	void Start () {
		ply = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {
		this.transform.position = this.transform.parent.transform.position + new Vector3(0, 1, 0);

		//this.transform.rotation = Quaternion.Euler(new Vector3(90, 0, 0));
		this.transform.rotation = Quaternion.Euler(ply.transform.right * 90);
	}
}

using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]
public class Debug_Clickable : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

	void OnMouseDown() {
		Debug.Log(this.name);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

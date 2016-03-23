using UnityEngine;
using System.Collections;


public enum BASE_ATTACHMENT {
	NONE,
	POWER_SUPPLY
}

public class MarsBaseAttachmentPoint : MonoBehaviour {

	public BASE_ATTACHMENT eAttachmentType;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider c) {
		if (c.gameObject.tag == "MarsDelivery") {
			Debug.Log("CLICK");
		}
	}
}

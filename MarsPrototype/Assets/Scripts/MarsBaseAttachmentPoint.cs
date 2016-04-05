using UnityEngine;
using System.Collections;


public class MarsBaseAttachmentPoint : MonoBehaviour {

	public BASE_ATTACHMENT eAttachmentType;
	public bool bIsEmpty {
		private set {
			//
		}
		get {
			return this.transform.childCount == 0;
		}
	}

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

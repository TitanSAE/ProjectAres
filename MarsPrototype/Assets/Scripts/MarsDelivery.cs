using UnityEngine;
using System.Collections;

public class MarsDelivery : MonoBehaviour {

	public BASE_ATTACHMENT eDeliveryType;
	public bool bAttachedToPlayer;
	public GameObject goPlayerTowbar;
	public bool bDelivered = false;

	//public bool bAttachedToBase;


	// Use this for initialization
	void Start () {
	

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter(Collision c) {
		if (c.gameObject.tag == "Player" && !bAttachedToPlayer && !c.gameObject.GetComponent<RoverPlayerTowbar>().bTowing && !bDelivered) {
			bAttachedToPlayer = true;
			this.GetComponent<BoxCollider>().isTrigger = true;
			goPlayerTowbar = c.gameObject;
			this.transform.position = goPlayerTowbar.GetComponent<RoverPlayerTowbar>().goTowbar.transform.position;
			goPlayerTowbar.GetComponent<RoverPlayerTowbar>().bTowing = true;
			this.transform.rotation = c.gameObject.transform.rotation;
			this.transform.position = ((this.transform.forward * -3) + this.transform.position);
			this.transform.SetParent(c.gameObject.transform);
			this.GetComponent<Rigidbody>().isKinematic = true;
			this.GetComponent<Rigidbody>().useGravity = false;
			Debug.Log ("Hit C");

			if (eDeliveryType == BASE_ATTACHMENT.Level1) {
				Debug.Log ("Hit PowerSupply");
			}

		}
	}

	void OnTriggerEnter(Collider c) {
		if (c.gameObject.tag == "BaseAttachment" && bAttachedToPlayer) {
			//if (eDeliveryType == c.GetComponent<MarsBaseAttachmentPoint>().eAttachmentType) {
				//bAttachedToPlayer = false;
				//bDelivered = true;
				//bAttachedToBase = true;
				//this.GetComponent<BoxCollider>().isTrigger = false;
				//this.transform.position = c.gameObject.transform.position;
				//this.transform.rotation = c.gameObject.transform.rotation;
				////goPlayerTowbar.GetComponent<RoverPlayerTowbar> ().bTowing = false;
				//this.transform.position = ((this.transform.forward * -3) + this.transform.position);
				//this.transform.SetParent(c.gameObject.transform);
			//}

//			if (c.gameObject.tag == "Player" && !bAttachedToPlayer && !bAttachedToBase) {
//				Debug.Log ("HIT");
//				bAttachedToPlayer = true;
//				this.GetComponent<BoxCollider>().isTrigger = true;
//				this.transform.position = c.gameObject.GetComponentInParent<RoverPlayerController>().goTowbar.transform.position;
//				this.transform.rotation = c.gameObject.transform.rotation;
//				this.transform.position = ((this.transform.forward * -3) + this.transform.position);
//				this.transform.SetParent(c.gameObject.transform);
//			}
		}
	}
}

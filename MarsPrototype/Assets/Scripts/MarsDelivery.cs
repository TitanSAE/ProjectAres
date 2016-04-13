using UnityEngine;
using System.Collections;

public class MarsDelivery : MonoBehaviour {

	public BASE_ATTACHMENT eDeliveryType;
	public bool bAttachedToPlayer;
	public GameObject goPlayerTowbar;
	public bool bDelivered = false;
	GameObject Level1;
	GameObject Level2;
	GameObject Level3;
	GameObject Level4;
	GameObject smallPanel;
	GameObject largePanel;

	private bool bImpact;

	//public bool bAttachedToBase;


	// Use this for initialization
	void Start () {
	

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter(Collision c) {
		if (!bImpact) {
			bImpact = true;
		}
		if (c.gameObject.tag == "Player" && !bAttachedToPlayer && !c.gameObject.GetComponent<MarsTowbar>().bTowing && !bDelivered) {
			bAttachedToPlayer = true;
			this.GetComponent<BoxCollider>().isTrigger = true;
			goPlayerTowbar = c.gameObject;
			this.transform.position = goPlayerTowbar.GetComponent<MarsTowbar>().goTowbar.transform.position;
			goPlayerTowbar.GetComponent<MarsTowbar>().goAttached = this.gameObject;
			goPlayerTowbar.GetComponent<MarsTowbar>().bTowing = true;
			this.transform.rotation = c.gameObject.transform.rotation;
			this.transform.position = ((this.transform.forward * -0.5f) + this.transform.position);
			this.transform.SetParent(c.gameObject.transform);
			this.GetComponent<Rigidbody>().isKinematic = true;
			this.GetComponent<Rigidbody>().useGravity = false;
			Debug.Log ("Hit C");

			if (eDeliveryType == BASE_ATTACHMENT.Level1) {
				Level1 = GameObject.Find ("SceneManager").GetComponent<GameManager> ().Level1;
				Level1.SetActive (true);
			}
			if (eDeliveryType == BASE_ATTACHMENT.Level2) {
				Level2 = GameObject.Find ("SceneManager").GetComponent<GameManager> ().Level2;
				Level2.SetActive (true);
			}
			if (eDeliveryType == BASE_ATTACHMENT.Level3) {
				Level3 = GameObject.Find ("SceneManager").GetComponent<GameManager> ().Level3;
				Level3.SetActive (true);
			}
			if (eDeliveryType == BASE_ATTACHMENT.Level4) {
				Level4 = GameObject.Find ("SceneManager").GetComponent<GameManager> ().Level4;
				Level4.SetActive (true);
			}
			if (eDeliveryType == BASE_ATTACHMENT.smallPanel) {
				smallPanel = GameObject.Find ("SceneManager").GetComponent<GameManager> ().smallPanel;
				smallPanel.SetActive (true);
			}
			if (eDeliveryType == BASE_ATTACHMENT.largePanel) {
				largePanel = GameObject.Find ("SceneManager").GetComponent<GameManager> ().largePanel;
				largePanel.SetActive (true);
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

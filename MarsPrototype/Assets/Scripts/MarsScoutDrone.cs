using UnityEngine;
using System.Collections;

public class MarsScoutDrone : MonoBehaviour {

	public float fMaxHeight = 40.0f;
	public float fTempMaxHeight;
	public float fVerticalThrustPower = 10.0f;
	public float fForwardThrustPower = 10.0f;
	public float fRotateSpeed = 0.1f;
	private Rigidbody rbBody;
	private Transform tTrans;
	private bool bHolding;
	public float fVelocityLockLimit = 50.0f;

	public GameObject goLocked;
	public GameObject goUnlocked;
	public UnityEngine.UI.Text txtHeightCap;
	public GameObject goLanded;

	public bool bTakeoff;

	private bool bAttachedToRover;

	private Vector3 vVel;

	void Start() {
		rbBody = this.GetComponentInParent<Rigidbody>();
		tTrans = this.transform.parent.transform;
	}

	void Update() {
		if (Input.GetKey(KeyCode.Space)) {
			bTakeoff = true;
		}
		else {
			bTakeoff = false;
		}

		if (Input.GetKeyDown(KeyCode.LeftShift) && !bAttachedToRover) {
			bHolding = !bHolding;

			if (bHolding) {
				fTempMaxHeight = Mathf.Ceil(this.transform.position.y);
			}
		}
			
		goLanded.SetActive(bAttachedToRover);

		if (bHolding && !bAttachedToRover) {
			goLocked.SetActive(true);
			goUnlocked.SetActive(false);
			txtHeightCap.gameObject.SetActive(true);
			txtHeightCap.text = Mathf.Clamp(Mathf.RoundToInt(fTempMaxHeight - this.transform.position.y), 0, 999).ToString("D3") + "m";
		}
		else if (!bHolding && !bAttachedToRover) {
			goLocked.SetActive(false);
			goUnlocked.SetActive(true);
			txtHeightCap.gameObject.SetActive(true);
			txtHeightCap.text = Mathf.Clamp(Mathf.RoundToInt(fMaxHeight - this.transform.position.y), 0, 999).ToString("D3") + "m";
		}

		if (bAttachedToRover) {
			goLocked.SetActive(false);
			goUnlocked.SetActive(false);
			txtHeightCap.gameObject.SetActive(false);
		}
	}

	public void AttachToRover(GameObject rov) {
		bAttachedToRover = true;
		rbBody.isKinematic = true;
		rbBody.useGravity = false;
		tTrans.GetComponent<BoxCollider>().isTrigger = true;
		tTrans.SetParent(rov.transform);
		bHolding = false;
		tTrans.position = rov.transform.position + rov.transform.up * 0.5f + rov.transform.right * -0.5f;
	}

	public void DetachFromRover() {
		bAttachedToRover = false;
		rbBody.isKinematic = false;
		rbBody.useGravity = true;
		bHolding = false;
		tTrans.GetComponent<BoxCollider>().isTrigger = false;
		tTrans.SetParent(null);
	}

	void LateUpdate() {
		if (Input.GetAxis("Horizontal") != 0 && !bAttachedToRover) {
			float sign = Mathf.Sign(Input.GetAxis("Horizontal"));

			rbBody.AddTorque(Vector3.up * sign * fRotateSpeed);
		}

		if (Input.GetAxis("Vertical") != 0 && !bAttachedToRover) {
			float sign = Mathf.Sign(Input.GetAxis("Vertical"));

			rbBody.AddForce(this.transform.forward * fForwardThrustPower * sign, ForceMode.Acceleration);
		}
			
		if (bHolding && rbBody.velocity.magnitude > fVelocityLockLimit) {
			bHolding = false;
		}

		if (Input.GetKey(KeyCode.Space)) {
			if (bAttachedToRover) {
				DetachFromRover();
			}

			if (!bHolding) {
				if (this.transform.position.y < fMaxHeight) {
					rbBody.AddForce(Vector3.up * fVerticalThrustPower, ForceMode.Acceleration);
				}
				else {
					//float loss = fMaxHeight / this.transform.position.y;
					//float cur = Mathf.SmoothStep(fMaxHeight, this.transform.position.y, 0.6f);
					//rbBody.AddForce(Vector3.down * cur, ForceMode.Acceleration);
					this.transform.position = new Vector3(this.transform.position.x, fMaxHeight, this.transform.position.z);
				}
			}
			else {
				if (this.transform.position.y < fTempMaxHeight) {
					rbBody.AddForce(Vector3.up * fVerticalThrustPower, ForceMode.Acceleration);
				}
				else {
					this.transform.position = new Vector3(this.transform.position.x, fTempMaxHeight, this.transform.position.z);
				}
			}
		}
	}
}

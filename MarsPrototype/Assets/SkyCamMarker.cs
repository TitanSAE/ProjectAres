using UnityEngine;
using System.Collections;

public class SkyCamMarker : MonoBehaviour {

	public Camera skycam;
	public Transform tTarget;
	public Vector3 vDesiredScale = new Vector3(1, 1, 1);
	public MarsPlayer ply;
	public bool bStayStillWhenMapSmall;

	void Start () {
		ply = GameObject.FindGameObjectWithTag("Player").GetComponent<MarsPlayer>();
	}

	void Update () {
		if (!bStayStillWhenMapSmall) {
			this.transform.rotation = Quaternion.Euler(0, tTarget.rotation.y, 0);
		}
		else {
			if (ply.bFullScreenMap) {
				this.transform.rotation = Quaternion.Euler(0, tTarget.rotation.y, 0);
			}
			else {
                //this.transform.rotation = Quaternion.Euler(0, tTarget.rotation.y, 0);
                this.transform.rotation = ply.transform.rotation;
                this.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
			}
		}
		this.transform.localPosition = tTarget.position;

		//Keep it the same size no matter how far away we are
		float dist = (skycam.transform.position - tTarget.position).magnitude;
		float size = dist * 0.001f;
		size = Mathf.Clamp(size, 0.1f, Mathf.Infinity);
		this.transform.localScale = new Vector3(size * vDesiredScale.x, 1, size * vDesiredScale.z);
	}
}

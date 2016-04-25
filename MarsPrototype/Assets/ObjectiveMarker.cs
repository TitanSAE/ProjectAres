using UnityEngine;
using System.Collections;

public class ObjectiveMarker : MonoBehaviour {

	//questid?
	public float fMinDistance = 20.0f;
	public float fFadeDistance = 100.0f;

	public float fMinScale = 0.01f;

	private UnityEngine.UI.Image imgMarker;

	void Start() {
		imgMarker = this.GetComponentInChildren<UnityEngine.UI.Image>();
	}

	void Update() {
		//Keep it the same size no matter how far away we are
		this.transform.LookAt(Camera.main.transform.position);
		float dist = (Camera.main.transform.position - transform.position).magnitude;
		float size = dist * 0.001f;
		size = Mathf.Clamp(size, fMinScale, 10.0f);
		this.transform.localScale = new Vector3(size, size, 1);


		//Fade
		float mid = Mathf.Abs(fFadeDistance - dist + fMinDistance) * 0.01f;
		SetImageAlpha(mid);

		//Debug.Log("DIST " + dist.ToString() + " | " + "%" + mid.ToString());
	}

	void SetImageAlpha(float alpha) {
		Color col = imgMarker.color;
		col.a = alpha;
		imgMarker.color = col;
	}
}

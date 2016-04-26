using UnityEngine;
using System.Collections;

public class ObjectiveMarker : MonoBehaviour {

	public int iObjectiveID = 0;
	public float fMinDistance = 20.0f;
	public float fFadeDistance = 100.0f;

	public float fMinScale = 0.01f;

	private UnityEngine.UI.Image imgMarker;
	private GameObject goPly;

	public GameObject goProtoText;
	public UnityEngine.UI.Text txtDistance;

	//public bool bShowHUDText = true;

	void Start() {
		imgMarker = this.GetComponentInChildren<UnityEngine.UI.Image>();

		Canvas canv = this.GetComponentInParent<Canvas>();
		//GameObject gotemp = (GameObject)GameObject.Instantiate(goProtoText, canv.transform.position, canv.transform.rotation);
		//txtDistance = gotemp.GetComponent<UnityEngine.UI.Text>();
		//gotemp.name = "NavMarker Text ID" + iObjectiveID.ToString();

		goPly = GameObject.FindGameObjectWithTag("Player");
	}

	void Update() {
		//txtDistance.gameObject.SetActive(bShowHUDText);
		//txtDistance.text = Mathf.Abs((goPly.transform.position - transform.position).magnitude).ToString() + "m";

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

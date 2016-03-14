using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIShowPivot : MonoBehaviour {

	public Transform tPivotBody;
	public Transform tPivotHead;
	//public Image imgBody;
	public Transform pointer;

	GameObject goBody;
	GameObject goHead;

	void Start() {
//		goBody = GameObject.CreatePrimitive(PrimitiveType.Cube);
//		goHead = GameObject.CreatePrimitive(PrimitiveType.Cube);
//		goBody.GetComponent<MeshRenderer>().enabled = false;
//		goHead.GetComponent<MeshRenderer>().enabled = false;
//		goBody.GetComponent<BoxCollider>().enabled = false;
//		goHead.GetComponent<BoxCollider>().enabled = false;
	}

	void Update() {
		float theta = Vector3.Angle(tPivotHead.transform.rotation.eulerAngles, tPivotBody.transform.rotation.eulerAngles);
		Quaternion qdif = Quaternion.Euler(new Vector3(0, 0, theta));
		qdif.eulerAngles = new Vector3(0, 0, qdif.eulerAngles.z + 90);

		pointer.rotation = qdif;
		//Vector3 vtemp = new Vector3(qdif.eulerAngles.x, qdif.eulerAngles.y, qdif.eulerAngles.z);
		//imgBody.rectTransform.rotation.eulerAngles = vtemp;
		//imgBody.rectTransform.rot
	}
}

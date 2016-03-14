using UnityEngine;
using System.Collections;

public class UIStickToTarget : MonoBehaviour {

	public Transform target;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		this.GetComponent<RectTransform>().anchoredPosition = Camera.main.WorldToScreenPoint(target.position);
	}
}

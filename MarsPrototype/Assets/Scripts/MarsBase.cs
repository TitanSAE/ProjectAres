using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MarsBase : MonoBehaviour {

	public List<MarsBuilding> l_buildings = new List<MarsBuilding>();
	public MarsPlayer ply;

	void Start() {
		ply = GameObject.FindGameObjectWithTag("Player").GetComponent<MarsPlayer>();
		l_buildings.AddRange(this.gameObject.GetComponentsInChildren<MarsBuilding>());
	}

	void Update() {
		foreach (MarsBuilding mb in l_buildings) {
			if (!mb.bGhosted || mb.bLerping) {
				mb.gameObject.SetActive(true);
			}

			if (mb.bGhosted && !mb.bLerping) {
				if (!ply.towbar.bTowing && mb.bGhosted) {
					mb.gameObject.SetActive(false);
				}
				else if (ply.towbar.bTowing && mb.bGhosted && Vector3.Distance(ply.transform.position, mb.transform.position) < 50) {
					mb.gameObject.SetActive(true);
				}
				else {
					mb.gameObject.SetActive(false);
				}
			}
		}
	}
}

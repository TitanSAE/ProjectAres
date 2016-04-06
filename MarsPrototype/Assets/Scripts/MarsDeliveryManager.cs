using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MarsDeliveryManager : MonoBehaviour {

	public List<Transform> l_tSpawnPoints = new List<Transform>();
	public Stack<BASE_ATTACHMENT> st_ePackages = new Stack<BASE_ATTACHMENT>();

	public GameObject goProtoPackage;
	public GameObject goTowbar;

	void Start() {
		for (int i = 0; i < 10; i++) {
			st_ePackages.Push((BASE_ATTACHMENT)Random.Range(1, 8));
		}
	}

	void Update() {
	
	}

	public void SpawnPackage() {
		int rspot = Random.Range(0, l_tSpawnPoints.Count);

		GameObject goTemp = (GameObject)GameObject.Instantiate(goProtoPackage, l_tSpawnPoints[rspot].position, Quaternion.identity);
		MarsDelivery crate = goTemp.GetComponent<MarsDelivery>();
		crate.goPlayerTowbar = goTowbar;
		crate.eDeliveryType = st_ePackages.Pop();
	}
}

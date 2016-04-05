using UnityEngine;
using System.Collections;

public class TimedDelivery : MonoBehaviour {

	public MarsDeliveryManager mngDelivery;

	public float fCounter;
	public float fDeliveryTime = 30.0f;

	public bool bActive;

	void Start() {
	
	}

	void Update() {
		if (mngDelivery.st_ePackages.Count > 0) {
			fCounter += Time.deltaTime;
		}

		if (fCounter >= fDeliveryTime) {
			fCounter = 0;
			mngDelivery.SpawnPackage();
		}
	}
}

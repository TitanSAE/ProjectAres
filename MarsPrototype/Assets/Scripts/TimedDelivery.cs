using UnityEngine;
using System.Collections;

public class TimedDelivery : MonoBehaviour {

	public MarsDeliveryManager mngDelivery;

	public float fCounter;
	public float fDeliveryTime = 30.0f;

	public bool bActive;
	int count = 0;
	void Start() {
	
	}

	void Update() {
		if (mngDelivery.st_ePackages.Count > 0) {
			fCounter += Time.deltaTime;
		}
		if (count < 6) {
			if (fCounter >= fDeliveryTime) {
				fCounter = 0;
				GameObject.Find ("SceneManager").GetComponent<GameManager> ().PackageParticle.Clear ();
				GameObject.Find ("SceneManager").GetComponent<GameManager> ().PackageParticle.Play ();
				count += 1;
			}
		}
	}
}

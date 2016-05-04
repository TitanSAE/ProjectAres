using UnityEngine;
using System.Collections;

public class MessageAlerter : MonoBehaviour {

	private Light alertlight;
	private AudioSource asAlert;
	private MarsMessageManager mngMessage;
	private bool bPlayOnce;

	void Start () {
		alertlight = this.GetComponent<Light>();
		asAlert = this.GetComponent<AudioSource>();
		mngMessage = GameObject.FindGameObjectWithTag("SceneManager").GetComponent<MarsMessageManager>();
	}

	void Update () {
		if (mngMessage.iUnread > 0) {
			if (!bPlayOnce) {
				bPlayOnce = true;
				asAlert.Play();
			}

			if (alertlight.intensity <= 0) {
				alertlight.intensity = 1.3f;
			}
		}
		else {
			asAlert.Stop();
			bPlayOnce = false;
		}

		if (alertlight.intensity > 0) {
			alertlight.intensity -= Time.deltaTime;
		}
	}
}

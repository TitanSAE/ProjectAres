using UnityEngine;
using System.Collections;

public class MessageAlerter : MonoBehaviour {

	private Light alertlight;
	private AudioSource asAlert;
	private MarsMessageManager mngMessage;

	void Start () {
		alertlight = this.GetComponent<Light>();
		asAlert = this.GetComponent<AudioSource>();
		mngMessage = GameObject.FindGameObjectWithTag("SceneManager").GetComponent<MarsMessageManager>();
	}

	void Update () {
		if (mngMessage.iUnread > 0) {
			if (!asAlert.isPlaying) {
				asAlert.Play();
				alertlight.intensity = 1.3f;
			}
		}

		if (alertlight.intensity > 0) {
			alertlight.intensity -= Time.deltaTime;
		}
	}
}

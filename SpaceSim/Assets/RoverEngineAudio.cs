using UnityEngine;
using System.Collections;

public class RoverEngineAudio : MonoBehaviour {

	public AudioSource asThruster;
	public AudioSource asDriving;
	public AudioSource asIdle;

	public FPS_Player ply;

	void Update () {
		if (ply.bJetting) {
			asThruster.volume = 0.5f;
			asDriving.volume = 1.0f;
			asIdle.volume = 0.1f;
		}
		else if (!ply.bIdle) {
			asThruster.volume = 0.0f;
			asDriving.volume = 1.0f;
			asIdle.volume = 0.1f;
		}
		else {
			asThruster.volume = 0.0f;
			asDriving.volume = 0.0f;
			asIdle.volume = 0.5f;
		}
	}
}

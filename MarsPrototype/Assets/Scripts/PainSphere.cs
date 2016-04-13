using UnityEngine;
using System.Collections;

public class PainSphere : MonoBehaviour {

	void OnCollisionEnter(Collision c) {
		if (c.gameObject.tag == "Player") {
			c.gameObject.GetComponent<MarsPlayer>().fHealth -= Random.Range(1, 30);
		}
	}
}

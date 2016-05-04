using UnityEngine;
using System.Collections;

public class TimedDelivery : MonoBehaviour {

	public MarsDeliveryManager mngDelivery;

	//public float fCounter;
	public float fDeliveryTime = 30.0f;

	public bool bActive;
	int count = 0;

	private MarsMessageManager mngMessages;

	public float fCountdown;
	public GAME_QUESTS eMission = GAME_QUESTS.INITIAL_PACKAGE;

	void Start() {
		mngMessages = GameObject.FindGameObjectWithTag("SceneManager").GetComponent<MarsMessageManager>();

		fCountdown = 6.0f;
	}

	public void SendPackage() {
		GameObject.Find("SceneManager").GetComponent<GameManager>().PackageParticle.Clear();
		GameObject.Find("SceneManager").GetComponent<GameManager>().PackageParticle.Play();
	}
	public void SendPackage1() {
		GameObject.Find("SceneManager").GetComponent<GameManager>().PackageParticle1.Clear();
		GameObject.Find("SceneManager").GetComponent<GameManager>().PackageParticle1.Play();
	}
	public void SendPackage2() {
		GameObject.Find("SceneManager").GetComponent<GameManager>().PackageParticle2.Clear();
		GameObject.Find("SceneManager").GetComponent<GameManager>().PackageParticle2.Play();
	}

	void Update() {
		if (fCountdown > 0) {
			fCountdown -= Time.deltaTime;

			if (fCountdown <= 0) {
				int mis = (int)eMission;
				mis++;
				eMission = (GAME_QUESTS)mis;

				if (eMission == GAME_QUESTS.INITIAL_PACKAGE) {
					mngMessages.AddMessage("Delivery Notification", "We've sent a package close to your destination. Retrieve it and bring it back to base to build the first building.", "Earth");
					fCountdown = 60.0f;

					SendPackage();
				}
//				else if (eMission == GAME_QUESTS.PACKAGE_2) {
//					//mngMessages.AddMessage("Delivery Notification", "We've sent some more building supplies. Hopefully they land nearby!", "sponsor");
//					fCountdown = 60.0f;
//
//					SendPackage1();
//				}
//				else if (eMission == GAME_QUESTS.PACKAGE_3) {
//					//mngMessages.AddMessage("Delivery Notification", "We've sent some more building supplies. Hopefully they land nearby!", "sponsor");
//					fCountdown = 120.0f;
//
//					SendPackage2();
//				}
//				else if (eMission == GAME_QUESTS.PACKAGE_4) {
//					//mngMessages.AddMessage("Delivery Notification", "We've sent some more building supplies. Hopefully they land nearby!", "sponsor");
//					fCountdown = 120.0f;
//
//					SendPackage();
//				}
//				else if (eMission == GAME_QUESTS.PACKAGE_5) {
//					//mngMessages.AddMessage("Delivery Notification", "We've sent some more building supplies. Hopefully they land nearby!", "sponsor");
//					fCountdown = 300.0f;
//
//					SendPackage1();
//				}
//				else if (eMission == GAME_QUESTS.PACKAGE_6) {
//					//mngMessages.AddMessage("Delivery Notification", "We've sent some more building supplies. Hopefully they land nearby!", "sponsor");
//					fCountdown = 300.0f;
//
//					SendPackage2();
//				}
//				else {
//					//
//				}
			}
		}

//		if (mngDelivery.st_ePackages.Count > 0) {
//			fCounter += Time.deltaTime;
//		}
//		if (count < 6) {
//			if (fCounter >= fDeliveryTime) {
//				fCounter = 0;
//				GameObject.Find ("SceneManager").GetComponent<GameManager> ().PackageParticle.Clear ();
//				GameObject.Find ("SceneManager").GetComponent<GameManager> ().PackageParticle.Play ();
//				count += 1;
//			}
//		}
	}
}

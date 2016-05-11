using UnityEngine;
using System.Collections;

public enum MARS_RESOURCE_TYPE {
	NONE,
	SOIL,
	ROCK
}

public class MarsResource : MonoBehaviour {

	public MARS_RESOURCE_TYPE eType;
	[SerializeField]
	private bool bHarvested = false;

	[SerializeField]
	private bool bOverlap;

	private DayNightCycle daynight;
	private MarsPlayer ply;
	private MarsMessageManager mngMsg;

	void Start() {
		daynight = GameObject.FindGameObjectWithTag("DayNight").GetComponent<DayNightCycle>();
		ply = GameObject.FindGameObjectWithTag("Player").GetComponent<MarsPlayer>();
		mngMsg = GameObject.FindGameObjectWithTag("SceneManager").GetComponent<MarsMessageManager>();
	}

	void Update() {
		if (Input.GetButton("ConfirmRepair") && bOverlap && !bHarvested) {
			daynight.SkipDay();
			Harvest();
			GameObject.Destroy(this.gameObject);
		}
	}

	public void Harvest() {
		ply.iRocksCarried++;
		bHarvested = true;
	}

	void OnTriggerEnter(Collider col) {
		if (col.gameObject.tag == "Harvester") {
			bOverlap = true;
			mngMsg.bExternalFlagFirstMineral = true;
		}
	}

	void OnTriggerExit(Collider col) {
		if (col.gameObject.tag == "Harvester") {
			bOverlap = false;
		}
	}
}

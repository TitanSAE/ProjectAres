using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MarsDayCounter : MonoBehaviour {

	//2190 days = 6 years

	private DayNightCycle daynight;
	private int iMaxDay = 2190;
	private Text txtCounter;
	public float fPercentYearsPassed;
	public Image imgDayBar;

	private MarsPlayer ply;

	void Start() {
		daynight = GameObject.FindGameObjectWithTag("SceneManager").GetComponentInChildren<DayNightCycle>();
		txtCounter = this.GetComponent<Text>();
		ply = GameObject.FindGameObjectWithTag("Player").GetComponent<MarsPlayer>();
	}

	void Update() {
		txtCounter.text = "Sol " + (daynight.iDayCount + 1).ToString("D3");
		fPercentYearsPassed = (float)((float)daynight.iDayCount / (float)iMaxDay);

		if (daynight.iDayCount > iMaxDay) {
			ply.fHealth -= 1;
		}

		imgDayBar.fillAmount = daynight.fDayPortion;
	}
}

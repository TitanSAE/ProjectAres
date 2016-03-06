using UnityEngine;
using System.Collections;

public class DayNightCycle : MonoBehaviour {

	public int iDayCount = 0;
	public float fDayPortion = 0.0f;
	public float fTimePassed = 0.0f;

	public float fCycleDuration = 120.0f;
	public float fTimeLerp;

	public float fHoldDuration = 180.0f;
	public float fHoldCount;

	public bool bDaytime = false;
	public bool bHold = false;

	public Light sun;

	public Color colDay = Color.white;
	public Color colNight = Color.white;

	public float fIntensityDay = 1.0f;
	public float fIntensityNight = 0.0f;

	public float fAmbientIntensityDay = 1.0f;
	public float fAmbientIntensityNight = 0.5f;

	public Color colAmbientSkyDay = new Color(1.0f, 0.33f, 0.0f);
	public Color colAmbientSkyNight = new Color(1.0f, 0.33f, 0.0f);

	public Color colAmbientEquatorDay = Color.black;
	public Color colAmbientEquatorNight = Color.black;

	public Color colAmbientGroundDay = Color.white;
	public Color colAmbientGroundNight = Color.white;

	void Start() {
		sun = GameObject.FindGameObjectWithTag("DayNight_Sun").GetComponent<Light>();

		if (sun == null) {
			Debug.LogError("DayNightSystem couldn't find the sun scene light!");
		}
	}

	void Update() {
		//Transition from day to night, or vice-versa
		if (!bHold) {
			if (fTimeLerp < 1) {
				fTimeLerp += Time.deltaTime / fCycleDuration;
			}
			else {
				bDaytime = !bDaytime;
				bHold = true;
				fHoldCount = 0;
				fTimeLerp = 0;
			}

			if (bDaytime) {
				sun.intensity = Mathf.Lerp(fIntensityDay, fIntensityNight, fTimeLerp);
				sun.color = Color.Lerp(colDay, colNight, fTimeLerp);

				RenderSettings.ambientSkyColor = Color.Lerp(colAmbientSkyDay, colAmbientSkyNight, fTimeLerp);
				RenderSettings.ambientEquatorColor = Color.Lerp(colAmbientEquatorDay, colAmbientEquatorNight, fTimeLerp);
				RenderSettings.ambientGroundColor = Color.Lerp(colAmbientGroundDay, colAmbientGroundNight, fTimeLerp);

				RenderSettings.ambientIntensity = Mathf.Lerp(fAmbientIntensityDay, fAmbientIntensityNight, fTimeLerp);
			}
			else {
				sun.intensity = Mathf.Lerp(fIntensityNight, fIntensityDay, fTimeLerp);
				sun.color = Color.Lerp(colNight, colDay, fTimeLerp);

				RenderSettings.ambientSkyColor = Color.Lerp(colAmbientSkyNight, colAmbientSkyDay, fTimeLerp);
				RenderSettings.ambientEquatorColor = Color.Lerp(colAmbientEquatorNight, colAmbientEquatorDay, fTimeLerp);
				RenderSettings.ambientGroundColor = Color.Lerp(colAmbientGroundNight, colAmbientGroundDay, fTimeLerp);

				RenderSettings.ambientIntensity = Mathf.Lerp(fAmbientIntensityNight, fAmbientIntensityDay, fTimeLerp);
			}
		}
		//Otherwise, keep the current light constant
		else {
			if (fHoldCount < fHoldDuration) {
				fHoldCount += Time.deltaTime;
			}
			else {
				fHoldCount = 0.0f;
				bHold = false;

				if (!bDaytime) {
					fTimePassed = 0;
					iDayCount++;
				}
			}
		}

		fTimePassed += Time.deltaTime;
		fDayPortion = fTimePassed / ((fHoldDuration * 2) + (fCycleDuration * 2));

		//float totalday = ((fHoldDuration * 2) + (fCycleDuration * 2));
	}

	public void ResetTime() {
		iDayCount = 0;
		fDayPortion = 0.0f;
		fTimePassed = 0.0f;

		fTimeLerp = 0.0f;
		fHoldCount = 0.0f;

		bDaytime = false;
		bHold = false;
	}

	public void SetTime(float time) {
		ResetTime();

		float totalday = ((fHoldDuration * 2) + (fCycleDuration * 2));
		float realtime = time * totalday;

		if (realtime > fCycleDuration && realtime < (fCycleDuration * 2) + fHoldDuration) {
			bDaytime = true;
		}


		//sunrise
		//float lerp1 = 0.0f;
		float lerp1_end = fCycleDuration;

		//day
		float day = fCycleDuration;
		float day_end = fCycleDuration + fHoldDuration;

		//sunset
		//float lerp2 = fCycleDuration + fHoldDuration;
		float lerp2_end = fCycleDuration * 2 + fHoldDuration;

		//night
		float night = fCycleDuration * 2 + fHoldDuration;
		//float night_end

		if ((realtime > day && realtime < day_end) || realtime > night) {
			bHold = true;
		}

		if (bHold) {
			float number_of_lerps = 0.0f;

			if (realtime > lerp1_end) {
				number_of_lerps += 1.0f;
			}

			if (realtime > lerp2_end) {
				number_of_lerps += 1.0f;
			}

			float finallerpcount = (fCycleDuration * number_of_lerps);

			float newhold = 0.0f;
			newhold = (realtime - finallerpcount) / (totalday - finallerpcount);

			fHoldCount = newhold;
		}
		else {
			float number_of_holds = 0.0f;

			if (realtime > day_end) {
				number_of_holds += 1.0f;
			}

			float finalholdcount = number_of_holds * fHoldDuration;

			float newlerp = 0.0f;
			newlerp = (realtime - finalholdcount) / (totalday - finalholdcount);

			fTimeLerp = newlerp;
		}

		fTimePassed = time * totalday;
	}
}

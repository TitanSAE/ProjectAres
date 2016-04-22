using UnityEngine;
using System.Collections;

public class AutoIntensity : MonoBehaviour {

	public Gradient nightDayColor;

	public float maxIntensity = 3f;
	public float minIntensity = 0f;
	public float minPoint = -0.2f;

	public float maxAmbient = 1f;
	public float minAmbient = 0f;
	public float minAmbientPoint = -0.2f;


	public Gradient nightDayFogColor;
	public AnimationCurve fogDensityCurve;
	public float fogScale = 1f;

	public float dayAtmosphereThickness = 0.4f;
	public float nightAtmosphereThickness = 0.87f;

	public Vector3 dayRotateSpeed;
	public Vector3 nightRotateSpeed;

	float skySpeed = 1;


	Light mainLight;
	Skybox sky;
	Material skyMat;

	public DayNightCycle cycle;
	public float dot;
	public float ambdot;

	public float fDay;
	public float fNight;
	public bool bDayOnce;

	void Start () 
	{
	
		mainLight = GetComponent<Light>();
		skyMat = RenderSettings.skybox;

	}

	void Update () 
	{
	
		float tRange = 1 - minPoint;
		dot = Mathf.Clamp01 ((Vector3.Dot (mainLight.transform.forward, Vector3.down) - minPoint) / tRange);
		float i = ((maxIntensity - minIntensity) * dot) + minIntensity;

		mainLight.intensity = i;

		tRange = 1 - minAmbientPoint;
		ambdot = Mathf.Clamp01 ((Vector3.Dot (mainLight.transform.forward, Vector3.down) - minAmbientPoint) / tRange);
		i = ((maxAmbient - minAmbient) * ambdot) + minAmbient;
		RenderSettings.ambientIntensity = i;


		mainLight.color = nightDayColor.Evaluate(ambdot);
		RenderSettings.ambientLight = mainLight.color;

		RenderSettings.fogColor = nightDayFogColor.Evaluate(ambdot);
		RenderSettings.fogDensity = fogDensityCurve.Evaluate(ambdot) * fogScale;

		i = ((dayAtmosphereThickness - nightAtmosphereThickness) * ambdot) + nightAtmosphereThickness;
		skyMat.SetFloat ("_AtmosphereThickness", i);

		if (ambdot > 0) {
			transform.Rotate (dayRotateSpeed * Time.deltaTime * skySpeed);
			fDay += -dayRotateSpeed.x * Time.deltaTime * skySpeed;
			fNight = 0;
		}
		else {
			fDay = 0;
			transform.Rotate (nightRotateSpeed * Time.deltaTime * skySpeed);
			fNight += -nightRotateSpeed.x * Time.deltaTime * skySpeed;
		}

		if (Input.GetKeyDown (KeyCode.F5)) skySpeed *= 0.5f;
		if (Input.GetKeyDown (KeyCode.F6)) skySpeed *= 2f;

		Debug.Log(dot.ToString() + " ||| " + fDay.ToString() + " /// " + fNight.ToString());

		if (fDay > 0) {
			//(fDay / dayRotateSpeed.x * -94)
			cycle.fDayPortion = (fDay / 188) * 0.5f;
		}
		else {
			//(fNight / nightRotateSpeed.x * -52)
			cycle.fDayPortion = 0.5f + ((fNight / 156) * 0.5f);
		}

		//Count days
		if (fDay > 0 && !bDayOnce) {
			bDayOnce = true;
			cycle.iDayCount++;
		}
		if (fNight > 0 && bDayOnce) {
			bDayOnce = false;
		}
	}
}

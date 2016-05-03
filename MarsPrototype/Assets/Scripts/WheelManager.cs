using UnityEngine;
using System.Collections.Generic;

public class WheelManager : MonoBehaviour {

	public List<WheelCollider> wheels;
	[Header("Wheel Settings")]
	public float mass = 20f;
	public float radius = 0.5f;
	public float wheelDampRate = 0.25f;
	public float suspensionDistance = 0.6f;
	public float forceAppPointDist = 0f;
	public Vector3 center = new Vector3(0f,0f,0f);
	[Space]
	public float wheelSpinDown = 3.0f;
	float lerpTime;
	float currentLerpTime = 0f;
	public float spinBrakeTorque = 1000f;
	enum LerpMode { Serp, Coserp, Exponential, Smoothstep, Smootherstep };
	[SerializeField]
	LerpMode smoothMode = LerpMode.Smootherstep;
	[Header("Spring Settings")]
	public float spring = 35000f;
	public float damper = 4500f;
	public float targetPosition = 0.5f;
	[Header("Forward Friction")]
	public float extremumSlipF = 1f;
	public float extremumValueF = 1.5f;
	public float asymptoteSlipF = 1f;
	public float asymptoteValueF = 1f;
	public float stiffnessF = 1f;
	[Header("Sideways Friction")]
	public float extremumSlipS = 0.8f;
	public float extremumValueS = 1f;
	public float asymptoteSlipS = 0.5f;
	public float asymptoteValueS = 0.75f;
	public float stiffnessS = 1f;
	[Header("Wheel Tread Marks")]
	public bool drawTreadMarks = true;

	AudioSource wheelGroundAudio;   

	void Start ()
	{
		foreach (WheelCollider wheel in wheels)
		{
			JointSpring wheelSpring = new JointSpring();
			wheelSpring.spring = spring;
			wheelSpring.targetPosition = targetPosition;
			wheelSpring.damper = damper;

			WheelFrictionCurve forwardFriction = new WheelFrictionCurve();
			forwardFriction.extremumSlip = extremumSlipF;
			forwardFriction.extremumValue = extremumValueF;
			forwardFriction.asymptoteSlip = asymptoteSlipF;
			forwardFriction.asymptoteValue = asymptoteValueF;
			forwardFriction.stiffness = stiffnessF;

			WheelFrictionCurve sidewaysFriction = new WheelFrictionCurve();
			sidewaysFriction.extremumSlip = extremumSlipS;
			sidewaysFriction.extremumValue = extremumValueS;
			sidewaysFriction.asymptoteSlip = asymptoteSlipS;
			sidewaysFriction.asymptoteValue = asymptoteValueS;
			sidewaysFriction.stiffness = stiffnessF;

			wheel.mass = mass;
			wheel.radius = radius;
			wheel.wheelDampingRate = wheelDampRate;
			wheel.suspensionDistance = suspensionDistance;
			wheel.forceAppPointDistance = forceAppPointDist;
			wheel.center = center;
			wheel.suspensionSpring = wheelSpring;
			wheel.forwardFriction = forwardFriction;
			wheel.sidewaysFriction = sidewaysFriction;

			ParticleSystem treadEmitter = wheel.GetComponentInChildren<ParticleSystem>();
			if (treadEmitter != null) treadEmitter.enableEmission = false;
			//print(treadEmitter);
		}
		lerpTime = wheelSpinDown;
		wheelGroundAudio = GetComponentInChildren<AudioSource>();
	}

	void Update()
	{
		if (drawTreadMarks) TreadMarkEnable();
		int wheelsGrounded = 0;
		bool wheelActive = false;
		foreach (WheelCollider wheel in wheels)
		{
			if (wheel.isGrounded == true) wheelsGrounded += 1;
			if (wheel.rpm > 15.0f || wheel.rpm < -15.0f) wheelActive = true;
			//print(wheel.rpm);
			//print(wheel.motorTorque);
			if (!wheel.isGrounded && wheel.motorTorque == 0 && wheel.rpm != 0)
			{
				currentLerpTime += Time.deltaTime;

				float time = CalculateLerp(); ;
				wheel.brakeTorque = Mathf.Lerp(wheel.brakeTorque, spinBrakeTorque, time);
				//print(wheel.rpm + " | " + Input.GetAxis("Vertical") + " | " + wheel.brakeTorque);

				if (currentLerpTime > lerpTime) currentLerpTime = lerpTime;                
			}
			if(Input.GetAxis("Vertical") != 0f) wheel.brakeTorque = 0.0f;
		}
		if (wheelsGrounded > 0 && wheelActive)
		{
			if (!wheelGroundAudio.isPlaying && wheelGroundAudio != null) wheelGroundAudio.Play();
		}
		else wheelGroundAudio.Stop();
	}

	float CalculateLerp()
	{
		float t = currentLerpTime / lerpTime;
		switch (smoothMode)
		{
		case LerpMode.Serp:
			t = Mathf.Sin(t * Mathf.PI * 0.5f);
			break;
		case LerpMode.Coserp:
			t = 1f - Mathf.Cos(t * Mathf.PI * 0.5f);
			break;
		case LerpMode.Exponential:
			t = t * t;
			break;
		case LerpMode.Smoothstep:
			t = t * t * (3f - 2f * t);
			break;
		case LerpMode.Smootherstep:
			t = t * t * t * (t * (6f * t - 15f) + 10f);
			break;
		}
		return t;
	}

	void TreadMarkEnable()
	{
		foreach (WheelCollider wheel in wheels)
		{
			// http://docs.unity3d.com/ScriptReference/TerrainData.GetSteepness.html
			/*
            RaycastHit hit;
            ParticleSystem treadEmitter = wheel.GetComponentInChildren<ParticleSystem>();
            Vector3 direction = wheel.transform.position;
            direction.y = direction.y - 8f;
            if (Physics.Raycast(wheel.transform.position, direction, out hit, 4.0f))
            {
                if (treadEmitter != null && !treadEmitter.enableEmission) treadEmitter.enableEmission = true;
                print("hit");
            }
            else
            {
                if (treadEmitter != null) treadEmitter.enableEmission = false;
                print("miss");
            }
            print(wheel.transform.position + "   |   " + direction);
            */
			ParticleSystem treadEmitter = wheel.GetComponentInChildren<ParticleSystem>();
			if (wheel.isGrounded)
			{
				if (treadEmitter != null && !treadEmitter.enableEmission) treadEmitter.enableEmission = true;
				print("hit");
			}
			else
			{
				if (treadEmitter != null) treadEmitter.enableEmission = false;
				print("miss");
			}
			print(wheel.rpm);
			//print(wheel.transform.position + "   |   " + direction);
		}
	}
}

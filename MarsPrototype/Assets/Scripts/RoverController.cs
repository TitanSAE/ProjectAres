using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class AxleWheelInfo
{
	public WheelCollider leftWheel;
	public WheelCollider rightWheel;
	public bool motor;
	public bool steering;
	public bool rearSteer;
}

public class RoverController : MonoBehaviour
{
	[Header("Wheel Settings")]
	public List<AxleWheelInfo> axleWheels; // the information about each individual axle
	/*
    [Space]
    public float brakeTime = 3.0f;
    float lerpTime;
    float currentLerpTime = 0f;
    
    enum LerpMode { Serp, Coserp, Exponential, Smoothstep, Smootherstep };
    [SerializeField]
    LerpMode mode = LerpMode.Smootherstep;
    */
	public bool subStepEnable = true;
	public float subStepSpeed = 5.0f;
	public int subStepStepsBelow = 15;
	public int subStepStepsAbove = 15;
	Rigidbody rigidBody;
	float rigidBodyMass;

	MarsPlayer ply;

	public float fNeoDamageAmount = 5;
	public float fNeoRollDamageMutliplier = 3;

	[Header("Rover Settings")]    
	public Vector3 centreOfMass = new Vector3(0f, -0.1f, 0f);
	public float maxSteeringAngle = 17; // maximum steer angle the wheel can have
	public float maxMotorTorque = 800; // maximum torque the motor can apply to wheel
	[Space]
	public bool enableAutoGear = true;
	public float torgueGearMultiplier = 2.0f;
	public float minGearRPM = 50.0f;
	public float minGearTorgue = 0.9f;
	[Space]
	public bool enableAutoBrake = true;
	public float RPMBrakeRange = 8.0f;
	public float minBreakUpright = 0.6f;
	//public bool stickyBreaking = false;
	int minBrakeWheels; // will be greater than this, meaning 4, so all have to be grounded
	[Space]
	public bool enableAutoUpright = true;
	public float autoRightCooldown = 5f;
	public float autoRightOffset = 3f;
	[Space]
	public bool enableAntiSki = true;
	public float roverSkiingCooldown = 0.5f;
	public float skiingMassYOffset = 0.1f; // this will be subtracted, meaning: negative Y
	public float skiingMassBoost = 0.5f;
	[Header("Rover Damage")]
	public bool allowDamage = true;
	public float carMaxHealth = 1.0f;
	public float carMinHealth = 0.6f;
	public float damageAmount = 0.005f;
	public float damageRollMultiplier = 10.0f;
	public GameObject dmgEmitter01;
	public GameObject dmgEmitter02;
	public GameObject dmgEmitter03;
	[Header("Audio Data")]
	public GameObject secondaryAudioSource;
	public GameObject tertiaryAudioSource;
	public AudioClip roverMotor;
	public AudioClip roverServo;
	public AudioClip roverDamage;
	public AudioClip roverFlipped;
	public AudioClip roverWheelGravel;


	float autoRightTimer;
	float roverSkiingTimer;
	Vector3 skiingCentreOfMass;
	bool roverRighted = false;

	bool steeringActive = false;

	// finds the corresponding visual wheel
	// correctly applies the transform
	public void ApplyLocalPositionToVisuals(WheelCollider collider, bool rearSteer)
	{
		if (collider.transform.childCount == 0)
		{
			return;
		}

		Transform visualWheel = collider.transform.GetChild(0);

		Vector3 position;
		//Vector3 wheelPosition;
		Quaternion wheelRotation;
		Quaternion rotation;

		collider.GetWorldPose(out position, out rotation);

		//print(rotation.y);
		if (rearSteer)
		{
			wheelRotation.x = rotation.x;
			wheelRotation.y = -rotation.y;
			wheelRotation.z = rotation.z;
			wheelRotation.w = rotation.w;
			visualWheel.transform.rotation = wheelRotation;
		}
		else { visualWheel.transform.rotation = rotation; }
		visualWheel.transform.position = position;
		//print(wheelRotation.y);

	}

	public void Awake()
	{
		rigidBody = GetComponent<Rigidbody>();
		rigidBody.centerOfMass = centreOfMass;
		skiingCentreOfMass = centreOfMass;
		skiingCentreOfMass.y = skiingCentreOfMass.y - skiingMassYOffset;
		rigidBodyMass = rigidBody.mass;
		if (subStepEnable)
		{
			foreach (AxleWheelInfo axleInfo in axleWheels)
			{
				axleInfo.leftWheel.ConfigureVehicleSubsteps(subStepSpeed, subStepStepsBelow, subStepStepsAbove);
				axleInfo.rightWheel.ConfigureVehicleSubsteps(subStepSpeed, subStepStepsBelow, subStepStepsAbove);
			}
		}
		GetComponentInChildren<AudioSource>().clip = roverMotor;
		secondaryAudioSource.GetComponentInChildren<AudioSource>().clip = roverServo;
		tertiaryAudioSource.GetComponentInChildren<AudioSource>().clip = roverWheelGravel;
		autoRightTimer = autoRightCooldown;
		roverSkiingTimer = roverSkiingCooldown;
		//print(axleWheels.Count);
		minBrakeWheels = axleWheels.Count + 1;
		//print(minBrakeWheels);
		//if (stickyBreaking) minBrakeWheels = 1; // will use "greater than" this as 1 means none
	}

	void Start() {
		ply = GameObject.FindGameObjectWithTag("Player").GetComponent<MarsPlayer>();
	}

	public void FixedUpdate()
	{
		float motor = (maxMotorTorque * carMaxHealth) * Input.GetAxis("Vertical");
		//float steering = maxSteeringAngle * Input.GetAxis("Horizontal");
		float steering = (maxSteeringAngle * carMaxHealth) * Input.GetAxis("Horizontal");
		foreach (AxleWheelInfo axleInfo in axleWheels)
		{
			if (axleInfo.steering && !axleInfo.rearSteer)
			{
				axleInfo.leftWheel.steerAngle = steering;
				axleInfo.rightWheel.steerAngle = steering;
			}
			if (axleInfo.steering && axleInfo.rearSteer)
			{
				axleInfo.leftWheel.steerAngle = -steering;
				axleInfo.rightWheel.steerAngle = -steering;
			}
			if (axleInfo.motor)
			{
				axleInfo.leftWheel.motorTorque = motor;
				axleInfo.rightWheel.motorTorque = motor;
				if (enableAutoGear)
				{
					if (motor > (maxMotorTorque * carMaxHealth) * minGearTorgue && (axleInfo.leftWheel.rpm < minGearRPM || axleInfo.rightWheel.rpm < minGearRPM))
					{
						axleInfo.leftWheel.motorTorque = motor * torgueGearMultiplier;
						axleInfo.rightWheel.motorTorque = motor * torgueGearMultiplier;
					}
				}
				//print(motor);
			}
			if (axleInfo.rearSteer)
			{
				ApplyLocalPositionToVisuals(axleInfo.leftWheel, false);
				ApplyLocalPositionToVisuals(axleInfo.rightWheel, false);
			}
			else
			{
				ApplyLocalPositionToVisuals(axleInfo.leftWheel, false);
				ApplyLocalPositionToVisuals(axleInfo.rightWheel, false);
			}
		}
	}

	public void Update()
	{
		Vector3 currentTransform = transform.up;
		Vector3 currentVelocity = transform.InverseTransformDirection(rigidBody.velocity);
		//print(carMaxHealth);
		//print(transform.InverseTransformDirection(rigidBody.velocity).z);
		//print(transform.up);
		//print(currentVelocity);
		//print(transform.localRotation);

		//print(autoRightTimer + " | RoverFlipping: " + roverRighted + " | " + transform.up.y);

		// the following is for the auto upright feature when the rover ends up stuck on its side or flipped over
		// not sure it is working *exactly* correct:
		// http://answers.unity3d.com/questions/684813/transformup-is-localspace-the-script-reference-say.html
		bool velocityStatic = false;
		if (currentVelocity.z > -0.2f && currentVelocity.z < 0.2f) { velocityStatic = true; }
		if (transform.up.y < 0.125f && velocityStatic == true && roverRighted == false && enableAutoUpright == true)        
		{
			Vector3 currentPosition = transform.position;
			currentPosition.y = currentPosition.y + autoRightOffset;
			transform.position = currentPosition;
			currentTransform.y = 1.0f;
			transform.up = currentTransform;
			rigidBody.velocity = new Vector3(0f, 0f, 0f);
			DamageHealth(damageAmount * damageRollMultiplier);
			NeoDamageHealth(fNeoDamageAmount * fNeoRollDamageMutliplier);
			secondaryAudioSource.GetComponentInChildren<AudioSource>().clip = roverFlipped;
			secondaryAudioSource.GetComponentInChildren<AudioSource>().Play();
			roverRighted = true;
		}
		if (autoRightTimer >= 0f && roverRighted == true)
		{
			autoRightTimer -= Time.deltaTime;
		}
		if (autoRightTimer < 0f && roverRighted == true)
		{
			autoRightTimer = autoRightCooldown;
			roverRighted = false;
		}

		// the following is if the rover is "skiing" on its side
		// this could may help in other situations as well
		if ((IsGrounded() == 2 || IsGrounded() == 3) && transform.up.y < 0.5f && enableAntiSki)
		{
			if (roverSkiingTimer >= 0f)
			{
				roverSkiingTimer -= Time.deltaTime;
			}
			if (roverSkiingTimer < 0f)
			{
				rigidBody.centerOfMass = skiingCentreOfMass;
				rigidBody.mass = rigidBodyMass + (rigidBodyMass * skiingMassBoost);
			}
		}
		if (IsGrounded() == 4)
		{
			rigidBody.centerOfMass = centreOfMass;
			rigidBody.mass = rigidBodyMass;
			roverSkiingTimer = roverSkiingCooldown;
		}

		//print(IsGrounded() + " | " + rigidBody.centerOfMass + " | " + rigidBody.mass + " | " + transform.up.y + " | " + roverSkiingTimer + " | " + carMaxHealth);
		//print(GetWheelsGrounded());

		if (Input.GetButton("Vertical"))
		{
			AudioSource audio = GetComponentInChildren<AudioSource>();
			if (!audio.isPlaying) audio.Play();
		}
		else GetComponentInChildren<AudioSource>().Stop();
		// if steering left or right, play servo sound
		if (Input.GetButtonDown("Horizontal"))
		{
			AudioSource audio = secondaryAudioSource.GetComponentInChildren<AudioSource>();
			//make sure to play only once, and only, if not with steering active
			if (!audio.isPlaying && !steeringActive)
			{
				audio.clip = roverServo;
				audio.Play();
			}
		}
		// for if steering active check above
		if (Input.GetButton("Horizontal")) steeringActive = true;
		else steeringActive = false;

		if (allowDamage) { RenderDamage(); }        
	}

	void LateUpdate()
	{

		float currentTorque = 0f;
		float rpm = 0f;
		foreach (AxleWheelInfo axleInfo in axleWheels)
		{            
			currentTorque = axleInfo.leftWheel.motorTorque;
			rpm = axleInfo.leftWheel.rpm;
			currentTorque = axleInfo.rightWheel.motorTorque;
			rpm = axleInfo.rightWheel.rpm;
		}
		//print(currentTorque + " | " + rpm);

		if (enableAutoBrake == true && currentTorque == 0f && rpm > -RPMBrakeRange && rpm < RPMBrakeRange)
		{
			//if(IsGrounded() > minBrakeWheels && transform.up.y > minBreakUpright) rigidBody.velocity = new Vector3(0f, 0f, 0f);
			if (GetWheelsGrounded() >= minBrakeWheels && transform.up.y > minBreakUpright) rigidBody.velocity = new Vector3(0f, 0f, 0f);
		}
		//print(rpm + " | " + transform.up.y + " | " + IsGrounded() + " > " + minBrakeWheels + " | " + breaking);
	}

	void RenderDamage()
	{
		if (carMaxHealth <= 0.85f && dmgEmitter01 != null) { dmgEmitter01.SetActive(true); }
		if (carMaxHealth <= 0.75f && dmgEmitter02 != null) { dmgEmitter02.SetActive(true); }
		if (carMaxHealth <= 0.65f && dmgEmitter03 != null) { dmgEmitter03.SetActive(true); }
	}

	void NeoDamageHealth(float damage) {
		ply.fHealth -= damage;
	}

	void OnCollisionEnter(Collision collision)
	{
		DamageHealth(damageAmount);
		NeoDamageHealth(fNeoDamageAmount);
		AudioSource audio = secondaryAudioSource.GetComponentInChildren<AudioSource>();
		audio.clip = roverDamage;
		audio.Play();
		//AutoRightVehicle();
	}

	void DamageHealth(float damage)
	{
		if (allowDamage)
		{
			carMaxHealth = carMaxHealth - damage;
			if (carMaxHealth < carMinHealth) { carMaxHealth = carMinHealth; }
			//print(carMaxHealth);
		}
	}

	// Return int: 1 none, 2 is left, 3 is right, 4 is both..?
	int IsGrounded()
	{
		int wheelsGrounded = 0;
		bool leftGrounded = true;
		bool rightGrounded = true;
		foreach (AxleWheelInfo axleInfo in axleWheels)
		{
			if(axleInfo.leftWheel.isGrounded == false) leftGrounded = false;
			if(axleInfo.rightWheel.isGrounded == false) rightGrounded = false;
		}
		if(leftGrounded == true && rightGrounded == true) wheelsGrounded = 4; // All are
		if (leftGrounded == false && rightGrounded == true) wheelsGrounded = 3; // Right are
		if (leftGrounded == true && rightGrounded == false) wheelsGrounded = 2; // Left are
		if (leftGrounded == false && rightGrounded == false) wheelsGrounded = 1; // None are
		return wheelsGrounded;
	}

	int GetWheelsGrounded()
	{
		int wheelsGrounded = 0;
		foreach (AxleWheelInfo axleInfo in axleWheels)
		{
			if (axleInfo.leftWheel.isGrounded == true) wheelsGrounded += 1;
			if (axleInfo.rightWheel.isGrounded == true) wheelsGrounded += 1;
		}
		return wheelsGrounded;
	}
	/*
    float CalculateLerp()
    {
        float t = currentLerpTime / lerpTime;
        switch (mode)
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
    */
	/*
	void OnCollisionExit(Collision collision)
	{
		//AutoRightVehicle();
	}

	void AutoRightVehicle()
	{
		Vector3 currentTransform = transform.up;
		Vector3 currentVelocity = transform.InverseTransformDirection(rigidBody.velocity);
		bool velocityStatic = false;
		if (currentVelocity.z > -0.2f && currentVelocity.z < 0.2f) { velocityStatic = true; }// print(velocityStatic); }
		if (transform.up.y < 0.125f && velocityStatic == true)
		{
			rigidBody.velocity = new Vector3(0f, 0f, 0f);
			Vector3 currentPosition = transform.position;
			currentPosition.y = currentPosition.y + 2.5f;
			transform.position = currentPosition;
			currentTransform.y = 1.0f;
			transform.up = currentTransform;
			DamageHealth(damageAmount * damageRollMultiplier);
			secondaryAudioSource.GetComponentInChildren<AudioSource>().clip = roverFlipped;
			secondaryAudioSource.GetComponentInChildren<AudioSource>().Play();
			print("CORRECTED!");
		}
	}
	*/
}

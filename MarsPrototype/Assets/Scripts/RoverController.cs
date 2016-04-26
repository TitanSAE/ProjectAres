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
    [Space]
    public bool subStepEnable = true;
    public float subStepSpeed = 5.0f;
    public int subStepStepsBelow = 15;
    public int subStepStepsAbove = 15;
    [Header("Rover Settings")]
    public float maxMotorTorque = 800; // maximum torque the motor can apply to wheel
    public float maxSteeringAngle = 17; // maximum steer angle the wheel can have
    public Vector3 centreOfMass = new Vector3(0f, -0.5f, 0f);
    Rigidbody rigidBody;
	public bool enableAutoGear = true;
	public float torgueGearMultiplier = 2.0f;
	public float minGearRPM = 50.0f;
	public float minGearTorgue = 0.9f;
	public bool enableAutoBrake = true;
	public float RPMBrakeRange = 6.0f;

    [Header("Rover Damage")]
    public bool allowDamage = true;
    public float carMaxHealth = 1.0f;
    public float carMinHealth = 0.6f;
    public float damageAmount = 0.005f;
    public float damageRollMultiplier = 10.0f;

	public float fNeoDamageAmount = 5;
	public float fNeoRollDamageMutliplier = 3;

    [Header("Audio Data")]
    public GameObject secondaryAudioSource;
    public GameObject tertiaryAudioSource;
    public AudioClip roverMotor;
    public AudioClip roverServo;
    public AudioClip roverDamage;
    public AudioClip roverFlipped;
    public AudioClip roverWheelGravel;

	public GameObject dmgEmitter01;
	public GameObject dmgEmitter02;
	public GameObject dmgEmitter03;

	//public int iGear;

	MarsPlayer ply;

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
                //print(axleInfo.rightWheel.rpm);

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

	void Start() {
		ply = GameObject.FindGameObjectWithTag("Player").GetComponent<MarsPlayer>();
	}
    
    public void Update()
    {
        Vector3 currentTransform = transform.up;
        Vector3 currentVelocity = transform.InverseTransformDirection(rigidBody.velocity);
        //Vector3 staticVelocity = new Vector3(0.5f, 0.5f, 0.5f);
        //print(carMaxHealth);
        //print(transform.InverseTransformDirection(rigidBody.velocity).z);
        //print(transform.up);
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
            DamageHealth(damageAmount* damageRollMultiplier);
			NeoDamageHealth(fNeoDamageAmount * fNeoRollDamageMutliplier);
            secondaryAudioSource.GetComponentInChildren<AudioSource>().clip = roverFlipped;
            secondaryAudioSource.GetComponentInChildren<AudioSource>().Play();
            //print("CORRECTED!");
        }

        // moving forward of backwards, play motor sound
        if (Input.GetButton("Vertical"))
        {
            AudioSource audio = GetComponentInChildren<AudioSource>();
            if(!audio.isPlaying) audio.Play();
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

		//Gears
		//float totalrpm = GetAverageWheelsRPM();
		//float totaltorque = GetAverageWheelsTorque();

		//Debug.Log("RPM: " + totalrpm.ToString() + " | TORQUE: " + totaltorque.ToString());

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
			rigidBody.velocity = new Vector3(0f, 0f, 0f);
		}
	}

	float GetAverageWheelsRPM() {
		float totalrpm = 0;
		foreach (AxleWheelInfo axleInfo in axleWheels) {
			//Debug.Log(axleInfo.leftWheel.rpm.ToString() + " | " + axleInfo.rightWheel.rpm.ToString());
			totalrpm += ((axleInfo.leftWheel.rpm + axleInfo.rightWheel.rpm) / 2);
		}
		totalrpm = totalrpm / axleWheels.Count;

		return totalrpm;
	}

	float GetAverageWheelsTorque() {
		float totaltorque = 0;
		foreach (AxleWheelInfo axleInfo in axleWheels) {
			totaltorque += ((axleInfo.leftWheel.motorTorque + axleInfo.rightWheel.motorTorque) / 2);
		}
		totaltorque = totaltorque / axleWheels.Count;

		return totaltorque;
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

	void NeoDamageHealth(float damage) {
		ply.fHealth -= damage;
	}

	void RenderDamage() {
		if (carMaxHealth <= 0.85f && dmgEmitter01 != null) {
			dmgEmitter01.SetActive(true);
		}
		if (carMaxHealth <= 0.75f && dmgEmitter02 != null) {
			dmgEmitter02.SetActive(true);
		}
		if (carMaxHealth <= 0.65f && dmgEmitter03 != null) {
			dmgEmitter03.SetActive(true);
		}
	}
    
    void OnCollisionExit(Collision collision)
    {
        //AutoRightVehicle();
    }
    /*
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

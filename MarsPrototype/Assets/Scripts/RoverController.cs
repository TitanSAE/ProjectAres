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
    [Header("Rover Damage")]
    public bool allowDamage = true;
    public float carMaxHealth = 1.0f;
    public float carMinHealth = 0.6f;
    public float damageAmount = 0.005f;
    public float damageRollMultiplier = 10.0f;
    [Header("Audio Data")]
    public GameObject secondaryAudioSource;
    public GameObject tertiaryAudioSource;
    public AudioClip roverMotor;
    public AudioClip roverServo;
    public AudioClip roverDamage;
    public AudioClip roverFlipped;
    public AudioClip roverWheelGravel;


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
    }
    
    void OnCollisionEnter(Collision collision)
    {
        DamageHealth(damageAmount);
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

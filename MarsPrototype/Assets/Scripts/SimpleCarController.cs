using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class AxleInfo
{
    public WheelCollider leftWheel;
    public WheelCollider rightWheel;
    public bool motor;
    public bool steering;
    public bool rearSteer;
}

public class SimpleCarController : MonoBehaviour
{
    public List<AxleInfo> axleInfos; // the information about each individual axle
    [Header("Car Settings")]
    public float maxMotorTorque; // maximum torque the motor can apply to wheel
    public float maxSteeringAngle; // maximum steer angle the wheel can have
    public Vector3 centreOfMass;
    Rigidbody rigidBody;
    [Header("Car Damage")]
    public bool allowDamage = true;
    public float carMaxHealth = 1.0f;
    public float carMinHealth = 0.6f;
    public float damageAmount = 0.005f;

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
        foreach (AxleInfo axleInfo in axleInfos)
        {
            axleInfo.leftWheel.ConfigureVehicleSubsteps(5, 15, 15);
            axleInfo.rightWheel.ConfigureVehicleSubsteps(5, 15, 15);
        }
    }
    
    public void FixedUpdate()
    {
        float motor = (maxMotorTorque * carMaxHealth) * Input.GetAxis("Vertical");
        //float steering = maxSteeringAngle * Input.GetAxis("Horizontal");
        float steering = (maxSteeringAngle * carMaxHealth) * Input.GetAxis("Horizontal");
        foreach (AxleInfo axleInfo in axleInfos)
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
        print(carMaxHealth);
        //print(transform.InverseTransformDirection(rigidBody.velocity).z);
        //print(transform.up);
        bool velocityStatic = false;
        if (currentVelocity.z > -0.15f && currentVelocity.z < 0.15f) { velocityStatic = true; }// print(velocityStatic); }
        //if (Mathf.Abs(Vector3.Dot(transform.up, Vector3.down)) < 0.125f && velocityStatic == true)
        if (transform.up.y < 0.125f && velocityStatic == true)
        {
            currentTransform.y = 1.0f;
            transform.up = currentTransform;
            //print("CORRECTED!");

            // Car is primarily neither up nor down, within 1/8 of a 90 degree rotation

            // Therefore, check whether it's on either side. Otherwise, it's on front/back
            if (Mathf.Abs(Vector3.Dot(transform.right, Vector3.down)) > 0.825f)
            {
                // Car is within 1/8 of a 90 degree rotation of either side
                
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (allowDamage)
        {
            carMaxHealth = carMaxHealth - damageAmount;
            if (carMaxHealth < carMinHealth) { carMaxHealth = carMinHealth; }
        }
    }
}
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

    //GameObject parent;
    AudioSource audio;

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
        //parent = GetComponentInParent<Transform>().gameObject.GetComponent<RoverController>().tertiaryAudioSource;
        //audio = GetComponentInParent<Transform>().gameObject.GetComponent<RoverController>().tertiaryAudioSource.GetComponent<AudioSource>();
        audio = GetComponentInChildren<AudioSource>();
    }

    void Update()
    {
        if (drawTreadMarks) TreadMarkEnable();
        foreach (WheelCollider wheel in wheels)
        {
            if (wheel.isGrounded && wheel.rpm > 15.0f || wheel.rpm < -15.0f)
            {
                if (!audio.isPlaying && audio != null) audio.Play();
            }
            else audio.Stop();
            //print(wheel.rpm);
        }
    }
    void TreadMarkEnable()
    {
        foreach (WheelCollider wheel in wheels)
        {
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

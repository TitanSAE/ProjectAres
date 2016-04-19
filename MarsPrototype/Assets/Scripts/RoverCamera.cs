// Based on this script: https://unity3d.com/learn/tutorials/projects/stealth/camera-movement

using UnityEngine;
using System.Collections;

public class RoverCamera : MonoBehaviour
{
    public float smooth = 2.0f;         // The relative speed at which the camera will catch up.
    public GameObject camPositionTarget;
    public GameObject camLookAtTarget;
    public GameObject navCompass;

    private Transform player;           // Reference to the player's transform.
    private Vector3 relCameraPos;       // The relative position of the camera from the player.
    private float relCameraPosMag;      // The distance of the camera from the player.
    private Vector3 newPos;             // The position the camera is trying to reach.

    public float speedH = -0.5f;
    public float speedV = -0.5f;

    private float yaw = 0.0f;
    private float pitch = 0.0f;

    void Awake()
    {
        // Setting up the reference.
        player = camPositionTarget.transform.parent.transform;
        // Setting the relative position as the initial relative position of the camera in the scene.
        relCameraPos = camPositionTarget.transform.position;
        // relCameraPos.sqrMagnitude has better performance, however, introduced a camera bug
        relCameraPosMag = relCameraPos.magnitude - 0.5f;
        // Move camera into position and rotate at start
        transform.position = camPositionTarget.transform.position;
        Vector3 relPlayerPosition = player.position - transform.position;
        relPlayerPosition = camLookAtTarget.transform.position - transform.position;
        transform.rotation = Quaternion.LookRotation(relPlayerPosition, Vector3.up);
    }

    void FixedUpdate()
    {
        navCompass.SetActive(true);

        // The standard position of the camera is the relative position of the camera from the player.
        Vector3 standardPos = camPositionTarget.transform.position;
        // The abovePos is directly above the player at the same distance as the standard position.
        Vector3 abovePos = player.position + Vector3.up * relCameraPosMag;
        // An array of 5 points to check if the camera can see the player.
        Vector3[] checkPoints = new Vector3[5];
        // The first is the standard position of the camera.
        checkPoints[0] = standardPos;
        // The next three are 25%, 50% and 75% of the distance between the standard position and abovePos.
        checkPoints[1] = Vector3.Lerp(standardPos, abovePos, 0.25f);
        checkPoints[2] = Vector3.Lerp(standardPos, abovePos, 0.5f);
        checkPoints[3] = Vector3.Lerp(standardPos, abovePos, 0.75f);
        // The last is the abovePos.
        checkPoints[4] = abovePos;
        // Run through the check points...
        
        for (int i = 0; i < checkPoints.Length; i++)
        {
            // ... if the camera can see the player...
            if (ViewingPosCheck(checkPoints[i]))
                // ... break from the loop.
                break;
        }
        
        // Lerp the camera's position between it's current position and it's new position.
        transform.position = Vector3.Lerp(transform.position, newPos, smooth * Time.deltaTime);
        // Make sure the camera is looking at the player.
        //SmoothLookAt();

        if (Input.GetMouseButton(1))
        {
            navCompass.SetActive(true);
            RotateCamera(true);
        }
        else
        {
            navCompass.SetActive(false);
            RotateCamera(false);
        }
        SmoothLookAt();
    }

    void RotateCamera(bool active)
    {
        Vector3 currentAngles = transform.eulerAngles;
        if (active)
        {
            yaw += speedH * Input.GetAxis("Mouse X");
            pitch -= speedV * Input.GetAxis("Mouse Y");

            transform.eulerAngles = new Vector3(pitch, yaw, transform.rotation.z);
            //Quaternion rotation = transform.rotation;
            //rotation.y = 0.0f;
            //transform.rotation = rotation;
        }
        else
        {
            
            yaw = currentAngles.y;
            pitch = currentAngles.x;
            transform.eulerAngles = new Vector3(pitch, yaw, currentAngles.z);
        }
    }

    bool ViewingPosCheck(Vector3 checkPos)
    {
        RaycastHit hit;
        // If a raycast from the check position to the player hits something...
        if (Physics.Raycast(checkPos, player.position - checkPos, out hit, relCameraPosMag))
            // ... if it is not the player...
            if (hit.transform != player)
                // This position isn't appropriate.
                return false;
        // If we haven't hit anything or we've hit the player, this is an appropriate position.
        newPos = checkPos;
        return true;
    }


    void SmoothLookAt()
    {
        // Create a vector from the camera towards the player.
        Vector3 relPlayerPosition = player.position - transform.position;
        relPlayerPosition = camLookAtTarget.transform.position - transform.position;
        // Create a rotation based on the relative position of the player being the forward vector.
        Quaternion lookAtRotation = Quaternion.LookRotation(relPlayerPosition, Vector3.up);
        // Lerp the camera's rotation between it's current rotation and the rotation that looks at the player.
        transform.rotation = Quaternion.Lerp(transform.rotation, lookAtRotation, smooth * Time.deltaTime);
    }
}

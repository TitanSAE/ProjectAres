using UnityEngine;
using System.Collections;

public class DontRotate : MonoBehaviour
{
    private float xRot;
    private float yRot;
    private float zRot;

    // Use this for initialization
    void Start()
    {
        xRot = .eulerAngles.x;
        yRot = transform.rotation.eulerAngles.y;
        zRot = transform.rotation.eulerAngles.z;
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Euler(xRot, yRot, zRot);
    }
}

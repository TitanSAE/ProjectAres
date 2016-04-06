using UnityEngine;
using System.Collections;

public class SimpleMove : MonoBehaviour {

    public float fSpeed = 10;
    public Transform backLeft;
    public Transform backRight;
    public Transform frontLeft;
    public Transform frontRight;
    private RaycastHit lr;
    private RaycastHit rr;
    private RaycastHit lf;
    private RaycastHit rf;
    private Vector3 upDir;

    void Start()
    {

    }

    void Update()
    {
        if(Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector3.forward * fSpeed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(Vector3.back * fSpeed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(Vector3.up, 10 * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.down, 10 * Time.deltaTime);
        }

        Physics.Raycast(backLeft.position + Vector3.up, Vector3.down, out lr);
        Physics.Raycast(backRight.position + Vector3.up, Vector3.down, out rr);
        Physics.Raycast(frontLeft.position + Vector3.up, Vector3.down, out lf);
        Physics.Raycast(frontRight.position + Vector3.up, Vector3.down, out rf);
        upDir = (Vector3.Cross(rr.point - Vector3.up, lr.point - Vector3.up) +
                 Vector3.Cross(lr.point - Vector3.up, lf.point - Vector3.up) +
                 Vector3.Cross(lf.point - Vector3.up, rf.point - Vector3.up) +
                 Vector3.Cross(rf.point - Vector3.up, rr.point - Vector3.up)
                ).normalized;
        Debug.DrawRay(rr.point, Vector3.up);
        Debug.DrawRay(lr.point, Vector3.up);
        Debug.DrawRay(lf.point, Vector3.up);
        Debug.DrawRay(rf.point, Vector3.up);
        transform.up = upDir;

    }
}

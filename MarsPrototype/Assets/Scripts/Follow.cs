using UnityEngine;
using System.Collections;

public class Follow : MonoBehaviour {

    public Transform tTarget;

	void Update ()
    {
        this.transform.position = new Vector3(tTarget.position.x, 0, tTarget.position.z);

        Vector3 ZRotOnly = new Vector3(0, 0, -tTarget.rotation.eulerAngles.y);
        transform.rotation = new Quaternion(0, 0, 0, 0);
    }
}

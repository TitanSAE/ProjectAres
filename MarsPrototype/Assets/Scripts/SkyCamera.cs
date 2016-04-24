using UnityEngine;
using System.Collections;

public class SkyCamera : MonoBehaviour {

	public Transform target;
	private bool bSpinToFace = true;
    public LayerMask WorldMapMask;
    public LayerMask TopographicalMapMask;
    private Camera camSelf;

	void Start() {
        camSelf = GetComponent<Camera>();
	}

	void Update() {
		this.transform.position = new Vector3(target.position.x, this.transform.position.y, target.position.z);

        if (Input.GetButtonDown("MapRegular"))
        {
            camSelf.cullingMask = WorldMapMask;
        }

        if (Input.GetButtonDown("MapTopo"))
        {
            camSelf.cullingMask = TopographicalMapMask;
        }

        if (bSpinToFace) {
			//Vector3 temprot = this.transform.rotation.eulerAngles;
			//this.transform.rotation = target.rotation;
			Vector3 tempvec = new Vector3(this.transform.rotation.eulerAngles.x, target.rotation.eulerAngles.y, this.transform.rotation.eulerAngles.z);
			this.transform.rotation = Quaternion.Euler(tempvec);
		}
	}
}

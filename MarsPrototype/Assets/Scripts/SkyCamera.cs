using UnityEngine;
using System.Collections;

public class SkyCamera : MonoBehaviour {

	public Transform target;
	public bool bSpinToFace = true;
	public bool bLockToCentre = false;
    public LayerMask WorldMapMask;
    public LayerMask TopographicalMapMask;
    public Shader TopographicalShader;
    private Camera camSelf;

	public float fHoverHeight = 72.1f;

	void Start() {
        camSelf = GetComponent<Camera>();
	}

	void Update() {
		if (!bLockToCentre) {
			this.transform.position = new Vector3(target.position.x, fHoverHeight, target.position.z);
		}
		else {
			this.transform.position = new Vector3(512, fHoverHeight, 512);
		}

        if (Input.GetButtonDown("MapRegular"))
        {
            camSelf.cullingMask = WorldMapMask;
        }

        if (Input.GetButtonDown("MapTopo"))
        {
            camSelf.cullingMask = TopographicalMapMask;
            camSelf.RenderWithShader(TopographicalShader, "Topographical");
        }

        if (bSpinToFace) {
			//Vector3 temprot = this.transform.rotation.eulerAngles;
			//this.transform.rotation = target.rotation;
			Vector3 tempvec = new Vector3(this.transform.rotation.eulerAngles.x, target.rotation.eulerAngles.y, this.transform.rotation.eulerAngles.z);
			this.transform.rotation = Quaternion.Euler(tempvec);
		}
		else {
			this.transform.rotation = Quaternion.Euler(new Vector3(90, 0, 0));
		}
	}
}

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SkyCamera : MonoBehaviour {

	public Transform target;
	public bool bSpinToFace = true;
	public bool bLockToCentre = false;
    public LayerMask WorldMapMask;
    public LayerMask TopographicalMapMask;
    public Shader TopographicalShader;
    private Camera camSelf;
	public Shader skycamshader;

	public float fHoverHeight = 72.1f;
	private float fLastShadowDist;

	public Light sun;
	public Light seeall;

    public Image imPlayer;
    public Image imHomeBase;
    public Image[] imDrops;

	void Start() {
        camSelf = GetComponent<Camera>();
		//camSelf.SetReplacementShader(skycamshader, "RenderType");
	}

	void Update() {
		if (Input.GetKeyDown(KeyCode.F12)) {
			camSelf.SetReplacementShader(skycamshader, "RenderType");
		}

		if (!bLockToCentre) {
			this.transform.position = new Vector3(target.position.x, fHoverHeight, target.position.z);
		}
		else {
			this.transform.position = new Vector3(512, fHoverHeight, 512);
		}

        if (Input.GetButtonDown("MapRegular"))
        {
            camSelf.cullingMask = WorldMapMask;
            camSelf.ResetReplacementShader();
        }

        if (Input.GetButtonDown("MapTopo"))
        {
            camSelf.cullingMask = TopographicalMapMask;
            camSelf.SetReplacementShader(TopographicalShader, "Opaque");
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

    void SizeUpUI()
    {

    }

	void OnPreCull() {
		sun.enabled = false;
		seeall.enabled = true;
	}

	void OnPreRender() {
		sun.enabled = false;
		seeall.enabled = true;

		//fLastShadowDist = QualitySettings.shadowDistance;
		//QualitySettings.shadowDistance = 0;
	}

	void OnPostRender() {
		sun.enabled = true;
		seeall.enabled = false;

		//QualitySettings.shadowDistance = fLastShadowDist;
	}

}

using UnityEngine;
using System.Collections;


public class MarsBuilding : MonoBehaviour {

	public bool bGhosted = true;
	public float fLerp = 0.0f;
	public float fLerpTime = 3.0f;
	public bool bLerping = false;
	//public Renderer rendTex;
	private MarsPlayer ply;

	//public Material matGhost;
	//public Material matSolid;

	// Use this for initialization
	void Start () {
		ply = GameObject.FindGameObjectWithTag("Player").GetComponent<MarsPlayer>();
	}	
	
	// Update is called once per frame
	void Update () {
		if (bLerping) {
			fLerp += Time.deltaTime;
			//rendTex.material.Lerp (matGhost, matSolid, fLerp / fLerpTime);

			//rendTex.material.SetFloat ("_Blend", fLerp / fLerpTime);

			for (int i = 0; i < transform.childCount; i++) {
				Renderer rendTex = transform.GetChild (i).GetComponent<MeshRenderer> ();
				rendTex.material.color = Vector4.one * (fLerp / fLerpTime);
			}

			if (fLerp >= fLerpTime) {
				bLerping = false;
				bGhosted = false;
			}
		}


	}
}

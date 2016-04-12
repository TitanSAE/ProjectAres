using UnityEngine;
using System.Collections;

public class MaterialLerp : MonoBehaviour {

    public Material mTransparent;
    public Material mSolid;
    public bool bIsSolid = true;
    public bool bIsTransparent = false;
    private float fTimer = 0f;
    private Material startingMaterial;
    private Material endMaterial;

	void Start ()
    {
        startingMaterial = GetComponent<Renderer>().material;
        endMaterial = GetComponent<Renderer>().material;
    }

void Update ()
    {
        fTimer = Mathf.Clamp(fTimer, 0, 1);

        if(bIsSolid)
        {
            fTimer += Time.deltaTime;

            GetComponent<Renderer>().material.Lerp(endMaterial, mSolid, fTimer);

            if(fTimer >= 1)
            {
                endMaterial = mSolid;
            }
        }

        if (bIsTransparent)
        {
            fTimer -= Time.deltaTime;

            GetComponent<Renderer>().material.Lerp(endMaterial, mTransparent, fTimer);

            if(fTimer <= 0)
            {
                endMaterial = mTransparent;
            }
        }
	}
}

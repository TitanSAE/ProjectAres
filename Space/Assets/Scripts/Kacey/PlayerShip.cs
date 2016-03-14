using UnityEngine;
using System.Collections;
using System;


public class PlayerShip : MonoBehaviour
{
    public ParticleSystem bottomThruster;
    public ParticleSystem leftThruster;
    public ParticleSystem rightThruster;

	bool bTest = false;

    public GuiInGame Gui;

	Transform padPos;
	public GameObject landPad;

	bool OnPad = false;
 
    void Awake()
    {
        Globals.BottomThruster = bottomThruster;
        Globals.LeftThruster = leftThruster;
        Globals.RightThruster = rightThruster;
        Globals.PlayerShip = GetComponent<Rigidbody>();
    }

   
    void Start()
    {
 
        Gui = GameObject.FindWithTag("Gui").GetComponent(typeof(GuiInGame)) as GuiInGame;
     
    }

    // Update is called once per frame
    private void Update()
    {
		if (OnPad) {
			transform.position = new Vector3(padPos.position.x,transform.position.y,padPos.position.z);
			transform.GetComponent<Rigidbody> ().freezeRotation = true;

		}
		float dist = Vector3.Distance (transform.position, landPad.transform.position);
		if (dist > 1 && OnPad == true) {
			OnPad = false;
			transform.SetParent (null);
			LandingPad landingPad;
			landingPad = landPad.gameObject.GetComponent ("LandingPad") as LandingPad;
			landingPad.DeActivate ();
		}
      
    }
		

    private void OnCollisionEnter(Collision hitInfo)
    {
		if (hitInfo.gameObject.name == "LandingPad" && !bTest) {
			Debug.Log ("Landed");
			LandingPad landingPad;
			landingPad = hitInfo.gameObject.GetComponent ("LandingPad") as LandingPad;
			landingPad.Activate ();
			padPos = hitInfo.transform;
			transform.SetParent (landPad.transform.parent);
			OnPad = true;
		}


    }





    private void explode(float magnitude)
    {
        Debug.Log(magnitude);
        Destroy(gameObject);

    }


}

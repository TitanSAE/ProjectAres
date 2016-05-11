using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	public bool roverControl = true;
	public bool copterControl = false;
	public GameObject Tornado;
	bool enabled = false;
	public GameObject module;
	public GameObject Establishment;
	public GameObject Car;


	public	GameObject Level1;
	public	GameObject Level2;
	public	GameObject Level3;
	public	GameObject Level4;
	public GameObject smallPanel;
	public GameObject largePanel;

	public ParticleSystem PackageParticle;
	public ParticleSystem PackageParticle1;
	public ParticleSystem PackageParticle2;

	public GameObject NavMarker;

	public int finishedBuildings = 0;

	// Use this for initialization
	void Start () {
		
	}

	GameObject GetClosestEnemy(GameObject[] enemies)
	{
		GameObject tMin = null;
		float minDist = Mathf.Infinity;
		Vector3 currentPos = transform.position;
		foreach (GameObject t in enemies)
		{
			float dist = Vector3.Distance(t.transform.position, currentPos);
			if (dist < minDist)
			{
				tMin = t;
				minDist = dist;
			}
		}
		return tMin;
	}

	// Update is called once per frame
	void Update () {
		try{
			NavMarker.transform.position = GetClosestEnemy (GameObject.FindGameObjectsWithTag ("MarsDelivery")).transform.position + new Vector3(0,transform.localScale.y,0);
		}catch{

		}
	
		if (Input.GetKey (KeyCode.T)) {
			
			enabled = !enabled;
			Tornado.SetActive (enabled);
		}
		if (Input.GetKey (KeyCode.U)) {

			Car.GetComponent<MarsPlayer> ().fHealth = 0;
		}

		if (Input.GetKeyDown (KeyCode.U)) {
			PackageParticle.Clear ();
			PackageParticle.Play ();
		}

		if (finishedBuildings == 6) {
			UnityEngine.SceneManagement.SceneManager.LoadScene("win");
		}


//		if (Input.GetKeyDown (KeyCode.Space)) {
//			GameObject.Find ("GameManager").GetComponent<Fading> ().ResetFade ();
//			GameObject.Find ("GameManager").GetComponent<Fading> ().BeginFade (-1);
//			Instantiate(Establishment, Car.transform.position + (Car.transform.forward*1) + new Vector3(0,1.3f,0), transform.rotation);
//		}
	
	}
}

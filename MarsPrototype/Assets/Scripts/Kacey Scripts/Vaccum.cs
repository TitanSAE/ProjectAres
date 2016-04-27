  using UnityEngine;
  using System.Collections;
  using System.Collections.Generic;

public class Vaccum : MonoBehaviour {

	public List<GameObject> ItemsToVacuum = new List<GameObject>();
	public float suctionPower = 1;
	public bool isVacuuming;
	Vector3 vacuumDirection;
	public GameObject Player;
	public GameObject MarsCopter;


	void Start(){
		if (Player == null || MarsCopter == null) {
			Debug.Log ("No player or Copter");
			return;
		}
	}
	void Update(){
		
			float distPlayer = Vector3.Distance (transform.position, Player.transform.position);
			if (distPlayer < 15) {
				isVacuuming = true;
			} else if(distPlayer > 15) {
				isVacuuming = false;
			}

			

		if(isVacuuming){
			VacuumItems();
		}

	//	transform.position = new Vector3(Mathf.PingPong(Time.time * 10, 20)-10, transform.position.y, Mathf.PingPong(Time.time, 20)-10);
	}
	void VacuumItems(){
		foreach(GameObject pulledObject in ItemsToVacuum){
			vacuumDirection = transform.position - pulledObject.transform.position;
			pulledObject.GetComponent<Rigidbody>().AddForce(vacuumDirection*suctionPower*(1/Vector3.Distance(transform.position,pulledObject.transform.position)), ForceMode.Impulse);
		}
	}


}

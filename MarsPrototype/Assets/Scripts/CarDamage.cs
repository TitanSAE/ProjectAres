using UnityEngine;
using System.Collections;

public class CarDamage : MonoBehaviour {

    public GameObject carRoot;
    public float carHealth = 100f;
    // Use this for initialization
    void Start ()
    {
        //Rigidbody carRootRigid;
        //carRootRigid = gameObject.GetComponentInParent(Rigidbody);
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        print(carHealth);
    }

    void OnCollisionEnter(Collision collision)
    {
        carHealth = carHealth - 5f;

    }
}

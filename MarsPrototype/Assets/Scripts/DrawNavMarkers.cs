using UnityEngine;
using System.Collections;

public class DrawNavMarkers : MonoBehaviour {

    public GameObject navMarker;
    public float drawRadius = 6f;
    public Color32 navColor = new Color32(255, 204, 129, 255);
    // Use this for initialization
    void Start ()
    {
        GetComponent<Renderer>().material.color = navColor; //.SetColor("_Color", navColor);
        //GetComponent<Renderer>().material.SetColor("_TintColor", navColor);
        //GetComponent<Renderer>().material.SetColor("_EmissionColor", navColor);
        //GetComponent<Renderer>().material.SetColor("_ReflectColor", navColor);
        //GetComponent<Renderer>().material.SetColor("_SpecColor", navColor);
    }
	
	// Update is called once per frame
	void Update ()
    {
        //relPlayerPosition = player.position - transform.position;
        //Vector3 relativePosition = navMarker.transform.position - transform.parent.position;
        
        Vector3 pos = transform.parent.position;
        //pos.y = pos.y + 3f;
        //transform.parent.position = position;
        Vector3 relativePosition;
        relativePosition.x = navMarker.transform.position.x - transform.parent.position.x;
        relativePosition.z = navMarker.transform.position.z - transform.parent.position.z;
        relativePosition.y = pos.y;
        Vector3 navPosition;
        navPosition.x = navMarker.transform.position.x;
        navPosition.z = navMarker.transform.position.z;
        navPosition.y = pos.y;
        transform.rotation = Quaternion.LookRotation(relativePosition, Vector3.up);
        transform.position = Vector3.MoveTowards(transform.parent.position, navPosition, drawRadius);
        
        /*
        float x;
        float z;
        float y = transform.parent.position.y;
        x = Mathf.Sin(Mathf.Deg2Rad * Vector3.Angle(transform.parent.position, navMarker.transform.position));
        z = Mathf.Cos(Mathf.Deg2Rad * Vector3.Angle(transform.parent.position, navMarker.transform.position));
        transform.position = new Vector3(x, y, z);
        */
        //transform.position = navMarker.transform.position - transform.parent.position * drawRadius;
    }
}

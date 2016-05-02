using UnityEngine;
using System.Collections;

public class zoomortho : MonoBehaviour {

	Camera cam;
 
     public void Start()
     {
         cam = this.GetComponent<Camera>();
     }
 
     public void Update()
     {
         if(Input.GetKey(KeyCode.KeypadPlus))
             cam.orthographicSize -= .1f;
         if(Input.GetKey(KeyCode.KeypadMinus))
             cam.orthographicSize += .1f;
     }
}

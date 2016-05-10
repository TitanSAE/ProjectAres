using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIFollow : MonoBehaviour {

    [Header("Position")]
    public bool bFollowPosition = true;
    public bool bFollowRotation = false;
    public Transform tObjectToFollow;
    private RectTransform RectTMyOwn;
    
    void Start()
    {
        RectTMyOwn = this.GetComponent<RectTransform>();
    }

    void Update()
    {
        //This is used to make the object follow the object of your choice.
        if (bFollowPosition)
        {
            
        }

        //This is used to make the object rotate in sync with the object of your chocie.
        if (bFollowRotation)
        {
            Vector3 ZRotOnly = new Vector3(0, 0, -tObjectToFollow.localRotation.eulerAngles.y);
            //RectTMyOwn.rotation = Quaternion.Euler(ZRotOnly);
        }
    }
}

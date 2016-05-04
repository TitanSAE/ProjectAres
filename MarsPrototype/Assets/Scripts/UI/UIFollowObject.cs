using UnityEngine;
using System.Collections;

public class UIFollowObject : MonoBehaviour
{
    [Header("Position")]
    public bool bFollowPosition = true;
    public bool bFollowRotation = false;
    public Vector3 vPositionOffset = new Vector3(0, 0, 0);

    [Header("UI References")]
    public RectTransform RectTMainCanvas;
    public RectTransform RectTMyOwn;
    public Transform tObjectToFollow;
    public Camera camToRenderTo;

    void Update()
    {
        //This is used to make the object follow the object of your choice.
        if(bFollowPosition)
        {
            Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(camToRenderTo, tObjectToFollow.position + vPositionOffset);

            RectTMyOwn.anchoredPosition = (screenPoint - RectTMainCanvas.sizeDelta / 2f);
        }

        //This is used to make the object rotate in sync with the object of your chocie.
        if(bFollowRotation)
        {
            Vector3 ZRotOnly = new Vector3(0, 0, -tObjectToFollow.localRotation.eulerAngles.y);
            RectTMyOwn.rotation = Quaternion.Euler(ZRotOnly);
        }
    }
}

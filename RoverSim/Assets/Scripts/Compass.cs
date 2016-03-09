using UnityEngine;
using System.Collections;

public class Compass : MonoBehaviour
{
    public Camera MainCamera;
    public RectTransform canvasRectT;
    public RectTransform ArrowRectT;
    public Transform tObjectToFollow;

    void Update()
    {
        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(MainCamera, tObjectToFollow.position + new Vector3(0f, 0f, 0f));

        ArrowRectT.anchoredPosition = screenPoint - canvasRectT.sizeDelta / 2f;
    }
}
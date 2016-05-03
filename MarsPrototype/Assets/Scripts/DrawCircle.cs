using UnityEngine;
using System.Collections;

public class DrawCircle : MonoBehaviour {

    public int segments = 72;
    public float radius = 6f;
    public float zOffset = 2f;
    public bool overrideColor = true;
    public Color32 lineColor = new Color32(255, 204, 129, 255);
    public bool alwaysUp = true;
    public bool rotate = false;
    LineRenderer line;

    void Start()
    {
        line = gameObject.GetComponent<LineRenderer>();
        if (overrideColor)
        {
            line.SetColors(lineColor, lineColor);
        }
        line.SetVertexCount(segments + 1);
        line.useWorldSpace = false;
        CreatePoints();
    }

    void Update()
    {
        if (alwaysUp)
        {
            Quaternion rotation = transform.rotation;
            rotation = Quaternion.LookRotation(new Vector3(0, 0, 0), Vector3.up);
            transform.rotation = rotation;
        }
        if (rotate) { transform.Rotate(Vector3.right * Time.deltaTime); }
    }


    void CreatePoints()
    {
        float x;
        float z;
        float y = zOffset;

        float angle = 20f;

        for (int i = 0; i < (segments + 1); i++)
        {
            x = Mathf.Sin(Mathf.Deg2Rad * angle) * radius;
            z = Mathf.Cos(Mathf.Deg2Rad * angle) * radius;

            line.SetPosition(i, new Vector3(x, y, z));

            angle += (360f / segments);
        }
    }



}

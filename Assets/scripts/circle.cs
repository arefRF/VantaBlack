using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class circle : MonoBehaviour {
    public int vertexcount = 40;
    public float line_width = 0.2f;
    public float radius = 0f;

    private LineRenderer linerenderer;

    private void awake()
    {
        linerenderer = GetComponent<LineRenderer>();
        setup_circle();
    }
    private void setup_circle()
    {
        linerenderer.widthMultiplier = line_width;
        float DeltaTheta = (2f * Mathf.PI) / vertexcount;
        float theta = 0f;
        linerenderer.positionCount = vertexcount;
        Vector3 OldPos = transform.position + new Vector3(radius, 0f, 0f);
        for (int i = 0; i <= linerenderer.positionCount; i++)
        {
            Vector3 pos = new Vector3(radius * Mathf.Cos(theta), radius * Mathf.Sin(theta), 0f);
            linerenderer.SetPosition(i, pos);
            OldPos = transform.position + pos;
            theta += DeltaTheta;
        }


    }
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        float DeltaTheta = (2f * Mathf.PI) / vertexcount;
        float theta = 0f;

        Vector3 OldPos = transform.position + new Vector3(radius,0f,0f);

        for (int i = 0; i <= vertexcount; i++)
        {
            Vector3 pos = new Vector3(radius * Mathf.Cos(theta), radius * Mathf.Sin(theta), 0f);
            Gizmos.DrawLine(OldPos, transform.position + pos);
            OldPos = transform.position + pos;

            theta += DeltaTheta;
        }

    }
#endif
}

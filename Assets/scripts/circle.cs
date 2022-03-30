using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Circle : MonoBehaviour {
    public int vertexcount = 40;
    public float line_width = 0.2f;
    public float radius = 0f;

    private LineRenderer linerenderer;
    private float waittime;
    private int currentN, currentColorNum;
    private Coroutine CircleCoroutine, ReverseCircleCoRoutine;
    private GameObject LockIcon;
    public bool CircleFinished { get; set; }
    void Start()
    {
        linerenderer = GetComponent<LineRenderer>();
        GameObject temp = Toolkit.GetObjectInChild(transform.parent.gameObject, "Icon");
        LockIcon = Toolkit.GetObjectInChild(temp, "LockIcon");
        waittime = 1.3f / vertexcount;
        currentN = vertexcount;
        currentColorNum = 255;
        CircleFinished = false;
    }
    public void StartCircleForLaser(Color color)
    {
        if (ReverseCircleCoRoutine != null)
            StopCoroutine(ReverseCircleCoRoutine);
        CircleCoroutine = StartCoroutine(setupCircleForLaser(color));
    }
    public void StartCircleForLaser()
    {
        if (ReverseCircleCoRoutine != null)
            StopCoroutine(ReverseCircleCoRoutine);
        CircleCoroutine = StartCoroutine(setupCircleForKey());
    }
    public void StopCircle(Color color)
    {
        CircleFinished = false;
        if (CircleCoroutine != null)
            StopCoroutine(CircleCoroutine);
        LockIcon.transform.rotation = Quaternion.Euler(0, 0, 0);
        ReverseCircleCoRoutine = StartCoroutine(reverseCircle(color));
    }
    private IEnumerator setupCircleForKey()
    {
        int n = currentN - 1;
        bool boolean = true;
        int i = 1;
        while (n >= -1)
        {
            Color color = new Color(1, 1, 1);
            setup_circle(n, color);
            if (boolean)
            {
                if (i >= 10)
                    boolean = false;
                LockIcon.transform.rotation = Quaternion.Euler(0, 0, i);
                i += 10;
            }
            else
            {
                if (i <= -10)
                    boolean = true;
                LockIcon.transform.rotation = Quaternion.Euler(0, 0, i);
                i -= 10;
            }
            n--;
            currentN = n;
            yield return new WaitForSeconds(waittime);
        }
        CircleFinished = true;
        SpriteRenderer tmpspr = LockIcon.GetComponent<SpriteRenderer>();
        tmpspr.sprite = Starter.GetEngine().initializer.sprite_Unlock;
        float tempwidth = line_width;
        float tempalpha = 255;
        while (tempwidth > 0)
        {
            tmpspr.color = new Color(tmpspr.color.r, tmpspr.color.g, tmpspr.color.b, tempalpha / 255f);
            tempalpha = Mathf.Max(0, tempalpha - 11);
            tempwidth -= 0.002f;
            linerenderer.startWidth = tempwidth;
            linerenderer.endWidth = tempwidth;
            yield return new WaitForSeconds(waittime / 2);
        }
        LockIcon.SetActive(false);
        tmpspr.color = new Color(tmpspr.color.r, tmpspr.color.g, tmpspr.color.b, 1);
        linerenderer.startWidth = 0;
        linerenderer.endWidth = 0;
        CircleFinished = false;
        CircleCoroutine = null;
    }

    private IEnumerator setupCircleForLaser(Color Circlecolor)
    {
        int n = currentN - 1;
        while (n >= -1)
        {
            Color color = new Color(1, currentColorNum / 255f, currentColorNum / 255f);
            setup_circle(n, color);
            LockIcon.GetComponent<SpriteRenderer>().color = color;
            currentColorNum = Mathf.Max(currentColorNum - 8, 0);
            n--;
            currentN = n;
            yield return new WaitForSeconds(waittime);
        }
        CircleFinished = true;
        float tempwidth = line_width;
        while (tempwidth > 0)
        {
            tempwidth -= 0.002f;
            linerenderer.startWidth = tempwidth;
            linerenderer.endWidth = tempwidth;
            yield return new WaitForSeconds(waittime / 2);
        }
        linerenderer.startWidth = 0;
        linerenderer.endWidth = 0;
        CircleFinished = false;
        CircleCoroutine = null;
    }
    private IEnumerator reverseCircle(Color Circlecolor)
    {
        int n = currentN;
        while (n <= vertexcount - 1)
        {
            Color color = new Color(1, currentColorNum / 255f, currentColorNum / 255f);
            setup_circle(n, color);
            LockIcon.GetComponent<SpriteRenderer>().color = color;
            currentColorNum = Mathf.Min(currentColorNum + 8, 255);
            n++;
            currentN = n;
            yield return new WaitForSeconds(waittime);
        }
        ReverseCircleCoRoutine = null;
    }

    private void setup_circle(int n, Color color)
    {
        linerenderer.widthMultiplier = line_width;
        float DeltaTheta = (2f * Mathf.PI) / vertexcount;
        float theta = 0f;
        linerenderer.positionCount = vertexcount - n;
        Vector3 OldPos = transform.position;
        linerenderer.startColor = color;
        linerenderer.endColor = color;
        for (int i = 0; i < linerenderer.positionCount; i++)
        {
            Vector3 pos = new Vector3(radius * Mathf.Cos(theta), radius * Mathf.Sin(theta), 0f) + OldPos;
            linerenderer.SetPosition(i, pos);
            theta += DeltaTheta;
        }


    }
    /*#if UNITY_EDITOR
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
    #endif*/
}

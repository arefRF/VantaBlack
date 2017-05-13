using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class EyeMoveTowardPosition : MonoBehaviour
{

    public Vector2 position;
    private float radius;
    private float cos, sin, constant;
    private Vector2 center_position;

    public void Invoke()
    {
        center_position = transform.parent.transform.position;
        constant = Mathf.Sqrt(Mathf.Pow(Mathf.Abs(position.x - center_position.x), 2) + Mathf.Pow(Mathf.Abs(position.y - center_position.y), 2));
        cos = (position.x - center_position.x) / constant;
        sin = (position.y - center_position.y) / constant;
        StartCoroutine(EyeSmoothMovetowardposition());
    }

    public IEnumerator EyeSmoothMovetowardposition()
    {

        constant = Mathf.Sqrt(Mathf.Pow(Mathf.Abs(position.x - center_position.x), 2) + Mathf.Pow(Mathf.Abs(position.y - center_position.y), 2));
        cos = (position.x - center_position.x) / constant;
        sin = (position.y - center_position.y) / constant;
        Vector3 target = new Vector3(center_position.x + radius * cos, center_position.y + radius * sin, 0);
        float remain = (transform.position - target).sqrMagnitude;
        while (remain > float.Epsilon)
        {
            constant = Mathf.Sqrt(Mathf.Pow(Mathf.Abs(position.x - center_position.x), 2) + Mathf.Pow(Mathf.Abs(position.y - center_position.y), 2));
            cos = (position.x - center_position.x) / constant;
            sin = (position.y - center_position.y) / constant;
            target = new Vector3(center_position.x + radius * cos, center_position.y + radius * sin, 0);
            remain = (transform.position - target).sqrMagnitude;
            transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime);
            yield return null;
        }
    }
}

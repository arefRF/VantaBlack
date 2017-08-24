using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Enemy : MonoBehaviour
{
    public List<Direction> MoveDirections;
    private Vector2 MoveToPosition;
    private Coroutine coroutine;
    void Start()
    {

    }


    void Update()
    {
        for (int i = 0; i < MoveDirections.Count; i++)
            CheckPlayer(MoveDirections[i]);
    }

    public void CheckPlayer(Direction direction)
    {
        Vector2 pos = Toolkit.VectorSum(transform.position, Toolkit.DirectiontoVector(direction) / 1.95f);
        RaycastHit2D hit = Physics2D.Raycast(pos, Toolkit.DirectiontoVector(direction));
        if (hit.collider != null && hit.collider.tag == "Player")
        {
            if (((Vector2)hit.collider.transform.position - pos).SqrMagnitude() <= 1)
                Debug.Log("sadadadasa");
            MoveToPosition = hit.collider.transform.position;
            if (coroutine == null)
            {
                coroutine = StartCoroutine(Move());
            }
        }
    }

    private IEnumerator Move()
    {
        float remain_distance = ((Vector2)transform.position - MoveToPosition).sqrMagnitude;
        float move_time = 0.2f;
        while (remain_distance > float.Epsilon)
        {
            remain_distance = ((Vector2)transform.position - MoveToPosition).sqrMagnitude;
            Vector3 new_pos = Vector3.MoveTowards(transform.position, MoveToPosition, Time.deltaTime * 1 / move_time);
            transform.position = new_pos;
            yield return null;
        }
        coroutine = null;
    }
}

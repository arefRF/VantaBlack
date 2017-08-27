using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Enemy : Unit
{
    public List<Direction> MoveDirections;
    public float move_time = 0.2f;
    public bool IsOn = true;
    private Vector2 MoveToPosition, PlayerPosition;
    private Coroutine coroutine;
    public Direction gravityDirection;

    void Start()
    {
        gravityDirection = Starter.GetGravityDirection();
    }


    void Update()
    {
        if(IsOn)
            for (int i = 0; i < MoveDirections.Count; i++)
                CheckPlayer(MoveDirections[i]);
    }

    public void CheckPlayer(Direction direction)
    {
        Vector2 pos = Toolkit.VectorSum(transform.position, Toolkit.DirectiontoVector(direction) / 1.95f);
        RaycastHit2D hit = Physics2D.Raycast(pos, Toolkit.DirectiontoVector(direction));
        if (hit.collider != null && hit.collider.tag == "Player")
        {
            Player tempplayer = hit.collider.gameObject.GetComponent<Player>();
            if ((tempplayer.position - pos).SqrMagnitude() <= 0.1f)
            {
                
                Starter.GetEngine().apigraphic.Laser_Player_Died(tempplayer);
            }
            else
            {
                PlayerPosition = Toolkit.RoundVector(tempplayer.transform.position);
                if (coroutine == null)
                {
                    coroutine = StartCoroutine(Move());
                }
            }
        }
    }

    private IEnumerator Move()
    {
        MoveToPosition = PlayerPosition;
        float remain_distance = ((Vector2)transform.position - MoveToPosition).sqrMagnitude;
        while (remain_distance > float.Epsilon)
        {
            if(remain_distance < 0.1f)
            {
                MoveToPosition = PlayerPosition;
                api.RemoveFromDatabase(this);
                position = Toolkit.RoundVector(transform.position);
                api.AddToDatabase(this);
            }
            remain_distance = ((Vector2)transform.position - MoveToPosition).sqrMagnitude;
            Vector3 new_pos = Vector3.MoveTowards(transform.position, MoveToPosition, Time.deltaTime * 1 / move_time);
            transform.position = new_pos;
            yield return null;
        }
        coroutine = null;
    }

    /*private Vector2 GetNextMovePosition()
    {

    }*/


    public override bool isLeanable()
    {
        return true;
    }

    public override bool isLeanableFromDirection(Direction direction)
    {
        return Toolkit.ReverseDirection(direction) == gravityDirection;
    }

    public void OnOff()
    {
        IsOn = !IsOn;
    }

}

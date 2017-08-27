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
    public EnemyState state { get; set; }

    void Start()
    {
        gravityDirection = Starter.GetGravityDirection();
    }


    void Update()
    {
        if(IsOn && state != EnemyState.Falling)
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
            if (tempplayer.lifestate == LifeState.Dead)
                return;
            if ((tempplayer.position - pos).SqrMagnitude() <= 1)
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
        MoveToPosition = position + (PlayerPosition - position).normalized;
        float remain_distance = (((Vector2)transform.position - MoveToPosition)).magnitude;
        bool MoveFinished = false;
        while (remain_distance > float.Epsilon)
        {
            if(remain_distance < 0.1f && !MoveFinished)
            {
                api.RemoveFromDatabase(this);
                position = Toolkit.RoundVector(transform.position);
                api.AddToDatabase(this);
                Vector2 temp = MoveToPosition;
                if (CanMove(position + (PlayerPosition - position).normalized))
                    MoveToPosition = position + (PlayerPosition - position).normalized;
                else
                    Debug.Log(":DDDD");
                Vector2 tempvector = temp - MoveToPosition;
                if (tempvector.x == 0 && tempvector.y == 0)
                {
                    Debug.Log("here");
                    transform.position = position;
                    ApplyGravity();
                    break;
                }
            }
            remain_distance = (((Vector2)transform.position - MoveToPosition)).magnitude;
            Vector3 new_pos = Vector3.MoveTowards(transform.position, MoveToPosition, Time.deltaTime * 1 / move_time);
            transform.position = new_pos;
            yield return null;
        }
        coroutine = null;
    }
    private bool CanMove(Vector2 pos)
    {
        return !CheckEmpty(pos + Toolkit.DirectiontoVector(api.engine.database.gravity_direction));
    }
    private new void ApplyGravity()
    {
        Vector2 endpos = position;
        Vector2 temppos = endpos;
        Debug.Log(CheckEmpty(position + Toolkit.DirectiontoVector(api.engine.database.gravity_direction)));
        while (CheckEmpty(position + Toolkit.DirectiontoVector(api.engine.database.gravity_direction)))
        {
            endpos += Toolkit.DirectiontoVector(api.engine.database.gravity_direction);
            position += Toolkit.DirectiontoVector(api.engine.database.gravity_direction);
        }
        if (temppos != endpos)
            state = EnemyState.Falling;
        Debug.Log(endpos);
    }

    private bool CheckEmpty(Vector2 pos)
    {
        Vector2 rootpos = position + new Vector2(0.5f, 0.5f) + Toolkit.DirectiontoVector(api.engine.database.gravity_direction) / 1.5f;
        Debug.Log(Toolkit.DirectiontoVector(api.engine.database.gravity_direction) / 1.5f);
        Vector2 temppos1, temppos2;
        if (Toolkit.isHorizontal(api.engine.database.gravity_direction))
        {
            temppos1 = rootpos + new Vector2(0,0.2f);
            temppos2 = rootpos + new Vector2(0, -0.2f);
        }
        else
        {
            temppos1 = rootpos + new Vector2(0.2f, 0);
            temppos2 = rootpos + new Vector2(-0.2f, 0);
        }
        Debug.Log(temppos1);
        Debug.Log(temppos2);
        RaycastHit2D hit = Physics2D.Raycast(temppos1, (pos - temppos1).normalized, (pos - temppos1).magnitude);
        RaycastHit2D hit2 = Physics2D.Raycast(temppos2, (pos - temppos2).normalized, (pos - temppos2).magnitude);
        if (hit.collider != null || hit2.collider != null)
        {
            Debug.Log(hit.collider);
           
            return false;
        }
        else
            Debug.Log("true");
        return true;
    }

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

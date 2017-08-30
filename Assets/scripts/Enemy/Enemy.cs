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
    public Direction gravityDirection { get; set; }
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
                    coroutine = StartCoroutine(Move(direction));
                }
            }
        }
    }

    private new IEnumerator Move(Direction direction)
    {
        MoveToPosition = position + (PlayerPosition - position).normalized;
        float remain_distance = (((Vector2)transform.position - MoveToPosition)).magnitude;
        bool MoveFinished = false;
        MoveNecessaryPlayers(direction);
        while (remain_distance > float.Epsilon)
        {
            if(remain_distance < 0.1f && !MoveFinished)
            {
                api.RemoveFromDatabase(this);
                position = Toolkit.RoundVector(transform.position);
                api.AddToDatabase(this);
                Vector2 temp = MoveToPosition;
                if (CanMove(position))
                    MoveToPosition = position + (PlayerPosition - position).normalized;
                Vector2 tempvector = temp - MoveToPosition;
                if (tempvector.x == 0 && tempvector.y == 0)
                {
                    transform.position = position;
                    ApplyGravity();
                    break;
                }
                MoveNecessaryPlayers(direction);
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
        while (CheckEmpty(position + Toolkit.DirectiontoVector(gravityDirection)))
        {
            api.RemoveFromDatabase(this);
            endpos += Toolkit.DirectiontoVector(gravityDirection);
            position += Toolkit.DirectiontoVector(gravityDirection);
            api.AddToDatabase(this);
            transform.position = position;
        }
        if (temppos != endpos)
            state = EnemyState.Falling;
        transform.position = endpos;
    }

    private bool CheckEmpty(Vector2 pos)
    {
        Vector2 temppos1, temppos2;
        if (Toolkit.isHorizontal(gravityDirection))
        {
            temppos1 = position + new Vector2(0,0.2f);
            temppos2 = position + new Vector2(0, -0.2f);
        }
        else
        {
            temppos1 = position + new Vector2(0.2f, 0);
            temppos2 = position + new Vector2(-0.2f, 0);
        }
        RaycastHit2D hit = Physics2D.Raycast(temppos1, (pos - position).normalized, (pos - position).magnitude);
        RaycastHit2D hit2 = Physics2D.Raycast(temppos2, (pos - position).normalized, (pos - position).magnitude);
        if (hit.collider != null || hit2.collider != null)
        {
            return false;
        }
        return true;
    }
    private void MoveNecessaryPlayers(Direction direction)
    {
        List<Player> players = GetPlayersToMove();
        for(int i=0; i<players.Count; i++)
        {
            if (players[i].CanMove(direction, transform.parent.gameObject))
            {
                api.RemoveFromDatabase(players[i]);
                players[i].position = Toolkit.VectorSum(players[i].position, direction);
                players[i].transform.position = players[i].position;
                api.AddToDatabase(players[i]);
            }
        }
    }
    private List<Player> GetPlayersToMove()
    {
        List<Player> players = new List<Player>();
        Vector2 tempvector = Toolkit.VectorSum(position, Toolkit.ReverseDirection(gravityDirection));
        List<Unit> units = api.engine.database.units[(int)tempvector.x, (int)tempvector.y];
        for(int i=0; i<units.Count; i++)
        {
            if (units[i] is Player)
            {
                Player tempplayer = units[i] as Player;
                Debug.Log(tempplayer.LeanedTo);
                if(tempplayer.LeanedTo == null || tempplayer.LeanedTo == this)
                    players.Add(units[i] as Player);
            }
            else if (units[i] is Branch)
                return new List<Player>();
            else if(units[i] is Ramp && (units[i] as Ramp).IsOnRampSide(Toolkit.ReverseDirection(gravityDirection)))
                return new List<Player>();
        }
        return players;
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

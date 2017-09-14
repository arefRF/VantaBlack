using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour {

    public int PatrolDistance;
    public List<Direction> PatrolDirection;
    public float move_time;
    public float WaitTime = 1;

    private Enemy enemy;
    public int moved;
    private Direction MovingDirection; //direction that enemy is patrolling now
    private Coroutine PatrolCoroutine, StartTimeCoRoutine;
    private bool StartTimeFinished = false;
	void Start () {
        enemy = GetComponent<Enemy>();
        moved = 0;
        MovingDirection = PatrolDirection[0];
        
    }
	
	// Update is called once per frame
	void Update () {
        if (enemy.state == EnemyState.Idle)
        {
            if (PatrolCoroutine == null)
            {
                if (StartTimeFinished)
                    PatrolCoroutine = StartCoroutine(PatrolMove(MovingDirection));
                else
                    StartTimeCoRoutine = StartCoroutine(PatrolStartWaitTime());
            }
        }
	}

    private IEnumerator PatrolMove(Direction direction)
    {
        int temppatroldistance = PatrolDistance;
        moved = 0;
        if (!CanMove(enemy.position, direction))
            direction = Toolkit.ReverseDirection(direction);
        if(CanMove(enemy.position, direction))
        {
            enemy.state = EnemyState.Patrolling;
            Vector2 MoveToPosition = enemy.position + Toolkit.DirectiontoVector(direction);
            float remain_distance = (((Vector2)transform.position - MoveToPosition)).magnitude;
            MoveNecessaryPlayers(direction);
            enemy.SendMessage(new EnemyMessage(EnemyMessage.MessageType.MoveAnimation));
            while (remain_distance > float.Epsilon )
            {
                remain_distance = (((Vector2)transform.position - MoveToPosition)).magnitude;
                Vector3 new_pos = Vector3.MoveTowards(transform.position, MoveToPosition, Time.deltaTime / move_time);
                transform.position = new_pos;
                if (remain_distance < 0.05f)
                {
                    moved++;
                    enemy.api.RemoveFromDatabase(enemy);
                    enemy.position = Toolkit.RoundVector(enemy.transform.position);
                    enemy.api.AddToDatabase(enemy);
                    if (moved >= temppatroldistance)
                    {
                        enemy.transform.position = Toolkit.RoundVector(enemy.transform.position);
                        if (temppatroldistance == PatrolDistance)
                            temppatroldistance = PatrolDistance * 2;
                        moved = 0;
                        direction = Toolkit.ReverseDirection(direction);
                        yield return new WaitForSeconds(WaitTime);
                    }
                    Vector2 temp = MoveToPosition;
                    if (CanMove(enemy.position, direction))
                        MoveToPosition = enemy.position + Toolkit.DirectiontoVector(direction);
                    else
                    {
                        enemy.transform.position = Toolkit.RoundVector(enemy.transform.position);
                        direction = Toolkit.ReverseDirection(direction);
                        moved = temppatroldistance - moved;
                        if (temppatroldistance == PatrolDistance)
                            temppatroldistance = PatrolDistance * 2;
                        if (CanMove(enemy.position, direction))
                            MoveToPosition = enemy.position + Toolkit.DirectiontoVector(direction);
                        else
                            break;
                        yield return new WaitForSeconds(WaitTime);
                    }
                    Vector2 tempvector = temp - MoveToPosition;
                    if (tempvector.x == 0 && tempvector.y == 0)
                    {
                        transform.position = enemy.position;
                        break;
                    }
                    MoveNecessaryPlayers(direction);
                    remain_distance = (((Vector2)transform.position - MoveToPosition)).magnitude;
                }
                yield return null;
            }
        }
        enemy.SendMessage(new EnemyMessage(EnemyMessage.MessageType.MoveAnimationStop));
        PatrolCoroutine = null;
        enemy.state = EnemyState.Idle;
    }

    private bool CheckEmpty(Vector2 CurrentPos, Vector2 DestinationPos, Direction direction)
    {
        Vector2 rootposition = CurrentPos + Toolkit.DirectiontoVector(direction) / 1.8f;
        Vector2 temppos1, temppos2;
        if (Toolkit.isHorizontal(direction))
        {
            temppos1 = rootposition + new Vector2(0, 0.2f);
            temppos2 = rootposition + new Vector2(0, -0.2f);
        }
        else
        {
            temppos1 = rootposition + new Vector2(0.2f, 0);
            temppos2 = rootposition + new Vector2(-0.2f, 0);
        }
        RaycastHit2D hit = Physics2D.Raycast(temppos1, (DestinationPos - CurrentPos).normalized, (DestinationPos - CurrentPos).magnitude / 2);
        RaycastHit2D hit2 = Physics2D.Raycast(temppos2, (DestinationPos - CurrentPos).normalized, (DestinationPos - CurrentPos).magnitude / 2);
        if ((hit.collider != null && hit.collider.tag != "Player") || (hit2.collider != null && hit.collider.tag != "Player"))
        {
            return false;
        }
        return true;
    }

    private bool CanMove(Vector2 pos, Direction direction)
    {
        if (!CheckEmpty(pos, pos + Toolkit.DirectiontoVector(direction), direction))
            return false;
        Vector2 temppos = pos + Toolkit.DirectiontoVector(direction);
        if (CheckEmpty(temppos, temppos + Toolkit.DirectiontoVector(enemy.gravityDirection), enemy.gravityDirection))
            return false;
        return true;
    }

    private void MoveNecessaryPlayers(Direction direction)
    {
        List<Player> players = GetPlayersToMove();
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i].CanMove(direction, transform.parent.gameObject))
            {
                enemy.api.RemoveFromDatabase(players[i]);
                players[i].position = Toolkit.VectorSum(players[i].position, direction);
                players[i].transform.position = players[i].position;
                enemy.api.AddToDatabase(players[i]);
            }
        }
    }
    private List<Player> GetPlayersToMove()
    {
        List<Player> players = new List<Player>();
        Vector2 tempvector = Toolkit.VectorSum(enemy.position, Toolkit.ReverseDirection(enemy.gravityDirection));
        List<Unit> units = enemy.api.engine.database.units[(int)tempvector.x, (int)tempvector.y];
        for (int i = 0; i < units.Count; i++)
        {
            if (units[i] is Player)
            {
                Player tempplayer = units[i] as Player;
                if (tempplayer.LeanedTo == null || tempplayer.LeanedTo == this)
                    players.Add(units[i] as Player);
            }
            else if (units[i] is Branch)
                return new List<Player>();
            else if (units[i] is Ramp && (units[i] as Ramp).IsOnRampSide(Toolkit.ReverseDirection(enemy.gravityDirection)))
                return new List<Player>();
        }
        return players;
    }

    private void StopPatrol()
    {
        if (PatrolCoroutine != null)
            StopCoroutine(PatrolCoroutine);
        if (StartTimeCoRoutine != null)
            StopCoroutine(StartTimeCoRoutine);
        StartTimeCoRoutine = null;
        PatrolCoroutine = null;
        StartTimeFinished = false;
        //enemy.transform.position = Toolkit.RoundVector(enemy.transform.position);
        enemy.api.RemoveFromDatabase(enemy);
        enemy.position = Toolkit.RoundVector(enemy.transform.position);
        enemy.api.AddToDatabase(enemy);
    }

    private void StartPatrol()
    {
        Debug.Log("not implemented");
    }

    public IEnumerator PatrolStartWaitTime()
    {
        yield return new WaitForSeconds(WaitTime);
        StartTimeFinished = true;

    }

    public void GetMessage(EnemyMessage message)
    {
        switch (message.messagetype)
        {
            case EnemyMessage.MessageType.StopPatrol: StopPatrol(); break;
            case EnemyMessage.MessageType.StartPatrol: StartPatrol(); break;
        }
    }


}

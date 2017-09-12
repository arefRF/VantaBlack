using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour {

    public List<Direction> MoveDirections;
    public float move_time = 0.4f;
    private Vector2 PlayerPosition;
    private Coroutine coroutine;
    private Enemy enemy;

    public void Start()
    {
        enemy = GetComponent<Enemy>();
    }

    public void Move(Vector2 playerposition, Direction direction)
    { 
        PlayerPosition = playerposition;
        if (coroutine == null)
        {
            coroutine = StartCoroutine(Move(direction));
        }
    }
    private IEnumerator Move(Direction direction)
    {
        enemy.state = EnemyState.Moving;
        Vector2 MoveToPosition = enemy.position + (PlayerPosition - enemy.position).normalized;
        float remain_distance = (((Vector2)transform.position - MoveToPosition)).magnitude;
        MoveNecessaryPlayers(direction);
        enemy.SendMessage(new EnemyMessage(EnemyMessage.MessageType.MoveAnimation));
        while (remain_distance > float.Epsilon)
        {
            if (remain_distance < 0.1f)
            {
                /*if(!audio.isPlaying)
                    audio.Play();*/
                enemy.api.RemoveFromDatabase(enemy);
                enemy.position = Toolkit.RoundVector(transform.position);
                enemy.api.AddToDatabase(enemy);
                
                Vector2 temp = MoveToPosition;
                if (CanMove(enemy.position, direction))
                    MoveToPosition = enemy.position + (PlayerPosition - enemy.position).normalized;
                Vector2 tempvector = temp - MoveToPosition;
                if (tempvector.x == 0 && tempvector.y == 0)
                {
                    transform.position = enemy.position;
                    break;
                }
                MoveNecessaryPlayers(direction);
            }
            remain_distance = (((Vector2)transform.position - MoveToPosition)).magnitude;
            Vector3 new_pos = Vector3.MoveTowards(transform.position, MoveToPosition, Time.deltaTime * 1 / move_time);
            transform.position = new_pos;
            yield return null;
        }
        enemy.SendMessage(new EnemyMessage(EnemyMessage.MessageType.MoveAnimationStop));
        coroutine = null;
        enemy.state = EnemyState.Idle;
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

    private bool CheckEmpty(Vector2 pos, Direction direction)
    {
        Vector2 rootposition = enemy.position + Toolkit.DirectiontoVector(direction) / 1.8f;
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
        RaycastHit2D hit = Physics2D.Raycast(temppos1, (pos - enemy.position).normalized, (pos - enemy.position).magnitude / 2);
        RaycastHit2D hit2 = Physics2D.Raycast(temppos2, (pos - enemy.position).normalized, (pos - enemy.position).magnitude / 2);
        if ((hit.collider != null && hit.collider.tag != "Player") || (hit2.collider != null && hit.collider.tag != "Player"))
        {
            return false;
        }
        return true;
    }

    private bool CanMove(Vector2 pos, Direction direction)
    {
        if (CheckEmpty(pos + Toolkit.DirectiontoVector(enemy.gravityDirection), enemy.gravityDirection))
            return false;
        if (!CheckEmpty(pos + Toolkit.DirectiontoVector(direction), direction))
            return false;
        return true;
    }

    private void StopMove()
    {
        if (coroutine != null)
            StopCoroutine(coroutine);
        coroutine = null;
        enemy.SendMessage(new EnemyMessage(EnemyMessage.MessageType.MoveAnimationStop));
        enemy.api.RemoveFromDatabase(enemy);
        enemy.position = Toolkit.RoundVector(transform.position);
        enemy.api.AddToDatabase(enemy);
        enemy.state = EnemyState.Idle;
    }

    public void GetMessage(EnemyMessage message)
    {
        switch (message.messagetype)
        {
            case EnemyMessage.MessageType.PhysicalMove: Move(message.position, message.direction); break;
            case EnemyMessage.MessageType.PhysicalMoveStop: StopMove(); break;
        }
    }
}

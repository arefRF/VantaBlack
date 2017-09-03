using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Enemy : Unit
{
    public bool IsOn = true;
    public Direction gravityDirection { get; set; }
    private AudioSource audio;
    private Animator animator;


    private EnemyMove enemymove;
    private EnemyFireLaser enemyfirelaser;
    private EnemyGraphics enemygraphics;
    public EnemyState state { get; set; }

    void Start()
    {
        animator = GetComponent<Animator>();
        audio = GetComponent<AudioSource>();
        gravityDirection = Starter.GetGravityDirection();
        enemymove = GetComponent<EnemyMove>();
        enemyfirelaser = GetComponent<EnemyFireLaser>();
        enemygraphics = GetComponent<EnemyGraphics>();
    }


    void Update()
    {
        if (enemymove != null && IsOn && state != EnemyState.Falling)
            for (int i = 0; i < enemymove.MoveDirections.Count; i++)
                CheckPlayer(enemymove.MoveDirections[i]);

    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "Player")
        {
            Starter.GetEngine().apigraphic.Laser_Player_Died(col.gameObject.GetComponent<Player>());
        }
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
            /*if ((tempplayer.position - pos).SqrMagnitude() <= 1)
            {
                Starter.GetEngine().apigraphic.Laser_Player_Died(tempplayer);
            }*/
            else
            {
                SendMessage(new EnemyMessage(EnemyMessage.MessageType.PhysicalMove, direction, Toolkit.RoundVector(tempplayer.transform.position)));
            }
        }
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
        SendMessage(new EnemyMessage(EnemyMessage.MessageType.OnOffChanged));
    }

    public void SendMessage(EnemyMessage message)
    {
        if(enemymove != null)
            enemymove.GetMessage(message);
        if(enemygraphics != null)
            enemygraphics.GetMessage(message);
    }
}

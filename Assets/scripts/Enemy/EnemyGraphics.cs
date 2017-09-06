using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyGraphics : MonoBehaviour {

    public float FallTime = 50;

    private Enemy enemy;
    private AudioSource sound;
    private Animator animator;
    private Vector2 FallPosition;
    private Coroutine fallCoroutine;
    void Start()
    {
        enemy = GetComponent<Enemy>();
        sound = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();

    }

    void Update()
    {
        ApplyGravity();
    }


    private void ApplyGravity()
    {
        FallPosition = GetFallPosition(enemy.gravityDirection);
        if(FallPosition != enemy.position)
        {
            if(fallCoroutine == null)
            {
                enemy.state = EnemyState.Falling;
                enemy.SendMessage(new EnemyMessage(EnemyMessage.MessageType.PhysicalMoveStop));
                fallCoroutine = StartCoroutine(FallToPosition());
            }
        }
    }

    private Vector2 GetFallPosition(Direction gravitydirection)
    {
        Vector2 rootposition;// = (Vector2)enemy.transform.position + Toolkit.DirectiontoVector(gravitydirection) / 1.8f;
        if(Toolkit.isHorizontal(gravitydirection))
            rootposition = new Vector2(enemy.transform.position.x, enemy.position.y) + Toolkit.DirectiontoVector(gravitydirection) / 1.8f;
        else
            rootposition = new Vector2(enemy.position.x, enemy.transform.position.y) + Toolkit.DirectiontoVector(gravitydirection) / 1.8f;
        Vector2 temppos1, temppos2;
        if (Toolkit.isHorizontal(enemy.gravityDirection))
        {
            temppos1 = rootposition + new Vector2(0, 0.2f);
            temppos2 = rootposition + new Vector2(0, -0.2f);
        }
        else
        {
            temppos1 = rootposition + new Vector2(0.2f, 0);
            temppos2 = rootposition + new Vector2(-0.2f, 0);
        }
        RaycastHit2D hit = Physics2D.Raycast(temppos1, Toolkit.DirectiontoVector(gravitydirection));
        RaycastHit2D hit2 = Physics2D.Raycast(temppos2, Toolkit.DirectiontoVector(gravitydirection));
        if (hit.collider != null)
        {
            /*if (hit.collider.gameObject == gameObject)
                return enemy.position*/
            return hit.collider.gameObject.GetComponent<Unit>().position + Toolkit.DirectiontoVector(Toolkit.ReverseDirection(gravitydirection));
        }
        else if(hit2.collider != null)
        {
            /*if (hit2.collider.gameObject == gameObject)
                return enemy.position;*/
            return hit2.collider.gameObject.GetComponent<Unit>().position + Toolkit.DirectiontoVector(Toolkit.ReverseDirection(gravitydirection));
        }
        switch (gravitydirection)
        {
            case Direction.Up: return new Vector2(enemy.position.x, Starter.GetDataBase().Ysize);
            case Direction.Right: return new Vector2(Starter.GetDataBase().Xsize, enemy.position.y);
            case Direction.Down: return new Vector2(enemy.position.x, 0);
            case Direction.Left: return new Vector2(0, enemy.position.y);
        }
        return new Vector2(0,0);
    }

    private IEnumerator FallToPosition()
    {
        Vector2 fallpos = enemy.position + (FallPosition - enemy.position).normalized;
        float remain_distance = ((Vector2)enemy.transform.position - fallpos).magnitude;
        while(remain_distance > float.Epsilon)
        {
            if (remain_distance < 0.1f)
            {
                enemy.api.RemoveFromDatabase(enemy);
                enemy.position = Toolkit.RoundVector(transform.position);
                enemy.api.AddToDatabase(enemy);
                //Debug.Log(enemy.position);
                //Debug.Log(FallPosition);
                fallpos = enemy.position + (FallPosition - enemy.position).normalized;

            }
            remain_distance = ((Vector2)enemy.transform.position - fallpos).magnitude;
            Vector3 new_pos = Vector3.MoveTowards(transform.position, fallpos, Time.deltaTime * 1 / FallTime);
            transform.position = new_pos;
            yield return null;
        }
        enemy.state = EnemyState.Idle;
        fallCoroutine = null;
    }
    public void MoveAnimation()
    {
        sound.Play();
        animator.SetInteger("Move", 1);
    }

    public void MoveAnimationStop()
    {
        sound.Stop();
        animator.SetInteger("Move", 0);
    }
    public void KillPlayerAnimation()
    {

    }

    public void KillPlayerSound()
    {

    }

    public void OnOffGraphics()
    {
        if (enemy.IsOn)
        {
            OpenCloseEyes(true);
            Toolkit.GetObjectInChild(gameObject, "Powers").SetActive(true);
        }
        else
        {
            OpenCloseEyes(false);
            Toolkit.GetObjectInChild(gameObject, "Powers").SetActive(false);
        }
    }

    private void OpenCloseEyes(bool open)
    {
        if (open)
        {
            Toolkit.GetObjectInChild(gameObject, "Eyes").SetActive(true);
            Toolkit.GetObjectInChild(gameObject, "Eyes Close").SetActive(false);
        }
        else
        {
            Toolkit.GetObjectInChild(gameObject, "Eyes").SetActive(false);
            Toolkit.GetObjectInChild(gameObject, "Eyes Close").SetActive(true);
        }
    }
    public void GetMessage(EnemyMessage message)
    {
        switch (message.messagetype)
        {
            case EnemyMessage.MessageType.MoveAnimation: MoveAnimation(); break;
            case EnemyMessage.MessageType.MoveAnimationStop: MoveAnimationStop(); break;
            case EnemyMessage.MessageType.OnOffChanged: OnOffGraphics(); break;
        }

    }
}

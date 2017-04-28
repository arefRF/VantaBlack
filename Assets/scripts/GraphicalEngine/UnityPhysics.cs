﻿using UnityEngine;
using System.Collections;

public class UnityPhysics : MonoBehaviour {
    private Rigidbody2D rb;
    private float hoverHeight =1;
    private float hoverForce = 50;
    private float powerInput;
    private float speed =50;
    private Player player;
    private Transform player_transform;
    public float move_time = 0.5f;
    private float remain = 0 ;
    private Coroutine co;
    private Direction move_dir = Direction.Up;
    private PhysicState state = PhysicState.Idle;
    private Vector2 adj_pos;
    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody2D>();
        player = GetComponent<Player>();
        player_transform = transform;
    }
	
	// Update is called once per frame
	void Update () {
        if (player.mode == GameMode.Real && player.state == PlayerState.Idle)
        {
            powerInput = Input.GetAxis("Horizontal");
            if (powerInput > 0)
            {
                move_dir = Direction.Right;
                state = PhysicState.Idle;
            }
            else if (powerInput < 0)
            {
                move_dir = Direction.Left;
                state = PhysicState.Idle;
            }
            if (powerInput == 0)
            {
                if (state != PhysicState.Adjust)
                {
                    /* adj_pos = (Vector2)transform.position;
                     if (move_dir == Direction.Right)
                         adj_pos = new Vector2(Mathf.Ceil(transform.position.x), transform.position.y);
                     else if (move_dir == Direction.Left)
                         adj_pos = new Vector2(Mathf.Floor(transform.position.x), transform.position.y);
                     if (Mathf.Abs(transform.position.x - adj_pos.x) > 0.05f)
                     {
                         Debug.Log(transform.position.x);
                         Debug.Log(adj_pos.x);
                         Debug.Log("Adjust");
                         state = PhysicState.Adjust;
                         rb.velocity = new Vector2( adj_pos.x- transform.position.x , 0);
                     }*/

                }
                // Adjust phase
                else
                {
                    if (move_dir == Direction.Right)
                        rb.velocity = new Vector2(2, 0);
                    else if (move_dir == Direction.Left)
                        rb.velocity = new Vector2(-2, 0);
                    if (Mathf.Abs(transform.position.x - adj_pos.x) < 0.1f)
                    {
                        Debug.Log("Adjust Finished");
                        rb.velocity = new Vector2(0, 0);
                        transform.position = new Vector2(adj_pos.x, transform.position.y);
                        state = PhysicState.Idle;
                        player.api.RemoveFromDatabase(player);
                        player.position = new Vector2((int)transform.position.x,(int)transform.position.y);
                        player.api.AddToDatabase(player);
                    }
                }
            }
                if (Mathf.Abs(rb.velocity.x) < 3)
                {
                    rb.AddRelativeForce(new Vector2(powerInput * speed, 0));
                }
        }
    }


    void FixedUpdate()
    {
        if(player.mode == GameMode.Real)
        {
             RaycastHit2D hit = Physics2D.Raycast(transform.position + new Vector3(0, 0.5f, 0), -transform.up);
               if (hit)
                {
                        float proportionalHeight = (hoverHeight - hit.distance) / hoverHeight;
                        Vector2 force = hit.normal;
                        Vector3 appliedHoverForce = force * proportionalHeight * hoverForce;
                        rb.AddForce(appliedHoverForce, ForceMode2D.Force);
               }


        }
    }

    public void Move(Vector2 pos)
    {
       co = StartCoroutine(Constant_Move(pos, move_time));
    }

    private IEnumerator Constant_Move(Vector2 end, float move_time)
    {
        float remain_distance = Mathf.Abs( transform.position.x - end.x);
        
        while (remain_distance > float.Epsilon)
        {
            remain_distance = Mathf.Abs(transform.position.x - end.x);
            transform.position = new Vector2(Mathf.MoveTowards(transform.position.x, end.x, Time.smoothDeltaTime / move_time),transform.position.y);
            yield return new WaitForSeconds(0.001f);
        }

        player.api.engine.inputcontroller.RealModePlayerTransitionMoveDone(player);
        
        // if it needs Call Finished Move of API
    }

}

public enum PhysicState
{
    Adjust,Idle
}

using UnityEngine;
using System.Collections;

public class PlayerPhysics : MonoBehaviour
{
    private Animator animation;
    private APIGraphic api;
    private LogicalEngine engine;
    private Vector2 target_pos;
    private Vector2 final_pos;
    private bool on_sharp;
    private bool moving;
    private Vector2 velocity;
    private bool on_ramp;
    private Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        engine = Starter.GetEngine();
        animation = GetComponent<Animator>();
        api = engine.apigraphic;
    }

    void FixedUpdate()
    {
        if (moving)
        {
            if (Mathf.Abs(target_pos.x - transform.position.x) < 0.05)
            {
                // if passed to destination
                rb.velocity = new Vector2(0, 0);
                velocity = new Vector2(0, 0);
                if(!on_sharp)
                    api.MovePlayerFinished(gameObject);
                moving = false;
                transform.position = target_pos;
            }
            else // To keep Velocity Constant
                rb.velocity = velocity;
        }
        else if (on_ramp)
        {
            // not to let it move
            rb.drag = 10000;
        }
        else if (on_sharp)
        {
            // Part 2 of Ramp to Sharp Move
            Sharp_To_Ramp_Move();
        }

    }

    public void Block_To_Ramp_Move(Vector2 pos)
    {
        rb.drag = 0;
        target_pos = pos;
        velocity = (pos - (Vector2)transform.position) * 2;
        moving = true;
        on_ramp = true;
        rb.velocity = velocity;
    }


    public void Ramp_To_Sharp_Move(Vector2 pos)
    {
        rb.drag = 0;
        final_pos = pos;
        target_pos = Ramp_To_Sharp_Pos(Direction.Down,pos);
        velocity = Ramp_To_Sharp_Velocity(Direction.Down,pos);
        moving = true;
        on_sharp = true;
        on_ramp = false;
        rb.velocity = velocity;
    }


    private Vector2 Ramp_To_Sharp_Pos(Direction gravity,Vector2 position)
    {
        if(gravity == Direction.Down)
        {
            if (position.x - transform.position.x > 0)
                return position + new Vector2(-0.5f, 1);
            else
                return position + new Vector2(0.5f, 1);
        }

        return new Vector2(0, 0);
    }
    private Vector2 Ramp_To_Sharp_Velocity(Direction gravity, Vector2 position)
    {
        if (gravity == Direction.Down)
        {
            if (position.x - transform.position.x > 0)
                return new Vector2(1, 1);
            else
                return new Vector2(-1, 1);
        }
        return new Vector2(0, 0);
    }

    private Vector2 Sharp_To_Ramp_Pos(Direction gravity, Vector2 position)
    {
        if(gravity == Direction.Down)
        {
            if (position.x - transform.position.x > 0)
                return position + new Vector2(0.5f, 0.5f);
            else
                return position + new Vector2(-0.5f, 0.5f);
        }

        return new Vector2(0, 0);
    }

    private Vector2 Sharp_To_Ramp_Velocity(Direction gravity, Vector2 position)
    {
        if (gravity == Direction.Down)
        {
            if (position.x - transform.position.x > 0)
                return new Vector2(1, -1);
            else
                return new Vector2(-1, -1);
        }
        return new Vector2(0, 0);
    }

    private void Sharp_To_Ramp_Move()
    {
        rb.drag = 0;
        velocity = Sharp_To_Ramp_Velocity(Direction.Down, final_pos);
        target_pos = Sharp_To_Ramp_Pos(Direction.Down, final_pos);
        moving = true;
        on_ramp = true;
        on_sharp = false;
    }
   



    
    public void Ramp_To_Ramp_Move(Vector2 pos)
    {
        rb.drag = 0;
        target_pos = pos;
        velocity = (pos - (Vector2)transform.position) * 1.2f;
        moving = true;
        on_ramp = true;
        rb.velocity = velocity;
    }
    public void Simple_Move(Vector2 pos)
    {
        on_ramp = false;
        rb.drag = 0;
        target_pos = pos;
        velocity = (pos - (Vector2)transform.position) * 2;
        moving = true;
        rb.velocity = velocity;
    }

    public void Ramp_To_Corner_Move(Vector2 pos)
    {
        target_pos = Ramp_To_Corner_Pos(Direction.Down,pos);
        moving = true;
        rb.drag = 0;
    }

    private Vector2 Ramp_To_Corner_Pos(Direction gravity,Vector2 target)
    {
        if(gravity == Direction.Down)
        {
            if (target.x - transform.position.x > 0)
                return target + new Vector2(0.2f, 0);
            else
                return target + new Vector2(-0.2f, 0);
        }

        return new Vector2(0, 0);
    }

    private enum MoveType
    {
        RampToRamp,RampToSharp,RampToCorner
    }

}

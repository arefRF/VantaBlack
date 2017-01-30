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
    private bool call_finish;
    private Rigidbody2D rb;
    private int sharp_type;
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
                // if passed or so near to destination
                Debug.Log("passed");
                rb.velocity = new Vector2(0, 0);
                velocity = new Vector2(0, 0);
                if(call_finish)
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
            Sharp_To_Ramp_Move(sharp_type);
        }
    }

    // when platform is moving move the player
    public void On_Platform_Move(Direction dir)
    {
        Debug.Log("On platform move");
        Vector2 vel = On_Platform_Move_Velocity(dir);
        // if in direction of gravity do nothing
        if(!In_Direction_Of_Gravity(Direction.Down,dir))
        {
            moving = true;
            call_finish = false;
            target_pos = (Vector2)transform.position + Toolkit.DirectiontoVector(dir);
            rb.drag = 0;
            velocity = vel;
        }

    }

    private bool In_Direction_Of_Gravity(Direction gravity, Direction dir)
    {
        if (gravity == Direction.Down || gravity == Direction.Up)
        {
            if (dir == Direction.Down || dir == Direction.Up)
                return true;
            else
                return false;
        }
        else
        {
            if (dir == Direction.Left || dir == Direction.Right)
                return true;
            else
                return false;
        }
    }
    private Vector2 On_Platform_Move_Velocity(Direction dir)
    {
        if (dir == Direction.Right)
            return new Vector2(1.1f, 0);
        else if (dir == Direction.Left)
            return new Vector2(-1.1f, 0);
        else if (dir == Direction.Up)
            return new Vector2(0, 1.1f);
        else
            return new Vector2(0, -1.1f);

    }
    public void Block_To_Ramp_Move(Vector2 pos, int type)
    {
        rb.drag = 0;
        target_pos = pos;
        velocity = (pos - (Vector2)transform.position) * 2;
        moving = true;
        on_ramp = true;
        call_finish = true;
        Rotate_On_Ramp(type);
        
    }


    private void Rotate_On_Ramp(int type)
    {
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, Ramp_Rotation_Value(type)));
    }
    
    private float Ramp_Rotation_Value(int type)
    {
        if (type == 4)
            return 45;
        else if (type == 1)
            return 315;
        else
            return 0;
    }
    public void Ramp_To_Sharp_Move(Vector2 pos,int type)
    {
        rb.drag = 0;
        final_pos = pos;
        target_pos = Ramp_To_Sharp_Pos(Direction.Down,pos);
        velocity = Ramp_To_Sharp_Velocity(Direction.Down,pos);
        moving = true;
        on_sharp = true;
        on_ramp = false;
        call_finish = false;
        rb.velocity = velocity;
        sharp_type = type;
    }

    public void Move_Player(Direction d)
    {
        call_finish = true;
        rb.drag = 0;
        rb.velocity = Toolkit.DirectiontoVector(d);
        moving = true;
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


    public void Lean_Stick_Move(Direction dir)
    {
        target_pos = Toolkit.DirectiontoVector(dir);
        moving = true;
        call_finish = false;
        velocity = On_Platform_Move_Velocity(dir);
        rb.drag = 0;
    }
    private void Sharp_To_Ramp_Move(int type)
    {
        rb.drag = 0;
        velocity = Sharp_To_Ramp_Velocity(Direction.Down, final_pos);
        target_pos = Sharp_To_Ramp_Pos(Direction.Down, final_pos);
        moving = true;
        on_ramp = true;
        on_sharp = false;
        call_finish = true;
        Rotate_On_Ramp(type);
    }
    
    public void Ramp_To_Ramp_Move(Vector2 pos)
    {
        rb.drag = 0;
        target_pos = pos;
        velocity = (pos - (Vector2)transform.position) * 1.2f;
        moving = true;
        on_ramp = true;
        call_finish = true;
        rb.velocity = velocity;
    }
    public void Simple_Move(Vector2 pos)
    {
        call_finish = true;
        on_ramp = false;
        rb.drag = 0;
        target_pos = pos;
        velocity = (pos - (Vector2)transform.position) * 2;
        moving = true;
        rb.velocity = velocity;
        Rotate_On_Block();
    }

    public void Ramp_To_Corner_Move(Vector2 pos,int type)
    {
        call_finish = true;
        target_pos = Ramp_To_Corner_Pos(Direction.Down,pos);
        velocity = Ramp_To_Corner_Velocity(Direction.Down, pos);
        moving = true;
        rb.drag = 0;
        Debug.Log(target_pos);
    }

    private Vector2 Ramp_To_Corner_Velocity(Direction gravity,Vector2 target)
    {
        if(gravity == Direction.Down)
        {
            if (target.x - transform.position.x > 0)
                return new Vector2(1, 0);
            else
                return new Vector2(-1, 0);
        }

        return new Vector2(0, 0);
    }
    private Vector2 Ramp_To_Corner_Pos(Direction gravity,Vector2 target)
    {
        if(gravity == Direction.Down)
        {
            if (target.x - transform.position.x > 0)
                return (Vector2)transform.position + new Vector2(0.1f, 0);
            else
                return (Vector2)transform.position + new Vector2(-0.1f, 0);
        }

        return new Vector2(0, 0);
    }

    private void Rotate_On_Block()
    {
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        if (GetComponent<Player>().direction == Direction.Right)
            transform.GetChild(0).rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        else if (GetComponent<Player>().direction == Direction.Left)
            transform.GetChild(0).rotation = Quaternion.Euler(new Vector3(0, 180, 0)); 
    }

    private enum MoveType
    {
        RampToRamp,RampToSharp,RampToCorner
    }

}

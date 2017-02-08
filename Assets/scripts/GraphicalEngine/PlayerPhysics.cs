using UnityEngine;
using System.Collections;

public class PlayerPhysics : MonoBehaviour
{
    public float move_time = 0.5f;
    public float fall_acceleration = 3;
    public float fall_velocity = 1;
    private float platform_move_time = 1;
    private APIGraphic api;
    private LogicalEngine engine;
    private int sharp_type;
    private Transform player_transofrm;
    private MoveType move_type;
    private Player player;
    private Vector2 real_end;
    private bool set_percent;
    void Start()
    {
        engine = Starter.GetEngine();
        api = engine.apigraphic;
        player_transofrm = transform;
        player = GetComponent<Player>();
        move_type = MoveType.Idle;
    }

    //ramp to fall
    public void Ramp_To_Fall(Vector2 pos,int type)
    {
        StopAllCoroutines();
        Rotate_On_Ramp(type);
        StartCoroutine(Constant_Move(pos, move_time, true));
    }

    // when platform is moving move the player
    public void On_Platform_Move(Direction dir)
    {
        Vector2 pos = Toolkit.DirectiontoVector(dir) + (Vector2)player_transofrm.position;
        Debug.Log("On platform move");
        StartCoroutine(Constant_Move(pos, platform_move_time, false));

    }

    //ramp to block
    public void Ramp_To_Block_Move(Vector2 pos,int type)
    {
        StopAllCoroutines();
        Vector2 end1 = pos  +Ramp_To_Block_Pos(type);
        StartCoroutine(Ramp_To_Block_Coroutine(end1, pos, move_time, true));
        
       
    }

    private Vector2 Ramp_To_Block_Pos(int type)
    {
        if(type == 4)
        {
            if (player.direction == Direction.Right)
                return new Vector2(-0.5f, -0.1f);
        }
        else if(type == 1)
        {
            if (player.direction == Direction.Left)
                return new Vector2(0.8f,-0.1f);
        }

        return new Vector2(0, 0);
    }
    public void Block_To_Ramp_Move(Vector2 pos, int type)
    {
        StopAllCoroutines();
        Debug.Log(type);
        Vector2 end1 = pos + Block_To_Ramp_Pos(type);
        Vector2 end2 = (Vector2)pos + On_Ramp_Pos(type);
        StartCoroutine(Block_To_Ramp_Coroutine(end1,end2,move_time,true,type));
        
    }
    
    public void Land(Vector2 position)
    {
        Rotate_On_Block();
    }
    public void Land_On_Ramp(Vector2 position,int type)
    {
        player_transofrm.position = position + On_Ramp_Pos(type);
        Rotate_On_Ramp(type);
    }

    private Vector2 Block_To_Ramp_Pos(int type)
    {
        if (type == 4)
        {
            if (player.direction == Direction.Left)
                return new Vector2(0.2f, 0.7f);
            else if (player.direction == Direction.Right)
                return new Vector2(-0.4f, 0);
        }
        else if(type == 1)
        {
            if (player.direction == Direction.Left)
                return new Vector2(0.5f, 0);
            else if (player.direction == Direction.Right)
                return new Vector2(-0.2f, 0.9f);
        }
        return new Vector2(0, 0);
    }
    
    public void Ramp_To_Sharp_Move(Vector2 pos,int type)
    {
        StopAllCoroutines();
        Vector2 end1 = Ramp_To_Sharp_Pos(Direction.Down, pos);
        Vector2 end2 = pos + On_Ramp_Pos(type);
        StartCoroutine(Ramp_To_Sharp_Coroutine(end1,end2,move_time,true,type));
        sharp_type = type;
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


    public void Lean_Stick_Move(Direction dir)
    {

        StopAllCoroutines();
        Vector2 target = Toolkit.DirectiontoVector(dir) + (Vector2)player_transofrm.position;
        StartCoroutine(Constant_Move(target,platform_move_time,false));
    }

    private void Sharp_To_Ramp_Move(int type)
    {

        Rotate_On_Ramp(type);
    }
    

    //ramp to ramp
    public void Ramp_To_Ramp_Move(Vector2 pos,int type)
    {
        StopAllCoroutines();
        Vector2 on_ramp_pos = On_Ramp_Pos(type);
        pos = (Vector2)pos + on_ramp_pos;
        StartCoroutine(Constant_Move(pos, move_time, true));
        Rotate_On_Ramp(type);
    }
   
    //fall 
    public void Fall(Vector2 pos)
    {
        StopAllCoroutines();
        move_type = MoveType.Falling;
        StartCoroutine(Accelerated_Move(pos,fall_velocity,fall_acceleration,true));
    }
    public void Simple_Move(Vector2 pos)
    {
        set_percent = true;
        real_end = pos;
           StopAllCoroutines();
            Rotate_On_Block();
            move_type = MoveType.BlockToBlock;
            StartCoroutine(Constant_Move(pos, move_time, true));

    }

    // ramp to corner move
    public void Ramp_To_Corner_Move(Vector2 pos,int type)
    {
        Rotate_On_Ramp(type);
        pos += Ramp_To_Corner_Pos(Direction.Down, pos);
        StartCoroutine(Constant_Move(pos, move_time, true));
    }


    // block to ramp co
    private IEnumerator Block_To_Ramp_Coroutine(Vector2 end1, Vector2 end2, float mvoe_time, bool call_finish,int type)
    {
        float remain_distance = ((Vector2)player_transofrm.position - end1).sqrMagnitude;
        while (remain_distance > float.Epsilon)
        {
            remain_distance = ((Vector2)player_transofrm.position - end1).sqrMagnitude;
            player_transofrm.position = Vector3.MoveTowards(player_transofrm.position, end1, Time.deltaTime * 1 / move_time);
            yield return null;
        }
        remain_distance = ((Vector2)player_transofrm.position - end2).sqrMagnitude;
        Rotate_On_Ramp(type);
        while (remain_distance > float.Epsilon)
        {
            remain_distance = ((Vector2)player_transofrm.position - end2).sqrMagnitude;
            player_transofrm.position = Vector3.MoveTowards(player.transform.position, end2, Time.deltaTime * 1 / move_time);
            yield return null;
        }
        if (call_finish)
            api.MovePlayerFinished(gameObject);
        api.Check_Camera(player);
       
    }

    private IEnumerator Ramp_To_Block_Coroutine(Vector2 end1,Vector2 end2,float mvoe_time,bool call_finish)
    {
        float remain_distance = ((Vector2)player_transofrm.position - end1).sqrMagnitude;
        while(remain_distance > float.Epsilon)
        {
            remain_distance = ((Vector2)player_transofrm.position - end1).sqrMagnitude;
            player_transofrm.position = Vector3.MoveTowards(player_transofrm.position, end1, Time.deltaTime * 1 / move_time);
            yield return null;
        }
        remain_distance = ((Vector2)player_transofrm.position - end2).sqrMagnitude;
        Rotate_On_Block();
        while (remain_distance > float.Epsilon)
        {
            remain_distance = ((Vector2)player_transofrm.position - end2).sqrMagnitude;
            player_transofrm.position = Vector3.MoveTowards(player.transform.position, end2, Time.deltaTime * 1 / move_time);
            yield return null;
        }
        if (call_finish)
            api.MovePlayerFinished(gameObject);
        api.Check_Camera(player);
        
    }
    private IEnumerator Ramp_To_Sharp_Coroutine(Vector2 end1, Vector2 end2, float move_time, bool call_finish,int type)
    {
        // 1st part of mvoe
        float remain_distance = ((Vector2)player_transofrm.position - end1).sqrMagnitude;
        while (remain_distance > float.Epsilon)
        {
            remain_distance = ((Vector2)player_transofrm.position -end1).sqrMagnitude;
            player_transofrm.position = Vector3.MoveTowards(player_transofrm.position, end1, Time.deltaTime * 1 /  move_time);
            yield return null;
        }

        // 2nd part of move
        remain_distance = ((Vector2)player_transofrm.position - end2).sqrMagnitude;
        Rotate_On_Ramp(type);
        while(remain_distance > float.Epsilon)
        {
            remain_distance = ((Vector2)player_transofrm.position - end2).sqrMagnitude;
            player_transofrm.position = Vector3.MoveTowards(player.transform.position, end2, Time.deltaTime * 1 /  move_time);
            yield return null;
        }
        if (call_finish)
            api.MovePlayerFinished(gameObject);
        api.Check_Camera(player);
    }

    public void Set_End(Vector2 pos)
    {
        Debug.Log("Set End");
        real_end = pos;
    }

    // For Simple Constant Velocity Moves
    private IEnumerator Constant_Move(Vector2 end,float move_time,bool call_finish)
    {
        float remain_distance = ((Vector2)player_transofrm.position - real_end).sqrMagnitude;
        while(remain_distance > float.Epsilon)
        {
            remain_distance = ((Vector2)player_transofrm.position - real_end).sqrMagnitude;
            player_transofrm.position = Vector2.MoveTowards(player_transofrm.position, real_end, Time.deltaTime * 1 / move_time);
            Set_Player_Move_Percent(remain_distance);
            yield return null;
        }
        move_type = MoveType.Idle;
        if (call_finish)
        {
            api.MovePlayerFinished(gameObject);
            player.movepercentage = 100;
        }
        api.Check_Camera(player);
        // if it needs Call Finished Move of API
    }


    private void Set_Player_Move_Percent(float remain)
    {
        if (remain >= 0.9f && set_percent)
        {
            set_percent = false;
            Debug.Log("90 percent");
            player.movepercentage = 90;
        }
    }

    // For Moves that have Acceleration Like Gravity
    private IEnumerator Accelerated_Move(Vector2 end,float velocity, float a,bool call_finish)
    {
        float remain_distance = ((Vector2)player_transofrm.position - end).sqrMagnitude;
        while(remain_distance > float.Epsilon)
        {
            remain_distance = ((Vector2)player_transofrm.position - end).sqrMagnitude;
            player_transofrm.position = Vector3.MoveTowards(player_transofrm.position, end, Time.deltaTime * velocity);
            velocity += Time.deltaTime * a;
            yield return null;
        }
        if(call_finish)
        {
            switch (move_type)
            {
                case MoveType.Falling: api.Fall_Finish(player); move_type = MoveType.Idle; break;
            }
        }
        api.Check_Camera(player);

    }
    private Vector2 Ramp_To_Corner_Pos(Direction gravity,Vector2 target)
    {
        if(gravity == Direction.Down)
        {
            if (target.x - transform.position.x > 0)
                return  new Vector2(-0.3f, 0.2f);
            else
                return  new Vector2(0.3f, 0.2f);
        }

        return new Vector2(0, 0);
    }


    private Vector2 On_Ramp_Pos(int type)
    {
        if (type == 4)
            return new Vector2(-0.22f, 0.2f);
        else if (type == 1)
            return new Vector2(0.19f,0.25f);

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

   private void Rotate_On_Ramp(int type)
    {
        Player player = GetComponent<Player>();
        if (player.direction == Direction.Right)
            transform.GetChild(0).rotation = Quaternion.Euler(new Vector3(0, 0, Ramp_Rotation_Value(type,player.direction)));
        else if (player.direction == Direction.Left)
            transform.GetChild(0).rotation = Quaternion.Euler(new Vector3(0, 180, Ramp_Rotation_Value(type,player.direction)));
    }

    private float Ramp_Rotation_Value(int type,Direction dir)
    {
        if (type == 4)
        {
            if (dir == Direction.Right)
                return 45;
            else
                return -45;
        }
        else if (type == 1)
        {
            if (dir == Direction.Right)
                return -45;
            else
                return 45;
        }

        else
            return 0;
    }

    private enum MoveType
    {
        RampToRamp,RampToSharp,RampToCorner,Falling,BlockToBlock,Idle
    }

}

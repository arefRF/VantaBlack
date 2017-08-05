using UnityEngine;
using System.Collections;

public class PlayerPhysics : MonoBehaviour
{
    public float move_time = 0.5f;
    public float fall_acceleration = 100;
    public float fall_velocity = 2;
    public float Jump_Acceleration = -0.01f;
    public float Jump_Velocity = 10;
    private float platform_move_time = 1;
    private APIGraphic api;
    private LogicalEngine engine;
    private int sharp_type;
    private Transform player_transofrm;
    private MoveType move_type;
    private Player player;
    private bool set_percent;
    private Jump jump_ability;
   // private Coroutine lean_stick_co;
    //private Coroutine on_Platform_co;
    private Coroutine last_co;

    void Start()
    {
        Init();
    }

    private void Init()
    {
        engine = Starter.GetEngine();
        api = engine.apigraphic;
        player_transofrm = transform;
        player = GetComponent<Player>();
    }

    //ramp to fall
    public void Ramp_To_Fall(Vector2 pos,int type)
    {
        move_type = MoveType.RampToFall;
        StopAllCoroutines();
        Rotate_On_Ramp(type);
        last_co =  StartCoroutine(Constant_Move(pos, move_time, true));
    }

    // when platform is moving move the player
    public void On_Platform_Move(Vector2 pos)
    {
        if (last_co != null)
            StopCoroutine(last_co);
        move_type = MoveType.OnPlatform;
        last_co =  StartCoroutine(Constant_Move(pos, platform_move_time, false));

    }

    public void On_Platform_Stay(Vector2 pos)
    {
        StopAllCoroutines();
    }

    //ramp to block
    public void Ramp_To_Block_Move(Vector2 pos,int type)
    {
        move_type = MoveType.RampToBlock;
        if (last_co != null)
            StopCoroutine(last_co);
        Vector2 end1 = pos  + Ramp_To_Block_Pos(type);
        last_co = StartCoroutine(Ramp_To_Block_Coroutine(end1, pos, move_time, true));
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
        
        move_type = MoveType.BlockToRamp;
        if(last_co != null)
            StopCoroutine(last_co);
        Vector2 end1 = pos + Block_To_Ramp_Pos(type);
        Vector2 end2 = (Vector2)pos + On_Ramp_Pos(type);
        last_co = StartCoroutine(Ramp_To_Block_Coroutine(end1,end2,move_time,true));
        
    }
    
    public void Land(Vector2 position)
    {
        StartCoroutine(WaitToLandRotate());
    }

    private IEnumerator WaitToLandRotate()
    {
        yield return new WaitForSeconds(0.3f);
        Rotate_On_Block();
    }
    public void Land_On_Ramp(Vector2 position,int type)
    {
        move_type = MoveType.Land;
        Vector2 pos  = position + On_Ramp_Pos(type);
        if (last_co != null)
            StopCoroutine(last_co);
        last_co =  StartCoroutine(LandOnRamp(pos, 0.1f));
    }

    private Vector2 Block_To_Ramp_Pos(int type)
    {
        if (type == 4)
        {
            if (player.direction == Direction.Left)
                return new Vector2(0f, 0.8f);
            else if (player.direction == Direction.Right)
                return new Vector2(-0.5f, 0.2f);
        }
        else if(type == 1)
        {
            if (player.direction == Direction.Left)
                return new Vector2(0.5f, 0);
            else if (player.direction == Direction.Right)
                return new Vector2(-0.3f, 0.9f);
        }
        return new Vector2(0, 0);
    }
    
    public void Ramp_To_Sharp_Move(Vector2 pos,int type)
    {
        move_type = MoveType.RampToSharp;
        if(last_co!=null)
            StopCoroutine(last_co);
        Vector2 end1 = Ramp_To_Sharp_Pos(Direction.Down, pos);
        Vector2 end2 = pos + On_Ramp_Pos(type);
        last_co = StartCoroutine(Ramp_To_Sharp_Coroutine(end1,end2,move_time,true,type));
        sharp_type = type;
    }

    public void Adjust(Vector2 pos,Direction dir, System.Action<Player, Direction> passingmethod)
    {
        Debug.Log("Adjust");
        StartCoroutine(AdjustCo(pos,dir,passingmethod));
    }


    private IEnumerator AdjustCo(Vector2 end,Direction dir, System.Action<Player, Direction> passingmethod)
    {
        set_percent = true;
        float remain_distance = Distance((Vector2)transform.position, end);
        while (remain_distance > float.Epsilon)
        {
            remain_distance = Distance((Vector2)transform.position, end);
            transform.position = Vector2.MoveTowards(transform.position, end, Time.smoothDeltaTime / move_time);
            api.Camera_AutoMove();
            yield return new WaitForSeconds(0.001f);
        }
        player.AdjustPlayerFinshed(dir, passingmethod);

        // if it needs Call Finished Move of API
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


    public void Lean_Stick_Move(Vector2 pos)
    {
        if (last_co != null)
            StopCoroutine(last_co);
        move_type = MoveType.LeanStick;
        last_co =  StartCoroutine(Constant_Move(pos,platform_move_time,true));
    }

    public void Lean_Stick_Stop()
    {
        if(last_co!=null)
            StopCoroutine(last_co);
    }
   

    //ramp to ramp
    public void Ramp_To_Ramp_Move(Vector2 pos,int type)
    {
        if(last_co!= null )
            StopCoroutine(last_co);
        Vector2 on_ramp_pos = On_Ramp_Pos(type);
        pos = (Vector2)pos + on_ramp_pos;
        last_co = StartCoroutine(Constant_Move(pos, move_time, true));
        Rotate_On_Ramp(type);
    }
   
    public void Player_Undo(Unit unit)
    {
        StopAllCoroutines();
        if(unit != null && unit is Ramp)
        {
            player_transofrm.position = On_Ramp_Pos(((Ramp)unit).type);
        }

    }

    //fall 
    public void Fall(Vector2 pos)
    {

        StopAllCoroutines();
        move_type = MoveType.Falling;
        last_co  = StartCoroutine(Accelerated_Move(pos,fall_velocity,fall_acceleration,true));
        float rotate_to = GetRotationGravity(player.gravity,player.direction);
        if (player.direction == Direction.Right || player.direction == Direction.Up)
            rotate_to -= 30;
        else
            rotate_to -= 30;
          float y = transform.GetChild(0).rotation.eulerAngles.y;
        // transform.GetChild(0).rotation = Quaternion.Euler(0, y, rotate_to);
        //StartCoroutine(FallRotation(rotate_to));
    }


    private float GetRotationGravity(Direction gravity_dir,Direction player_dir)
    {
        if (gravity_dir == Direction.Down)
            return 0;
        else if (gravity_dir == Direction.Up)
            return 180;
        else if(gravity_dir == Direction.Right)
        {
            if (player_dir == Direction.Up)
                return 90;
            else
                return 270;
        }
        else
        {
            if (player_dir == Direction.Up)
                return 90;
            else
                return 270;
        }
    }

    private IEnumerator FallRotation(float rotate_to)
    {
        Debug.Log("Start of Couroutine");
        Debug.Log(rotate_to);
        float y = transform.GetChild(0).rotation.eulerAngles.y;
        float degree = transform.GetChild(0).rotation.eulerAngles.z;
        float remain = Mathf.Abs(degree - rotate_to);
        while (remain > 2 )
        {
            remain = Mathf.Abs(transform.GetChild(0).rotation.eulerAngles.z - rotate_to);
            degree = MoveToward(transform.GetChild(0).rotation.eulerAngles.z, rotate_to,1);
            transform.GetChild(0).rotation = Quaternion.Euler(0, y, degree);
            yield return null;
        }
      
    }

    private float MoveToward(float current,float target,float delta)
    {
        Debug.Log("current "+ current);
        Debug.Log("target " + target);
        if(current > 0 && target > 0)
        {
            if (current < target)
                return current += delta;
            else
                return current -= delta;
        }
        else
        {
            if (Mathf.Abs(current - target) > 180)
                return current -= delta;
            else
                return current += delta;
        }
    }
    public void Fall_Die(Vector2 pos)
    {
        Camera.main.GetComponent<CameraController>().auto_move = false;
        move_type = MoveType.FallDie;
        StopAllCoroutines();
        StartCoroutine(Accelerated_Move(pos, fall_velocity, fall_acceleration, false));
    }
    public void Branch_Branch(Vector2 pos)
    {
        move_type = MoveType.BranchToBranch;
        set_percent = true;
        if (last_co != null)
            StopCoroutine(last_co);
        last_co = StartCoroutine(Constant_Move(pos, 0.1f, true));

    }
    public void Simple_Move(Vector2 pos)
    {
        set_percent = true;
        if (last_co != null)
            StopCoroutine(last_co);
        
        move_type = MoveType.BlockToBlock;
        last_co = StartCoroutine(Constant_Move(pos, move_time, true));

    }

    // jump takes jump ability to call function
    public void Jump(Vector2 pos,Jump ability,Direction dir,bool hit)
    {
        jump_ability = ability;
        if (last_co != null)
            StopCoroutine(last_co);
        last_co = StartCoroutine(Jump_couroutine(pos, 2, dir, ability,hit));

    }

    
    // ramp to corner move
    public void Ramp_To_Corner_Move(Vector2 pos,int type)
    {
        if(last_co!=null)
            StopCoroutine(last_co);
        move_type = MoveType.RampToCorner;
        Rotate_On_Ramp(type);
        pos += Ramp_To_Corner_Pos(Direction.Down, pos);
        last_co = StartCoroutine(Constant_Move(pos, move_time, true));
    }


    // block to ramp co
    private IEnumerator Block_To_Ramp_Coroutine(Vector2 end1, Vector2 end2, float move_time, bool call_finish,int type)
    {
        set_percent = true;
        float remain_distance = ((Vector2)player_transofrm.position - end1).sqrMagnitude;
        while (remain_distance > float.Epsilon)
        {
            remain_distance = ((Vector2)player_transofrm.position - end2).sqrMagnitude;
            player_transofrm.position = Vector3.MoveTowards(player.transform.position, end2, Time.deltaTime * 1 / move_time);
            Set_Player_Move_Percent(remain_distance);
            api.Camera_AutoMove();
            yield return null;
        }
        if (call_finish)
            api.MovePlayerFinished(gameObject);
        player.movepercentage = 0;

    }

    private IEnumerator Ramp_To_Block_Coroutine(Vector2 end1,Vector2 end2,float mvoe_time,bool call_finish)
    {
        set_percent = true;
        float remain_distance = ((Vector2)player_transofrm.position - end1).sqrMagnitude;
        while(remain_distance > float.Epsilon)
        {
            remain_distance = ((Vector2)player_transofrm.position - end1).sqrMagnitude;
            player_transofrm.position = Vector3.MoveTowards(player_transofrm.position, end1, Time.deltaTime * 1 / move_time);
            yield return null;
        }
        remain_distance = ((Vector2)player_transofrm.position - end2).sqrMagnitude;
        while (remain_distance > float.Epsilon)
        {
            remain_distance = ((Vector2)player_transofrm.position - end2).sqrMagnitude;
            player_transofrm.position = Vector3.MoveTowards(player.transform.position, end2, Time.deltaTime * 1 / move_time);
            Set_Player_Move_Percent(remain_distance);
            yield return null;
        }

        if (call_finish)
            api.MovePlayerFinished(gameObject);
        player.movepercentage = 0;
        
    }
    private IEnumerator Ramp_To_Sharp_Coroutine(Vector2 end1, Vector2 end2, float move_time, bool call_finish,int type)
    {
        set_percent = true;
        // 1st part of mvoe
        float remain_distance = ((Vector2)player_transofrm.position - end1).sqrMagnitude;
        while (remain_distance > float.Epsilon)
        {
            remain_distance = ((Vector2)player_transofrm.position -end1).sqrMagnitude;
            
            player_transofrm.position = Vector3.MoveTowards(player_transofrm.position, end1, Time.deltaTime * 1 /  move_time);
            api.Camera_AutoMove();
            yield return null;
        }

        // 2nd part of move
        remain_distance = ((Vector2)player_transofrm.position - end2).sqrMagnitude;
        Rotate_On_Ramp(type);
        while(remain_distance > float.Epsilon)
        {
            remain_distance = ((Vector2)player_transofrm.position - end2).sqrMagnitude;
            player_transofrm.position = Vector3.MoveTowards(player.transform.position, end2, Time.deltaTime * 1 /  move_time);
            Set_Player_Move_Percent(remain_distance);
            api.Camera_AutoMove();
            yield return null;
        }
        if (call_finish)
            api.MovePlayerFinished(gameObject);
        player.movepercentage = 0;
    }

    public void Set_End(Vector2 pos)
    {

    }

    // For Simple Constant Velocity Moves
    private IEnumerator Constant_Move(Vector2 end,float move_time,bool call_finish)
    {
        set_percent = true;
        float remain_distance = Distance((Vector2)transform.position, end);
        while (remain_distance > float.Epsilon)
        {
            remain_distance = Distance((Vector2)transform.position, end);
            transform.position = Vector2.MoveTowards(transform.position, end, Time.smoothDeltaTime  / move_time );
            if(move_type != MoveType.BranchToBranch)
                Set_Player_Move_Percent(remain_distance);
            yield return new WaitForSeconds(0.001f);
        }
       
        if (call_finish)
        {
            if (move_type == MoveType.BranchToBranch)
            {
                yield return new WaitForSeconds(0.3f);
            }

            if (move_type != MoveType.Land)
                api.MovePlayerFinished(gameObject);
            else
            {
                player.LandOnRampFinished();   
            }
        }

        //cuz it messed with falling
        if(move_type!= MoveType.Falling)
            move_type = MoveType.Idle;
        player.movepercentage = 0;
        // if it needs Call Finished Move of API
    }

    private void Set_Player_Move_Percent(float remain)
    {
        if (remain < 0.02f && set_percent)
        {
            if (move_type == MoveType.LeanStick)
                api.LeanStickFinished(player);
            else if (move_type == MoveType.OnPlatform)
                api.MovePlayerFinished(gameObject);
            set_percent = false;
            player.movepercentage = 98;
        }
    }

    // For Moves that have Acceleration Like Gravity
    private IEnumerator Accelerated_Move(Vector2 end,float velocity, float a,bool call_finish)
    {

        float remain_distance = 0;
        try
        {
             remain_distance = ((Vector2)player_transofrm.position - end).sqrMagnitude;
        }
        catch
        {
            Init();
            remain_distance = ((Vector2)player_transofrm.position - end).sqrMagnitude;
        }
        while(remain_distance > float.Epsilon)
        {
            remain_distance = ((Vector2)player_transofrm.position - end).sqrMagnitude;
            player_transofrm.position = Vector3.MoveTowards(player_transofrm.position, end, Time.deltaTime * velocity);
            velocity += 0.1f * a;
            api.Camera_AutoMove();
            yield return null;
        }
        if(call_finish)
        {
            switch (move_type)
            {
                case MoveType.Falling: api.Fall_Finish(player); move_type = MoveType.Idle; break;
            }
        }
    }

    private IEnumerator LandOnRamp(Vector2 pos,float time)
    {
        float remain_distance = ((Vector2)player_transofrm.position - pos).sqrMagnitude;

        while (remain_distance > float.Epsilon)
        {
            remain_distance = ((Vector2)player_transofrm.position - pos).sqrMagnitude;
            player_transofrm.position = Vector3.MoveTowards(player_transofrm.position, pos, Time.deltaTime / time);
            yield return null;
        }

        player.LandOnRampFinished();
    }
    private IEnumerator Jump_couroutine(Vector2 pos,float jump_time,Direction direction, Jump jump,bool hit)
    {
        float j_velocity = Jump_Velocity;
        float remain_distance = ((Vector2)player_transofrm.position - pos).sqrMagnitude;
        //float remain_distance = Mathf.Sqrt(Mathf.Pow(player_transofrm.position.x - pos.x, 2) + Mathf.Pow(player_transofrm.position.y - pos.y, 2));
        while (remain_distance > float.Epsilon)
        {
            remain_distance = ((Vector2)player_transofrm.position - pos).sqrMagnitude;
            player_transofrm.position = Vector3.MoveTowards(player_transofrm.position, pos, Time.deltaTime * j_velocity);
            j_velocity += Jump_Acceleration/2f;
            api.Camera_AutoMove();
            yield return null;
        }
        if (!hit)
            api.Jump_Finish(player, pos, jump);
        else
        {
            GetComponent<PlayerGraphics>().Hit();
            api.Jump_Hit_Finish(player, jump, pos);
        }
    }
    public void Roll(Vector2 pos)
    {
        StartCoroutine(RollCouroutine(pos));
    }

    public IEnumerator RollCouroutine(Vector2 pos)
    {
        float velocity =3;
        float remain_distance = ((Vector2)player_transofrm.position - pos).sqrMagnitude;

        while (remain_distance > float.Epsilon)
        {
            remain_distance = ((Vector2)player_transofrm.position - pos).sqrMagnitude;
            player_transofrm.position = Vector3.MoveTowards(player_transofrm.position, pos, Time.deltaTime * velocity);
            yield return null;
        }
        player.RollingFinished();

    }
    
    private float Distance(Vector2 pos1, Vector2 pos2)
    {
        return Mathf.Sqrt(Mathf.Pow(pos1.x - pos2.x,2) + Mathf.Pow(pos1.y - pos2.y,2));
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
            return new Vector2(-0.35f, 0.4f);
        else if (type == 1)
            return new Vector2(0.35f,0.4f);

        return new Vector2(0, 0);
    }
    private void Rotate_On_Block()
    {
        float target = GetRotationGravity(player.gravity,player.direction);
        float y = transform.GetChild(0).rotation.eulerAngles.y;
        transform.GetChild(0).rotation = Quaternion.Euler(0, y, target);
        //StartCoroutine(FallRotation(target));
    }

    private IEnumerator RotateOnBlockCouroutine()
    {
        float y = transform.GetChild(0).rotation.y;
        float degree = -30;
        while (degree < 0)
        {
            degree += 0.5f;
            transform.GetChild(0).rotation = Quaternion.Euler(0, y, degree);
            yield return null;
        }
    }
   private void Rotate_On_Ramp(int type)
    {

    }



    private enum MoveType
    {
        RampToRamp,RampToSharp,RampToCorner,Falling,BlockToBlock,Idle,LeanStick,RampToFall,BlockToFall,OnPlatform,RampToBlock,BlockToRamp,FallDie,Land,Adjust,BranchToBranch
    }

}

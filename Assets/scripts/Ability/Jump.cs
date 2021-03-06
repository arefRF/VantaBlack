using UnityEngine;
using System.Collections;

public class Jump : Ability {

    public int number { get; set; }
    private int maxJump;
    LogicalEngine engine;
    public Coroutine coroutine;
    private Vector2 final_pos;
    private Direction leandirection;
	public Jump()
    {
       abilitytype = AbilityType.Jump;
    }

    public Jump(int number)
    {
        abilitytype = AbilityType.Jump;
        this.number = number;
    }

    public void Action(Player player, Direction direction)
    {
        System.Diagnostics.StackTrace stackTrace = new System.Diagnostics.StackTrace();
        Debug.Log(stackTrace.GetFrame(1).GetMethod().Name);
        leandirection = direction;
        Vector2 playerpos = player.position;
        Vector2 playerpos2 = player.position;
        switch (direction)
        {
            case Direction.Up: playerpos.x = player.transform.position.x; break;
            case Direction.Right: playerpos.y = player.transform.position.y; break;
            case Direction.Down: playerpos.x = player.transform.position.x; break;
            case Direction.Left: playerpos.y = player.transform.position.y; break;
        }
        if (player.mode == GameMode.Real)
        {
            if (player.GetComponent<UnityPhysics>().state == PhysicState.Falling)
                return;
            playerpos = player.transform.position;
            playerpos2 = player.transform.position;
            player.GetComponent<Rigidbody2D>().isKinematic = true;
        }
        Starter.GetDataBase().StopTimer();
        player.SetState(PlayerState.Busy);
        player.currentAbility = this;
        if (engine == null)
            engine = Starter.GetEngine();
        Vector2 finalpos = playerpos + number * Toolkit.DirectiontoVector(direction);
        final_pos = playerpos2 + number * Toolkit.DirectiontoVector(direction);
        maxJump = GetShouldJump(player.position, direction);
        engine.apiunit.AddToSnapshot(player);
        engine.inputcontroller.LeanUndo(player, player.leandirection, PlayerState.Busy);
        player.jumpdirection = direction;
        if (number <= maxJump)
        {
            engine.apigraphic.Jump(player, this, finalpos, direction);
        }
        else
        {
            Debug.Log("jump hit");
            // calculate where to hit and call graphic hit
            Vector2 hitPos = playerpos + maxJump * Toolkit.DirectiontoVector(direction);
            final_pos = hitPos;
            if (!PlayerLean(player))
                engine.apigraphic.Jump_Hit(player, direction, this, hitPos);

        }
        
    }

    public void JumpFinished(Player player)
    {
        Debug.Log("jump finished");
        engine.apiunit.RemoveFromDatabase(player);
        player.position = final_pos;
        engine.apiunit.AddToDatabase(player);
        player.SetState(PlayerState.Jumping);
        /*Vector2 temppos = Toolkit.VectorSum(player.position, engine.database.gravity_direction);
        Ramp ramp = Toolkit.GetRamp(temppos);
        if (Toolkit.IsEmpty(Toolkit.VectorSum(player.position, engine.database.gravity_direction)) || (ramp != null && !Toolkit.IsdoubleRamp(temppos) && ramp.IsOnRampSide(Toolkit.ReverseDirection(engine.database.gravity_direction))))
        {
            if (engine.apiinput.isAnyArrowKeyDown())
            {
                Direction direction = engine.apiinput.GetArrowKeyDown();
                if (PlayerMoveDirection(player, direction))
                    return;
                if(PlayerLeanDireciton(player, direction))
                    return;
                
            }
            if (!PlayerMove(player))
                if (!PlayerLean(player))
                    player.ApplyGravity();
        }
        else
            player.ApplyGravity();*/
    }

    private bool PlayerMoveDirection(Player player, Direction direction)
    {
        if (!Toolkit.IsEmpty(Toolkit.VectorSum(player.position, direction)) && !Toolkit.HasBranch(Toolkit.VectorSum(player.position, direction)))
            return false;
        Vector2 pos = Toolkit.VectorSum(Toolkit.VectorSum(player.position, direction), player.gravity);
        if (Toolkit.IsEmpty(pos) && !Toolkit.HasBranch(Toolkit.VectorSum(player.position, direction)))
            return false;
        player.direction = direction;
        player.SetState(PlayerState.Jumping);
        engine.inputcontroller.JumpingPlayerMove(player, direction);
        return true;
    }

    public bool PlayerMove(Player player)
    {
        if (!Toolkit.IsEmpty(Toolkit.VectorSum(player.position, player.direction)) && !Toolkit.HasBranch(Toolkit.VectorSum(player.position, player.direction)))
            return false;
        Vector2 pos = Toolkit.VectorSum(Toolkit.VectorSum(player.position, player.direction), Starter.GetGravityDirection());
        if (Toolkit.IsEmpty(pos) && !Toolkit.HasBranch(Toolkit.VectorSum(player.position, player.direction)))
            return false;
        player.SetState(PlayerState.Jumping);
        engine.inputcontroller.JumpingPlayerMove(player, player.direction);
        return true;
    }

    private bool PlayerLeanDireciton(Player player, Direction direction)
    {
        if (!Toolkit.IsEmpty(Toolkit.VectorSum(player.position, direction)) && Toolkit.GetUnit(Toolkit.VectorSum(player.position, direction)).isLeanable())
        {
            engine.inputcontroller.LeanOnAir(player, direction);
            return true;
        }
        return false;
    }

    private bool PlayerLean(Player player)
    {
        if (!Toolkit.IsEmpty(Toolkit.VectorSum(player.position, leandirection)) && Toolkit.GetUnit(Toolkit.VectorSum(player.position, leandirection)).isLeanable())
        {
            engine.inputcontroller.LeanOnAir(player, leandirection);
            return true;
        }
        else if (!Toolkit.IsEmpty(Toolkit.VectorSum(player.position, player.direction)) && Toolkit.GetUnit(Toolkit.VectorSum(player.position, player.direction)).isLeanable())
        {
            engine.inputcontroller.LeanOnAir(player, player.direction);
            return true;
        }
        else
        {
            for (int i = 0; i < 4; i++)
            {
                Vector2 temppos = Toolkit.VectorSum(player.position, Toolkit.NumberToDirection(i + 1));
                if (Toolkit.NumberToDirection(i + 1) != player.gravity)
                    if (!Toolkit.IsEmpty(temppos))
                        if (Toolkit.GetUnit(temppos).isLeanable())
                        {
                            Debug.Log(Toolkit.NumberToDirection(i + 1));
                            engine.inputcontroller.LeanOnAir(player, Toolkit.NumberToDirection(i + 1));
                            return true;
                        }
            }
        }
        return false;
    }

    public void JumpHitFinished(Player player)
    {
        engine.apiunit.RemoveFromDatabase(player);
        player.position = final_pos;
        engine.apiunit.AddToDatabase(player);
        Debug.Log("Jump Hit Finished");
        player.SetState(PlayerState.Idle);
        player.ApplyGravity();
    }
    public void StartTimer(Player player,Direction direction)
    {
        maxJump = GetShouldJump(player.position,direction);
        number = 2;
        Starter.GetDataBase().timer =  GameObject.Find("GetInput").GetComponent<GetInput>().StartCoroutine(Timer());
    }
    private IEnumerator Timer()
    {
        yield return new WaitForSeconds(0.75f);
        number++;
        number %= 5;
        if (number == 0)
            number = 2;
        Starter.GetDataBase().timer = GameObject.Find("GetInput").GetComponent<GetInput>().StartCoroutine(Timer());

    }

    /*private IEnumerator JumpWait(float f,Player player)
    {
        yield return new WaitForSeconds(f);
        if(player.mode == GameMode.Real)
            player.GetComponent<Rigidbody2D>().isKinematic = false;
        player.ApplyGravity();

    }*/



    private int GetShouldJump(Vector2 position, Direction direction)
    {
        int num = 0;
        for (int i = 0; i < number; i++)
        {
            position = Toolkit.VectorSum(position, direction);
            if (Toolkit.IsEmpty(position))
            {
                num++;
            }
            else if (Toolkit.HasRamp(position) && !Toolkit.IsdoubleRamp(position))
            {
                if (Toolkit.GetRamp(position).IsOnRampSide(Toolkit.ReverseDirection(direction)))
                    num++;
                else
                    break;
            }
            else
            {
                break;
            }
        }
        return num;
    }

    public override Ability ConvertContainerAbilityToPlayer(Player player)
    {
        number = 1;
        owner = player;
        return this;
    }

    public override Ability ConvertPlayerAbilityToContainer(Container container)
    {
        number = 4;
        owner = container;
        return this;
    }
}

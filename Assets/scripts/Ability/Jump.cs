﻿using UnityEngine;
using System.Collections;

public class Jump : Ability {

    public int number;
    private int maxJump;
    LogicalEngine engine;
    public Coroutine coroutine;
    private Vector2 final_pos;
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
            engine.apigraphic.Jump(player, this, finalpos, direction);
        else
        {
            Debug.Log("jump hit");
            // calculate where to hit and call graphic hit
            Vector2 hitPos = playerpos + maxJump * Toolkit.DirectiontoVector(direction);
            final_pos = hitPos;
            engine.apigraphic.Jump_Hit(player, direction, this, hitPos);

        }
        
    }

    public void JumpFinished(Player player)
    {
        player.SetState(PlayerState.Transition);
        engine.apiunit.RemoveFromDatabase(player);
        player.position = final_pos;
        engine.apiunit.AddToDatabase(player);
        Vector2 temppos = Toolkit.VectorSum(player.position, engine.database.gravity_direction);
        Ramp ramp = Toolkit.GetRamp(temppos);
        if (Toolkit.IsEmpty(Toolkit.VectorSum(player.position, engine.database.gravity_direction)) || (ramp != null && !Toolkit.IsdoubleRamp(temppos) && ramp.IsOnRampSide(Toolkit.ReverseDirection(engine.database.gravity_direction))))
            coroutine = GameObject.Find("GetInput").GetComponent<GetInput>().StartCoroutine(JumpWait(0.5f, player));
        else
            player.ApplyGravity();
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
        Debug.Log(number);
        Starter.GetDataBase().timer = GameObject.Find("GetInput").GetComponent<GetInput>().StartCoroutine(Timer());

    }

    private IEnumerator JumpWait(float f,Player player)
    {
        yield return new WaitForSeconds(f);
        if(player.mode == GameMode.Real)
            player.GetComponent<Rigidbody2D>().isKinematic = false;
        player.ApplyGravity();

    }



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
        number = 2;
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

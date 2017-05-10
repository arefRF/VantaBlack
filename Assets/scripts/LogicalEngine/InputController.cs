﻿using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class InputController {

    LogicalEngine engine;
    Database database;
    bool idlemovelock;
    public InputController(LogicalEngine engine)
    {
        this.engine = engine;
        database = engine.database;
    }

    public void PlayerMoveAction(Player player, Direction direction)
    {
        if (player.state == PlayerState.Idle)
        {
            IdlePLayerMove(player, direction);
        }
        else if(player.state == PlayerState.Moving)
        {
            MovingPlayerMove(player, direction);
        }
        else if (player.state == PlayerState.Jumping)
        {
            if (player.jumpdirection == Toolkit.ReverseDirection(direction))
                return;
            //JumpingPlayerMove(player, direction);
        }
        else if(player.state == PlayerState.Transition)
        {
            if (player.jumpdirection == Toolkit.ReverseDirection(direction))
                return;
            TransitionPlayerMove(player, direction);
        }
        else if(player.state == PlayerState.LeanTransition)
        {
            LeanTransitionPlayerMove(player, direction);
        }
    }

    private void LeanTransitionPlayerMove(Player player, Direction direction)
    {
        if (player.Can_Lean(direction))
        {
            Lean(player, direction);
        }
    }

    private void TransitionPlayerMove(Player player, Direction direction)
    {
        if (player.jumpdirection == direction || player.jumpdirection == Toolkit.ReverseDirection(direction))
        {
            if (Toolkit.HasBranch(Toolkit.VectorSum(player.position, direction)))
            {
                Branch branch = Toolkit.GetBranch(Toolkit.VectorSum(player.position, direction));
                if (branch.islocked)
                {
                    Lean(player, direction);
                }
                else if (player.JumpingMove(direction))
                {
                    player.direction = direction;
                    GameObject.Find("GetInput").GetComponent<GetInput>().StopCoroutine(((Jump)player.currentAbility).coroutine);
                    if (!player.isonejumping)
                        player.UseAbility(player.abilities[0]);
                    player.currentAbility = null;
                    player.SetState(PlayerState.Moving);
                }
            }
            else if (player.Can_Lean(direction))
            {
                GameObject.Find("GetInput").GetComponent<GetInput>().StopCoroutine(((Jump)player.currentAbility).coroutine);
                if (!player.isonejumping)
                    player.UseAbility(player.abilities[0]);
                Lean(player, direction);
            }
        }
        else if (direction != database.gravity_direction)
        {
            if (player.JumpingMove(direction))
            {
                player.direction = direction;
                GameObject.Find("GetInput").GetComponent<GetInput>().StopCoroutine(((Jump)player.currentAbility).coroutine);
                if (!player.isonejumping && player.abilities.Count != 0)
                    player.UseAbility(player.abilities[0]);
                player.currentAbility = null;
                player.SetState(PlayerState.Moving);
            }
            else if (player.Can_Lean(direction))
            {
                GameObject.Find("GetInput").GetComponent<GetInput>().StopCoroutine(((Jump)player.currentAbility).coroutine);
                if (!player.isonejumping && player.abilities.Count != 0)
                    player.UseAbility(player.abilities[0]);
                Lean(player, direction);
            }
        }
    }

    private void JumpingPlayerMove(Player player, Direction direction)
    {
        if (!player.lean && player.state == PlayerState.Jumping)
        {
            
            if (player.Can_Move_Direction(direction))
            {
                if (player.Should_Change_Direction(direction))
                {
                    Direction olddir = player.direction;
                    player.direction = direction;

                    engine.apigraphic.PlayerChangeDirection(player, olddir, player.direction);
                }
                else if (direction != player.jumpdirection && direction != Toolkit.ReverseDirection(player.jumpdirection) && player.JumpingMove(direction))
                {
                    GameObject.Find("GetInput").GetComponent<GetInput>().StopCoroutine(((Jump)player.currentAbility).coroutine);
                    player.currentAbility = null;
                    player.SetState(PlayerState.Moving);
                }
                else
                {
                    if (player.Can_Lean(Toolkit.VectorSum(player.position, direction)))
                    {
                        GameObject.Find("GetInput").GetComponent<GetInput>().StopCoroutine(((Jump)player.currentAbility).coroutine);
                        if (!player.isonejumping && player.abilities.Count > 0)
                        {
                            player.UseAbility(player.abilities[0]);
                        }
                        else
                        {
                            player.isonejumping = false;
                        }
                        //player.currentAbility = null;
                        Lean(player, direction);

                    }
                    else if (Toolkit.HasBranch(player.position))
                    {
                        Debug.Log("unhandeled!");
                    }
                }
            }
            else
            {
                if (player.Can_Lean(Toolkit.VectorSum(player.position, direction)))
                {
                    GameObject.Find("GetInput").GetComponent<GetInput>().StopCoroutine(((Jump)player.currentAbility).coroutine);
                    if (!player.isonejumping)
                    {
                        player.UseAbility(player.abilities[0]);
                    }
                    else
                    {
                        player.isonejumping = false;
                    }
                    Lean(player, direction);
                    //player.currentAbility = null;
                }
                else if (Toolkit.HasBranch(Toolkit.VectorSum(player.position, direction)))
                {
                    player.SetState(PlayerState.Falling);
                    engine.MovePlayer(player, direction);
                }
            }
        }
    }

    public void Jump(Player player)
    {
        if(player.state == PlayerState.Idle)
        {

            // Idle and simple jump
            Direction direction;
            if (player.lean)
                direction = Toolkit.ReverseDirection(player.leandirection);
            else
                direction = Toolkit.ReverseDirection(player.GetGravity());
            if(!Toolkit.IsInsideBranch(player) || Toolkit.HasBranch(Toolkit.VectorSum(player.position, direction)))
            {
                player.isonejumping = true;
                player.oneJump.Action(player, direction);
            }
            
        }
        else if(player.state == PlayerState.Lean)
        {
            Direction direction = Toolkit.ReverseDirection(player.leandirection);
            if (!Toolkit.IsInsideBranch(player) && Toolkit.IsEmpty(Toolkit.VectorSum(player.position, direction)))
            {
              
                player.isonejumping = true;
                player.oneJump.Action(player, direction);
            }
        }
        else if (player.state == PlayerState.Transition && player.isonejumping && player.abilities.Count != 0 && player.abilities[0].abilitytype == AbilityType.Jump)
        {
            player.isonejumping = false;
            Direction direction = Toolkit.ReverseDirection(Toolkit.ReverseDirection(player.jumpdirection));
            if (!Toolkit.IsInsideBranch(player) && Toolkit.IsEmpty(Toolkit.VectorSum(player.position, direction)))
            {
                GameObject.Find("GetInput").GetComponent<GetInput>().StopCoroutine(player.oneJump.coroutine);
                //player.currentAbility = player.abilities[0];
                ((Jump)player.abilities[0]).number = 1;
                ((Jump)player.abilities[0]).Action(player, direction);
            }
        }
    }

    private void IdlePLayerMove(Player player, Direction direction)
    {
        if (!player.lean)
        {
            if (player.Can_Move_Direction(direction))
            {
                if (player.Should_Change_Direction(direction))
                {
                    Direction olddir = player.direction;
                    player.direction = direction;
                    engine.apigraphic.PlayerChangeDirection(player, olddir, player.direction);
                }
                if (!player.Move(direction))
                {
                    Lean(player, direction);
                }
                else
                {
                    player.SetState(PlayerState.Moving);
                }
            }
            else
            {
                Lean(player, direction);
            }
        }
    }

    private void MovingPlayerMove(Player player, Direction direction)
    {
        if (player.direction == direction)
        {
            //Debug.Log("calling graphicals");

            if (player.movepercentage == 98)
            {
                if (!player.ApplyGravity()){
                    if (!player.Move(direction))
                    {
                        /*Debug.Log("here");
                        Lean(player, direction);*/
                    }
                    else
                    {
                        player.movepercentage = 99;
                        player.SetState(PlayerState.Moving);
                        /*engine.apiunit.RemoveFromDatabase(player);
                        player.position += Toolkit.DirectiontoVector(direction);
                        engine.apiunit.AddToDatabase(player);
                        Debug.Log("calling graphicals");
                        engine.apigraphic.Move_Update(player, player.position);*/
                    }
                }
            }
        }
        else if (direction == Toolkit.ReverseDirection(player.direction))
        {
            if (Toolkit.HasBranch(player.position) || Toolkit.HasBranch(Toolkit.VectorSum(player.position, direction)))
                return;
            player.movepercentage = 0;  
            Direction olddir = player.direction;
            player.direction = direction;
            engine.apigraphic.PlayerChangeDirection(player, olddir, player.direction);
            if (!player.Move(direction))
            {
                
            }
            else
            {
                player.SetState(PlayerState.Moving);
            }
        }
    }

    public void Absorb()
    {
        for (int i = 0; i < engine.database.player.Count; i++)
        {
            if (engine.database.player[i].lean) //for release
            {
                if (database.player[i].LeanedTo is Container)
                {
                    engine.database.player[i].Absorb((Container)database.player[i].LeanedTo);
                }
                else if (database.player[i].LeanedTo is Fountain)
                {
                    ((Fountain)database.player[i].LeanedTo).Action(database.player[i]);
                }
            }
        }
    }

    public void Release()
    {
        for (int i = 0; i < engine.database.player.Count; i++)
        {
            if (engine.database.player[i].lean) //for release
            {
                    if (database.player[i].LeanedTo is Container)
                    {
                        engine.database.player[i].Release((Container)database.player[i].LeanedTo);
                        break;
                    }
            }
        }
    }

    public void AbsorbHold()
    {
        for (int i = 0; i < engine.database.player.Count; i++)
        {
            if (engine.database.player[i].lean) //for release
            {
                List<Unit> units = engine.GetUnits(Toolkit.VectorSum(engine.database.player[i].position, Toolkit.DirectiontoVector(engine.database.player[i].leandirection)));
                for (int j = 0; j < units.Count; j++)
                {
                    if (units[j] is Container)
                    {
                        engine.database.player[i].AbsorbHold((Container)units[j]);
                        break;
                    }
                }
            }
        }
    }

    public void ReleaseHold()
    {
        for (int i = 0; i < engine.database.player.Count; i++)
        {
            if (engine.database.player[i].lean) //for release
            {
                List<Unit> units = engine.GetUnits(Toolkit.VectorSum(engine.database.player[i].position, Toolkit.DirectiontoVector(engine.database.player[i].leandirection)));
                for (int j = 0; j < units.Count; j++)
                {
                    if (units[j] is Container)
                    {
                        engine.database.player[i].ReleaseHold((Container)units[j]);
                        break;
                    }
                }
            }
        }
    }

    public void ArrowkeyReleased(Direction direction)
    {
        for (int i = 0; i < database.player.Count; i++)
        {
            if(database.player[i].state == PlayerState.Lean && engine.apiinput.isFunctionKeyDown())
            {
                FunctionalContainer container = Toolkit.GetContainer(Toolkit.VectorSum(database.player[i].position, direction)) as FunctionalContainer;
                if (container != null && container.abilities.Count != 0 && container.abilities[0] is Jump)
                {
                    container.Action(database.player[i], Toolkit.ReverseDirection(direction));
                }
            }
            else if (FakeLeanUndo(database.player[i], direction))
            {
                database.player[i].ApplyGravity();
            }
            else
            {
                LeanUndo(database.player[i], direction, PlayerState.LeanTransition);
            }
        }
        //Applygravity();
    }

    public bool LeanUndo(Player player, Direction direction, PlayerState nextstate)
    {
        if (player.lean && player.leandirection == direction)
        {
            Starter.GetDataBase().StopTimer();
            player.SetState(nextstate);
            player.lean = false;
            engine.apigraphic.LeanFinished(player);
            if (engine.leanmove.Contains(player) && !engine.shouldmove.Contains(player))
            {
                engine.apiunit.AddToDatabase(player);
                engine.apigraphic.LeanStickStop(player);
            }
            if(nextstate == PlayerState.Idle)
                player.ApplyGravity();
            else if(nextstate == PlayerState.LeanTransition)
            {
                if (Toolkit.IsEmpty(Toolkit.VectorSum(player.position, database.gravity_direction)))
                    player.leancoroutine = GameObject.Find("GetInput").GetComponent<GetInput>().StartCoroutine(LeanWait(0.5f, player));
                else
                {
                    player.ApplyGravity();
                }
            }
            return true;
        }
        return false;
    }

    public void Lean(Player player, Direction direction)
    {
        if (!player.lean)
        {
            Vector2 pos = Toolkit.VectorSum(player.position, direction);
            if (player.Can_Lean(pos))
            {
                if(player.leancoroutine != null)
                {
                    Debug.Log("herererere");
                    GameObject.Find("GetInput").GetComponent<GetInput>().StopCoroutine(player.leancoroutine);
                }
                if (Toolkit.HasBranch(pos))
                {
                    Toolkit.GetBranch(pos).PlayerLeaned(player, direction);
                    return;
                }
                player.movepercentage = 0;
                player.LeanedTo = Toolkit.GetUnit(pos);
                player.api.RemoveFromDatabase(player);
                player.position = Toolkit.VectorSum(pos, Toolkit.ReverseDirection(direction));
                player.api.AddToDatabase(player);
                player.SetState(PlayerState.Lean);
                player.transform.position = player.position;
                player.isonejumping = false;
                engine.apigraphic.Player_Co_Stop(player);
                player.lean = true;
                player.leandirection = direction;
                player.currentAbility = null;
                engine.apigraphic.Lean(player);
            }
            else
            {
                FakeLean(player, direction);
            }
        }
    }

    public void FakeLean(Player player, Direction direction)
    {
        if (player.state == PlayerState.Idle || player.state == PlayerState.Fakelean)
        {
            if (!Toolkit.HasBranch(player.position))
            {
                player.SetState(PlayerState.Fakelean);
                player.leandirection = direction;
                engine.apigraphic.Fake_Lean(player, direction);
            }
        }
    }

    public bool FakeLeanUndo(Player player, Direction direction)
    {
        if (player.state == PlayerState.Fakelean && player.leandirection == direction)
        {
            player.SetState(PlayerState.Idle);
            engine.apigraphic.Fake_Lean_Undo(player);
            return true;
        }
        return false;
    }

    public void RealModePlayerTransitionMove(Player player, Direction direction)
    {
        player.SetState(PlayerState.Busy);
        player.GetComponent<UnityPhysics>().Move(Toolkit.VectorSum(player.transform.position, direction));
    }

    public void RealModePlayerTransitionMoveDone(Player player)
    {
        if (player.mode == GameMode.Real)
        {
            GameObject.Find("GetInput").GetComponent<GetInput>().StopCoroutine(((Jump)player.currentAbility).coroutine);
            player.SetState(PlayerState.Idle);
            player.GetComponent<Rigidbody2D>().isKinematic = false;
        }
    }

    private IEnumerator LeanWait(float f, Player player)
    {
        Debug.Log("wait");
        yield return new WaitForSeconds(f);
        Debug.Log("appling gravity");
        player.ApplyGravity();

    }
}

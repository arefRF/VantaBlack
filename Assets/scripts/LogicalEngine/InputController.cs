using UnityEngine;
using System.Collections.Generic;

public class InputController {

    LogicalEngine engine;
    Database database;
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
            JumpingPlayerMove(player, direction);
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
                    player.state = PlayerState.Moving;
                }
                else
                {
                    Unit nearest;
                    bool tempbool = false;
                    if(Toolkit.GetDeltaPositionAndTransformPosition(player, direction) > 0.9)
                    {
                        nearest = Toolkit.GetNearestUnitForJumpingPlayer(player, Toolkit.VectorSum(player.position, direction), direction);
                        tempbool = true;
                    }
                    else
                        nearest = Toolkit.GetNearestUnitForJumpingPlayer(player, player.position, direction);
                    if (nearest != null && player.Can_Lean(nearest.position))
                    {
                        engine.apiunit.RemoveFromDatabase(player);
                        if(tempbool)
                            player.position = Toolkit.VectorSum(player.position, direction);
                        engine.apiunit.AddToDatabase(player);
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
                Vector2 pos = player.position;
                if (Toolkit.GetDeltaPositionAndTransformPosition(player, player.GetGravity()) > 0.9)
                {
                    pos = Toolkit.VectorSum(pos, direction);
                }
                if (player.Can_Lean(Toolkit.VectorSum(pos, direction)))
                {
                    engine.apiunit.RemoveFromDatabase(player);
                    player.position = pos;
                    engine.apiunit.AddToDatabase(player);
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
                else if (Toolkit.HasBranch(Toolkit.VectorSum(pos, direction)))
                {
                    player.state = PlayerState.Falling;
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
                Direction direction = Toolkit.ReverseDirection(player.GetGravity());
                if(!Toolkit.IsInsideBranch(player) && (Toolkit.IsEmpty(Toolkit.VectorSum(player.position, direction)) || Toolkit.HasBranch(Toolkit.VectorSum(player.position, direction))))
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
        else if (player.state == PlayerState.Jumping && player.isonejumping && player.abilities.Count != 0 && player.abilities[0].abilitytype == AbilityType.Jump)
        {
            player.isonejumping = false;
            Direction direction = Toolkit.ReverseDirection(player.GetGravity());
            if (!Toolkit.IsInsideBranch(player) && Toolkit.IsEmpty(Toolkit.VectorSum(player.position, direction)))
            {
                GameObject.Find("GetInput").GetComponent<GetInput>().StopCoroutine(player.oneJump.coroutine);
                player.currentAbility = player.abilities[0];
                ((Jump)player.currentAbility).number = 1;
                ((Jump)player.currentAbility).Action(player, direction);
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
                else if (!player.Move(direction))
                {
                    Lean(player, direction);
                }
                else
                {
                    player.state = PlayerState.Moving;
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
                        player.state = PlayerState.Moving;
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
                player.state = PlayerState.Moving;
            }
        }
    }

    public void Absorb()
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
                        engine.database.player[i].Absorb((Container)units[j]);
                        break;
                    }
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
                List<Unit> units = engine.GetUnits(Toolkit.VectorSum(engine.database.player[i].position, Toolkit.DirectiontoVector(engine.database.player[i].leandirection)));
                for (int j = 0; j < units.Count; j++)
                {
                    if (units[j] is Container)
                    {
                        engine.database.player[i].Release((Container)units[j]);
                        break;
                    }
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
            if (LeanUndo(database.player[i], direction, PlayerState.Idle) || FakeLeanUndo(database.player[i], direction))
            {
                database.player[i].ApplyGravity();
            }
        }
        //Applygravity();
    }

    public bool LeanUndo(Player player, Direction direction, PlayerState nextstate)
    {
        if (player.lean && player.leandirection == direction)
        {
            player.state = nextstate;
            player.lean = false;
            engine.apigraphic.LeanFinished(player);
            if (engine.leanmove.Contains(player) && !engine.shouldmove.Contains(player))
            {
                engine.apiunit.AddToDatabase(player);
                engine.apigraphic.LeanStickStop(player);
            }
            if(nextstate == PlayerState.Idle)
                player.ApplyGravity();
            return true;
        }
        return false;
    }

    public void Lean(Player player, Direction direction)
    {
        if (!player.lean)
        {
            Vector2 pos = Toolkit.VectorSum(player.position, direction);
            if (player.state == PlayerState.Jumping)
            {
                if (direction == Toolkit.ReverseDirection(player.jumpdirection))
                    return;
                Unit nearest = Toolkit.GetNearestUnitForJumpingPlayer(player,player.position, direction);
                if (nearest == null)
                    return;
                pos = nearest.position;
            }
            Debug.Log("here hereh");
            if (player.Can_Lean(pos))
            {
                if (Toolkit.HasBranch(pos))
                {
                    Toolkit.GetBranch(pos).PlayerLeaned(player);
                }
                player.api.RemoveFromDatabase(player);
                player.position = Toolkit.VectorSum(pos, Toolkit.ReverseDirection(direction));
                player.api.AddToDatabase(player);
                player.state = PlayerState.Lean;
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
                player.state = PlayerState.Fakelean;
                player.leandirection = direction;
                engine.apigraphic.Fake_Lean(player, direction);
            }
        }
    }

    public bool FakeLeanUndo(Player player, Direction direction)
    {
        if (player.state == PlayerState.Fakelean && player.leandirection == direction)
        {
            player.state = PlayerState.Idle;
            engine.apigraphic.Fake_Lean_Undo(player);
            return true;
        }
        return false;
    }
}

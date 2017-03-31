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
            JumpingPlayerMove(player, direction);
        }

    }

    private void JumpingPlayerMove(Player player, Direction direction)
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
                else if (player.JumpingMove(direction))
                    player.state = PlayerState.Moving;

                else if (player.Can_Lean(direction))
                {
                    player.UseAbility(player.abilities[0]);
                    Lean(player, direction);

                }
            }
            else if (player.Can_Lean(direction))
            {
                player.UseAbility(player.abilities[0]);
                Lean(player, direction);
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
                player.UseAbility(player.abilities[0]);
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
                if (!player.ApplyGravity(engine.database.gravity_direction, engine.database.units)){
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
            if(LeanUndo(database.player[i], direction) || FakeLeanUndo(database.player[i], direction))
                database.player[i].ApplyGravity(database.gravity_direction, database.units);
        }
        //Applygravity();
    }

    public bool LeanUndo(Player player, Direction direction)
    {
        if (player.lean)
        {
            player.lean = false;
            engine.apigraphic.LeanFinished(player);
            if (engine.leanmove.Contains(player) && !engine.shouldmove.Contains(player))
            {
                engine.apiunit.AddToDatabase(player);
                engine.apigraphic.LeanStickStop(player);
            }
            return true;
        }
        return false;
    }

    public void Lean(Player player, Direction direction)
    {
        if (!player.lean)
        {
            if (player.state == PlayerState.Jumping && direction == Toolkit.ReverseDirection(player.jumpdirection))
                return;
            if (player.Can_Lean(direction))
            {
                engine.apigraphic.Player_Co_Stop(player);
                player.lean = true;
                player.leandirection = direction;
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
        if (player.state == PlayerState.Fakelean)
        {
            player.state = PlayerState.Idle;
            engine.apigraphic.Fake_Lean_Undo(player);
            return true;
        }
        return false;
    }
}

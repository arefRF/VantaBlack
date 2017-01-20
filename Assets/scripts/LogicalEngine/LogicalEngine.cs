﻿using UnityEngine;
using System.Collections.Generic;

public class LogicalEngine {

    public APIGraphic apigraphic;
    public APIInput apiinput;
    public APIUnit apiunit;
    public Database database;
    SubEngine_Initializer initializer;
    int sizeX, sizeY;
    public LogicalEngine(int x, int y)
    {
        sizeX = x;
        sizeY = y;
        apigraphic = new APIGraphic(this);
        apiinput = new APIInput(this);
        apiunit = new APIUnit(this);
        database = Starter.GetDataBase();
        initializer = new SubEngine_Initializer(x,y, apiunit);
    }

    public void Run()
    {
        database.units = initializer.init();
        database.state = State.Idle;
    }

    public bool MoveUnit(Unit unit, Direction dir)
    {
        if(!(unit is Box))
        {
            for(int i=0; i<unit.ConnectedUnits.Count; i++)
                if (!unit.ConnectedUnits[i].CanMove(dir))
                    return false;
            for (int i = 0; i < unit.ConnectedUnits.Count; i++)
            {
                Unit u = unit.ConnectedUnits[i];
                Vector2 newpos = Toolkit.VectorSum(u.position, Toolkit.DirectiontoVector(dir));
                database.units[(int)u.position.x, (int)u.position.y].Remove(u);

            }
        }
        else
        {
            
        }
        return false;
    }

    public void MovePlayer(Player player, Direction dir)
    {
        Vector2 nextpos;
        if (player.onramp)
        {
            List<Unit> units = GetUnits(Toolkit.VectorSum(player.position, Toolkit.DirectiontoVector(database.gravity_direction)));
            Ramp ramp;
            bool goingup = true;
            if (units[0] is Ramp)
                ramp = (Ramp)units[0];
            else
                ramp = (Ramp)units[1];
            switch (database.gravity_direction)
            {
                case Direction.Down:
                    switch (dir)
                    {
                        case Direction.Right: if (ramp.type == 1) goingup = false; break;
                        case Direction.Left: if (ramp.type == 4) goingup = false; break;
                    }
                    break;
                case Direction.Right:
                    switch (dir)
                    {
                        case Direction.Up: if (ramp.type == 4) goingup = false; break;
                        case Direction.Down: if (ramp.type == 3) goingup = false; break;
                    }
                    break;
                case Direction.Up:
                    switch (dir)
                    {
                        case Direction.Right: if (ramp.type == 2) goingup = false; break;
                        case Direction.Left: if (ramp.type == 3) goingup = false; break;
                    }
                    break;
                case Direction.Left:
                    switch (dir)
                    {
                        case Direction.Up: if (ramp.type == 1) goingup = false; break;
                        case Direction.Down: if (ramp.type == 2) goingup = false; break;
                    }
                    break;
            }
            if (goingup)
            {
                nextpos = Toolkit.VectorSum(player.position, Toolkit.VectorSum(Toolkit.DirectiontoVector(Toolkit.ReverseDirection(database.gravity_direction)), Toolkit.DirectiontoVector(dir)));
                units = GetUnits(nextpos);
                if (units.Count == 0)
                {
                    units = GetUnits(Toolkit.VectorSum(nextpos, Toolkit.DirectiontoVector(database.gravity_direction)));
                    if (units.Count == 0)
                    {
                        apigraphic.MovePlayer_Ramp_3(player, nextpos);
                    }
                    else
                    {
                        if (units[0] is Ramp)
                        {
                            apigraphic.MovePlayer_Ramp_5(player, Toolkit.VectorSum(nextpos, Toolkit.DirectiontoVector(database.gravity_direction)));
                        }
                        else
                        {
                            apigraphic.MovePlayer_Ramp_2(player, nextpos);
                        }
                    }
                }
                else
                {
                    apigraphic.MovePlayer_Ramp_1(player, nextpos);
                }
            }
            else
            {
                nextpos = Toolkit.VectorSum(player.position, Toolkit.DirectiontoVector(dir));
                units = GetUnits(nextpos);
                if(units.Count != 0)
                {
                    apigraphic.MovePlayer_Ramp_4(player, nextpos);
                }
                else
                {
                    units = GetUnits(Toolkit.VectorSum(nextpos, Toolkit.DirectiontoVector(database.gravity_direction)));
                    if (units[0] is Ramp)
                        apigraphic.MovePlayer_Ramp_1(player, Toolkit.VectorSum(nextpos, Toolkit.DirectiontoVector(database.gravity_direction)));
                    else
                        apigraphic.MovePlayer_Ramp_2(player, nextpos);
                }
            }
        }
        else
        {
            if (Toolkit.IsInsideBranch(player))
            {
                nextpos = Toolkit.VectorSum(player.position, Toolkit.DirectiontoVector(dir));
                Vector2 temp = Toolkit.VectorSum(nextpos, Toolkit.DirectiontoVector(database.gravity_direction));
                List<Unit> units = GetUnits(temp);
                if(units.Count != 0)
                {
                    if(units[0] is Ramp)
                    {
                        apigraphic.MovePlayer_Branch_3(player, temp, ((Ramp)units[0]).type);
                    }
                    else
                    {
                        apigraphic.MovePlayer_Branch_1(player, nextpos);
                    }
                }
                else
                {
                    apigraphic.MovePlayer_Branch_2(player, nextpos);
                }
            }
            else
            {
                nextpos = nextpos = Toolkit.VectorSum(player.position, Toolkit.DirectiontoVector(dir));
                Vector2 temp = Toolkit.VectorSum(nextpos, Toolkit.DirectiontoVector(database.gravity_direction));
                List<Unit> units = GetUnits(nextpos);
                if (units.Count != 0)
                {
                    if(units[0] is Branch)
                    {
                        apigraphic.MovePlayer_Simple_2(player, nextpos);
                    }
                    else if(units[0] is Ramp)
                    {
                        apigraphic.MovePlayer_Simple_3(player, nextpos, ((Ramp)units[0]).type);
                    }
                }
                else
                {
                    units = GetUnits(temp);
                    if(units.Count != 0)
                    {
                        if(units[0] is Ramp)
                        {
                            apigraphic.MovePlayer_Simple_5(player, temp, ((Ramp)units[0]).type);
                        }
                        else
                        {
                            apigraphic.MovePlayer_Simple_1(player, nextpos);
                        }
                    }
                    else
                    {
                        apigraphic.MovePlayer_Simple_4(player, nextpos);
                    }
                }
            }
        }
    }
    private void Applygravity()
    {
        for(int i=0; i<database.player.Count; i++)
        {
            ApplyGravity_Player(database.player[i]);
        }
        //apiinput.PlayerMoveFinished();
    }
    public void ApplyGravity_Player(Player player)
    {
        List<Unit> units;
        Vector2 nextpos;
        units = GetUnits(player.position);
        if(units.Count == 2)
        {
            Ramp ramp = null;
            if (units[0] is Ramp)
                ramp = (Ramp)units[0];
            else if (units[1] is Ramp)
                ramp = (Ramp)units[1];
            if (ramp != null)
            {
                switch (database.gravity_direction)
                {
                    case Direction.Down: if (ramp.type == 1 || ramp.type == 4) return; break;
                    case Direction.Left: if (ramp.type == 1 || ramp.type == 2) return; break;
                    case Direction.Right: if (ramp.type == 3 || ramp.type == 4) return; break;
                    case Direction.Up: if (ramp.type == 2 || ramp.type == 3) return; break;
                }
            }
        }
        nextpos = Toolkit.VectorSum(player.position, Toolkit.DirectiontoVector(database.gravity_direction));
        units = GetUnits(nextpos);
        if (units.Count != 0)
        {
            if (units.Count == 1)
            {
                units[0].fallOn(player, Toolkit.ReverseDirection(database.gravity_direction));
                
            }
            else
            {
                Unit fallonunit = Toolkit.GetUnitToFallOn(units, Toolkit.ReverseDirection(database.gravity_direction));
                fallonunit.fallOn(player, Toolkit.ReverseDirection(database.gravity_direction));
                
            }
        }
        else
        {
            database.units[(int)player.position.x, (int)player.position.y].Remove(player);
            database.units[(int)nextpos.x, (int)nextpos.y].Add(player);
            player.position = nextpos;
            apigraphic.Fall(player, nextpos);
            
        }
    }

    public void graphic_FallFinished(Player player)
    {
        bool isonunit = true;
        if (GetUnits(Toolkit.VectorSum(Toolkit.DirectiontoVector(database.gravity_direction), player.position)).Count == 0)
            isonunit = false;
        if (isonunit)
        {
            player.state = PlayerState.Steady;
        }
        else
            ApplyGravity_Player(player);
    }

    public void graphic_LandFinished(Player player)
    {
        player.state = PlayerState.Steady;
    }

    public void Lean(Player player, Direction direction)
    {
        if (!player.lean)
        {
            player.lean = true;
            player.leandirection = direction;
            apigraphic.Lean(player);
        }
    }

    public List<Unit> GetUnits(Vector2 position)
    {
        return database.units[(int)position.x, (int)position.y];
    }

    public void Input_Move(Direction direction)
    {
        for(int i=0; i<database.player.Count; i++)
        {
            if (!database.player[i].lean)
            {
                for(int j=0; j<database.player[i].move_direction.Count; j++)
                {
                    if(database.player[i].move_direction[j] == direction)
                    {
                        if(direction == database.player[i].direction)
                        {
                            if (!database.player[i].Move(direction))
                            {
                                Lean(database.player[i], direction);
                                apiinput.PlayerMoveFinished();
                            }
                        }
                        else
                        {
                            Direction olddir = database.player[i].direction;
                            database.player[i].direction = direction;
                            apiinput.PlayerMoveStarted();
                            apigraphic.PlayerChangeDirection(database.player[i], olddir, database.player[i].direction);
                        }
                    }
                }
            }
        }
    }

    public void Input_AbsorbRlease(Direction dir)
    {
        for(int i=0; i<database.player.Count; i++)
        {
            if (database.player[i].lean)
            {
                if (database.player[i].leandirection == dir)
                {
                    List<Unit> units = GetUnits(Toolkit.VectorSum(database.player[i].position, Toolkit.DirectiontoVector(dir)));
                    for (int j = 0; j < units.Count; j++)
                    {
                        if (units[j] is Container)
                        {
                            database.player[i].Release((Container)units[j]);
                            break;
                        }
                    }
                }
                else
                {
                    List<Unit> units = GetUnits(Toolkit.VectorSum(database.player[i].position, Toolkit.DirectiontoVector(Toolkit.ReverseDirection(dir))));
                    for (int j = 0; j < units.Count; j++)
                    {
                        if (units[j] is Container)
                        {
                            database.player[i].Absorb((Container)units[j]);
                            break;
                        }
                    }
                }
            }
        }
    }

    public void ArrowkeyReleased(Direction direction)
    {
        for(int i=0; i<database.player.Count; i++)
        {
            if (database.player[i].lean)
            {
                database.player[i].lean = false;
                apigraphic.LeanFinished(database.player[i]);
            }
        }
    }

    public void ActionKeyPressed()
    {
        for(int i=0; i<database.player.Count; i++)
        {
            if (database.player[i].lean)
            {
                Vector2 newpos = Toolkit.VectorSum(database.player[i].position, Toolkit.DirectiontoVector(database.player[i].leandirection));
                List<Unit> units = GetUnits(newpos);
                for(int j=0; j<units.Count; j++)
                {
                    if (units[i] is ParentContainer)
                        ((ParentContainer)units[i]).Action(database.player[i], Toolkit.ReverseDirection(database.player[i].leandirection));
                }
            }
            else
            {
                database.player[i].Action();
            }
        }
    }

    public void ActionKeyPressed(Direction dir)
    {
        for(int i=0; i<database.player.Count; i++)
        {
            if (database.player[i].lean)
                continue;
            database.player[i].Action(dir);
        }
    }

    public void graphic_MoveAnimationFinished(Player player)
    {
        Debug.Log("move animation finished");
        Applygravity();
        apiinput.PlayerMoveFinished();
    }

    public void graphic_AbsorbReleaseFinished(Player player)
    {

    }

    public void graphic_PlayerChangeDirectionFinished(Player player)
    {
        apiinput.PlayerMoveFinished();
    }

    public void UnitToGraphic_Land(Unit unit, Unit landingunit,Vector2 landingposition)
    {
        apigraphic.Land((Player)unit, landingposition, landingunit);
    }

    public void UnitToGraphic_LandOnRamp(Unit unit, Ramp landingunit, Vector2 landingposition, int landingtype)
    {
        apigraphic.LandOnRamp((Player)unit, landingposition, landingunit, landingtype);
    }

    public void UnitToGraphic_Absorb(Player player, Container container, AbilityType ability)
    {

    }

    public void UnitToGraphic_Release(Player player, Container container, AbilityType ability)
    {

    }
    public void UnitToGraphic_Swap(Player player, Container container, AbilityType ability)
    {

    }
}

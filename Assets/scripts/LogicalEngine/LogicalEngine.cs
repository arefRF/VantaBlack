using UnityEngine;
using System.Collections.Generic;

public class LogicalEngine {

    public APIGraphic apigraphic;
    public APIInput apiinput;
    public APIUnit apiunit;
    Database database;
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
        Debug.Log(database.units[8,1].Count);
    }

    public void MoveUnit(Unit unit, Vector2 position)
    {
        if(unit is Box)
        {
            
        }
        else
        {
            //apigraphic.
        }
    }

    public void MovePlayer(Player player, Direction dir)
    {
        bool simplemove = true;
        apiinput.PlayerMoveStarted();
        List<Unit> units = GetUnits(player.position);
        bool onramp = false;
        Vector2 newpos = Toolkit.VectorSum(Toolkit.DirectiontoVector(dir), player.position);
        for (int i=0; i<units.Count; i++)
        {
            if(units[i] is Ramp)
            {
                onramp = true;
            }
        }
        if (onramp)
            units = GetUnits(Toolkit.VectorSum(Toolkit.VectorSum(player.position, Toolkit.DirectiontoVector(dir)), Toolkit.DirectiontoVector(database.gravity_direction)));
        else
            units = GetUnits(Toolkit.VectorSum(player.position, Toolkit.DirectiontoVector(dir)));
        for(int i=0; i<units.Count; i++)
        {
            if(units[i] is Ramp)
            {
                if (onramp) 
                    newpos = Toolkit.VectorSum(Toolkit.VectorSum(player.position, Toolkit.DirectiontoVector(dir)), Toolkit.DirectiontoVector(database.gravity_direction));
                else
                    newpos = units[i].position;
                simplemove = false;
                apigraphic.MovePlayerOnRamp(player, newpos, onramp);
                break;
            }
            if(units[i] is Branch)
            {
                simplemove = false;
                apigraphic.MovePlayerToBranch(player, newpos, onramp);
                apiinput.PlayerMoveFinished();
                break;
            }
        }
        if(simplemove)
            apigraphic.MovePlayer(player, newpos, onramp);
        database.units[(int)player.position.x, (int)player.position.y].Remove(player);
        database.units[(int)newpos.x, (int)newpos.y].Add(player);
        player.position = newpos;
    }

    private void Applygravity()
    {
        bool flag = true;
        for(int i=0; i<database.player.Count; i++)
        {
            

        }
        apiinput.PlayerMoveFinished();
    }
    public void ApplyGravity_Player(Player player)
    {
        List<Unit> units;
        Vector2 nextpos;
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
                return;
            }
        }
        database.units[(int)player.position.x, (int)player.position.y].Remove(database.player[i]);
        database.units[(int)nextpos.x, (int)nextpos.y].Add(player);
        apigraphic.Fall(player, nextpos);
    }

    public void FallFinished(Player player, bool isonunit)
    {
        if (isonunit)
        {
            player.state = PlayerState.Steady;
        }
        else
            ApplyGravity_Player(player);
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
                if (!database.player[i].Move(direction))
                {
                    Lean(database.player[i], direction);
                    apiinput.PlayerMoveFinished();
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

    public void graphic_MoveAnimationFinished(Player player)
    {
        Applygravity();
    }
}

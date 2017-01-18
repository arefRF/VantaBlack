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
                apigraphic.MovePlayerToBranch(player, newpos, onramp);
                break;
            }
            if(units[i] is Branch)
            {
                apigraphic.MovePlayerToBranch(player, newpos, onramp);
                apiinput.PlayerMoveFinished();
                break;
            }
        }
        database.units[(int)player.position.x, (int)player.position.y].Remove(player);
        database.units[(int)newpos.x, (int)newpos.y].Add(player);
        player.position = newpos;
        Applygravity();
        apiinput.PlayerMoveFinished();
    }

    private void Applygravity()
    {

    }

    public void Lean(Player player, Direction direction)
    {
        player.lean = true;
        player.leandirection = direction;
    }

    public List<Unit> GetUnits(Vector2 position)
    {
        Debug.Log(position);
        return database.units[(int)position.x, (int)position.y];
    }

    public void Input_Move(Direction direction)
    {
        for(int i=0; i<database.player.Count; i++)
        {
            database.player[i].Move(direction);
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
}

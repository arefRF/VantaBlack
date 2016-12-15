using UnityEngine;
using System.Collections.Generic;

public class Action{
    Player player;
    Database database;
    GraphicalEngine Gengine;
    Move move;
    LogicalEngine engine;
    public Action(LogicalEngine engine)
    {
        this.player = engine.player;
        this.database = engine.database;
        this.Gengine = engine.Gengine;
        this.move = engine.moveObject;
        this.engine = engine;
    }

    public void ChangeGravity(Direction direction)
    {
        for (int i = 0; i < player.ability.direction.Count; i++)
        {
            if (direction == player.ability.direction[i])
            {
                Direction temp = database.gravity_direction;
                database.gravity_direction = direction;
                player.ability.direction[i] = temp;
            }
        }
    }
    public void Blink(Direction dir)
    {
        if (CheckBlink(dir))
        {
            Gengine._blink(dir);
            engine.NextTurn();
        }
    }
    public void Teleport(Vector2 position)
    {
        database.player.transform.position = position;
        engine.NextTurn();
    }
    public void Rope()
    {
        database.timeLaps.Add(new TimeLaps(player.ability.number, database.player));
    }
    public void ChangeDirection()
    {
        engine.AddToSnapshot(player);
        switch (player.move_direction[0])
        {
            case Direction.Down: player.move_direction[0] = Direction.Up; break;
            case Direction.Up: player.move_direction[0] = Direction.Down; break;
            case Direction.Right: player.move_direction[0] = Direction.Left; break;
            case Direction.Left: player.move_direction[0] = Direction.Right; break;
        }
        engine.NextTurn();
    }
    private bool CheckBlink(Direction direction)
    {
        bool value = false;
        foreach (Direction d in player.move_direction)
            if (direction == d)
                value = true;
        if (!value)
            return false;

        switch (direction)
        {
            case Direction.Up: return isvoid1(0, 2);
            case Direction.Down: return isvoid1(0, -2);
            case Direction.Right: return isvoid1(2, 0);
            case Direction.Left: return isvoid1(-2, 0);
            default: return false;
        }

    }
    private bool isvoid1(int x, int y)
    {
        foreach (Unit u in database.units[(int)player.position.x + x, (int)player.position.y + y])
        {
            if (u.unitType == UnitType.Block || u.unitType == UnitType.Container)
                return false;
        }
        return true;
    }

    public void RunContainer(Container container)
    {
        container.Run();
    }

    public void RunBlockSwitch(BlockSwitch blockswitch)
    {

    }
    public void CheckAutomaticSwitch(Vector2 position)
    {
        //Wall.print(position);
        int isempty = 0;
        for (int i = 0; i < database.units[(int)position.x, (int)position.y].Count; i++)
        {
            Unit u = database.units[(int)position.x, (int)position.y][i];
            //Wall.print(u);
            if (u.unitType == UnitType.Block || u.unitType == UnitType.BlockSwitch || u.unitType == UnitType.Container || u.unitType == UnitType.Rock)
                isempty = 1;
            else if(u.unitType == UnitType.Player || u.unitType == UnitType.Box)
            {
                isempty = 2;
            }
        }
        for (int i=0; i<database.units[(int)position.x, (int)position.y].Count; i++)
        {
            
            Unit u = database.units[(int)position.x, (int)position.y][i];
            if (u.unitType == UnitType.Switch && ((Switch)u).isAutomatic)
            {
                Wall.print(i);
                if(isempty == 1)
                {
                    return;
                    if(!((Switch)u).isOn)
                        ((Switch)u).Run();
                }
                else if(isempty == 2)
                {
                    if (database.gravity_direction == ((Switch)u).direction)
                    {
                        //engine.AddToSnapshot(u);
                        ((Switch)u).Run();
                    }
                }
                else
                {
                    if (((Switch)u).isOn)
                    {
                        //engine.AddToSnapshot(u);
                        ((Switch)u).Run();
                    }
                }
            }
        }
    }
}

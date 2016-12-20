using UnityEngine;
using System.Collections.Generic;
using System.Threading;

public class Move{
    /// <summary>
    /// dec
    /// </summary>

    public GraphicalEngine Gengine;
    public Player player;
    public Database database;
    LogicalEngine engine;

    
       
    public Move(LogicalEngine engine)
    {
        this.Gengine = engine.Gengine;
        this.player = engine.player;
        this.database = engine.database;
        this.engine = engine;
    }

    //0 normal, 1 fall
    public void MoveUnit(Unit unit, Vector2 sinkPos, int condition)
    {
        if(unit.unitType == UnitType.Player)
        {
            Toolkit.RemoveUnit(unit);
        }
        else
        {

        }
    }
    public bool move(Direction dir)
    {
        for (int i = 0; i < database.units[(int)player.transform.position.x, (int)player.transform.position.y].Count; i++)
        {
            Unit u = database.units[(int)player.transform.position.x, (int)player.transform.position.y][i];
            if (u.CanBeMoved)
            {
                if (!MoveObjects(u, dir))
                    break;
                return false;
            }
            else
                return false;
        }
        engine.AddToSnapshot(player);
        Toolkit.RemoveUnit(player);
        player.position = Toolkit.VectorSum(player.position, Toolkit.DirectiontoVector(dir));
        Gengine._Move_Object(player.obj, Toolkit.VectorSum(player.transform.position, Toolkit.DirectiontoVector(dir)));
        player.x = (int)player.position.x; player.y = (int)player.position.y;
        database.units[(int)player.transform.position.x, (int)player.transform.position.y].Add(player);
        return true;
    }

    private bool MoveObjects(Unit unit, Direction d)
    {
        Vector2 temp;
        bool flag = true;
        if (unit.unitType != UnitType.Wall)
        {
            if (Toolkit.IsWallOnTheWay(unit.obj.transform.position, d))
                return false;
            Door door = Toolkit.IsDoorOnTheWay(unit.obj.transform.position, d);
            if (door != null)
            {
                if (!door.isOpen)
                    return false;
            }
        }
        /*for(int i=0; i<database.units[5,9].Count; i++)
        {
            Wall.print(database.units[5,9][i]);
        }
        Wall.print("-----------------------------------------");*/
        if (unit.unitType == UnitType.Wall)
        {
            switch (((Wall)unit).direction)
            {
                case Direction.Left: if (d == Direction.Down || d == Direction.Up) flag = false; break;
                case Direction.Right: if (d == Direction.Down || d == Direction.Up) flag = false; break;
                case Direction.Up: if (d == Direction.Left || d == Direction.Right) flag = false; break;
                case Direction.Down: if (d == Direction.Right || d == Direction.Left) flag = false; break;
            }
        }
        switch (d)
        {
            case Direction.Down: temp = Toolkit.VectorSum(unit.transform.position, new Vector2(0, -1)); break;
            case Direction.Up: temp = Toolkit.VectorSum(unit.transform.position, new Vector2(0, 1)); break;
            case Direction.Left: temp = Toolkit.VectorSum(unit.transform.position, new Vector2(-1, 0)); break;
            case Direction.Right: temp = Toolkit.VectorSum(unit.transform.position, new Vector2(1, 0)); break;
            default: temp = new Vector2(0, 0); break;
        }

        if (flag)
        {
            for (int i = 0; i < database.units[(int)temp.x, (int)temp.y].Count; i++)
            {
                Unit u = database.units[(int)temp.x, (int)temp.y][i];
                if (u.layer == 2)
                    continue;
                if (u.unitType == UnitType.Wall)
                    continue;
                if (u.unitType == UnitType.Switch)
                    continue;
                if (u.unitType == UnitType.Door)
                {
                    if (((Door)u).direction != d)
                        continue;
                }

                if (unit.CanMove(u.unitType) || u.CanBeMoved)
                {
                    if (!MoveObjects(u, d))
                        return false;
                    else
                    {
                        i = -1;
                        continue;
                    }
                }
                else
                {
                    return false;
                }

            }
        }
        engine.AddToSnapshot(unit);
        Vector2 tmpvec = Toolkit.VectorSum(unit.obj.transform.position, Toolkit.DirectiontoVector(Toolkit.ReverseDirection(database.gravity_direction)));
        for (int i = 0; i < database.units[(int)tmpvec.x, (int)tmpvec.y].Count; i++)
        {
            if (database.units[(int)tmpvec.x, (int)tmpvec.y][i].CanBeMoved && database.gravity_direction != d )
            {
                if (! Toolkit.IsWallOnTheWay(Toolkit.VectorSum(unit.obj.transform.position, Toolkit.DirectiontoVector(d)), Toolkit.ReverseDirection(database.gravity_direction)))
                    MoveObjects(database.units[(int)tmpvec.x, (int)tmpvec.y][i], d);
            }
        }

        database.units[(int)unit.transform.position.x, (int)unit.transform.position.y].Remove(unit);
        if (unit.unitType == UnitType.Player)
        {
            for (int i = 0; i < database.units[(int)unit.transform.position.x, (int)unit.transform.position.y].Count; i++)
            {
                Unit u = database.units[(int)unit.transform.position.x, (int)unit.transform.position.y][i];
                if (u.unitType == UnitType.Switch && ((Switch)u).isAutomatic && ((Switch)u).isOn)
                {
                    ((Switch)u).Run();
                }
            }
        }
        if (unit.unitType == UnitType.Wall)
        {
            switch (((Wall)unit).direction)
            {
                case Direction.Right:
                    engine.AddToSnapshot(unit.obj.GetComponents<Wall>()[1]);
                    database.units[(int)unit.transform.position.x + 1, (int)unit.transform.position.y].Remove(unit.obj.GetComponents<Wall>()[1]);
                    break;
                case Direction.Left:
                    engine.AddToSnapshot(unit.obj.GetComponents<Wall>()[0]);
                    database.units[(int)unit.transform.position.x - 1, (int)unit.transform.position.y].Remove(unit.obj.GetComponents<Wall>()[0]);
                    break;
                case Direction.Up:
                    engine.AddToSnapshot(unit.obj.GetComponents<Wall>()[1]);
                    database.units[(int)unit.transform.position.x, (int)unit.transform.position.y + 1].Remove(unit.obj.GetComponents<Wall>()[1]);
                    break;
                case Direction.Down:
                    engine.AddToSnapshot(unit.obj.GetComponents<Wall>()[0]);
                    database.units[(int)unit.transform.position.x, (int)unit.transform.position.y - 1].Remove(unit.obj.GetComponents<Wall>()[0]);
                    break;
            }
        }
        engine.Gengine._Move_Object(unit.obj, temp);
        database.units[(int)temp.x, (int)temp.y].Add(unit);
      
        if (unit.unitType == UnitType.Wall)
        {
            for (int i = 0; i < ((Wall)unit).connectedUnits.Count; i++)
            {
                database.units[(int)((Wall)unit).connectedUnits[i].obj.transform.position.x, (int)((Wall)unit).connectedUnits[i].obj.transform.position.y].Remove((Switch)((Wall)unit).connectedUnits[i]);
                Vector2 temppos = Toolkit.VectorSum((Toolkit.DirectiontoVector(Toolkit.ReverseDirection(((Switch)((Wall)unit).connectedUnits[i]).direction))), unit.gameObject.transform.position);
                database.units[(int)temppos.x, (int)temppos.y].Add((Switch)((Wall)unit).connectedUnits[i]);
                GraphicalEngine.MoveObject(((Wall)unit).connectedUnits[i].obj, temppos);
            }
            switch (((Wall)unit).direction)
            {
                case Direction.Right:
                    unit.obj.GetComponents<Wall>()[1].x = (int)unit.obj.transform.position.x + 1; unit.obj.GetComponents<Wall>()[1].y = (int)unit.obj.transform.position.y;
                    database.units[(int)temp.x + 1, (int)temp.y].Add(unit.obj.GetComponents<Wall>()[1]);
                    break;
                case Direction.Left:
                    unit.obj.GetComponents<Wall>()[1].x = (int)unit.obj.transform.position.x - 1; unit.obj.GetComponents<Wall>()[1].y = (int)unit.obj.transform.position.y;
                    database.units[(int)temp.x - 1, (int)temp.y].Add(unit.obj.GetComponents<Wall>()[0]);
                    break;
                case Direction.Up:
                    unit.obj.GetComponents<Wall>()[1].x = (int)unit.obj.transform.position.x; unit.obj.GetComponents<Wall>()[1].y = (int)unit.obj.transform.position.y + 1;
                    database.units[(int)temp.x, (int)temp.y + 1].Add(unit.obj.GetComponents<Wall>()[1]);
                    break;
                case Direction.Down:
                    unit.obj.GetComponents<Wall>()[1].x = (int)unit.obj.transform.position.x; unit.obj.GetComponents<Wall>()[1].y = (int)unit.obj.transform.position.y - 1;
                    database.units[(int)temp.x, (int)temp.y - 1].Add(unit.obj.GetComponents<Wall>()[0]);
                    break;
            }
        }
        else if (unit.unitType == UnitType.Rock)
        {
            for (int i = 0; i < ((Rock)unit).connectedUnits.Count; i++)
            {
                //engine.reserved.Add(unit);
                database.units[(int)((Rock)unit).connectedUnits[i].obj.transform.position.x, (int)((Rock)unit).connectedUnits[i].obj.transform.position.y].Remove(((Rock)unit).connectedUnits[i]);
                Vector2 temppos = Toolkit.VectorSum((Toolkit.DirectiontoVector(Toolkit.ReverseDirection(((Switch)((Rock)unit).connectedUnits[i]).direction))), unit.gameObject.transform.position);
                GraphicalEngine.MoveObject(((Rock)unit).connectedUnits[i].obj, temppos);
                database.units[(int)temppos.x, (int)temppos.y].Add((Switch)((Rock)unit).connectedUnits[i]);
                ((Switch)((Rock)unit).connectedUnits[i]).position = ((Switch)((Rock)unit).connectedUnits[i]).obj.transform.position;
            }
        }
        unit.x = (int)unit.obj.transform.position.x; unit.y = (int)unit.obj.transform.position.y;
        unit.position = unit.obj.transform.position;
        return true;

    }
        

    public int MoveObjects(Unit unit, Direction d, int distance)
    {
        int counter = 0;
        for (int i = 0; i < distance; i++){
            if (MoveObjects(unit, d))
            {
                lock(database.units){
                    engine.ApplyGravity();
                }
                counter++;
            }
        }
        return counter;
        
    }
    public Unit CheckMovableItem(Vector2 position)
    {
        foreach (Unit u in database.units[(int)position.x, (int)position.y])
        {
            if (u.unitType == UnitType.Box || u.unitType == UnitType.Player)
            {

                return u;
            }
        }
        return null;
    }
    public bool Jump(int distance)
    {
        for (int i = 0; i < distance; i++)
        {
            if (CheckJump(Toolkit.VectorSum(player.transform.position, Toolkit.DirectiontoVector(database.gravity_direction))))
            {
                Toolkit.RemoveUnit(player);
                player.position = Toolkit.VectorSum(player.transform.position, Toolkit.DirectiontoVector(database.gravity_direction));
                Toolkit.AddUnit(player);
                Gengine._move(Toolkit.ReverseDirection(database.gravity_direction));
            }
            else
            {
                return false;
            }
        }
        return true;
    }
    public bool Jump()
    {
        for (int i = 0; i < player.ability.number; i++)
        {
            if (CheckJump(Toolkit.VectorSum(player.transform.position, Toolkit.DirectiontoVector(database.gravity_direction))))
            {
                Toolkit.RemoveUnit(player);
                player.position = Toolkit.VectorSum(player.transform.position, Toolkit.DirectiontoVector(database.gravity_direction));
                Toolkit.AddUnit(player);
                Gengine._move(Toolkit.ReverseDirection(database.gravity_direction));
            }
            else
            {
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// jump
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    private bool CheckJump(Vector2 position)
    {
        foreach (Unit u in database.units[(int)position.x, (int)position.y])
        {
            if (u.unitType == UnitType.Block || u.unitType == UnitType.Container || u.unitType == UnitType.Container)
                return false;
            if (u.unitType == UnitType.Wall)
                if (database.gravity_direction == Toolkit.ReverseDirection(((Wall)u).direction))
                    return false;
        }
        return true;
    }

    public int FallPlayer()
    {
        int counter = 0;
        Vector2 pos;
        while (true)
        {
            pos = Toolkit.VectorSum(player.transform.position, Toolkit.DirectiontoVector(player.direction));
            for (int i = 0; i < database.units[(int)pos.x, (int)pos.y].Count; i++)
            {
                Unit u = database.units[(int)pos.x, (int)pos.y][i];
                if (u.unitType == UnitType.Block || u.unitType == UnitType.BlockSwitch || u.unitType == UnitType.Container || u.unitType == UnitType.Rock)
                {
                    player.state = PlayerState.Hanging;
                    return counter;
                }
            }
            pos = Toolkit.VectorSum(player.transform.position, Toolkit.DirectiontoVector(database.gravity_direction));
            for(int i=0; i<database.units[(int)pos.x, (int)pos.y].Count; i++)
            {
                Unit u = database.units[(int)pos.x, (int)pos.y][i];
                if (u.CanBeMoved)
                {
                    if(FallUnit(u) == 0)
                    {
                        player.state = PlayerState.Steady;
                        return counter;
                    }
                    break;
                }
                else
                {
                    player.state = PlayerState.Steady;
                    return counter;
                }
            }
            MoveUnit(player,pos, 1);
            counter++;
        }
    }
    public int FallUnit(Unit unit)
    {
        int counter = 0;
        Vector2 pos;
        while (true)
        {
            pos = Toolkit.VectorSum(unit.transform.position, Toolkit.DirectiontoVector(database.gravity_direction));
            for (int i = 0; i < database.units[(int)pos.x, (int)pos.y].Count; i++)
            {
                Unit u = database.units[(int)pos.x, (int)pos.y][i];
                if (u.CanBeMoved)
                {
                    if(u.unitType == UnitType.Player)
                    {
                        if (FallPlayer() == 0)
                        {
                            return counter;
                        }
                        break;
                    }
                    if (FallUnit(u) == 0)
                    {
                        return counter;
                    }
                    break;
                }
                else
                {
                    return counter;
                }
            }
            MoveUnit(unit,pos, 1);
            counter++;
        }
    }
    private bool CheckBlockandContainer(Vector2 position)
    {
        foreach (Unit u in database.units[(int)position.x, (int)position.y])
        {
            if (u.unitType == UnitType.Block || u.unitType == UnitType.Container)
                return false;
        }

        return true;
    }

    public bool Blink(Direction direction)
    {
        if (Toolkit.IsVoid(Toolkit.DirectionToVectorWithMultiplier(direction, player.ability.number)))
        {
            Toolkit.RemoveUnit(player);
            player.position = Toolkit.DirectionToVectorWithMultiplier(direction, player.ability.number);
            Gengine._Player_Blink(player.position);
            Toolkit.AddUnit(player);
            return true;
        }
        return false;
    }

    public bool ContainerBlink(Vector2 position)
    {
        if (Toolkit.IsVoid(position))
        {
            Toolkit.RemoveUnit(player);
            player.position = position;
            Gengine._Player_Blink(player.position);
            Toolkit.AddUnit(player);
            return true;
        }
        return false;
    }
}

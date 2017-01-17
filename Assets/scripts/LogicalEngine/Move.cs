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
        Vector2 temp = Toolkit.VectorSum(player.position, Toolkit.DirectiontoVector(dir));
        for (int i = 0; i < database.units[(int)temp.x, (int)temp.y].Count; i++)
        {
            Unit u = database.units[(int)temp.x, (int)temp.y][i];
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
        Toolkit.AddUnit(player);
        //Gengine._Move_Object(player.obj, Toolkit.VectorSum(player.transform.position, Toolkit.DirectiontoVector(dir)));
        engine.playergraphics.Player_Move(player.gameObject, dir);
        player.x = (int)player.position.x; player.y = (int)player.position.y;
        return true;
    }

    private bool MoveObjects(Unit unit, Direction d)
    {
        Vector2 temp;
        bool flag = true;
        if (unit.unitType != UnitType.Wall)
        {
        }
        /*for(int i=0; i<database.units[5,9].Count; i++)
        {
            Wall.print(database.units[5,9][i]);
        }
        Wall.print("-----------------------------------------");*/
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
              //Reza  if (! Toolkit.IsWallOnTheWay(Toolkit.VectorSum(unit.obj.transform.position, Toolkit.DirectiontoVector(d)), Toolkit.ReverseDirection(database.gravity_direction)))
              // Reza      MoveObjects(database.units[(int)tmpvec.x, (int)tmpvec.y][i], d);
            }
        }

        database.units[(int)unit.transform.position.x, (int)unit.transform.position.y].Remove(unit);
        if (unit.unitType == UnitType.Player)
        {
            for (int i = 0; i < database.units[(int)unit.transform.position.x, (int)unit.transform.position.y].Count; i++)
            {
                Unit u = database.units[(int)unit.transform.position.x, (int)unit.transform.position.y][i];

            }
        }
        engine.Gengine._Move_Object(unit.obj, temp);
        database.units[(int)temp.x, (int)temp.y].Add(unit);
      
      
        if (unit.unitType == UnitType.Rock)
        {
            for (int i = 0; i < ((Rock)unit).connectedUnits.Count; i++)
            {
                //engine.reserved.Add(unit);
                database.units[(int)((Rock)unit).connectedUnits[i].obj.transform.position.x, (int)((Rock)unit).connectedUnits[i].obj.transform.position.y].Remove(((Rock)unit).connectedUnits[i]);
           // Reza     Vector2 temppos = Toolkit.VectorSum((Toolkit.DirectiontoVector(Toolkit.ReverseDirection(((Switch)((Rock)unit).connectedUnits[i]).direction))), unit.gameObject.transform.position);
           // Reza     GraphicalEngine.MoveObject(((Rock)unit).connectedUnits[i].obj, temppos);
           // Reza     database.units[(int)temppos.x, (int)temppos.y].Add((Switch)((Rock)unit).connectedUnits[i]);
           // Reza     ((Switch)((Rock)unit).connectedUnits[i]).position = ((Switch)((Rock)unit).connectedUnits[i]).obj.transform.position;
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

    public void RollPlayer(Direction direction, int distance)
    {
        Toolkit.RemoveUnit(player);
        player.position = Toolkit.VectorSum(player.position, Toolkit.DirectionToVectorWithMultiplier(Toolkit.ReverseDirection(direction), distance));
        Toolkit.AddUnit(player);
        engine.playergraphics.Player_Roll(player.gameObject, Toolkit.ReverseDirection(direction), distance);
    }
}

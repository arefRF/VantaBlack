using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public sealed class Toolkit{
    public static Vector2 VectorSum(Vector2 a, Vector2 b)
    {
        return new Vector2(a.x + b.x, a.y + b.y);
    }

    public static Direction VectorToDirection(Vector2 vector)
    {
        if (vector.x == 1 && vector.y == 0)
            return Direction.Right;

        else if (vector.x == 0 && vector.y == 1)
            return Direction.Up;

        else if (vector.x == -1 && vector.y == 0)
            return Direction.Left;

        else if (vector.x == 0 && vector.y == -1)
            return Direction.Down;

        return Direction.Down;
    }

    public static bool IsWallOnTheWay(Vector2 position, Direction dir)
    {
        Database database = Starter.GetDataBase();
        if (dir == Direction.Right)
        {
            for (int i = 0; i < database.units[(int)position.x, (int)position.y].Count; i++)
            {
                Unit u = database.units[(int)position.x, (int)position.y][i];
                if (u.unitType == UnitType.Wall)
                {
                    if (((Wall)u).direction == Direction.Right)
                        return true;
                }
            }
            return false;
        }
        else if (dir == Direction.Left)
        {
            for (int i = 0; i < database.units[(int)position.x, (int)position.y].Count; i++)
            {
                Unit u = database.units[(int)position.x, (int)position.y][i];
                if (u.unitType == UnitType.Wall)
                {
                    if (((Wall)u).direction == Direction.Left)
                        return true;
                }
            }
            return false;
        }
        else if (dir == Direction.Up)
        {
            for (int i = 0; i < database.units[(int)position.x, (int)position.y].Count; i++)
            {
                Unit u = database.units[(int)position.x, (int)position.y][i];
                if (u.unitType == UnitType.Wall)
                {
                    if (((Wall)u).direction == Direction.Up)
                        return true;
                }
            }
            return false;
        }
        else if (dir == Direction.Down)
        {
            for (int i = 0; i < database.units[(int)position.x, (int)position.y].Count; i++)
            {
                Unit u = database.units[(int)position.x, (int)position.y][i];
                if (u.unitType == UnitType.Wall)
                {
                    if (((Wall)u).direction == Direction.Down)
                        return true;
                }
            }
            return false;
        }
        return false;
    }

    public static Direction ReverseDirection(Direction d)
    {
        switch (d)
        {
            case Direction.Up: return Direction.Down;
            case Direction.Down: return Direction.Up;
            case Direction.Left: return Direction.Right;
            case Direction.Right: return Direction.Left;
            default: return Direction.Up;
        }
    }

    public static bool IsEmptySpace(Vector2 position,  Direction d)
    {
        Database database = Starter.GetDataBase();
        try {
            /*foreach(Unit u in Database.database.units[1,1])
            {
                Wall.print(u.unitType);
            }*/
            Vector2 temp = DirectiontoVector(ReverseDirection(d));
            if (IsWallOnTheWay(VectorSum(position, temp), d))
                return false;
            for (int i = 0; i < database.units[(int)position.x, (int)position.y].Count; i++)
            {
                Unit u = database.units[(int)position.x, (int)position.y][i];
                if (u.unitType == UnitType.Wall || u.unitType == UnitType.Switch || u.unitType == UnitType.Pipe)
                    continue;
                else if (u.unitType == UnitType.Door)
                {
                    if (((Door)u).direction == d && !((Door)u).isOpen)
                        return false;
                }
                else { return false; }
            }
            return true;
        }
        catch
        {
            return false;
        }
    }

    public static Vector2 DirectiontoVector(Direction d)
    {
        switch (d)
        {
            case Direction.Right: return new Vector2(1, 0);
            case Direction.Left: return new Vector2(-1, 0);
            case Direction.Down: return new Vector2(0, -1);
            case Direction.Up: return new Vector2(0, 1);
            default: return new Vector2(0, 0);
        }
    }

    public static void ClonableUnitToUnit(CloneableUnit u, Unit unit)
    {
        unit.position = u.position;
        unit.movable = u.movable;
        unit.CanBeMoved = u.CanBeMoved;
        switch (unit.unitType)
        {
            case UnitType.Block: _CBlockToBlock((CloneableBlock)u, (Block)unit); break;
            case UnitType.BlockSwitch: _CBlockSwitchToBlockSwitch((CloneableBlockSwitch)u, (BlockSwitch)unit); break;
            case UnitType.Box: _CBoxToBox((CloneableBox)u, (Box)unit); break;
            case UnitType.Container: _CContainerToContainer((CloneableContainer)u, (Container)unit); break;
            case UnitType.Door: _CDoorToDoor((CloneableDoor)u, (Door)unit); break;
            case UnitType.Magnet: break;
            case UnitType.Pipe: break;
            case UnitType.Player: _CPlayerToPlayer((CloneablePlayer)u, (Player)unit); break;
            case UnitType.Rock: _CRockToRock((CloneableRock)u, (Rock)unit); break;
            case UnitType.Switch: _CSwitchToSwitch((CloneableSwitch)u, (Switch)unit); break;
            case UnitType.Wall: _CWAllToWall((CloneableWall)u, (Wall)unit); break;
        }
    }

    private static void _CDoorToDoor(CloneableDoor d, Door door)
    {
        door.isOpen = d.isOpen;
    }
    private static void _CWAllToWall(CloneableWall w, Wall wall)
    {
        Database database = Starter.GetDataBase();
        wall.direction = w.direction;
        wall.magnetic = w.magnetic;
        Snapshot snp = Starter.GetEngine().GetCurrentSnapshot();
        for (int i = 0; i < wall.connectedUnits.Count; i++)
        {
            bool flag = true;
            for(int j = 0; j < snp.units.Count; j++)
            {
                if (snp.units[j].codeNumber == wall.connectedUnits[i].codeNumber)
                {
                    for(int k=0;k<database.units[(int)snp.units[j].position.x, (int)snp.units[j].position.y].Count; k++)
                    {
                        if (snp.units[j].codeNumber == database.units[(int)snp.units[j].position.x, (int)snp.units[j].position.y][k].codeNumber)
                        {
                            database.units[(int)snp.units[j].position.x, (int)snp.units[j].position.y].RemoveAt(k);
                        }
                    }
                    break;
                }

            }
                database.units[(int)wall.connectedUnits[i].obj.transform.position.x, (int)wall.connectedUnits[i].obj.transform.position.y].Remove((Switch)(wall.connectedUnits[i]));
                Vector2 temppos = Toolkit.VectorSum((Toolkit.DirectiontoVector(Toolkit.ReverseDirection(((Switch)(wall.connectedUnits[i])).direction))), wall.position);
                database.units[(int)temppos.x, (int)temppos.y].Add((Switch)(wall.connectedUnits[i]));
                GraphicalEngine.MoveObject(wall.connectedUnits[i].obj, temppos);
        }
    }

    private static void _CSwitchToSwitch(CloneableSwitch s, Switch sw)
    {
        sw.singlestate = s.singlestate;
        sw.isOn = s.isOn;
        sw.isAutomatic = s.isAutomatic;
        sw.disabled = s.disabled;
        sw.counter = s.counter;
    }

    private static void _CContainerToContainer(CloneableContainer c, Container container)
    {
        container.ability = c.ability;
        container._lastAbility = c._lastAbility;
        container.forward = c.forward;
        container.counter = c.counter;
    }

    private static void _CBlockToBlock(CloneableBlock b, Block block)
    {
        block.ability = b.ability;
    }

    private static void _CPlayerToPlayer(CloneablePlayer p, Player player)
    {
        player.ability = p.ability;
        player.move_direction = p.move_direction;
    }

    private static void _CBoxToBox(CloneableBox b, Box box)
    {

    }

    private static void _CRockToRock(CloneableRock r, Rock rock)
    {
        Database database = Starter.GetDataBase();
        for (int i = 0; i < rock.connectedUnits.Count; i++)
        {
            database.units[(int)rock.connectedUnits[i].obj.transform.position.x, (int)rock.connectedUnits[i].obj.transform.position.y].Remove((Switch)(rock.connectedUnits[i]));
            Vector2 temppos = Toolkit.VectorSum((Toolkit.DirectiontoVector(Toolkit.ReverseDirection(((Switch)(rock.connectedUnits[i])).direction))), rock.position);
            database.units[(int)temppos.x, (int)temppos.y].Add((Switch)(rock.connectedUnits[i]));
            GraphicalEngine.MoveObject(rock.connectedUnits[i].obj, temppos);
        }
    }

    private static void _CBlockSwitchToBlockSwitch(CloneableBlockSwitch b, BlockSwitch blockswitch)
    {
         
    }

    public static bool CanMove(Vector2 position, Direction d)
    {
        Database database = Starter.GetDataBase();
        try
        {
            /*foreach(Unit u in Database.database.units[1,1])
            {
                Wall.print(u.unitType);
            }*/
            Vector2 temp = DirectiontoVector(ReverseDirection(d));
            if (IsWallOnTheWay(VectorSum(position, temp), d))
                return false;
            for (int i = 0; i < database.units[(int)position.x, (int)position.y].Count; i++)
            {
                Unit u = database.units[(int)position.x, (int)position.y][i];
                if (u.unitType == UnitType.Wall || u.unitType == UnitType.Switch || u.unitType == UnitType.Pipe)
                    continue;
                else if (u.unitType == UnitType.Door)
                {
                    if (((Door)u).direction == d && !((Door)u).isOpen)
                        return false;
                }
                else if (u.CanBeMoved)
                {
                    
                }
                else { return false; }
            }
            return true;
        }
        catch
        {
            return false;
        }
    }

    public static Door IsDoorOnTheWay(Vector2 position, Direction d)
    {
        Database database = Starter.GetDataBase();
        for (int i=0;i<database.units[(int)position.x, (int)position.y].Count;i++)
        {
            Unit u = database.units[(int)position.x, (int)position.y][i];
            if (u.unitType == UnitType.Door)
            {
                if (((Door)u).direction == d)
                    return (Door)u;
            }
        }
        try
        {
            Vector2 pos2 = Toolkit.VectorSum(position, Toolkit.DirectiontoVector(d));
            for (int i = 0; i < database.units[(int)pos2.x, (int)pos2.y].Count; i++)
            {
                Unit u = database.units[(int)pos2.x, (int)pos2.y][i];
                if (u.unitType == UnitType.Door)
                {
                    if (((Door)u).direction == Toolkit.ReverseDirection(d))
                        return (Door)u;
                }
            }
        }
        catch {
            Wall.print("cathing");
        }
        return null;
    }

    public static Unit GetUnitByCodeNumber(int codenumber)
    {
        Database database = Starter.GetDataBase();
        for (int i=0; i<database.units.GetLength(0); i++)
        {
            for(int j=0; j<database.units.GetLength(1); j++)
            {
                for(int k=0; k<database.units[i,j].Count; k++)
                {
                    if (codenumber == database.units[i, j][k].codeNumber)
                        return database.units[i, j][k];
                }
            }
        }
        return null;
    }

    public static void RemoveWall(Wall wall)
    {
        Database database = Starter.GetDataBase();
        switch (wall.direction)
        {
            case Direction.Right:
                database.units[(int)wall.obj.transform.position.x, (int)wall.obj.transform.position.y].Remove(wall);
                database.units[(int)wall.obj.transform.position.x + 1, (int)wall.obj.transform.position.y].Remove(wall.obj.GetComponents<Wall>()[1]);
                break;
            case Direction.Left:
                database.units[(int)wall.obj.transform.position.x + 1, (int)wall.obj.transform.position.y].Remove(wall);
                database.units[(int)wall.obj.transform.position.x, (int)wall.obj.transform.position.y].Remove(wall.obj.GetComponents<Wall>()[0]);
                break;
            case Direction.Up:
                database.units[(int)wall.obj.transform.position.x, (int)wall.obj.transform.position.y].Remove(wall);
                database.units[(int)wall.obj.transform.position.x, (int)wall.obj.transform.position.y + 1].Remove(wall.obj.GetComponents<Wall>()[1]);
                break;
            case Direction.Down:
                database.units[(int)wall.obj.transform.position.x, (int)wall.obj.transform.position.y - 1].Remove(wall);
                database.units[(int)wall.obj.transform.position.x, (int)wall.obj.transform.position.y].Remove(wall.obj.GetComponents<Wall>()[0]);
                break;
        }
        wall.obj.SetActive(false);
    }
}


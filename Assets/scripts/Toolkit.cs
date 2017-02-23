using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public sealed class Toolkit{
    public static Database database;
    public static Vector2 VectorSum(Vector2 a, Vector2 b)
    {
        return new Vector2(a.x + b.x, a.y + b.y);
    }

    public static Vector2 VectorSum(Vector2 a, Direction d)
    {
        return VectorSum(a, DirectiontoVector(d));
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
        try {
            Vector2 temp = DirectiontoVector(ReverseDirection(d));
            temp = Toolkit.VectorSum(position, Toolkit.DirectiontoVector(d));
            for (int i = 0; i < database.units[(int)temp.x, (int)temp.y].Count; i++)
            {
                Unit u = database.units[(int)temp.x, (int)temp.y][i];
                if (u.unitType == UnitType.Wall || u.unitType == UnitType.Switch || u.unitType == UnitType.Pipe)
                    continue;
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
    public static Vector2 DirectionToVectorWithMultiplier(Direction d, int multiplier)
    {
        switch (d)
        {
            case Direction.Right: return new Vector2(multiplier, 0);
            case Direction.Left: return new Vector2(-multiplier, 0);
            case Direction.Down: return new Vector2(0, -multiplier);
            case Direction.Up: return new Vector2(0, multiplier);
            default: return new Vector2(0, 0);
        }
    }

    public static bool CanMove(Vector2 position, Direction d)
    {
        try
        {
            /*foreach(Unit u in Database.database.units[1,1])
            {
                Wall.print(u.unitType);
            }*/
            Vector2 temp = DirectiontoVector(ReverseDirection(d));

            for (int i = 0; i < database.units[(int)position.x, (int)position.y].Count; i++)
            {
                Unit u = database.units[(int)position.x, (int)position.y][i];
                if (u.unitType == UnitType.Wall || u.unitType == UnitType.Switch || u.unitType == UnitType.Pipe)
                    continue;
                else { return false; }
            }
            return true;
        }
        catch
        {
            return false;
        }
    }

    public static Unit GetUnitByCodeNumber(int codenumber)
    {
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

    public static bool IsHigherThan(Vector2 position1, Vector2 position2)
    {
        Direction gravity_direction = database.gravity_direction;
        switch (gravity_direction)
        {
            case Direction.Down: if (position1.y > position2.y) return true; return false;
            case Direction.Left: if (position1.x > position2.x) return true; return false;
            case Direction.Up: if (position1.y < position2.y) return true; return false;
            case Direction.Right: if (position1.x < position2.x) return true; return false;
            default: return false;
        }
    }

    public static bool CanplayerGoOnRampSideFromFromNoneRamp(Ramp ramp, Direction gravitydirection, Direction comingfromdirection)
    {
        switch (ramp.type)
        {
            case 1:
                switch (gravitydirection)
                {
                    case Direction.Down: if (comingfromdirection == Direction.Left) return true; return false;
                    case Direction.Left: if (comingfromdirection == Direction.Down) return true; return false;
                    default: return false;
                }
            case 2:
                switch (gravitydirection)
                {
                    case Direction.Left: if (comingfromdirection == Direction.Up) return true; return false;
                    case Direction.Up: if (comingfromdirection == Direction.Left) return true; return false;
                    default: return false;
                }
            case 3:
                switch (gravitydirection)
                {
                    case Direction.Right: if (comingfromdirection == Direction.Up) return true; return false;
                    case Direction.Up: if (comingfromdirection == Direction.Right) return true; return false;
                    default: return false;
                }
            case 4:
                switch (gravitydirection)
                {
                    case Direction.Down: if (comingfromdirection == Direction.Right) return true; return false;
                    case Direction.Right: if (comingfromdirection == Direction.Down) return true; return false;
                    default: return false;
                }
            default: return false;
        }
    }

    public static bool CanplayerGoOnRampSideFromRamp(Ramp ramp, Direction gravitydirection, Direction comingfromdirection)
    {
        switch (gravitydirection)
        {
            case Direction.Down:
                switch (ramp.type)
                {
                    case 1: if (comingfromdirection == Direction.Left) return true; return false;
                    case 3: if (comingfromdirection == Direction.Left) return true; return false;
                    case 4: if (comingfromdirection == Direction.Right) return true; return false;
                    default: return false;
                }
            case Direction.Left: if (comingfromdirection == Direction.Up || comingfromdirection == Direction.Down) return true; return false;
            case Direction.Up: if (comingfromdirection == Direction.Left || comingfromdirection == Direction.Right) return true; return false;
            case Direction.Right: if (comingfromdirection == Direction.Up || comingfromdirection == Direction.Left) return true; return false;
            default: return false;
        }
    }

    public static void AddUnitToPosition(Unit u, Vector2 position)
    {
        if (u.unitType == UnitType.Wall)
        {
            return;
        }
        database.units[(int)position.x, (int)position.y].Add(u);
    }
    public static Unit GetUnit(GameObject gameobject)
    {
        for (int i = 0; i < database.Xsize; i++)
        {
            for (int j = 0; j < database.Ysize; j++)
            {
                foreach (Unit u in database.units[i, j])
                {
                    if (u.gameObject == gameobject)
                        return u;
                }
            }
        }

        return null;
    }

    public static bool IsOnRamp(Unit unit)
    {
        for(int i=0; i<database.units[(int)unit.position.x, (int)unit.position.y].Count; i++)
        {
            Unit u = database.units[(int)unit.position.x, (int)unit.position.y][i];
            if (u.unitType == UnitType.Ramp)
                return true;
        }
        return false;
    }

    public static Unit GetUnitToFallOn(List<Unit> units, Direction dir)
    {
        if(units[0] is Ramp && units[1] is Ramp)
        {
            switch (dir)
            {
                case Direction.Up:
                    if (((Ramp)units[0]).type == 2 && ((Ramp)units[0]).type == 3)
                        return units[0];
                    return units[1];
                case Direction.Right:
                    if (((Ramp)units[0]).type == 3 && ((Ramp)units[0]).type == 4)
                        return units[0];
                    return units[1];
                case Direction.Down:
                    if (((Ramp)units[0]).type == 1 && ((Ramp)units[0]).type == 4)
                        return units[0];
                    return units[1];
                case Direction.Left:
                    if (((Ramp)units[0]).type == 1 && ((Ramp)units[0]).type == 2)
                        return units[0];
                    return units[1];
                default: return units[0];
            }
        }
        else if(units[0] is Branch && units[1] is Player)
        {
            return units[0];
        }
        else if(units[1] is Branch && units[0] is Player)
        {
            return units[1];
        }
        else if(units[0] is Ramp && units[1] is Player)
        {
            switch (dir)
            {
                case Direction.Up:
                    if (((Ramp)units[0]).type == 2 && ((Ramp)units[0]).type == 3)
                        return units[0];
                    return units[1];
                case Direction.Right:
                    if (((Ramp)units[0]).type == 3 && ((Ramp)units[0]).type == 4)
                        return units[0];
                    return units[1];
                case Direction.Down:
                    if (((Ramp)units[0]).type == 1 && ((Ramp)units[0]).type == 4)
                        return units[0];
                    return units[1];
                case Direction.Left:
                    if (((Ramp)units[0]).type == 1 && ((Ramp)units[0]).type == 2)
                        return units[0];
                    return units[1];
                default: return units[0];
            }
        }
        else if (units[1] is Ramp && units[0] is Player)
        {
            switch (dir)
            {
                case Direction.Up:
                    if (((Ramp)units[1]).type == 2 && ((Ramp)units[1]).type == 3)
                        return units[1];
                    return units[0];
                case Direction.Right:
                    if (((Ramp)units[1]).type == 3 && ((Ramp)units[1]).type == 4)
                        return units[1];
                    return units[0];
                case Direction.Down:
                    if (((Ramp)units[1]).type == 1 && ((Ramp)units[1]).type == 4)
                        return units[1];
                    return units[0];
                case Direction.Left:
                    if (((Ramp)units[1]).type == 1 && ((Ramp)units[1]).type == 2)
                        return units[1];
                    return units[0];
                default: return units[1];
            }
        }
        return null;
    }





    public static bool IsInsideBranch(Player player)
    {
        for (int i=0; i<database.units[(int)player.position.x, (int)player.position.y].Count; i++)
        {
            if (database.units[(int)player.position.x, (int)player.position.y][i] is Branch)
                return true;
        }
        return false;
    }
    public static bool IsInsideBranch(Vector2 position)
    {
        for (int i = 0; i < database.units[(int)position.x, (int)position.y].Count; i++)
        {
            if (database.units[(int)position.x, (int)position.y][i] is Branch)
                return true;
        }
        return false;
    }

    public static Direction Comparison(Vector2 source, Vector2 sink)
    {
        if(source.x > sink.x)
            return Direction.Right;
        else
        {
            if (source.y > sink.y)
                return Direction.Up;
            else if (source.y == sink.y)
                return Direction.Left;
        }
        return Direction.Down;
    }


    public static bool IsEmpty(Vector2 position)
    {
        List<Unit>[,] units = Starter.GetDataBase().units;
        if (units[(int)position.x, (int)position.y].Count == 0)
            return true;
        return false;
    }
    public static bool IsdoubleRamp(Vector2 position)
    {
        List<Unit>[,] units = Starter.GetDataBase().units;
        int counter = 0;
        for (int i = 0; i < units[(int)position.x, (int)position.y].Count; i++)
        {
            if (units[(int)position.x, (int)position.y][i] is Ramp)
                counter++;
        }
        if (counter == 2)
            return true;
        return false;
    }

    public static bool HasRamp(Vector2 position)
    {
        List<Unit>[,] units = Starter.GetDataBase().units;
        for (int i=0; i<units[(int)position.x, (int)position.y].Count; i++)
        {
            if (units[(int)position.x, (int)position.y][i] is Ramp)
                return true;
        }
        return false;
    }
    public static bool HasBranch(Vector2 position)
    {
        List<Unit>[,] units = Starter.GetDataBase().units;
        for (int i = 0; i < units[(int)position.x, (int)position.y].Count; i++)
        {
            if (units[(int)position.x, (int)position.y][i] is Branch)
                return true;
        }
        return false;
    }
    public static bool HasContainer(Vector2 position)
    {
        List<Unit>[,] units = Starter.GetDataBase().units;
        for (int i = 0; i < units[(int)position.x, (int)position.y].Count; i++)
        {
            if (units[(int)position.x, (int)position.y][i] is Container)
                return true;
        }
        return false;
    }
    public static bool HasRock(Vector2 position)
    {
        List<Unit>[,] units = Starter.GetDataBase().units;
        for (int i = 0; i < units[(int)position.x, (int)position.y].Count; i++)
        {
            if (units[(int)position.x, (int)position.y][i] is Rock)
                return true;
        }
        return false;
    }
    public static bool HasGate(Vector2 position)
    {
        List<Unit>[,] units = Starter.GetDataBase().units;
        for (int i = 0; i < units[(int)position.x, (int)position.y].Count; i++)
        {
            if (units[(int)position.x, (int)position.y][i] is Gate)
                return true;
        }
        return false;
    }

    public static Unit GetUnit(Vector2 position)
    {
        List<Unit>[,] units = Starter.GetDataBase().units;
        for (int i = 0; i < units[(int)position.x, (int)position.y].Count; i++)
        {
            if (!(units[(int)position.x, (int)position.y][i] is Player))
                return units[(int)position.x, (int)position.y][i];
        }
        return null;
    }
    public static Ramp GetRamp(Vector2 position)
    {
        List<Unit>[,] units = Starter.GetDataBase().units;
        for (int i = 0; i < units[(int)position.x, (int)position.y].Count; i++)
        {
            if (units[(int)position.x, (int)position.y][i] is Ramp)
                return (Ramp)units[(int)position.x, (int)position.y][i];
        }
        return null;
    }

    public static Branch GetBranch(Vector2 position)
    {
        List<Unit>[,] units = Starter.GetDataBase().units;
        for (int i = 0; i < units[(int)position.x, (int)position.y].Count; i++)
        {
            if (units[(int)position.x, (int)position.y][i] is Branch)
                return (Branch)units[(int)position.x, (int)position.y][i];
        }
        return null;
    }

    public static bool[] GetConnectedSidesForContainer(Unit unit)
    {
        bool[] result = new bool[4];
        result[0] = !IsConnectedFromPositionForContainer(unit, VectorSum(unit.position, new Vector2(0, 1)));
        result[1] = !IsConnectedFromPositionForContainer(unit, VectorSum(unit.position, new Vector2(1, 0)));
        result[2] = !IsConnectedFromPositionForContainer(unit, VectorSum(unit.position, new Vector2(0, -1)));
        result[3] = !IsConnectedFromPositionForContainer(unit, VectorSum(unit.position, new Vector2(-1, 0)));

        return result;
    }

    public static bool[] GetConnectedSides(Unit unit)
    {
        bool[] result = new bool[4];
        result[0] = !IsConnectedFromPosition(unit, VectorSum(unit.position, new Vector2(0, 1)));
        result[1] = !IsConnectedFromPosition(unit, VectorSum(unit.position, new Vector2(1, 0)));
        result[2] = !IsConnectedFromPosition(unit, VectorSum(unit.position, new Vector2(0, -1)));
        result[3] = !IsConnectedFromPosition(unit, VectorSum(unit.position, new Vector2(-1, 0)));
        
        return result;
    }

    public static bool IsConnectedFromPosition(Unit unit, Vector2 pos)
    {
        for (int i = 0; i < database.units[(int)pos.x, (int)pos.y].Count; i++)
        {
            Unit u = database.units[(int)pos.x, (int)pos.y][i];
            if (u.gameObject.transform.parent == unit.gameObject.transform.parent)
            {
                if (u is Gate)
                    return false;
                return true;
            }
        }         
        return false;
    }

    public static bool IsConnectedFromPositionForContainer(Unit unit, Vector2 pos)
    {
        for (int i = 0; i < database.units[(int)pos.x, (int)pos.y].Count; i++)
        {
            Unit u = database.units[(int)pos.x, (int)pos.y][i];
            if (u.gameObject.transform.parent == unit.gameObject.transform.parent)
            {
                if (u is Ramp && ((Ramp)u).IsOnRampSide(VectorToDirection(unit.position - pos)))
                    return false;
                return true;
            }
        }
        return false;
    }
}


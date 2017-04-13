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
        else if (units[(int)position.x, (int)position.y].Count == 1 && units[(int)position.x, (int)position.y][0] is Pipe)
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
        result[0] = !IsConnectedFromPositionForContainer(unit, VectorSum(unit.position, new Vector2(0, 1)), Direction.Up);
        result[1] = !IsConnectedFromPositionForContainer(unit, VectorSum(unit.position, new Vector2(1, 0)), Direction.Right);
        result[2] = !IsConnectedFromPositionForContainer(unit, VectorSum(unit.position, new Vector2(0, -1)), Direction.Down);
        result[3] = !IsConnectedFromPositionForContainer(unit, VectorSum(unit.position, new Vector2(-1, 0)), Direction.Left);

        return result;
    }

    public static bool[] GetConnectedSides(Unit unit)
    {
        bool[] result = new bool[4];
        result[0] = !IsConnectedFromPosition(unit, VectorSum(unit.position, Direction.Up));
        result[1] = !IsConnectedFromPosition(unit, VectorSum(unit.position, Direction.Right));
        result[2] = !IsConnectedFromPosition(unit, VectorSum(unit.position, Direction.Down));
        result[3] = !IsConnectedFromPosition(unit, VectorSum(unit.position, Direction.Left));
        
        return result;
    }
    public static bool[] GetConnectedSidesForRamp(Unit unit)
    {
        bool[] result = new bool[4];
        result[0] = !IsConnectedFromPositionForRamp(unit, VectorSum(unit.position, Direction.Up));
        result[1] = !IsConnectedFromPositionForRamp(unit, VectorSum(unit.position, Direction.Right));
        result[2] = !IsConnectedFromPositionForRamp(unit, VectorSum(unit.position, Direction.Down));
        result[3] = !IsConnectedFromPositionForRamp(unit, VectorSum(unit.position, Direction.Left));

        return result;
    }

    public static bool[] GetConnectedSidesForBranch(Unit unit)
    {
        bool[] result = new bool[4];
        result[0] = !IsConnectedFromPositionToBranch(unit, Direction.Up);
        result[1] = !IsConnectedFromPositionToBranch(unit, Direction.Right);
        result[2] = !IsConnectedFromPositionToBranch(unit, Direction.Down);
        result[3] = !IsConnectedFromPositionToBranch(unit, Direction.Left);
        return result;
    }

    public static bool IsConnectedFromPosition(Unit unit, Vector2 pos)
    {
        for (int i = 0; i < database.units[(int)pos.x, (int)pos.y].Count; i++)
        {
            Unit u = database.units[(int)pos.x, (int)pos.y][i];
            if (u.gameObject.transform.parent == unit.gameObject.transform.parent)
            {
                if (u is Gate || u is Branch)
                    return false;
                return true;
            }
        }         
        return false;
    }

    public static bool IsConnectedFromPositionForRamp(Unit unit, Vector2 pos)
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

    public static bool IsConnectedFromPositionToBranch(Unit unit, Direction direction)
    {
        Vector2 pos = VectorSum(unit.position, direction);
        for (int i = 0; i < database.units[(int)pos.x, (int)pos.y].Count; i++)
        {
            Unit u = database.units[(int)pos.x, (int)pos.y][i];
            if (u.gameObject.transform.parent == unit.gameObject.transform.parent)
            {
                if (u is Branch)
                    return true;
                return false;
            }
        }
        return false;
    }
    public static bool IsConnectedFromPositionForContainer(Unit unit, Vector2 pos, Direction direction)
    {
        for (int i = 0; i < database.units[(int)pos.x, (int)pos.y].Count; i++)
        {
            Unit u = database.units[(int)pos.x, (int)pos.y][i];
            if (u.gameObject.transform.parent == unit.gameObject.transform.parent)
            {
                if (u is Ramp)
                {
                    if (!IsdoubleRamp(u.position) && ((Ramp)u).IsOnRampSide(ReverseDirection(direction)))
                    {
                        return false;
                    }
                }
                return true;
            }
        }
        return false;
    }

    public static double GetDeltaPositionAndTransformPosition(Unit unit, Direction gravitydirection)
    {
        if(gravitydirection == Direction.Up || gravitydirection == Direction.Down)
        {
            return Mathf.Abs(unit.position.y - unit.transform.position.y);
        }
        else
        {
            return Mathf.Abs(unit.position.x - unit.transform.position.x);
        }
    }
    

    /// <summary>
    /// fgt bara graity payin
    /// </summary>
    /// <param name="From"></param>
    /// <param name="dir"></param>
    /// <returns></returns>
    public static Unit GetNearestUnit(Unit From, Direction dir)
    {
        List<Unit> to = new List<Unit>();
        Vector2 pos = VectorSum(From.position, dir);
        to.AddRange(database.GetUnits(pos));
        Direction dirtemp;
        if (dir == Direction.Down || dir == Direction.Up)
            dirtemp = Direction.Right;
        else
            dirtemp = Direction.Up;
        pos = VectorSum(pos, ReverseDirection(dirtemp));
        to.AddRange(database.GetUnits(pos));
        pos = VectorSum(pos, 2 * DirectiontoVector(dirtemp));
        to.AddRange(database.GetUnits(pos));
        return GetNearestUnit(From, to.ToArray());

    }

    public static Unit GetNearestUnitForJumpingPlayer(Player player, Direction dir)
    {
        List<Unit> to = new List<Unit>();
        Vector2 pos = VectorSum(player.position, dir);
        if (player.jumpdirection == dir)
            to.AddRange(database.GetUnits(pos));
        else if (((Jump)player.currentAbility).jumped != 0)
            if (GetDeltaPositionAndTransformPosition(player, player.GetGravity()) < 0.5)
                to.AddRange(database.GetUnits(pos));
        Direction dirtemp;
        if (dir == Direction.Down || dir == Direction.Up) {
            dirtemp = Direction.Right;
            if (player.position.x == player.transform.position.x)
                return GetNearestUnit(player, to.ToArray());
        }
        else {
            dirtemp = Direction.Up;
            if (player.position.y == player.transform.position.y)
                return GetNearestUnit(player, to.ToArray());
        }
        pos = VectorSum(pos, ReverseDirection(dirtemp));
        to.AddRange(database.GetUnits(pos));
        pos = VectorSum(pos, 2 * DirectiontoVector(dirtemp));
        to.AddRange(database.GetUnits(pos));
        return GetNearestUnit(player, to.ToArray());
    }
        
    public static Unit GetNearestUnit(Unit From, Unit[] To)
    {
        if (To.Length == 0)
            return null;
        int result = 0;
        double min = (From.position - To[0].position).sqrMagnitude;
        for(int i=1; i < To.Length; i++)
        {
            double temp = (From.position - To[1].position).sqrMagnitude;
            if (min > temp)
            {
                temp = min;
                result++;
            }
        }
        return To[result];
    }

    public static List<Unit> SortByDirection(List<Unit> units, Direction direction)
    {
        List<Unit> result = new List<Unit>();
        result.AddRange(units);
        for (int i=0; i<result.Count; i++)
        {
            for (int j = i+1; j < result.Count; j++)
            {
                switch (direction)
                {
                    case Direction.Down: if (result[i].position.y > result[j].position.y) { Unit temp = result[i]; result[i] = result[j];result[j] = temp;  } break;
                    case Direction.Up: if (result[i].position.y < result[j].position.y) { Unit temp = result[i]; result[i] = result[j]; result[j] = temp; } break;
                    case Direction.Right: if (result[i].position.x < result[j].position.x) { Unit temp = result[i]; result[i] = result[j]; result[j] = temp; } break;
                    case Direction.Left:if (result[i].position.x > result[j].position.x) { Unit temp = result[i]; result[i] = result[j]; result[j] = temp; } break;
                }
            }
        }
        if (result.Count != units.Count)
            throw new System.Exception();
        return result;
    }
}


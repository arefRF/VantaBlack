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

    public static Unit GetUnitByCodeNumber(long codenumber)
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
    public static Unit GetUnitFromGameObject(GameObject gameobject)
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

    
    public static bool IsEmpty(Vector2 position)
    {
        List<Unit>[,] units = Starter.GetDataBase().units;
        if (units[(int)position.x, (int)position.y].Count == 0)
            return true;
        else if (units[(int)position.x, (int)position.y].Count == 1 && units[(int)position.x, (int)position.y][0] is Pipe)
            return true;
        return false;
    }

    public static bool IsEmptyIncludingTransform(Vector2 position)
    {
        List<Unit>[,] units = Starter.GetDataBase().units;
        if (units[(int)position.x, (int)position.y].Count != 0)
            return false;
        for(int i=0; i<4; i++)
        {
            Vector2 temppos = VectorSum(position, NumberToDirection(i + 1));
            for(int j=0; j<database.units[(int)temppos.x, (int)temppos.y].Count; j++)
            {
                Vector2 temppos2 = RoundVector(database.units[(int)temppos.x, (int)temppos.y][j].transform.position);
                Debug.Log(temppos);
                Debug.Log(temppos2);
                if (temppos2.x == position.x && temppos2.y == temppos.y)
                    return false;
            }
        }
        return true;
    }

    public static bool IsEmptyExcludingLaser(Vector2 position)
    {
        List<Unit>[,] units = Starter.GetDataBase().units;
        if (units[(int)position.x, (int)position.y].Count == 0)
            return true;
        else if (units[(int)position.x, (int)position.y].Count == 1 && units[(int)position.x, (int)position.y][0] is Laser)
            return true;
        return false;
    }

    public static bool IsEmptySameParent(GameObject obj,Vector2 position)
    {
        List<Unit>[,] units = Starter.GetDataBase().units;
        if (units[(int)position.x, (int)position.y].Count == 0)
            return true;
        if (obj.transform.parent == units[(int)position.x, (int)position.y][0].transform.parent)
        {
            if (units[(int)position.x, (int)position.y].Count == 1 && units[(int)position.x, (int)position.y][0] is Pipe)
                return true;
            return false;
        }
        else
            return true;
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
    public static bool HasPlayer(Vector2 position)
    {
        List<Unit>[,] units = Starter.GetDataBase().units;
        for (int i = 0; i < units[(int)position.x, (int)position.y].Count; i++)
        {
            if (units[(int)position.x, (int)position.y][i] is Player)
                return true;
        }
        return false;
    }

    public static bool HasFountain(Vector2 position)
    {
        List<Unit>[,] units = Starter.GetDataBase().units;
        for (int i = 0; i < units[(int)position.x, (int)position.y].Count; i++)
        {
            if (units[(int)position.x, (int)position.y][i] is Fountain)
                return true;
        }
        return false;
    }

    public static bool Hasleanable(Vector2 position)
    {
        List<Unit>[,] units = Starter.GetDataBase().units;
        for (int i = 0; i < units[(int)position.x, (int)position.y].Count; i++)
        {
            if (units[(int)position.x, (int)position.y][i] is Leanable)
                return true;
        }
        return false;
    }

    public static bool HasDrainer(Vector2 position)
    {
        List<Unit>[,] units = Starter.GetDataBase().units;
        for (int i = 0; i < units[(int)position.x, (int)position.y].Count; i++)
        {
            if (units[(int)position.x, (int)position.y][i] is Drainer)
                return true;
        }
        return false;
    }

    public static bool HasLaser(Vector2 position)
    {
        List<Unit>[,] units = Starter.GetDataBase().units;
        for (int i = 0; i < units[(int)position.x, (int)position.y].Count; i++)
        {
            if (units[(int)position.x, (int)position.y][i] is Laser)
                return true;
        }
        return false;
    }

    public static Laser GetLaser(Vector2 position)
    {
        List<Unit>[,] units = Starter.GetDataBase().units;
        for (int i = 0; i < units[(int)position.x, (int)position.y].Count; i++)
        {
            if (units[(int)position.x, (int)position.y][i] is Laser)
                return (Laser)units[(int)position.x, (int)position.y][i];
        }
        return null;
    }

    public static Drainer GetDrianer(Vector2 position)
    {
        List<Unit>[,] units = Starter.GetDataBase().units;
        for (int i = 0; i < units[(int)position.x, (int)position.y].Count; i++)
        {
            if (units[(int)position.x, (int)position.y][i] is Drainer)
                return (Drainer)units[(int)position.x, (int)position.y][i];
        }
        return null;
    }

    public static Leanable GetLeanable(Vector2 position)
    {
        List<Unit>[,] units = Starter.GetDataBase().units;
        for (int i = 0; i < units[(int)position.x, (int)position.y].Count; i++)
        {
            if (units[(int)position.x, (int)position.y][i] is Leanable)
                return (Leanable)units[(int)position.x, (int)position.y][i];
        }
        return null;
    }

    public static Fountain GetFountain(Vector2 position)
    {
        List<Unit>[,] units = Starter.GetDataBase().units;
        for (int i = 0; i < units[(int)position.x, (int)position.y].Count; i++)
        {
            if (units[(int)position.x, (int)position.y][i] is Fountain)
                return (Fountain)units[(int)position.x, (int)position.y][i];
        }
        return null;
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

    public static Unit GetUnitIncludingPlayer(Vector2 position)
    {
        List<Unit>[,] units = Starter.GetDataBase().units;
        if (units[(int)position.x, (int)position.y].Count != 0)
            return units[(int)position.x, (int)position.y][0];
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

    public static Player GetPlayer(Vector2 position)
    {
        List<Player> units = Starter.GetDataBase().player;
        for (int i = 0; i < units.Count; i++)
        {
            if (units[i].position.x == position.x && units[i].position.y == position.y)
                return units[i];
        }
        return null;
    }

    public static Container GetContainer(Vector2 position)
    {
        List<Unit>[,] units = Starter.GetDataBase().units;
        for (int i = 0; i < units[(int)position.x, (int)position.y].Count; i++)
        {
            if (units[(int)position.x, (int)position.y][i] is Container)
                return (Container)units[(int)position.x, (int)position.y][i];
        }
        return null;
    }

    public static bool[] GetConnectedSidesForContainer(Unit unit)
    {
        bool[] result = new bool[4];
        result[0] = IsConnectedFromPositionForContainer(unit, VectorSum(unit.position, new Vector2(0, 1)), Direction.Up);
        result[1] = IsConnectedFromPositionForContainer(unit, VectorSum(unit.position, new Vector2(1, 0)), Direction.Right);
        result[2] = IsConnectedFromPositionForContainer(unit, VectorSum(unit.position, new Vector2(0, -1)), Direction.Down);
        result[3] = IsConnectedFromPositionForContainer(unit, VectorSum(unit.position, new Vector2(-1, 0)), Direction.Left);

        return result;
    }

    // Empty Sides for The Same PArent
    public static bool[] GetEmptySidesSameParent(Unit unit)
    {
        bool[] result = new bool[4];
        for(int i = 0; i < 4; i++)
        {
           result[i] = IsEmptySameParent(unit.gameObject,VectorSum(unit.position , NumberToDirection(i + 1)));
        }

        return result;
    }
    
    public static Color Ability_Color(List<Ability> ability)
    {
        float[] color = new float[4];
        if (ability.Count != 0)
        {
            switch (ability[0].abilitytype)
            {
                case AbilityType.Key: color = new float[] { 1, 1, 1, 1 }; break;
                case AbilityType.Fuel: color = new float[] { 1, 0.674f, 0.211f, 1 }; break;
                case AbilityType.Jump: color = new float[] { 0.59f, 0.78f, 1, 1 }; break;
                case AbilityType.Teleport: color = new float[] { 0.92f, 0.36f, 0.44f, 1 }; break;
                case AbilityType.Gravity: color = new float[] { 0.81f, 0.60f, 0.96f, 1 }; break;
                case AbilityType.Rope: color = new float[] { 1, 0.60f, 0.30f, 1 }; break;
            }
        }
        else
            color = new float[] { 1, 1, 1, 0 };
        return new Color(color[0], color[1], color[2], color[3]);
    }

    public static bool[] GetConnectedSides(Unit unit)
    {
        bool[] result = new bool[4];
        result[0] = !IsConnectedFromPosition(unit, Direction.Up);
        result[1] = !IsConnectedFromPosition(unit, Direction.Right);
        result[2] = !IsConnectedFromPosition(unit, Direction.Down);
        result[3] = !IsConnectedFromPosition(unit, Direction.Left);
        
        return result;
    }

    public static bool[] GetConnectedSidesForLaser(Unit unit)
    {
        bool[] result = new bool[4];
        result[0] = IsConnectedFromPositionForLaser(unit, VectorSum(unit.position, Direction.Up));
        result[1] = IsConnectedFromPositionForLaser(unit, VectorSum(unit.position, Direction.Right));
        result[2] = IsConnectedFromPositionForLaser(unit, VectorSum(unit.position, Direction.Down));
        result[3] = IsConnectedFromPositionForLaser(unit, VectorSum(unit.position, Direction.Left));

        return result;
    }

    public static bool[] GetConnectedSidesForRamp(Ramp ramp)
    {
        bool[] result = new bool[4];
        if(ramp.type == 2 || ramp.type == 3)
            result[0] = IsConnectedFromPositionForRamp(ramp, VectorSum(ramp.position, Direction.Up));
        if (ramp.type == 3 || ramp.type == 4)
            result[1] = IsConnectedFromPositionForRamp(ramp, VectorSum(ramp.position, Direction.Right));
        if (ramp.type == 1 || ramp.type == 4)
            result[2] = IsConnectedFromPositionForRamp(ramp, VectorSum(ramp.position, Direction.Down));
        if (ramp.type == 1 || ramp.type == 2)
            result[3] = IsConnectedFromPositionForRamp(ramp, VectorSum(ramp.position, Direction.Left));
        return result;
    }

    public static bool[] GetConnectedSidesForBranch(Unit unit)
    {
        bool[] result = new bool[4];
        result[0] = IsConnectedFromPositionToBranch(unit, Direction.Up);
        result[1] = IsConnectedFromPositionToBranch(unit, Direction.Right);
        result[2] = IsConnectedFromPositionToBranch(unit, Direction.Down);
        result[3] = IsConnectedFromPositionToBranch(unit, Direction.Left);
        return result;
    }

    public static bool IsConnectedFromPosition(Unit unit, Direction direction)
    {
        Vector2 pos = VectorSum(unit.position, direction);
        for (int i = 0; i < database.units[(int)pos.x, (int)pos.y].Count; i++)
        {
            Unit u = database.units[(int)pos.x, (int)pos.y][i];
            if (u.gameObject.transform.parent == unit.gameObject.transform.parent)
            {
                if (u is Gate || u is Branch || u is Laser)
                    return false;
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

    public static bool IsConnectedFromPositionForLaser(Unit unit, Vector2 pos)
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

    public static bool IsConnectedFromPositionForRamp(Unit unit, Vector2 pos)
    {
        for (int i = 0; i < database.units[(int)pos.x, (int)pos.y].Count; i++)
        {
            Unit u = database.units[(int)pos.x, (int)pos.y][i];
            if (u.gameObject.transform.parent == unit.gameObject.transform.parent)
            {
                if (u is Gate || u is Branch || u is Laser)
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
        return true;
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
                else if (u is Laser || u is Branch)
                    return false;
                return true;
            }
        }
        return false;
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
                    case Direction.Left: if (result[i].position.x > result[j].position.x) { Unit temp = result[i]; result[i] = result[j]; result[j] = temp; } break;
                }
            }
        }
        if (result.Count != units.Count)
            throw new System.Exception();
        return result;
    }
    public static List<Unit> SortByDirectionNearest(List<Unit> units, Direction direction,Unit unit)
    {
        List<Unit> result = new List<Unit>();
        result.AddRange(units);
        for (int i = 0; i < result.Count; i++)
        {
            for (int j = i + 1; j < result.Count; j++)
            {
                switch (direction)
                {
                    case Direction.Down: if (result[i].position.y > result[j].position.y) { Unit temp = result[i]; result[i] = result[j]; result[j] = temp; } break;
                    case Direction.Up: if (result[i].position.y < result[j].position.y) { Unit temp = result[i]; result[i] = result[j]; result[j] = temp; } break;
                    case Direction.Right:
                        if (result[i].position.x < result[j].position.x)
                        { Unit temp = result[i]; result[i] = result[j]; result[j] = temp; }
                        else if((result[i].position.x == result[j].position.x))
                            if(Mathf.Abs(result[i].position.y -  unit.position.y) < Mathf.Abs(result[j].position.y - unit.position.y))
                            {
                                Unit temp = result[i]; result[i] = result[j]; result[j] = temp;
                            }
                        break;
                    case Direction.Left:
                        if (result[i].position.x > result[j].position.x) {
                            Unit temp = result[i]; result[i] = result[j]; result[j] = temp;
                        }
                        else if ((result[i].position.x == result[j].position.x))
                            if (Mathf.Abs(result[i].position.y - unit.position.y) > Mathf.Abs(result[j].position.y - unit.position.y))
                            {
                                Unit temp = result[i]; result[i] = result[j]; result[j] = temp;
                            }
                        break;
                }
            }
        }
        if (result.Count != units.Count)
            throw new System.Exception();
        return result;
    }

    public static double GetDistance(Vector2 pos1, Vector2 pos2)
    {
        return Mathf.Sqrt(Mathf.Pow(Mathf.Abs(pos1.x - pos1.x), 2) + Mathf.Pow(Mathf.Abs(pos1.y - pos2.y), 2));
    }

    public static int DirectionToNumber(Direction direction)
    {
        switch (direction)
        {
            case Direction.Up: return 1;
            case Direction.Right: return 2;
            case Direction.Down: return 3;
            case Direction.Left: return 4;
            default: return -1;
        }
    }

    public static Direction NumberToDirection(int num)
    {
        switch (num)
        {
            case 1: return Direction.Up;
            case 2: return Direction.Right;
            case 3: return Direction.Down;
            case 4: return Direction.Left;
            default: throw new System.Exception();
        }
        
    }

    public static GameObject GetObjectInChild(GameObject parent, string name)
    {
        List<GameObject> objects = new List<GameObject>();
        objects.Add(parent);
        while (objects.Count != 0)
        {
            GameObject u = objects[0];
            objects.RemoveAt(0);

            if (u.name == name)
                return u;
            for (int i = 0; i < u.transform.childCount; i++)
            {
                objects.Insert(objects.Count, u.transform.GetChild(i).gameObject);
            }
        }
        return null;
    }


    public static string Icon_Path(AbilityType type)
    {
        string path = @"Containers\Icons\";
        if (type == AbilityType.Fuel)
        {
            path += "Fuel Off";
        }
        else if (type == AbilityType.Key)
            path += @"Key";
        return path;
    }

    public static bool isVertical(Direction direction)
    {
        return direction == Direction.Up || direction == Direction.Down;
    }

    public static bool isHorizontal(Direction direction)
    {
        return direction == Direction.Right || direction == Direction.Left;
    }

    public static int GetPlayerNumberInDatabase(Player player)
    {
        for(int i=0; i<database.player.Count; i++)
        {
            if (player == database.player[i])
                return i;
        }
        return -1;
    }

    public static bool AreNeighbours(Unit unit1, Unit unit2)
    {
        if (unit1.position.x == unit2.position.x && Mathf.Abs(unit1.position.y - unit2.position.y) == 1)
            return true;
        if (unit1.position.y == unit2.position.y && Mathf.Abs(unit1.position.x - unit2.position.x) == 1)
            return true;
        return false;
    }

    public static Vector3 RoundVector(Vector3 vector)
    {
        return new Vector3(Mathf.Round(vector.x), Mathf.Round(vector.y), Mathf.Round(vector.z));
    }
}



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
        Database database = Starter.GetDataBase();
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

    public static void RemoveUnit(Unit u)
    {
        Database database = Starter.GetDataBase();
        database.units[(int)u.position.x, (int)u.position.y].Remove(u);
    }

    public static void AddUnit(Unit u)
    {
        Database database = Starter.GetDataBase();
        if (u.unitType == UnitType.Wall)
        {
            return;
        }
        database.units[(int)u.position.x, (int)u.position.y].Add(u);
    }
    public static void AddUnitToPosition(Unit u, Vector2 position)
    {
        Database database = Starter.GetDataBase();
        if (u.unitType == UnitType.Wall)
        {
            return;
        }
        database.units[(int)position.x, (int)position.y].Add(u);
    }
    public static Unit GetUnit(GameObject gameobject)
    {
        Database database = Starter.GetDataBase();
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

    public static bool IsVoid(Vector2 position)
    {
        Database database = Starter.GetDataBase();
        for(int i=0; i<database.units[(int)position.x, (int)position.y].Count; i++)
        {
            Unit u = database.units[(int)position.x, (int)position.y][i];
            if (u.unitType == UnitType.Block || u.unitType == UnitType.BlockSwitch || u.unitType == UnitType.Box || u.unitType == UnitType.Container || u.unitType == UnitType.Player || u.unitType == UnitType.Rock)
                return false;
        }
        return true;
    }

    public static bool IsOnRamp(Unit unit)
    {
        Database database = Starter.GetDataBase();
        for(int i=0; i<database.units[(int)unit.position.x, (int)unit.position.y].Count; i++)
        {
            Unit u = database.units[(int)unit.position.x, (int)unit.position.y][i];
            if (u.unitType == UnitType.Ramp)
                return true;
        }
        return false;
    }

    public static Ramp GetRamp(Unit unit)
    {
        Database database = Starter.GetDataBase();
        for (int i = 0; i < database.units[(int)unit.position.x, (int)unit.position.y].Count; i++)
        {
            Unit u = database.units[(int)unit.position.x, (int)unit.position.y][i];
            if (u.unitType == UnitType.Ramp)
                return (Ramp)u;
        }
        return null;
    }
}


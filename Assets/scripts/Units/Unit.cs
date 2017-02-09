using UnityEngine;
using System.Collections.Generic;

public class Unit : MonoBehaviour {
    public Vector2 position { get; set; }
    public UnitType unitType { get; set; }
    public long codeNumber { get; set; }
    public APIUnit api { get; set; }

    public static int Code = 0;

    public List<Unit> ConnectedUnits { get; set; }

    public List<Unit> players { get; set; }

    // public abstract bool CanMove(UnitType unittype);

    public virtual Unit Clone()
    {
        return (Unit)MemberwiseClone();
    }

    public virtual bool PlayerMoveInto(Direction dir)
    {
        return false;
    }

    public virtual bool Move(Direction dir)
    {
        List<Unit> units = api.engine_GetUnits(this, dir);
        if (units.Count != 0)
            return false;
        api.unit_Move(this, dir);
        return true;
    }

    public virtual Vector2 fallOn(Unit fallingunit, Direction dir)
    {
        api.engine_Land(this, fallingunit, dir);
        return fallingunit.position;
    }

    public virtual bool CanMove(Direction dir, GameObject parent)
    {
        List<Unit> units = api.engine_GetUnits(this, dir);
        players = new List<Unit>();
        for(int i=0; i<units.Count; i++)
        {
            if(units[i] is Player)
            {
                players.Add(units[i]);
                continue;
            }
            bool flag = false;
            for (int j=0; j<ConnectedUnits.Count; j++)
            {
                if(units[i] == ConnectedUnits[j])
                {
                    flag = true;
                    break;
                }
            }
            if (!flag)
                return false;
        }
        int bound = players.Count;
        for (int i=0; i < bound; i++)
        {
            if (!players[i].CanMove(dir, parent))
                return false;
            players.AddRange(players[i].players);
        }
        //friction
        players.AddRange(EffectedUnits(Toolkit.ReverseDirection(Starter.GetDataBase().gravity_direction)));
        return true;
    }

    public virtual List<Unit> EffectedUnits(Direction dir)
    {
        List<Unit> units = api.engine_GetUnits(this, dir);
        List<Unit> result = new List<Unit>();
        for(int i=0; i<units.Count; i++)
        {
            if(units[i] is Player)
            {
                result.Add(units[i]);
                result.AddRange(units[i].EffectedUnits(dir));
            }
        }
        return result;
    }

    public void SetConnectedUnits(List<Unit> units)
    {
        for(int i=0; i<units.Count; i++)
        {
            if (units[i] == this)
                continue;
            ConnectedUnits.Add(units[i]);
        }
    }

    public virtual bool ApplyGravity(Direction gravitydirection, List<Unit>[,] units)
    {
        return false;
    }
}

public class CloneableUnit
{
    Vector2 position;

    public CloneableUnit(Vector2 position)
    {
        this.position = position;
    }
}


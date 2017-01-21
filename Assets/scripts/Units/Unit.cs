using UnityEngine;
using System.Collections.Generic;

public class Unit : MonoBehaviour {
    public Vector2 position { get; set; }
    public UnitType unitType { get; set; }
    public GameObject obj { get; set; }
    public long codeNumber { get; set; }

    public APIUnit api { get; set; }

    public static int Code = 0;

    public List<Unit> ConnectedUnits { get; set; }

    // public abstract bool CanMove(UnitType unittype);

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

    public virtual void fallOn(Unit fallingunit, Direction dir)
    {
        api.engine_Land(this, fallingunit, dir);
    }

    public virtual bool CanMove(Direction dir)
    {
        List<Unit> units = api.engine_GetUnits(this, dir);
        for(int i=0; i<units.Count; i++)
        {
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
        return true;
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
}

public class CloneableUnit
{

}


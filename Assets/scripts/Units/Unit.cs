using UnityEngine;
using System.Collections.Generic;

public class Unit : MonoBehaviour {
    public Vector2 position { get; set; }
    public UnitType unitType { get; set; }
    public GameObject obj { get; set; }
    public long codeNumber { get; set; }

    public APIUnit api { get; set; }

    public static int Code = 0;



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

}

public class CloneableUnit
{

}


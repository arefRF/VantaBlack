using UnityEngine;
using System.Collections.Generic;

public class APIUnit {
    LogicalEngine engine;

    public void engine_Move(Unit unit, Direction dir)
    {
        if(unit is Player)
        {
            engine.MovePlayer((Player)unit, dir);
        }
        else
        {
            engine.MoveUnit(unit, Toolkit.VectorSum(unit.position, Toolkit.DirectiontoVector(dir)));
        }
    }

    public void engine_Lean(Player player, Direction dir)
    {
        engine.Lean(player, dir);
    }

    public List<Unit> engine_GetUnits(Unit unit, Direction dir)
    {
        return engine.GetUnits(Toolkit.VectorSum(Toolkit.DirectiontoVector(dir), unit.position));
    }

    public void unit_Move(Unit unit, Direction dir)
    {
        if (unit is Player)
            ((Player)unit).Move(dir);
    }
}

using UnityEngine;
using System.Collections.Generic;

public class APIUnit {
    LogicalEngine engine;

    public void Move(Unit unit, Direction dir)
    {
        if(unit is Player)
        {
            engine.MovePlayer((Player)unit, Toolkit.VectorSum(unit.position, Toolkit.DirectiontoVector(dir)));
        }
        else
        {
            engine.MoveUnit(unit, Toolkit.VectorSum(unit.position, Toolkit.DirectiontoVector(dir)));
        }
    }

    public void Lean(Player player, Direction dir)
    {
        engine.Lean(player, dir);
    }

    public List<Unit> GetUnits(Unit unit, Direction dir)
    {
        return engine.GetUnits(Toolkit.VectorSum(Toolkit.DirectiontoVector(dir), unit.position));
    }
}

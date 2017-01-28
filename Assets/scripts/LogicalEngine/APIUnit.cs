using UnityEngine;
using System.Collections.Generic;

public class APIUnit {
    LogicalEngine engine;

    public APIUnit(LogicalEngine engine)
    {
        this.engine = engine;
    }

    public void engine_Move(Unit unit, Direction dir)
    {
        if(unit is Player)
        {
            engine.MovePlayer((Player)unit, dir);
        }
        else
        {
            //engine.MoveUnit(unit, Toolkit.VectorSum(unit.position, Toolkit.DirectiontoVector(dir)));
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

    public List<Unit> engine_GetUnits(Vector2 position)
    {
        return engine.GetUnits(position);
    }

    public void engine_Land(Unit unit, Unit laningunit, Direction landingdirection)
    {
        engine.UnitToGraphic_Land(laningunit, unit, unit.position);
    }

    public void engine_LandOnRamp(Ramp ramp, Unit landingunit, int ramptype)
    {
        engine.UnitToGraphic_LandOnRamp(landingunit, ramp, ramp.position, ramptype);
        Debug.Log(landingunit.position);
        Debug.Log("landing on ramp");
    }
    public void unit_Move(Unit unit, Direction dir)
    {
        if (unit is Player)
            ((Player)unit).Move(dir);
    }

    public void Absorb(Player player, Container container, AbilityType ability)
    {
        engine.UnitToGraphic_Absorb(player, container, ability);
    }

    public void Release(Player player, Container container, AbilityType ability)
    {
        engine.UnitToGraphic_Release(player, container, ability);
    }

    public void Swap(Player player, Container container, AbilityType ability)
    {
        engine.UnitToGraphic_Swap(player, container, ability);
    }

    public bool MoveUnit(Unit unit, Direction direction)
    {
        return engine.MoveUnit(unit, direction);
    }
    public void AddToStuckList(Unit unit)
    {
        if (!engine.stuckedunits.Contains(unit))
            engine.stuckedunits.Add(unit);
    }

    public void RemoveFromStuckList(Unit unit)
    {
        engine.stuckedunits.Remove(unit);
    }

    public bool isStucked(Unit unit)
    {
        for(int i=0; i<engine.stuckedunits.Count; i++)
        {
            if (engine.stuckedunits[i] == unit)
                return true;
        }
        return false;
    }

    public void ChangeSprite(Unit unit)
    {
        engine.apigraphic.UnitChangeSprite(unit);
    }
}

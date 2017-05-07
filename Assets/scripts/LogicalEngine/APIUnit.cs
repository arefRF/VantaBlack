using UnityEngine;
using System.Collections.Generic;

public class APIUnit {
    public LogicalEngine engine;
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
    public List<Unit> engine_GetUnits(Unit unit, Direction dir)
    {
        return engine.GetUnits(Toolkit.VectorSum(unit.position, dir));
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
        
    }

    public void graphicalengine_Land(Unit unit, Vector2 landposition)
    {
        engine.apigraphic.Land((Player)unit, landposition, Toolkit.GetUnit(landposition));
        
    }

    public void graphicalengine_Fall(Unit unit, Vector2 fallposition)
    {
        engine.apigraphic.Fall((Player)unit, fallposition);
    }

    public void graphicalengine_LandOnRamp(Unit unit, Vector2 landposition)
    {
        Ramp ramp = Toolkit.GetRamp(landposition);
        engine.apigraphic.LandOnRamp((Player)unit, landposition, ramp, ramp.type);
    }
    public void unit_Move(Unit unit, Direction dir)
    {
        if (unit is Player)
            ((Player)unit).Move(dir);
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

    public void RemoveFromDatabase(Unit unit)
    {
        engine.database.units[(int)unit.position.x, (int)unit.position.y].Remove(unit);
    }

    public void AddToDatabase(Unit unit)
    {
        if(!engine.database.units[(int)unit.position.x, (int)unit.position.y].Contains(unit))
            engine.database.units[(int)unit.position.x, (int)unit.position.y].Add(unit);
    }

    public void CheckstuckedList()
    {
        engine.CheckStuckedUnit();
    }

    public void CheckstuckedList(Unit exceptthis)
    {
        engine.CheckStuckedUnit(exceptthis);
    }

    public void GameObjectAnimationFinished(FunctionalContainer unit)
    {
        unit.Action_Fuel();
    }

    public void AddToSnapshot(Unit unit)
    {
        engine.snpmanager.AddToSnapShot(unit);
    }

    public void AddToSnapshot(List<Unit> units)
    {
        engine.snpmanager.AddToSnapShot(units);
    }

    public void TakeSnapshot()
    {
        engine.snpmanager.takesnapshot();
    }

    public void MergeSnapshot()
    {
        engine.snpmanager.MergeSnapshot();
    }

    public void StopPlayerCoroutine(Player player)
    {
        Unit unit = null;
        if (Toolkit.HasRamp(player.position) && !Toolkit.IsdoubleRamp(player.position))
            unit = Toolkit.GetRamp(player.position);
        engine.apigraphic.Undo_Player(player, unit);
    }
}


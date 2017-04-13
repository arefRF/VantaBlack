using UnityEngine;
using System.Collections.Generic;

public class Unit : MonoBehaviour {
    public Vector2 position { get; set; }
    public long codeNumber { get; set; }
    public APIUnit api { get; set; }

    public static int Code = 0;

    public List<Unit> ConnectedUnits { get; set; }

    public List<Unit> players { get; set; }

    // public abstract bool CanMove(UnitType unittype);
    public virtual void Run()
    {
        SetInitialSprite();
    }
    public virtual void SetInitialSprite()
    {

    }
    public virtual CloneableUnit Clone()
    {
        return null;
    }
    public virtual void Undo()
    {

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
            if (units[i] is Pipe)
                continue;
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
                if (Toolkit.HasBranch(Toolkit.VectorSum(position, dir)) || Toolkit.HasRamp(Toolkit.VectorSum(position, dir)))
                    continue;
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

    public virtual bool ApplyGravity()
    {
        return false;
    }
}

public class CloneableUnit
{
    public Vector2 position;
    public Unit original;
    public CloneableUnit(Vector2 position)
    {
        this.position = new Vector2(position.x, position.y);
    }

    public virtual void Undo()
    {
        original.api.RemoveFromDatabase(original);
        original.position = position;
        original.api.AddToDatabase(original);
        SetPosition();
    }

    public virtual void SetPosition()
    {
        original.gameObject.transform.position = original.position;
    }
}


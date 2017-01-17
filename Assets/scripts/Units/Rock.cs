using UnityEngine;
using System.Collections.Generic;

public class Rock : Block
{

    public List<Unit> connectedUnits;

    void Awake()
    {
        unitType = UnitType.Rock;
        obj = this.gameObject;
        position = gameObject.transform.position;
        movable = true;
        layer = 1;
    }
    public override bool CanMove(UnitType unittype)
    {
        if (unittype == UnitType.Box || unittype == UnitType.Player)
            return true;
        return false;
    }

    public CloneableRock Clone()
    {
        return CloneableRock.Clone(this);
    }
}
public class CloneableRock : CloneableUnit
{

    public static CloneableRock Clone(Rock rock)
    {
        CloneableRock r = new CloneableRock();
        CloneableUnit.init(rock, r);
        return r;
    }
}
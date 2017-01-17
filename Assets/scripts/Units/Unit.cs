using UnityEngine;
using System.Collections;

public abstract  class Unit : MonoBehaviour {
    public Vector2 position { get; set; }
    public UnitType unitType { get; set; }
    public GameObject obj { get; set; }
    public long codeNumber { get; set; }
    public bool movable { get; set; }

    public static int Code = 0;

    public bool CanBeMoved;

    public int x { get; set; }
    public int y { get; set; }
    public int layer { get; set; }

   // public abstract bool CanMove(UnitType unittype);

    public abstract bool MoveInto(Direction dir);
}

public class CloneableUnit
{
    public Vector2 position { get; set; }
    public UnitType unitType { get; set; }
    public long codeNumber { get; set; }
    public bool movable { get; set; }
    public bool CanBeMoved;
    public int x { get; set; }
    public int y { get; set; }
    public int layer { get; set; }

    public static void init(Unit unit, CloneableUnit cu)
    {
        unit.x = (int)unit.obj.transform.position.x;
        unit.y = (int)unit.obj.transform.position.y;
        cu.position = unit.position;
        cu.unitType = unit.unitType;
        cu.codeNumber = unit.codeNumber;
        cu.movable = unit.movable;
        cu.CanBeMoved = unit.CanBeMoved;
        cu.x = unit.x;
        cu.y = unit.y;
        cu.layer = unit.layer;
    }
}

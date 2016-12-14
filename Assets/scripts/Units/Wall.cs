using UnityEngine;
using System.Collections.Generic;

public class Wall : Unit
{

    public Direction direction;
    public bool magnetic;
    public List<Unit> connectedUnits;

    // Use this for initialization
    void Awake()
    {
        unitType = UnitType.Wall;
        obj = this.gameObject;
        position = gameObject.transform.position;
        movable = false;
        layer = 1;
    }

    // Update is called once per frame
    void Update()
    {

    }
    public override bool CanMove(UnitType unittype)
    {

        return false;
    }

    public CloneableWall Clone()
    {
        return CloneableWall.Clone(this);
    }
}
public class CloneableWall : CloneableUnit
{
    public Direction direction;
    public bool magnetic;
    public static CloneableWall Clone(Wall wall)
    {
        CloneableWall w = new CloneableWall();
        CloneableUnit.init(wall, w);
        w.direction = wall.direction;
        w.magnetic = wall.magnetic;
        return w;
    }
}

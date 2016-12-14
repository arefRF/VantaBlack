using UnityEngine;
using System.Collections.Generic;

public class Pipe : Unit
{
    public GameObject source, sink;
    bool isOpen;

    void Awake()
    {
        isOpen = true;
        movable = false;
        layer = 2;
    }
    public override bool CanMove(UnitType unittype)
    {

        return false;
    }

    public Unit Clone()
    {
        Pipe u = new Pipe();
        u.unitType = UnitType.Block;
        u.obj = obj;
        u.position = transform.position;
        u.movable = movable;
        u.codeNumber = codeNumber;
        u.CanBeMoved = CanBeMoved;
        u.layer = layer;
        return u;
    }
}

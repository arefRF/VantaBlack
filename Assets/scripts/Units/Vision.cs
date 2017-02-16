using UnityEngine;
using System.Collections;
using System;

public class Vision : Unit {

    public override bool PlayerMoveInto(Direction dir)
    {
        return false;
    }


    public override CloneableUnit Clone()
    {
        return new CloneableVision(this);
    }
}

public class CloneableVision : CloneableUnit
{
    public CloneableVision(Vision vision) : base(vision.position)
    {
        original = vision;
    }
}

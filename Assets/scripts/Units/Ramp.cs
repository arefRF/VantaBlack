using UnityEngine;
using System.Collections;
using System;

public class Ramp : Unit {
    public Direction direction;

    public override bool PlayerMoveInto(Direction dir)
    {
        return true;
    }

}

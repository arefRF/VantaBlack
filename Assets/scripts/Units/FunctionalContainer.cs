using UnityEngine;
using System.Collections;
using System;

public class FunctionalContainer : Container {
    public Direction direction;

    public override bool PlayerMoveInto(Direction dir)
    {
        return false;
    }
}

using UnityEngine;
using System.Collections.Generic;
using System;

public class Rock : Unit
{

    public override bool PlayerMoveInto(Direction dir)
    {
        return false;
    }
}

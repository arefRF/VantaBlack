using UnityEngine;
using System.Collections;
using System;

public class Ramp : Unit {
    public Direction direction;
    public int type { get; set;}

    public override bool PlayerMoveInto(Direction dir)
    {
        if (dir == Direction.Left)
        {
            if (type == 3 || type == 4)
                return true;
        }
        else if (dir == Direction.Right)
        {
            if (type == 1 || type == 2)
                return true;
        }
        else if (dir == Direction.Up)
        {
            if (type == 1 || type == 4)
                return true;
        }
        else if (dir == Direction.Down)
        {
            if (type == 2 || type == 3)
                return true;
        }
        return false;
    }

}

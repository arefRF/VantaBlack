using UnityEngine;
using System.Collections;
using System;

public class Ramp : Unit {
    public Direction direction;
    public int type;

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


    public override void fallOn(Unit fallingunit, Direction dir)
    {
        switch (dir)
        {
            case Direction.Up:
                if (type == 2 || type == 3)
                    api.engine_Land(this, fallingunit, dir);
                else
                    api.engine_LandOnRamp(this, fallingunit, type);
                return;
            case Direction.Right:
                if (type == 3 || type == 4)
                    api.engine_Land(this, fallingunit, dir);
                else
                    api.engine_LandOnRamp(this, fallingunit, type);
                return;
            case Direction.Down:
                if (type == 1 || type == 4)
                    api.engine_Land(this, fallingunit, dir);
                else
                    api.engine_LandOnRamp(this, fallingunit, type);
                return;
            case Direction.Left:
                if (type == 1 || type == 2)
                    api.engine_Land(this, fallingunit, dir);
                else
                    api.engine_LandOnRamp(this, fallingunit, type);
                return;
        }
    }
}

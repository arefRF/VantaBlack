using UnityEngine;
using System.Collections.Generic;

public class Ramp : Unit {
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

    public override bool CanMove(Direction dir)
    {
        List<Unit> units = api.engine_GetUnits(this, dir);
        for (int i = 0; i < units.Count; i++)
        {
            bool flag = false;
            for (int j = 0; j < ConnectedUnits.Count; j++)
            {
                if (units[i] == ConnectedUnits[j])
                {
                    flag = true;
                    break;
                }
            }
            if (!flag)
            {
                if (units[i] is Ramp)
                {
                    Ramp ramp = (Ramp)units[i];
                    Debug.Log(type);
                    Debug.Log(ramp.type);
                    switch (type)
                    {
                        case 1:
                            switch (dir)
                            {
                                case Direction.Down: return false;
                                case Direction.Right: if (ramp.type == 3) return true; return false;
                                case Direction.Up: if (ramp.type == 3)return true; return false;
                                case Direction.Left: return false;
                            }
                            break;
                        case 2:
                            switch (dir)
                            {
                                case Direction.Down: if (ramp.type == 4) return true; return false;
                                case Direction.Right: if (ramp.type == 4) return true; return false;
                                case Direction.Up: return false;
                                case Direction.Left: return false;
                            }
                            break;
                        case 3:
                            switch (dir)
                            {
                                case Direction.Down: if (ramp.type == 1) return true; return false;
                                case Direction.Right: return false;
                                case Direction.Up: return false;
                                case Direction.Left: if (ramp.type == 1) return true; return false;
                            }
                            break;
                        case 4:
                            switch (dir)
                            {
                                case Direction.Down: return false;
                                case Direction.Right: return false;
                                case Direction.Up: if (ramp.type == 2) return true; return false;
                                case Direction.Left: if (ramp.type == 2) return true; return false;
                            }
                            break;
                    }

                }
                else
                    return false;
            }
        }
        return true;
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

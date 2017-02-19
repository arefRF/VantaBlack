using UnityEngine;
using System.Collections.Generic;

public class Ramp : Unit {
    public int type;

    public override void SetInitialSprite()
    {
        string ramprootpath = "Ramps\\Ramp-type";
        string ramp_path = "";
        bool[] notconnected = Toolkit.GetConnectedSides(this);
        if (type == 1)
        {
            ramp_path = ramprootpath + "1-";
            // bot and left connected
            if (!notconnected[2] && !notconnected[3])
                ramp_path += "2";
            // just bot connected
            else if (!notconnected[2] && notconnected[3])
                ramp_path += "down";
            // just left connected
            else if (notconnected[2] && !notconnected[3])
                ramp_path += "left";
        }
        else if (type == 2)
        {
            ramp_path = ramprootpath + "2-";
            if (!notconnected[0] && !notconnected[3])
                ramp_path += "2";
            else if (!notconnected[0] && notconnected[3])
                ramp_path += "top";
            else if (notconnected[0] && !notconnected[3])
                ramp_path += "left";
        }
        else if (type == 3)
        {
            ramp_path = ramprootpath + "3-";
            if (!notconnected[0] && !notconnected[1])
                ramp_path += "2";
            //right connected
            else if (notconnected[0] && !notconnected[1])
                ramp_path += "right";
            else if (!notconnected[0] && notconnected[1])
                ramp_path += "top";
            //not connected to anything
            else
                ramp_path += "0";

        }
        else if (type == 4)
        {
            ramp_path = ramprootpath + "4-";
            if (!notconnected[1] && !notconnected[2])
                ramp_path += "2";
            else if (!notconnected[1] && notconnected[2])
                ramp_path += "right";
            else if (notconnected[1] && !notconnected[2])
                ramp_path += "down";
        }
        GetComponent<SpriteRenderer>().sprite = (Sprite)Resources.Load(ramp_path, typeof(Sprite));
    }

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

    public override bool CanMove(Direction dir, GameObject parent)
    {
        List<Unit> units = api.engine_GetUnits(this, dir);
        players = new List<Unit>();
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
                else if (units[i] is Player)
                {
                    players.Add(units[i]);
                }
                else
                    return false;
            }
        }
        return true;
    }

    public override List<Unit> EffectedUnits(Direction dir)
    {
        List<Unit> units = api.engine_GetUnits(this.position);
        List<Unit> result = new List<Unit>();
        for (int i = 0; i < units.Count; i++)
        {
            if (units[i] is Player)
            {
                result.Add(units[i]);
                result.AddRange(units[i].EffectedUnits(dir));
            }
        }
        return result;
    }

    public override Vector2 fallOn(Unit fallingunit, Direction dir)
    {
        switch (dir)
        {
            case Direction.Up:
                if (type == 2 || type == 3)
                {
                    api.engine_Land(this, fallingunit, dir);
                    return fallingunit.position;
                }
                else
                {
                    api.engine_LandOnRamp(this, fallingunit, type);
                    return position;
                }
            case Direction.Right:
                if (type == 3 || type == 4)
                {
                    api.engine_Land(this, fallingunit, dir);
                    return fallingunit.position;
                }
                else
                {
                    api.engine_LandOnRamp(this, fallingunit, type);
                    return position;
                }
            case Direction.Down:
                if (type == 1 || type == 4)
                {
                    api.engine_Land(this, fallingunit, dir);
                    return fallingunit.position;
                }
                else
                {
                    api.engine_LandOnRamp(this, fallingunit, type);
                    return position;
                }
            case Direction.Left:
                if (type == 1 || type == 2)
                {
                    api.engine_Land(this, fallingunit, dir);
                    return fallingunit.position;
                }
                else
                {
                    api.engine_LandOnRamp(this, fallingunit, type);
                    return position;
                }
            default: return position;
        }
    }

    public bool IsOnRampSide(Direction d)
    {
        switch(type)
        {
            case 1: if (d == Direction.Up || d == Direction.Right) return true; return false;
            case 2: if (d == Direction.Down || d == Direction.Right) return true; return false;
            case 3: if (d == Direction.Down || d == Direction.Left) return true; return false;
            case 4: if (d == Direction.Up || d == Direction.Left) return true; return false;
        }
        return false;
    }

    public bool ComingOnRampSide(Vector2 pos)
    {
        switch (Starter.GetGravityDirection()){
            case Direction.Down:
                switch (type)
                {
                    case 1: if (pos.x > position.x) return true; return false;
                    case 2: return false;
                    case 3: return false;
                    case 4: if (pos.x < position.x) return true; return false;
                    default: return false;
                }
            default: return false;
        }
    }

    public override CloneableUnit Clone()
    {
        return new CloneableRamp(this);
    }
}

public class CloneableRamp : CloneableUnit
{
    public CloneableRamp(Ramp ramp) : base(ramp.position)
    {
        original = ramp;
    }
}
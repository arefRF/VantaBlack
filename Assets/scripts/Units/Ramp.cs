using UnityEngine;
using System.Collections.Generic;

public class Ramp : Unit {
    public int type;

    public override void SetInitialSprite()
    {
        if (Starter.Blockinvis)
            GetComponent<SpriteRenderer>().sprite = null;
        else
        {
            bool[] connected = Toolkit.GetConnectedSidesForRamp(this);
            int sidecount = 0;
            for (int i = 0; i < 4; i++)
                sidecount += System.Convert.ToInt32(connected[i]);
            switch (sidecount)
            {
                case 0: Connected_0(connected); break;
                case 1: Connected_1(connected); break;
                case 2: Connected_2(connected); break;
            }
            /*string ramprootpath = "Ramps\\Version 4\\Rock Half ";
            string ramp_path = "";
            bool[] notconnected = Toolkit.GetConnectedSidesForRamp(this);
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
                else
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
                else
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
                else
                    ramp_path += "down";
            }
            GetComponent<SpriteRenderer>().sprite = (Sprite)Resources.Load(ramp_path, typeof(Sprite));
        }*/
        }
    }


    private void Connected_0(bool[] connected)
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = api.engine.initializer.sprite_Ramp[2];
    }
    private void Connected_1(bool[] connected)
    {
        switch (type)
        {
            case 1:
                if (connected[2])
                    gameObject.GetComponent<SpriteRenderer>().sprite = api.engine.initializer.sprite_Ramp[1];
                else if(connected[3])
                    gameObject.GetComponent<SpriteRenderer>().sprite = api.engine.initializer.sprite_Ramp[3];
                break;
            case 2:
                if (connected[0])
                    gameObject.GetComponent<SpriteRenderer>().sprite = api.engine.initializer.sprite_Ramp[3];
                else if (connected[3])
                    gameObject.GetComponent<SpriteRenderer>().sprite = api.engine.initializer.sprite_Ramp[1];
                break;
            case 3:
                if (connected[0])
                    gameObject.GetComponent<SpriteRenderer>().sprite = api.engine.initializer.sprite_Ramp[1];
                else if (connected[1])
                    gameObject.GetComponent<SpriteRenderer>().sprite = api.engine.initializer.sprite_Ramp[3];
                break;
            case 4:
                if (connected[1])
                    gameObject.GetComponent<SpriteRenderer>().sprite = api.engine.initializer.sprite_Ramp[1];
                else if (connected[2])
                    gameObject.GetComponent<SpriteRenderer>().sprite = api.engine.initializer.sprite_Ramp[3];
                break;
        }
    }
    private void Connected_2(bool[] connected)
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = api.engine.initializer.sprite_Ramp[0];
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
            if (units[i] is Pipe)
                continue;
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
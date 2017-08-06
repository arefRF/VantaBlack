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

            switch (type)
            {
                case 1: Type1Sprite(connected); break;
                case 2: Type2Sprite(connected); break;
                case 3: Type3Sprite(connected); break;
                case 4: Type4Sprite(connected); break;
            }

        }
    }

    private void Type1Sprite(bool[] connected)
    {
        if (connected[2] && connected[3])
            GetComponent<SpriteRenderer>().sprite = api.engine.initializer.sprite_Ramp[0, 3];
        else if (connected[2])
            GetComponent<SpriteRenderer>().sprite = api.engine.initializer.sprite_Ramp[0, 1];
        else if (connected[3])
            GetComponent<SpriteRenderer>().sprite = api.engine.initializer.sprite_Ramp[0, 2];
        else
            GetComponent<SpriteRenderer>().sprite = api.engine.initializer.sprite_Ramp[0, 0];

    }
    private void Type2Sprite(bool[] connected)
    {
        if (connected[3] && connected[0])
            GetComponent<SpriteRenderer>().sprite = api.engine.initializer.sprite_Ramp[1, 3];
        else if (connected[3])
            GetComponent<SpriteRenderer>().sprite = api.engine.initializer.sprite_Ramp[1, 1];
        else if (connected[0])
            GetComponent<SpriteRenderer>().sprite = api.engine.initializer.sprite_Ramp[1, 2];
        else
            GetComponent<SpriteRenderer>().sprite = api.engine.initializer.sprite_Ramp[1, 0];
    }
    private void Type3Sprite(bool[] connected)
    {
        if (connected[0] && connected[1])
            GetComponent<SpriteRenderer>().sprite = api.engine.initializer.sprite_Ramp[2, 3];
        else if (connected[0])
            GetComponent<SpriteRenderer>().sprite = api.engine.initializer.sprite_Ramp[2, 1];
        else if (connected[1])
            GetComponent<SpriteRenderer>().sprite = api.engine.initializer.sprite_Ramp[2, 2];
        else
            GetComponent<SpriteRenderer>().sprite = api.engine.initializer.sprite_Ramp[2, 0];
    }
    private void Type4Sprite(bool[] connected)
    {
        if (connected[1] && connected[2])
            GetComponent<SpriteRenderer>().sprite = api.engine.initializer.sprite_Ramp[3, 3];
        else if (connected[1])
            GetComponent<SpriteRenderer>().sprite = api.engine.initializer.sprite_Ramp[3, 1];
        else if (connected[2])
            GetComponent<SpriteRenderer>().sprite = api.engine.initializer.sprite_Ramp[3, 2];
        else
            GetComponent<SpriteRenderer>().sprite = api.engine.initializer.sprite_Ramp[3, 0];
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
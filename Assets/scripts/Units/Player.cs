using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Player : Unit
{
    public List<AbilityType> abilities;
    public List<Direction> move_direction;
    public Direction direction { get; set; }
    public PlayerState state { get; set; }

    public Direction leandirection { get; set; }
    public bool lean { get; set; }

    public bool onramp { get; set; }
    public Direction gravity {get;set; }

    public void Awake()
    {
        direction = move_direction[0];
    }

    public bool Should_Change_Direction(Direction dir)
    {
        for (int i = 0; i < move_direction.Count; i++)
            if (dir == move_direction[i])
                if (dir != direction)
                    return true;
        return false;
    }

    public bool Can_Move_Direction(Direction dir)
    {
        for (int i = 0; i < move_direction.Count; i++)
            if (dir == move_direction[i])
                return true;
        return false;
    }
    public bool Can_Lean( Direction dir)
    {
        if (dir == Direction.Up || dir == Direction.Down)
            return true;
        else
            return false;
    }

    public override bool Move(Direction dir)
    {
        Ramp ramp = null;
        List<Unit> units  = api.engine_GetUnits(this, dir);
        onramp = false;
        List<Unit> temp = api.engine_GetUnits(position);
        for (int i = 0; i < temp.Count; i++)
        {
            if (temp[i] is Ramp)
            {
                ramp = (Ramp)temp[i];
                if (ramp.IsOnRampSide(Toolkit.ReverseDirection(Starter.GetGravityDirection())))
                {
                    onramp = true;
                }
            }
        }
        if (onramp)
        {
            bool goingup = true;
            Direction gravitydirection = Starter.GetDataBase().gravity_direction;
            switch (gravitydirection)
            {
                case Direction.Down:
                    switch (dir)
                    {
                        case Direction.Right: if (ramp.type == 1) goingup = false; break;
                        case Direction.Left: if (ramp.type == 4) goingup = false; break;
                    }
                    break;
                case Direction.Right:
                    switch (dir)
                    {
                        case Direction.Up: if (ramp.type == 4) goingup = false; break;
                        case Direction.Down: if (ramp.type == 3) goingup = false; break;
                    }
                    break;
                case Direction.Up:
                    switch (dir)
                    {
                        case Direction.Right: if (ramp.type == 2) goingup = false; break;
                        case Direction.Left: if (ramp.type == 3) goingup = false; break;
                    }
                    break;
                case Direction.Left:
                    switch (dir)
                    {
                        case Direction.Up: if (ramp.type == 1) goingup = false; break;
                        case Direction.Down: if (ramp.type == 2) goingup = false; break;
                    }
                    break;
            }
            if(goingup)
                units = api.engine_GetUnits(Toolkit.VectorSum(Toolkit.VectorSum(Toolkit.DirectiontoVector(Toolkit.ReverseDirection(gravitydirection)), Toolkit.DirectiontoVector(dir)), position));
        }
        for (int i = 0; i < units.Count; i++) {
            if (!units[i].PlayerMoveInto(Toolkit.ReverseDirection(dir)))
                return false;
        }
        api.engine_Move(this, dir);
        return true;
    }

    public override bool CanMove(Direction dir)
    {
        List<Unit> units = api.engine_GetUnits(this, dir);
        players = new List<Unit>();
        for (int i = 0; i < units.Count; i++)
        {
            if (units[i] is Player)
            {
                players.Add(units[i]);
                continue;
            }
            else if (units[i] is Branch)
            {
                continue;
            }
            else if (units is Ramp)
            {

            }
            else
                return false;
        }
        for (int i = 0; i < players.Count; i++)
        {
            if (!players[i].CanMove(dir))
                return false;
        }
        int bound = players.Count;
        for(int i = 0; i< bound; i++)
        {
            players.AddRange(players[i].players);
        }
        return true;
    }

    public void Absorb(Container container)
    {
        container.PlayerAbsorb(this);
    }
    public void Release(Container container)
    {
        container.PlayerRelease(this);
    }
    public override bool PlayerMoveInto(Direction dir)
    {
        return false;
    }

    public bool Action()
    {
        if (abilities.Count == 0)
            return false;
        switch (abilities[0])
        {
            case AbilityType.Fuel: return false;
            case AbilityType.Direction: return true;
            case AbilityType.Jump: return true;
            case AbilityType.Blink: return false;
            case AbilityType.Gravity: return false;
            case AbilityType.Rope: return false;
            default: return false;
        }
    }

    public bool Action(Direction dir)
    {
        switch (abilities[0])
        {
            case AbilityType.Fuel: return true;
            case AbilityType.Direction: return false;
            case AbilityType.Jump: return false;
            case AbilityType.Blink: return true;
            case AbilityType.Gravity: return true;
            case AbilityType.Rope: return true;
            default: return false;
        }
    }
}


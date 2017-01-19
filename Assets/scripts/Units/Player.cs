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

    public void Awake()
    {
        direction = move_direction[0];
    }

    public override bool Move(Direction dir)
    {
        List<Unit> list  = api.engine_GetUnits(this, dir);
        for(int i = 0; i < list.Count; i++) {
            if (!list[i].PlayerMoveInto(Toolkit.ReverseDirection(dir)))
                return false;
        }
        api.engine_Move(this, dir);
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


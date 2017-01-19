﻿using UnityEngine;
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

    public bool Action()
    {
        return true;
    }

    public void Action(Direction dir)
    {

    }
    public void Release(Container container)
    {
        container.PlayerRelease(this);
    }  

    public override bool PlayerMoveInto(Direction dir)
    {
        return false;
    }
    
}


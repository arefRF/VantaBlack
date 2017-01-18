using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Player : Unit
{
    public Ability ability;
    public List<Direction> move_direction;
    public Direction direction { get; set; }
    public PlayerState state { get; set; }


    public bool Move(Direction dir)
    {
        Debug.Log(dir);
        List<Unit> list  = api.engine_GetUnits(this, dir);
        for(int i = 0; i < list.Count; i++)
        {
            if (!list[i].PlayerMoveInto(Toolkit.ReverseDirection(dir)))
                return false;
        }
        Debug.Log("Move");
        api.engine_Move(this, dir);
        return true;
    }

    public override bool PlayerMoveInto(Direction dir)
    {
        return false;
    }

}


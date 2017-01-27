using UnityEngine;
using System.Collections.Generic;
using System;

public class FunctionalContainer : Container {
    public Direction direction;
    bool on;
    public int moved { get; set; }
    protected int shouldmove;
    public override bool PlayerMoveInto(Direction dir)
    {
        return false;
    }

    public override void Action(Player player, Direction dir)
    {
        if (abilities.Count == 0)
            return;
        switch (abilities[0])
        {
            case AbilityType.Fuel: Action_Fuel(); break; 
        }
    }

    public void Action_Fuel()
    {
        Debug.Log("action fuel");
        if (moved == abilities.Count)
            return;
        Direction dir = direction;
        if (on)
        {
            dir = Toolkit.ReverseDirection(dir);
        }
        if (api.MoveUnit(this, dir))
        {
            moved++;
            if (moved == shouldmove)
            {
                on = !on;
                moved = 0;
            }
        }
        else
        {
            api.AddToStuckList(this);
            on = !on;
            shouldmove = moved;
        }
        
    }
}

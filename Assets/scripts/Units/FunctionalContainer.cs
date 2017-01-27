using UnityEngine;
using System.Collections.Generic;
using System;

public class FunctionalContainer : Container {
    public Direction direction;
    bool on;
    public int moved { get; set; }

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
            case AbilityType.Fuel: Action_Fuel(player); break; 
        }
    }

    private void Action_Fuel(Player player)
    {
        Debug.Log("action fuel");
        
        Direction dir = direction;
        if (on)
        {
            dir = Toolkit.ReverseDirection(dir);
        }
        if (api.MoveUnit(this, dir))
        {
            moved++;
            if (moved == abilities.Count)
                on = !on;
        }
        else
        {
            api.AddToStuckList(this);
            on = !on;
        }
        
    }
}

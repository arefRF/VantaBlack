using UnityEngine;
using System.Collections.Generic;
using System;

public class FunctionalContainer : Container {
    public Direction direction;
    bool on;
    public int moved;

    public override bool PlayerMoveInto(Direction dir)
    {
        return false;
    }

    public override void Action(Player player, Direction dir)
    {
        Debug.Log("container found");
        if (abilities.Count == 0)
            return;
        switch (abilities[0])
        {
            case AbilityType.Fuel: Action_Fuel(player); on = !on; break;
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
        int temp = 0;
        for(int i=0; i<moved; i++)
        {
            if (api.MoveUnit(this, dir))
                temp++;
            else
            {
                api.AddToStuckList(this);
                break;
            }
        }
        moved = temp;
    }
}

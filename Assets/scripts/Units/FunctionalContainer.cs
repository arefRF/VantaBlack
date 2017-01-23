using UnityEngine;
using System.Collections.Generic;
using System;

public class FunctionalContainer : Container {
    public Direction direction;
    bool on;
    int moved;

    public void Start()
    {
        moved = -1;
    }
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
            case AbilityType.Fuel: Action_Fuel(player); on = !on; break;
        }
    }

    private void Action_Fuel(Player player)
    {
       /* Direction dir = direction;
        if (forward)
            dir = Toolkit.ReverseDirection(direction);
        int number = moved;
        if (moved == -1)
            number = abilities.Count;
        for(int k=0; k<number; k++)
        {
            for (int i = 0; i < ConnectedUnits.Count; i++)
            {
                if (!ConnectedUnits[i].CanMove(dir))
                {
                    //add to wait list
                    return;
                }
            }
            Move(dir);
            for(int i = 0; i<ConnectedUnits.Count; i++)
            {
                ConnectedUnits[i].Move(dir);
            }
            moved = k;
        }*/

    }
}

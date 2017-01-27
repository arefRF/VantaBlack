using UnityEngine;
using System.Collections.Generic;
using System;

public class FunctionalContainer : Container {
    public Direction direction;
    public bool on;
    public int moved { get; set; }
    protected int shouldmove;
    private bool movedone = false;
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
        Debug.Log(shouldmove);
        if (movedone)
        {
            Debug.Log("move done");
            movedone = false;
            moved = 0;
            on = !on;
            shouldmove = abilities.Count;
            return;
        }
        Direction dir = direction;
        if (on)
        {
            dir = Toolkit.ReverseDirection(dir);
        }
        if (api.MoveUnit(this, dir))
        {
            moved++;
            if (moved == shouldmove)
                movedone = true;
        }
        else
        {
            api.AddToStuckList(this);
            on = !on;
            shouldmove = moved;
            moved = 0;
        }
    }
    public void Action_Fuel_Continue()
    {
        if (movedone)
        {
            Debug.Log("move done");
            movedone = false;
            moved = 0;
            on = !on;
            shouldmove = abilities.Count;
            return;
        }
        Direction dir = direction;
        if (!on)
        {
            dir = Toolkit.ReverseDirection(dir);
        }
        if (api.MoveUnit(this, dir))
        {
            moved++;
            if (moved == shouldmove)
                movedone = true;
        }
        else
        {
            api.AddToStuckList(this);
            on = !on;
            shouldmove = moved;
            moved = 0;
        }
    }
    protected override void ContainerAbilityChanged(bool increased)
    {
        Action_Fuel();
        shouldmove = abilities.Count;
    }
}

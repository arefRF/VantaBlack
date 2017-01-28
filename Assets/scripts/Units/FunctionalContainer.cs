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
        Debug.Log("should move: " + shouldmove);
        api.RemoveFromStuckList(this);
        if (movedone)
        {
            Debug.Log("move done");
            movedone = false;
            moved = 0;
            on = !on;
            shouldmove = abilities.Count;
            if (abilities.Count == 0)
                on = false;
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
            //on = !on;
            shouldmove = moved;
            moved = 0;
        }
    }
    public void Action_Fuel_Continue(Direction dir)
    {
        if (api.MoveUnit(this, dir))
        {
            moved = abilities.Count;
            movedone = true;
            on = !on;
            shouldmove = abilities.Count;
        }
        else
        {
            api.AddToStuckList(this);
        }
    }

    public void Action_Fuel_Stucked()
    {
        if (movedone)
        {
            return;
        }

    }
    protected override void ContainerAbilityChanged(bool increased)
    {
        Debug.Log("increased: " + increased);
        Debug.Log("on: " + on);
        if (increased && abilities.Count == 1)
        {
            shouldmove = 1;
            return;
        }
        if (on)
        {
            if (increased)
                Action_Fuel_Continue(direction);
            else
                Action_Fuel_Continue(Toolkit.ReverseDirection(direction));
        }
        else if (api.isStucked(this))
        {
            if (!increased)
                Action_Fuel_Continue(Toolkit.ReverseDirection(direction));
        }
        else
        {
            if (increased)
                shouldmove++;
            else
                shouldmove--;
        }
    }
}

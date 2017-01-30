using UnityEngine;
using System.Collections.Generic;
using System;

public class FunctionalContainer : Container {
    public Direction direction;
    public bool on;
    public int moved { get; set; }
    protected int shouldmove;
    private bool movedone = false;
    private int stucklevel = 0;
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
            case AbilityType.Fuel: Action_Fuel(true); break; 
        }
    }

    public void Action_Fuel(bool first)
    {
        gameObject.transform.parent.gameObject.GetComponent<ParentScript>().movelock = true;
        api.RemoveFromStuckList(this);
        if (first)
        {
            on = !on;
            stucklevel = 0;
        }
        else if (stucklevel != 0)
        {
            api.AddToStuckList(this);
            gameObject.transform.parent.gameObject.GetComponent<ParentScript>().movelock = false;
            Debug.Log("wtf");
            return;
        }
        if (movedone)
        {
            Debug.Log("move done");
            movedone = false;
            moved = 0;
            shouldmove = abilities.Count;
            if (abilities.Count == 0)
                on = false;
            gameObject.transform.parent.gameObject.GetComponent<ParentScript>().movelock = false;
            api.CheckstuckedList();
            return;
        }
        if (shouldmove == 0)
            shouldmove = abilities.Count;
        Direction dir = direction;
        if (!on)
        {
            dir = Toolkit.ReverseDirection(dir);
        }
        if (api.MoveUnit(this, dir))
        {
            moved++;
            Debug.Log(moved);
            if (moved == shouldmove)
                movedone = true;
        }
        else
        {
            Debug.Log("stucking");
            bool flag = false;
            Debug.Log(moved);
            if (moved != 0)
            {
                flag = true;
                stucklevel++;
            }
            if (/*first && */stucklevel == 0)
                stucklevel = abilities.Count - moved;
            api.AddToStuckList(this);
            shouldmove = moved;
            moved = 0;
            Debug.Log(flag);
            if (flag)
            {
                api.CheckstuckedList(this);
            }
            gameObject.transform.parent.gameObject.GetComponent<ParentScript>().movelock = false;
        }
    }
    public void Action_Fuel_Continue(Direction dir)
    {
        if(abilities.Count == 0)
        {
            on = !on;
        }
        if (api.MoveUnit(this, dir))
        {
            moved = abilities.Count;
            movedone = true;
            shouldmove = abilities.Count;
        }
        else
        {
            api.AddToStuckList(this);
            stucklevel++;
            Debug.Log(stucklevel);
        }
    }

    public void Action_Fuel_Stucked()
    {
        Debug.Log("action fuel stucked");
        gameObject.transform.parent.gameObject.GetComponent<ParentScript>().movelock = true;
        api.RemoveFromStuckList(this);
        if (movedone)
        {
            Debug.Log("move done");
            movedone = false;
            moved = 0;
            shouldmove = abilities.Count;
            if (abilities.Count == 0)
                on = false;
            gameObject.transform.parent.gameObject.GetComponent<ParentScript>().movelock = false;
            api.CheckstuckedList();
            return;
        }
        if (shouldmove == 0)
            shouldmove = abilities.Count;
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
            Debug.Log("stucking");
            bool flag = false;
            if (moved != 0)
            {
                flag = true;
                stucklevel++;
            }
            Debug.Log(stucklevel);
            api.AddToStuckList(this);
            shouldmove = moved;
            moved = 0;
            if (flag)
            {
                gameObject.transform.parent.gameObject.GetComponent<ParentScript>().movelock = false;
                api.CheckstuckedList();
            }
        }
    }
    protected override void ContainerAbilityChanged(bool increased)
    {
        try
        {
            if (abilities[0] != AbilityType.Fuel)
                return;
        }
        catch
        {
            return;
        }
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
            {
                Debug.Log(stucklevel);
                if (stucklevel < 1)
                {
                    Action_Fuel_Continue(Toolkit.ReverseDirection(direction));
                }
                else
                    stucklevel--;
            }
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

    public void ResetStuckLevel()
    {
        stucklevel = 0;
    }
}

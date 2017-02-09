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

    public List<int> reservedmoveint { get; set; }
    protected List<bool> reservedmovebool;

    private bool resetstucked;
    protected bool laston;
    protected Direction stuckdirection;
    public int stuckstatus {get; set; }
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
        reservedmovebool.Clear();
        reservedmoveint.Clear();
        Direction dir = direction;
        if (!on)
        {
            dir = Toolkit.ReverseDirection(dir);
        }
        int count = abilities.Count;
        if (count != 0 && abilities[0] != AbilityType.Fuel)
            count = 0;
        if (first)
        {
            on = !on;
            dir = Toolkit.ReverseDirection(dir);
            laston = !on;
            api.ChangeSprite(this);
            if (stucklevel > 0)
            {
                if (!on && stuckdirection == dir)
                {
                    api.AddToStuckList(this);
                    stuckdirection = dir;
                    gameObject.transform.parent.gameObject.GetComponent<ParentScript>().movelock = false;
                    return;
                }
                else if (on && stuckdirection != dir)
                {
                    shouldmove = count - stucklevel;
                    stucklevel = 0;
                }
            }
            //stucklevel = 0;
        }
        if (stucklevel > 0 && !first)
        {
            dir = stuckdirection;
            shouldmove = stucklevel;
        }
        else if (stucklevel != 0 && !resetstucked)
        {
            //shayad bug bede
            if (dir != stuckdirection)
            {
                stucklevel--;
                if (stucklevel == 0)
                    api.RemoveFromStuckList(this);
                resetstucked = false;
                gameObject.transform.parent.gameObject.GetComponent<ParentScript>().movelock = false;
                Action_Fuel(false);
                return;
            }
            else
            {
                laston = !on;
                resetstucked = false;
                api.AddToStuckList(this);
                gameObject.transform.parent.gameObject.GetComponent<ParentScript>().movelock = false;
                return;
            }
        }
        if (movedone)
        {
            Debug.Log("move done");
            //if (moved == abilities.Count)
            resetstucked = false;
            movedone = false;
            moved = 0;
            shouldmove = count;
            if (count == 0)
            {
                on = false;
                api.ChangeSprite(this);
            }
            gameObject.transform.parent.gameObject.GetComponent<ParentScript>().movelock = false;
            api.CheckstuckedList();
            return;
        }
        if (shouldmove == 0)
            shouldmove = count;
        if (laston == on)
        {
            shouldmove = stucklevel;
        }
        Debug.Log("trying to move ");
        if (api.MoveUnit(this, dir))
        {
            resetstucked = true;
            laston = !on;
            if (stucklevel > 0)
                stucklevel--;
            moved++;
            if (moved == shouldmove)
                movedone = true;
        }
        else
        {
            Debug.Log("couldnt move");
            laston = on;
            bool flag = false;
            if (moved != 0)
            {
                flag = true;
                //stucklevel++;
            }
            if (/*first && */stucklevel == 0)
                stucklevel = count - moved;
            stuckdirection = dir;
            api.AddToStuckList(this);
            shouldmove = moved;
            moved = 0;
            if (flag)
            {
                api.CheckstuckedList(this);
            }
            gameObject.transform.parent.gameObject.GetComponent<ParentScript>().movelock = false;
        }
    }
    public void Action_Fuel_Continue(Direction dir,int count)
    {
        gameObject.transform.parent.gameObject.GetComponent<ParentScript>().movelock = true;
        api.RemoveFromStuckList(this);
        if (count == 0)
        {
            on = !on;
            api.ChangeSprite(this);
        }
        if (api.MoveUnit(this, dir))
        {
            moved = count;
            movedone = true;
            shouldmove = count;
            api.CheckstuckedList(this);
        }
        else
        {
            api.AddToStuckList(this);
            stuckdirection = dir;
            stucklevel++;
            CheckReservedList();
        }
        gameObject.transform.parent.gameObject.GetComponent<ParentScript>().movelock = false;
    }
    protected override void ContainerAbilityChanged(bool increased, int count)
    {
        try
        {
            if (abilities[0] != AbilityType.Fuel)
                return;
        }
        catch
        {
        }
        if (increased && count == 1)
        {
            shouldmove = 1;
            return;
        }
        if (on)
        {
            if (increased) {
                if (stucklevel < 1)
                    Action_Fuel_Continue(direction, count);
                else if (direction == stuckdirection)
                {
                    stucklevel++;
                    api.AddToStuckList(this);
                }
                else
                {
                    stucklevel--;
                    if (stucklevel == 0)
                        api.RemoveFromStuckList(this);
                }
            }
            else
            {
                if (stucklevel < 1)
                {
                    Action_Fuel_Continue(Toolkit.ReverseDirection(direction), count);
                }
                else
                {
                    Debug.Log(direction);
                    Debug.Log(stuckdirection);
                    if (direction != stuckdirection && !increased)
                    {
                        stucklevel++;
                        api.AddToStuckList(this);
                    }
                    else
                        stucklevel--;
                    Debug.Log(stucklevel);
                    if (abilities.Count == 0)
                    {
                        on = false;
                        api.ChangeSprite(this);
                    }
                    if (stucklevel == 0)
                        api.RemoveFromStuckList(this);
                    Debug.Log(stucklevel);
                }
            }
        }
        else if (api.isStucked(this))
        {
            if (on)
            {
                if (!increased)
                    Action_Fuel_Continue(Toolkit.ReverseDirection(direction), count);
            }
            /*else
            {
                if (increased)
                {
                    stucklevel--;
                    if (stucklevel == 0)
                        api.RemoveFromStuckList(this);
                }
            }*/
        }
        else
        {
            if (increased)
                shouldmove++;
            else
                shouldmove--;
        }
    }

    public override void CheckReservedList()
    {
        if (reservedmovebool.Count == 0)
            return;
        int count = reservedmoveint[0];
        bool increased = reservedmovebool[0];
        reservedmoveint.RemoveAt(0);
        reservedmovebool.RemoveAt(0);
        ContainerAbilityChanged(increased, count);
    }

    public void ResetStuckLevel()
    {
        resetstucked = true;
    }

    protected override void AddToReservedMove(bool increased, int count)
    {
        if (increased && count == 1)
        {
            shouldmove = 1;
            return;
        }
        if (on)
        {
            if (increased)
            {
                if (stucklevel >= 1)
                    stucklevel--;
                /*else
                    stucklevel--;*/
            }
            else
            {
                if (stucklevel >= 1)
                {
                    stucklevel--;
                }
                /*else
                {
                    stucklevel--;
                }*/
            }
        }
        else if (api.isStucked(this))
        {
            if (!increased)
                return;
        }
        else
        {
            if (increased)
                shouldmove++;
            else
                shouldmove--;
        }
        reservedmovebool.Add(increased);
        reservedmoveint.Add(count);
    }
}

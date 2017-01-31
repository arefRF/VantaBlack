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

    public List<int> reservedmoveint;
    protected List<bool> reservedmovebool;
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
        if (first)
        {
            on = !on;
            api.ChangeSprite(this);
            if (on && stucklevel > 0)
            {
                api.AddToStuckList(this);
                gameObject.transform.parent.gameObject.GetComponent<ParentScript>().movelock = false;
                return;
            }
            //stucklevel = 0;
        }
        else if (stucklevel != 0)
        {
            Debug.Log("not zero");
            api.AddToStuckList(this);
            gameObject.transform.parent.gameObject.GetComponent<ParentScript>().movelock = false;
            return;
        }
        if (movedone)
        {
            movedone = false;
            moved = 0;
            shouldmove = abilities.Count;
            if (abilities.Count == 0)
            {
                on = false;
                api.ChangeSprite(this);
            }
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
            bool flag = false;
            Debug.Log(moved);
            if (moved != 0)
            {
                flag = true;
                //stucklevel++;
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
            Debug.Log(stucklevel);
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
                else
                    stucklevel--;
            }
            else
            {
                if (stucklevel < 1)
                {
                    Action_Fuel_Continue(Toolkit.ReverseDirection(direction), count);
                }
                else
                {
                    stucklevel--;
                }
            }
        }
        else if (api.isStucked(this))
        {
            if (!increased)
                Action_Fuel_Continue(Toolkit.ReverseDirection(direction), count);
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
        int count = reservedmoveint[reservedmoveint.Count - 1];
        bool increased = reservedmovebool[reservedmovebool.Count - 1];
        reservedmoveint.RemoveAt(reservedmoveint.Count - 1);
        reservedmovebool.RemoveAt(reservedmovebool.Count - 1);
        ContainerAbilityChanged(increased, count);
    }

    public void ResetStuckLevel()
    {
        stucklevel = 0;
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

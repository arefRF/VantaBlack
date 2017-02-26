using UnityEngine;
using System.Collections.Generic;
using System;

public class FunctionalContainer : Container {
    public Direction direction;
    public bool on;
    public int moved { get; set; }
    public int shouldmove { get; set; }
    public bool movedone { get; set; }
    public int stucklevel {get;set;}

    public List<int> reservedmoveint { get; set; }
    public List<bool> reservedmovebool { get; set; }

    public bool resetstucked { get; set; }
    public bool laston { get; set; }
    public Direction stuckdirection { get; set; }
    public int stuckstatus {get; set; }
    public bool firstmove { get; set; } // baraye inke fgt dafeye avval snapshot begire

    public override bool PlayerMoveInto(Direction dir)
    {
        return false;
    }

    public override void Action(Player player, Direction dir)
    {
        if (gameObject.transform.parent.gameObject.GetComponent<ParentScript>().movelock)
            return;
        if (abilities.Count == 0)
            return;
        api.AddToSnapshot(this);
        api.AddToSnapshot(ConnectedUnits);
        api.AddToSnapshot(player);
        //api.TakeSnapshot();
        switch (abilities[0].abilitytype)
        {
            case AbilityType.Fuel: Action_Fuel(true); break;
            case AbilityType.Jump: ((Jump)abilities[0]).Action(player, dir); break;
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
        if (count != 0 && abilities[0].abilitytype != AbilityType.Fuel)
            count = 0;
        if (first)
        {
            on = !on;
            dir = Toolkit.ReverseDirection(dir);
            laston = !on;
            api.ChangeSprite(this);
            if (stucklevel > 0)
            {
                if (!on)
                {
                    if (stuckdirection == dir)
                    {
                        api.AddToStuckList(this);
                        stuckdirection = dir;
                        gameObject.transform.parent.gameObject.GetComponent<ParentScript>().movelock = false;
                        return;
                    }
                    else
                    {
                        shouldmove = count - stucklevel;
                        stucklevel = 0;
                        api.RemoveFromStuckList(this);
                        if (shouldmove == 0)
                            return;
                    }
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
                if (count < stucklevel + 1)
                {
                    Action_Fuel(false);
                }
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
            firstmove = true;
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

        if (api.MoveUnit(this, dir))
        {
            firstmove = false;
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
            firstmove = true;
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
        Debug.Log(count);
        gameObject.transform.parent.gameObject.GetComponent<ParentScript>().movelock = true;
        api.RemoveFromStuckList(this);
        if (count == 0)
        {
            on = !on;
            api.ChangeSprite(this);
        }
        if (api.MoveUnit(this, dir))
        {
            firstmove = false;
            moved = count;
            movedone = true;
            shouldmove = count;
            CheckReservedList();
        }
        else
        {
            firstmove = true;
            api.AddToStuckList(this);
            stuckdirection = dir;
            stucklevel++;
            CheckReservedList();
        }
        //gameObject.transform.parent.gameObject.GetComponent<ParentScript>().movelock = false;
        //firstmove = true;
    }
    protected override void ContainerAbilityChanged(bool increased, int count)
    {
        /*try
        {
            if (abilities[0].abilitytype != AbilityType.Fuel)
                return;
        }
        catch
        {
        }*/
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
                    if (direction != stuckdirection && !increased)
                    {
                        stucklevel++;
                        api.AddToStuckList(this);
                    }
                    else
                    {
                        stucklevel--;
                        CheckReservedList();
                    }
                    if (abilities.Count == 0)
                    {
                        on = false;
                        api.ChangeSprite(this);
                    }
                    if (stucklevel == 0)
                        api.RemoveFromStuckList(this);
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
        api.MergeSnapshot();
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
                /*if (stucklevel >= 1)
                {
                    stucklevel--;
                }*/
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
            else if(shouldmove > 0)
                shouldmove--;
        }
        reservedmovebool.Add(increased);
        reservedmoveint.Add(count);
    }
}

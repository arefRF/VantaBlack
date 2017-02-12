using UnityEngine;
using System.Collections.Generic;
using System;

public class DynamicContainer : FunctionalContainer {
    

    // Use this for initialization
    void Start () {
        moved = 0;
        shouldmove = abilities.Count;
        reservedmoveint = new List<int>();
        reservedmovebool = new List<bool>();
        laston = !on;
        stucklevel = 0;
        stuckstatus = 0;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public override bool PlayerMoveInto(Direction dir)
    {
        return false;
    }

    public override CloneableUnit Clone()
    {
        CloneableDynamicContainer clone = new CloneableDynamicContainer(this);
        for (int i = 0; i < abilities.Count; i++)
            clone.abilities.Add(abilities[i]);
        //clone.direction = direction;
        clone.on = on;
        clone.moved = moved;
        clone.shouldmove = shouldmove;
        clone.movedone = movedone;
        clone.stucklevel = stucklevel;
        for (int i = 0; i < reservedmoveint.Count; i++)
        {
            clone.reservedmoveint.Add(reservedmoveint[i]);
            clone.reservedmovebool.Add(reservedmovebool[i]);
        }
        clone.resetstucked = resetstucked;
        clone.laston = laston;
        clone.stuckdirection = stuckdirection;
        clone.stuckstatus = stuckstatus;
        return clone;
    }
}

public class CloneableDynamicContainer : CloneableUnit
{
    public List<AbilityType> abilities;
    public bool on;
    public int moved;
    public int shouldmove;
    public bool movedone;
    public int stucklevel;
    public List<int> reservedmoveint;
    public List<bool> reservedmovebool;
    public bool resetstucked;
    public bool laston;
    public Direction stuckdirection;
    public int stuckstatus;
    public CloneableDynamicContainer(DynamicContainer container) : base(container.position)
    {
        reservedmovebool = new List<bool>();
        reservedmoveint = new List<int>();
        abilities = new List<AbilityType>();
        for (int i = 0; i < container.abilities.Count; i++)
            abilities.Add(container.abilities[i]);
        for (int i = 0; i < container.reservedmovebool.Count; i++)
            reservedmovebool.Add(container.reservedmovebool[i]);
        for (int i = 0; i < container.reservedmoveint.Count; i++)
            reservedmoveint.Add(container.reservedmoveint[i]);
        on = container.on;
        moved = container.moved;
        shouldmove = container.shouldmove;
        movedone = container.movedone;
        stucklevel = container.stucklevel;
        resetstucked = container.resetstucked;
        laston = container.laston;
        stuckdirection = container.stuckdirection;
        stuckstatus = container.stuckstatus;
    }
}

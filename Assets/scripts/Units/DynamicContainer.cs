using UnityEngine;
using System.Collections.Generic;
using System;

public class DynamicContainer : FunctionalContainer {

    // Use this for initialization
    public override void Run() {
        abilities = new List<Ability>();
        for (int i = 0; i < abilitycount; i++)
        {
            abilities.Add(Ability.GetAbilityInstance(abilitytype));
            if (abilitytype == AbilityType.Jump)
                ((Jump)abilities[i]).number = 4;
        }
        api.ChangeSprite(this);
        moved = 0;
        shouldmove = abilities.Count;
        reservedmoveint = new List<int>();
        reservedmovebool = new List<bool>();
        laston = !on;
        stucklevel = 0;
        stuckstatus = 0;
        firstmove = true;
        base.Run();
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
        return new CloneableDynamicContainer(this);
    }
}

public class CloneableDynamicContainer : CloneableUnit
{
    public List<Ability> abilities;
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
    public bool firstmove;
    public CloneableDynamicContainer(DynamicContainer container) : base(container.position)
    {
        original = container;
        reservedmovebool = new List<bool>();
        reservedmoveint = new List<int>();
        abilities = new List<Ability>();
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
        firstmove = container.firstmove;
    }

    public override void Undo()
    {
        base.Undo();
        DynamicContainer original = (DynamicContainer)base.original;
        original.abilities = new List<Ability>();
        original.reservedmovebool = new List<bool>();
        original.reservedmoveint = new List<int>();
        for (int i = 0; i < abilities.Count; i++)
            original.abilities.Add(abilities[i]);
        for (int i = 0; i < reservedmovebool.Count; i++)
            original.reservedmovebool.Add(reservedmovebool[i]);
        for (int i = 0; i < reservedmoveint.Count; i++)
            original.reservedmoveint.Add(reservedmoveint[i]);
        original.on = on;
        original.moved = moved;
        original.movedone = movedone;
        original.stucklevel = stucklevel;
        original.resetstucked = resetstucked;
        original.laston = laston;
        original.stuckdirection = stuckdirection;
        original.stuckstatus = stuckstatus;
        original.firstmove = firstmove;
        original.shouldmove = shouldmove;
        
        original.api.engine.apigraphic.UnitChangeSprite(original);
    }
}

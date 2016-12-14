using UnityEngine;
using System.Collections.Generic;

public class Container : Unit{
    public int number;
    public Direction dirrection;
    public Ability ability;
    public Ability _lastAbility { get; set; }
    LogicalEngine engine;

    public bool forward { get; set; }
    public int counter { get; set; }

    public void Awake()
    {
        unitType = UnitType.Container;
        obj = this.gameObject;
        position = gameObject.transform.position;
        movable = true;
        layer = 1;
        forward = true;
        counter = 0;
    }

    public void Run()
    {
        if (ability == null)
            return;
        switch (ability.abilitytype)
        {
            case AbilityType.Fuel: RunFuel(); return;
            case AbilityType.Direction: RunDirection(); return;
            case AbilityType.Jump: RunJump(); return;
            case AbilityType.Blink: RunBlink(); return;
        }
    }
    private void RunFuel()
    {

    }
    private void RunDirection()
    {

    }
    private void RunJump()
    {

    }
    private void RunBlink()
    {

    }
    public bool IsEmpty()
    {
        if (ability == null)
            return true;
        return false;
    }

    public override bool CanMove(UnitType unittype)
    {
        if (unittype == UnitType.Box || unittype == UnitType.Player)
            return true;
        return false;
    }

    public CloneableContainer Clone()
    {
        return CloneableContainer.Clone(this);
    }
}

public class CloneableContainer : CloneableUnit
{
    public Ability ability;
    public Ability _lastAbility { get; set; }

    public bool forward { get; set; }
    public int counter { get; set; }
    public int number;
    public static CloneableContainer Clone(Container container)
    {
        CloneableContainer c = new CloneableContainer();
        CloneableUnit.init(container, c);
        c.number = container.number;
        c.ability = container.ability;
        c._lastAbility = container._lastAbility;
        c.forward = container.forward;
        c.counter = container.counter;
        return c;
    }
}

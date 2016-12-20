using UnityEngine;
using System.Collections.Generic;

public class Container : Unit{
    public int number;
    public Direction direction;
    public Ability ability;
    public List<Unit> units;
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
        for(int i=0; i<number; i++)
        {
            bool flag = true;
            for(int j=0; j< units.Count; j++)
            {
                //moshkel dare
                if (!Toolkit.IsEmptySpace(units[j].transform.position, direction))
                    flag = false;
            }
            if (flag)
            {
                for (int j = 0; j < units.Count; j++)
                {
                    engine.moveObject.MoveObjects(units[j], direction, 1);
                }
            }
        }
    }
    private void RunDirection()
    {
        Starter.GetEngine().action.ChangeDirection();
    }
    private void RunJump()
    {
        Starter.GetEngine().ContainerJump(ability.number * 2);
    }
    private void RunBlink()
    {
        Starter.GetEngine().ContainerBlink(this);
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

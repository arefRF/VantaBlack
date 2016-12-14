using UnityEngine;
using System.Collections;
using System;

public class BlockSwitch : Unit {

    public Ability ability;
    public Direction direction;
    public int number;
    // Use this for initialization
    void Awake () {
        unitType = UnitType.BlockSwitch;
        obj = this.gameObject;
        position = gameObject.transform.position;
        layer = 1;
    }
	
	// Update is called once per frame
	void Update () {

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
    public override bool CanMove(UnitType unittype)
    {
        if (unittype == UnitType.Box || unittype == UnitType.Player)
            return true;
        return false;
    }

    public CloneableBlockSwitch Clone()
    {
        return CloneableBlockSwitch.Clone(this);
    }
}

public class CloneableBlockSwitch : CloneableUnit
{
    public static CloneableBlockSwitch Clone(BlockSwitch blockswitch)
    {
        CloneableBlockSwitch b = new CloneableBlockSwitch();
        CloneableUnit.init(blockswitch, b);
        
        return b;
    }
}


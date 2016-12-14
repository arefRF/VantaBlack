using UnityEngine;
using System.Collections.Generic;

public class Switch : Unit
{

    public Direction direction;
    public bool singlestate;
    public bool isOn;

    public bool isAutomatic;

    public bool disabled;

    public int counter { get; set; }

    public List<Switch> parentswitches { get; set; }

    private bool tempon;

    public bool isbusy { get; set; }
    
    // Use this for initialization
    public void Awake()
    {
        unitType = UnitType.Switch;
        obj = this.gameObject;
        position = gameObject.transform.position;
        movable = false;
        layer = 2;
        isbusy = false;
        counter = 0;
        
    }

    public override bool CanMove(UnitType unittype)
    {

        return false;
    }
    public bool Run()
    {
        
        return true;
    }

    public CloneableSwitch Clone()
    {
        return CloneableSwitch.Clone(this);
    }
}
public class CloneableSwitch : CloneableUnit
{
    public bool singlestate;
    public bool isOn;
    public bool isAutomatic;
    public bool disabled;
    public int counter;

    public static CloneableSwitch Clone(Switch sw)
    {
        CloneableSwitch s = new CloneableSwitch();
        CloneableUnit.init(sw, s);
        s.singlestate = sw.singlestate;
        s.isOn = sw.isOn;
        s.isAutomatic = sw.isAutomatic;
        s.disabled = sw.disabled;
        s.counter = sw.counter;
        return s;
    }
}
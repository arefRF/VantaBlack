using UnityEngine;
using System.Collections;
using System;

public class Ramp : Unit {
    Direction direction;

    // Use this for initialization
    void Awake()
    {
        unitType = UnitType.Ramp;
        obj = this.gameObject;
        position = gameObject.transform.position;
        movable = true;
        layer = 3;
    }

    // Update is called once per frame
    void Update () {
	
	}

    public override bool CanMove(UnitType unittype)
    {
        return false;
    }
}

using UnityEngine;
using System.Collections;
using System;

public class GoalUnit : Unit {
    

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public override bool CanMove(UnitType unittype)
    {
        return false;
    }
}

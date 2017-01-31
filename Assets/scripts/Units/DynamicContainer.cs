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
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public override bool PlayerMoveInto(Direction dir)
    {
        return false;
    }
}

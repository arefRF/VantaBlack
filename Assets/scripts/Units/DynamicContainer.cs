using UnityEngine;
using System.Collections;
using System;

public class DynamicContainer : FunctionalContainer {
    

    // Use this for initialization
    void Start () {
        moved = abilities.Count;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public override bool PlayerMoveInto(Direction dir)
    {
        return false;
    }
}

using UnityEngine;
using System.Collections;

public class Box : Unit {

	// Use this for initialization
	void Start () {
        unitType = UnitType.Box;
        obj = this.gameObject;
        position = gameObject.transform.position;
	}
	
	// Update is called once per frame
    
    public override bool PlayerMoveInto(Direction dir)
    {
        return false;
    }



}


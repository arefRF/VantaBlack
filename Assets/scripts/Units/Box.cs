using UnityEngine;
using System.Collections;

public class Box : Unit {

	// Use this for initialization
	void Start () {
        unitType = UnitType.Box;
        obj = this.gameObject;
        position = gameObject.transform.position;
        movable = true;
        CanBeMoved = true;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public override bool CanMove(UnitType unittype)
    {
        if (unittype == UnitType.Box)
            return true;
        return false;
    }

    public CloneableBox Clone()
    {
        return CloneableBox.Clone(this);
    }
}

public class CloneableBox: CloneableUnit
{

    public static CloneableBox Clone(Box box)
    {
        CloneableBox b = new CloneableBox();
        CloneableUnit.init(box, b);
        return b;
    }
}

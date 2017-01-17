using UnityEngine;
using System.Collections;
using System;

public class SimpleContainer : Container {

    public override bool PlayerMoveInto(Direction dir)
    {
        return false;
    }

    
}

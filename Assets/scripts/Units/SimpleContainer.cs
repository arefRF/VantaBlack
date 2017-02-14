using UnityEngine;
using System.Collections;
using System;

public class SimpleContainer : Container {

    public override bool PlayerMoveInto(Direction dir)
    {
        return false;
    }

    public override CloneableUnit Clone()
    {
        return new CloneableSimpleContainer(this);
    }
}

public class CloneableSimpleContainer : CloneableUnit
{
    public CloneableSimpleContainer(SimpleContainer container) : base(container.position)
    {
        original = container;
    }
}

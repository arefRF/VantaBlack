using UnityEngine;
using System.Collections.Generic;
using System;

public class Rock : Unit
{
    public override CloneableUnit Clone()
    {
        return new CloneableRock(this);
    }
}


public class CloneableRock : CloneableUnit
{
    public CloneableRock(Rock rock) : base(rock.position)
    {
        original = rock;
    }
}
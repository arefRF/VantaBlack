using UnityEngine;
using System.Collections;

public class Branch : Unit {


    public override bool PlayerMoveInto(Direction dir)
    {
        return true;
    }

    public override CloneableUnit Clone()
    {
        return new CloneableBranch(this);
    }

}

public class CloneableBranch : CloneableUnit
{
    public CloneableBranch(Branch branch) : base(branch.position)
    {
        original = branch;
    }
}

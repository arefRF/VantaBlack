using UnityEngine;
using System.Collections;

public abstract class ParentContainer : Unit {



    public abstract void Action(Player player, Direction dir);

    public override bool isLeanable()
    {
        return true;
    }
}

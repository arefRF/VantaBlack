using UnityEngine;
using System.Collections.Generic;

public class Leanable : Container {

    public bool canLean = true;
    public override void PlayerAbsorb(Player player)
    {
        
    }

    public override void PlayerAbsorbHold(Player player)
    {
        
    }

    public virtual void LeanedAction(Player player, Direction direction)
    {

    }
}

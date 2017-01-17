using UnityEngine;
using System.Collections;

public class Container : Block {
    public Direction direction;
    public Ability ability { get; set; }

    public override bool MoveInto(Direction dir)
    {
        return false;
    }
}

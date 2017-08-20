using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class PlayerGraphicsV5 : PlayerGraphics
{
    public override void MoveToBranch(Direction dir)
    {
        StopAllCoroutines();
        StartCoroutine(Simple_Move(player.position, 0.3f, true));
    }
}

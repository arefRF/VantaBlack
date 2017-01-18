using UnityEngine;
using System.Collections;

public class Gate : Container {

    public string sceneName;
    public int capacity;
    public bool linked;



    public override bool PlayerMoveInto(Direction dir)
    {
        return false;
    }
}

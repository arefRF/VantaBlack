using UnityEngine;
using System.Collections;

public class StaticContainer : FunctionalContainer {

    public void Start()
    {
        moved = 0;
        shouldmove = abilities.Count;
    }
}

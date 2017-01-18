using UnityEngine;
using System.Collections;

public class APIInput{

    LogicalEngine engine;

    public void MovePressed(Direction dir)
    {
        Debug.Log(dir);
        engine.Input_Move(dir);
    }
}

using UnityEngine;
using System.Collections;

public class APIInput{

    LogicalEngine engine;

    public APIInput()
    {
        engine = Starter.GetEngine();
    }

    public void MovePressed(Direction dir)
    {
        Debug.Log(dir);
        engine.Input_Move(dir);
    }
}

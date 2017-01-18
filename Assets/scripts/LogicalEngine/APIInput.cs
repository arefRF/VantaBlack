using UnityEngine;
using System.Collections;

public class APIInput{

    LogicalEngine engine;

    public APIInput(LogicalEngine engine)
    {
        this.engine = engine;
    }

    public void MovePressed(Direction dir)
    {
        engine.Input_Move(dir);
    }
}

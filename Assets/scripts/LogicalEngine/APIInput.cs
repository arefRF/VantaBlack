using UnityEngine;
using System.Collections;

public class APIInput{

    LogicalEngine engine;
    public GetInput input { get; set; }
    
  
    
    public APIInput(LogicalEngine engine)
    {
        this.engine = engine;
    }

    public void MovePressed(Direction dir)
    {
        engine.Input_Move(dir);
    }

    public void PlayerMoveFinished()
    {
        input.Player_Move_Finished();
    }

    public void PlayerMoveStarted()
    {
        input.Player_Move_Started();
    }

    public void ArrowRelease(Direction dir)
    {
        engine.ArrowkeyReleased(dir);
    }

    public void AbsorbRelease(Direction dir)
    {
        engine.Input_AbsorbRlease(dir);
    }
}

using UnityEngine;
using System.Collections;

public class APIInput{

    LogicalEngine engine;
    public GetInput input { get; set; }
    
  
    public void Action_Key()
    {
        engine.ActionKeyPressed();
    }

    public void Action_Key(Direction dir)
    {
        engine.ActionKeyPressed(dir);
    }
    
    public APIInput(LogicalEngine engine)
    {
        this.engine = engine;
    }

    public void MovePressed(Direction dir)
    {
        engine.Input_Move(dir);
    }

    public void UndoPressed()
    {
        engine.Undo();
    }

    public void ArrowRelease(Direction dir)
    {
        engine.inputcontroller.ArrowkeyReleased(dir);
    }

    public void AbsorbRelease(Direction dir)
    {
        
    }

    public void Absorb()
    {
        engine.inputcontroller.Absorb();
        engine.pipecontroller.CheckPipes();
    }

    public void Release()
    {
        engine.inputcontroller.Release();
        engine.pipecontroller.CheckPipes();
    }

    public void Absorb_Hold()
    {
        engine.inputcontroller.AbsorbHold();
        engine.pipecontroller.CheckPipes();
    }

    public void Release_Hold()
    {
        engine.inputcontroller.ReleaseHold();
        engine.pipecontroller.CheckPipes();
    }

    public void AbsorbReleaseHold(Direction dir)
    {
       
    }
}

using UnityEngine;
using System.Collections;

public class APIInput{

    LogicalEngine engine;
    public GetInput input { get; set; }
    
  
    public bool Action_Key()
    {
        engine.ActionKeyPressed();
        return true;
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
        engine.ArrowkeyReleased(dir);
    }

    public void AbsorbRelease(Direction dir)
    {
        
    }

    public void Absorb()
    {
        engine.inputcontroller.Absorb();
    }

    public void Release()
    {
        engine.inputcontroller.Release();
    }

    public void Absorb_Hold()
    {
        engine.inputcontroller.AbsorbHold();
    }

    public void Release_Hold()
    {
        engine.inputcontroller.ReleaseHold();
    }

    public void AbsorbReleaseHold(Direction dir)
    {
       
    }
    public void ContainerAction()
    {
        engine.ActionKeyPressed();
    }

    public void ContainerActionFinished()
    {

    }
}

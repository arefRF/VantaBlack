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
        
    }
    
    public APIInput(LogicalEngine engine)
    {
        this.engine = engine;
    }

    public void MovePressed(Direction dir)
    {
        engine.Input_Move(dir);
    }

    public void ArrowRelease(Direction dir)
    {
        engine.ArrowkeyReleased(dir);
    }

    public void AbsorbRelease(Direction dir)
    {
        engine.Input_AbsorbRlease(dir);
    }

    public void AbsorbReleaseHold(Direction dir)
    {
        engine.Input_AbsorbRleaseHold(dir);
    }
    public void ContainerAction()
    {
        engine.ActionKeyPressed();
    }

    public void ContainerActionFinished()
    {

    }
}

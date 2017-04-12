using UnityEngine;
using System.Collections;

public class APIInput{

    LogicalEngine engine;
    public GetInput input { get; set; }
    private GameMode mode = GameMode.Play;
    private Teleport portal;
    public void Action_Key()
    {
        if (mode == GameMode.Play)
            engine.ActionKeyPressed();
        else if (mode == GameMode.Portal)
            portal.Port();
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
        if (mode == GameMode.Play)
            engine.Input_Move(dir);
        else if (mode == GameMode.Portal)
            portal.ArrowKeyPressed(dir);
    }

    public void UndoPressed()
    {
        engine.Undo();
    }

    public void ArrowRelease(Direction dir)
    {
        if(mode == GameMode.Play)
            engine.inputcontroller.ArrowkeyReleased(dir);
    }

    public void AbsorbRelease(Direction dir)
    {
        
    }

    public void Absorb()
    {
        if (mode == GameMode.Play)
        {
            engine.inputcontroller.Absorb();
            engine.pipecontroller.CheckPipes();
        }
    }

    public void Release()
    {
        if (mode == GameMode.Play)
        {
            engine.inputcontroller.Release();
            engine.pipecontroller.CheckPipes();
        }
    }

    public void Absorb_Hold()
    {
        if (mode == GameMode.Play)
        {
            engine.inputcontroller.AbsorbHold();
            engine.pipecontroller.CheckPipes();
        }
    }

    public void Release_Hold()
    {
        if (mode == GameMode.Play)
        {
            engine.inputcontroller.ReleaseHold();
            engine.pipecontroller.CheckPipes();
        }
    }

    public void AbsorbReleaseHold(Direction dir)
    {
       
    }

    private void SetInput()
    {
        input = GameObject.Find("GetInput").GetComponent<GetInput>();
    }
    public void EnterPortalMode(Teleport tp)
    {
        portal = tp;
        if (input == null)
            SetInput();
        input.getOnce = true;
        mode = GameMode.Portal;
    }

    public void QuitPortalMode()
    {
        input.getOnce = false;
        mode = GameMode.Play;
    }
    enum GameMode
    {
        Play,Portal,Menu
    }
}

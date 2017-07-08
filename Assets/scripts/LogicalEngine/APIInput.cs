using UnityEngine;
using System.Collections;

public class APIInput{

    LogicalEngine engine;
    public GetInput input { get; set; }
    private GameMode mode = GameMode.Play;
    private Teleport portal;
    public bool leanlock;
    public void Action_Key(bool KeyUp)
    {
        if (mode == GameMode.Play)
            engine.ActionKeyPressed(KeyUp);
        else if (mode == GameMode.Portal)
            portal.Port();
    }

    public void Jump()
    {
        engine.JumpKeyprssed();
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
        else if (mode == GameMode.Real)
        {
            for (int i = 0; i < engine.database.player.Count; i++)
            {
                Debug.Log("real mode have bug click here!!!!");
                /*if (engine.database.player[i].state == PlayerState.Transition)
                {
                    engine.inputcontroller.RealModePlayerTransitionMove(engine.database.player[i], dir);
                }
                else
                {
                    engine.database.player[i].GetComponent<PlayerGraphics>().Move_Animation(dir);
                }*/
            }
        }
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

    public void SetMode(GameMode newMode)
    {
        if (newMode == GameMode.Portal)
            throw new System.Exception("Do not change to protal mode with this function");
        else if (mode == GameMode.Portal)
            throw new System.Exception("Portal mode should not change from here");
        mode = newMode;
    }

    public bool isFunctionKeyDown()
    {
        return input.isFunctionKeyDown();
    }

    public bool isArrowKeyDown(Direction direction)
    {
        return input.isArrowKeyDown(direction);
    }

    public bool isAnyArrowKeyDown()
    {
        return input.isAnyArrowKeyDown();
    }

    public Direction GetArrowKeyDown()
    {
        return input.GetArrowKeyDown();
    }

    public void Zoom(float zoom)
    {
        if (zoom < 0)
        {
            Camera.main.orthographicSize -= zoom;
        }
        else if (zoom > 0)
        {
            Camera.main.orthographicSize -= zoom;
        }
    }
}

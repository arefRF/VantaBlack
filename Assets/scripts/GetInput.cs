using UnityEngine;
using System.Collections;

public class GetInput : MonoBehaviour {
    private LogicalEngine engine;
    private static LogicalEngine staticengine;

    Database database;

    public APIInput api;
    private Direction lean_direction;
    private bool is_space;
    private bool is_holding;
    private Direction hold_direction;
    private Direction lean;
    // check to call for lean undo once
    private bool move_input;
    private bool ar_input = false;
    private bool action_lock = false;
    public bool joystick;
    public bool getOnce = false;
    private Coroutine last_co;
    // Use this for initialization
    void Start()
    {
        database = Starter.GetDataBase();
        engine = Starter.GetEngine();
        is_space = false;
        api = engine.apiinput;
        api.input = this;
    }
    // Update is called once per frame
    void Update()
    {
        if (joystick)
            Get_JoyStick();
        else
            Get_Keyboard();
    }


    private void Get_Keyboard()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            GameObject.Find("UI").GetComponent<Get>().inMenu_Show();

        Get_Lean_Undo();
        // Directional Abilities use
        if (Input.GetKeyUp(KeyCode.Space))
        {
            api.Action_Key(true);
            is_space = false;
        }
        if (is_space)
            Get_Space_Arrows();
        else
        {
            //risky
            Get_Move();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            api.Action_Key(false);
            is_space = true;
        }
        /*if (Input.GetKeyDown(KeyCode.Space))
        {
            api.Jump();
        }*/
        if (is_space)
            Get_Space_Arrows();

        /*if (Input.GetKeyDown(KeyCode.Q))
        {
            is_holding = true;
            if (last_co != null)
                StopCoroutine(last_co);
            last_co = StartCoroutine(Wait_For_Absorb_Hold());
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            is_holding = true;
            if (last_co != null)
                StopCoroutine(last_co);
            StartCoroutine(Wait_For_Release_Hold());
        }*/
        /*if (Input.GetKeyUp(KeyCode.E))
        {
            api.Release();
            is_holding = false;
        }
        if (Input.GetKeyUp(KeyCode.Q))
        {
            api.Absorb();
            is_holding = false;
        }*/
        if (Input.GetKey(KeyCode.Equals))
        {
            api.Zoom(0.1f);
        }
        if (Input.GetKey(KeyCode.Minus))
        {
            api.Zoom(-0.1f);
        }
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            api.ShowHologram();
        }
        if (Input.GetKeyUp(KeyCode.Tab))
        {
            api.HideHologram();
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            api.AbsorbReleaseController(Direction.Up);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            api.AbsorbReleaseController(Direction.Right);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            api.AbsorbReleaseController(Direction.Left);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            api.AbsorbReleaseController(Direction.Down);
        }
    }

    private void Get_JoyStick()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            GameObject.Find("UI").GetComponent<Get>().inMenu_Show();
        // Joy Stick Move
        Get_Joy_Move();

        //Joystick absorb release action and jump
        Get_Joy_AR();
    }



    // Absorb Release and jump
    private void Get_Joy_AR()
    {
        if (Input.GetButtonDown("Absorb"))
            api.Absorb();
        else if (Input.GetButtonDown("Release"))
            api.Release();


        if (Input.GetButtonDown("Jump"))
            api.Jump();
        if (Input.GetButtonDown("Action"))
        {
            api.Action_Key(false);
            is_space = true;
        }
        if (Input.GetButtonUp("Action"))
        {
            api.Action_Key(true);
            is_space = false;
        }
        
        
    }


    private IEnumerator Wait_For_AR_Hold_Joy()
    {
       
        yield return new WaitForSeconds(0.5f);
        if (is_holding)
            api.AbsorbReleaseHold(hold_direction);
        is_holding = false;

    }
    private void Get_Joy_Move()
    {
        if (Input.GetAxis("Horizontal") > 0.9)
        {
            api.MovePressed(Direction.Right);
            lean = Direction.Right;
            move_input = true;
        }
        else if (Input.GetAxis("Horizontal") < -0.9)
        {
            api.MovePressed(Direction.Left);
            lean = Direction.Left;
            move_input = true;
        }
        else if (Input.GetAxis("Vertical") > 0.9)
        {
            api.MovePressed(Direction.Up);
            lean = Direction.Up;
            move_input = true;
        }
        else if (Input.GetAxis("Vertical") < -0.9)
        {
            api.MovePressed(Direction.Down);
            lean = Direction.Down;
            move_input = true;
        }
        else if (move_input)
        {
            api.ArrowRelease(lean);
            move_input = false;
        }
    }
    // this is responsibile for Absorb and Release hold
    private IEnumerator Wait_For_Absorb_Hold()
    {
        yield return new WaitForSeconds(0.5f);
        if (is_holding)
        {
            api.Absorb_Hold();
            is_holding = false;
        }
    }

    private IEnumerator Wait_For_Release_Hold()
    {
        yield return new WaitForSeconds(0.5f);
        if (is_holding)
        {
            api.Release_Hold();
            is_holding = false;
        }
    }
    // Get Arrows if Ability needs it
    private void Get_Space_Arrows()
    {
        if (Input.GetKeyDown(KeyCode.D))
           api.Action_Key(Direction.Right);
        else if (Input.GetKeyDown(KeyCode.A))
            api.Action_Key(Direction.Left);
        else if (Input.GetKeyDown(KeyCode.S))
            api.Action_Key(Direction.Down);
        else if (Input.GetKeyDown(KeyCode.W))
            api.Action_Key(Direction.Up);
    }

    private void Get_Lean_Undo()
    {
        if (Input.GetKeyUp(KeyCode.D))
            api.ArrowRelease(Direction.Right);
        else if (Input.GetKeyUp(KeyCode.A))
            api.ArrowRelease(Direction.Left);
        else if (Input.GetKeyUp(KeyCode.S))
            api.ArrowRelease(Direction.Down);
        else if (Input.GetKeyUp(KeyCode.W))
            api.ArrowRelease(Direction.Up);
    }

    private void Get_Ability()
    {

    }
    private void Get_Move()
    {
        if (getOnce)
        {
            if (Input.GetKeyDown(KeyCode.D))
                api.MovePressed(Direction.Right);
            if (Input.GetKeyDown(KeyCode.A))
                api.MovePressed(Direction.Left);
            if (Input.GetKeyDown(KeyCode.S))
                api.MovePressed(Direction.Down);
            if (Input.GetKeyDown(KeyCode.W))
                api.MovePressed(Direction.Up);
        }
        else
        {
            //Take Arrows to move or lean
            if (Input.GetKey(KeyCode.D))
                api.MovePressed(Direction.Right);
            if (Input.GetKey(KeyCode.A))
                api.MovePressed(Direction.Left);
            if (Input.GetKey(KeyCode.S))
                api.MovePressed(Direction.Down);
            if (Input.GetKey(KeyCode.W))
                api.MovePressed(Direction.Up);
        }
    }

    private void Get_Move_Once()
    {
        if (Input.GetKeyDown(KeyCode.D))
            api.MovePressed(Direction.Right);
        if (Input.GetKeyDown(KeyCode.A))
            api.MovePressed(Direction.Left);
        if (Input.GetKeyDown(KeyCode.S))
            api.MovePressed(Direction.Down);
        if (Input.GetKeyDown(KeyCode.W))
            api.MovePressed(Direction.Up);
    }

    private void Set_Camera(Vector3 pos)
    {
        pos = Toolkit.VectorSum(Camera.main.transform.position, pos);
        pos.z = -10;
        Camera.main.transform.position = pos;
    }

    public bool isFunctionKeyDown()
    {
        return Input.GetKey(KeyCode.Space);
    }

    public bool isMoveKeyDown(Direction direction)
    {
        switch (direction)
        {
            case Direction.Up: return Input.GetKey(KeyCode.W);
            case Direction.Right: return Input.GetKey(KeyCode.D);
            case Direction.Left: return Input.GetKey(KeyCode.A);
            case Direction.Down: return Input.GetKey(KeyCode.S);
            default: return false;
        }
    }

    public bool isAnyKeyDown()
    {
        return (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S));
    }

    public Direction GetMoveKeyDown()
    {
        if (Input.GetKey(KeyCode.W))
            return Direction.Up;
        if (Input.GetKey(KeyCode.D))
            return Direction.Right;
        if (Input.GetKey(KeyCode.A))
            return Direction.Left;
        if (Input.GetKey(KeyCode.S))
            return Direction.Down;
        Debug.Log("error error big error");
        return Direction.Up;
    }

    public int MoveKeysDownCount()
    {
        int count = 0;
        if (Input.GetKey(KeyCode.W))
            count++;
        if (Input.GetKey(KeyCode.D))
            count++;
        if (Input.GetKey(KeyCode.A))
            count++;
        if (Input.GetKey(KeyCode.S))
            count++;
        return count;
    }
}

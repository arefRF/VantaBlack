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
        {
            // Joy Stick Move
            Get_Joy_Move();

            //Joystick absorb release
            Get_Joy_AR();
            // Lean Keys Up
            Get_Action_Joy();
            if (Input.GetKeyDown(KeyCode.Joystick1Button4))
                api.UndoPressed();

        }
        else
        {

           Get_Lean_Undo();

            // Directional Abilities use
            if (Input.GetKeyUp(KeyCode.Space))
                is_space = false; 
            Get_Move();
                  if (Input.GetKeyDown(KeyCode.Space))
                  {
                      if (!api.Action_Key())
                          is_space = true;
                  }
                  if (is_space)
                      Get_Space_Arrows();
                  if (Input.GetKeyDown(KeyCode.A))
                  {
                      is_holding = true;
                      StopAllCoroutines();
                      StartCoroutine(Wait_For_Absorb_Hold());
                  }
                  if (Input.GetKeyDown(KeyCode.D))
                  {
                      is_holding = true;
                      StopAllCoroutines();
                      StartCoroutine(Wait_For_Release_Hold());
                  }
                  if (Input.GetKeyUp(KeyCode.D)  )
                  {
                      api.Release();
                      is_holding = false;
                  }
                  if(Input.GetKeyUp(KeyCode.A))
            {
                api.Absorb();
                is_holding = false;
            }
                  if (Input.GetKeyUp(KeyCode.R))
                  {
                      api.UndoPressed();
                  }
              
        }
    }



    private void Get_Action_Joy()
    {
        if (Mathf.Abs(Input.GetAxis("Action")) > 0.5f)
        {
            if (!action_lock)
            {
                action_lock = true;
                api.Action_Key();
            }
        }
        else if (Input.GetAxis("Action") == 0)
            action_lock = false;
    }
    private void Get_Joy_AR()
    {
        if (Input.GetAxis("AR-H") == 1)
        {
            if (!ar_input)
            {
                is_holding = true;
                hold_direction = Direction.Right;
                StopAllCoroutines();
                StartCoroutine(Wait_For_AR_Hold_Joy());
                ar_input = true;
            }
        }
        else if (Input.GetAxis("AR-H") == -1)
        {
            if (!ar_input)
            {
                is_holding = true;
                hold_direction = Direction.Left;
                StopAllCoroutines();
                StartCoroutine(Wait_For_AR_Hold_Joy());
                ar_input = true;
            }
        }
        else if (Input.GetAxis("AR-V") == 1)
        {
            if (!ar_input)
            {
                is_holding = true;
                hold_direction = Direction.Down;
                StopAllCoroutines();
                StartCoroutine(Wait_For_AR_Hold_Joy());
                ar_input = true;
            }
        }
        else if (Input.GetAxis("AR-V") == -1)
        {
            if (!ar_input)
            {
                is_holding = true;
                hold_direction = Direction.Up;
                StopAllCoroutines();
                StartCoroutine(Wait_For_AR_Hold_Joy());
                ar_input = true;
            }
        }
        else if (ar_input)
        {
            api.AbsorbRelease(hold_direction);
            ar_input = false;
            is_holding = false;
        }
        else
            is_holding = false;
        
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
        if (Input.GetAxis("Horizontal") == 1)
        {
            api.MovePressed(Direction.Right);
            lean = Direction.Right;
            move_input = true;
        }
        else if (Input.GetAxis("Horizontal") == -1)
        {
            api.MovePressed(Direction.Left);
            lean = Direction.Left;
            move_input = true;
        }
        else if (Input.GetAxis("Vertical") == 1)
        {
            api.MovePressed(Direction.Up);
            lean = Direction.Up;
            move_input = true;
        }
        else if (Input.GetAxis("Vertical") == -1)
        {
            api.MovePressed(Direction.Down);
            lean = Direction.Left;
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
        if (Input.GetKeyDown(KeyCode.RightArrow))
           api.Action_Key(Direction.Right);
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
            api.Action_Key(Direction.Left);
        else if (Input.GetKeyDown(KeyCode.DownArrow))
            api.Action_Key(Direction.Down);
        else if (Input.GetKeyDown(KeyCode.UpArrow))
            api.Action_Key(Direction.Up);
    }

    private void Get_Lean_Undo()
    {
        if (Input.GetKeyUp(KeyCode.RightArrow))
            api.ArrowRelease(Direction.Right);
        else if (Input.GetKeyUp(KeyCode.LeftArrow))
            api.ArrowRelease(Direction.Left);
        else if (Input.GetKeyUp(KeyCode.DownArrow))
            api.ArrowRelease(Direction.Down);
        else if (Input.GetKeyUp(KeyCode.UpArrow))
            api.ArrowRelease(Direction.Up);
    }

    private void Get_Ability()
    {

    }
    private void Get_Move()
    {

        //Take Arrows to move or lean
        if (Input.GetKey(KeyCode.RightArrow))
            api.MovePressed(Direction.Right);

        if (Input.GetKey(KeyCode.LeftArrow))
            api.MovePressed(Direction.Left);

        if (Input.GetKey(KeyCode.DownArrow))
            api.MovePressed(Direction.Down);
        if (Input.GetKey(KeyCode.UpArrow))
            api.MovePressed(Direction.Up);
    }


    private void Set_Camera(Vector3 pos)
    {

        pos = Toolkit.VectorSum(Camera.main.transform.position, pos);
        pos.z = -10;
        Camera.main.transform.position = pos;

    }
}

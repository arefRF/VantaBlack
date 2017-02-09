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
        // Lean Keys Up
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
        if (Input.GetKeyDown(KeyCode.W))
        {
            is_holding = true;
            hold_direction = Direction.Up;
            StopAllCoroutines();
            StartCoroutine(Wait_For_Absorb_Hold());
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            is_holding = true;
            hold_direction = Direction.Down;
            StopAllCoroutines();
            StartCoroutine(Wait_For_Absorb_Hold());
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            hold_direction = Direction.Left;
            is_holding = true;
            StopAllCoroutines();
            StartCoroutine(Wait_For_Absorb_Hold());
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            is_holding = true;
            hold_direction = Direction.Right;
            StopAllCoroutines();
            StartCoroutine(Wait_For_Absorb_Hold());
        }
        if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.S))
        {
            api.AbsorbRelease(hold_direction);
            is_holding = false;
        }
        if (Input.GetKeyUp(KeyCode.R))
        {
            api.UndoPressed();
        }
    }

    // this is responsibile for Absorb and Release hold
    private IEnumerator Wait_For_Absorb_Hold()
    {
        yield return new WaitForSeconds(0.5f);
        if (is_holding)
        {
            api.AbsorbReleaseHold(hold_direction);
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

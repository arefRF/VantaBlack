using UnityEngine;
using System.Collections;

public class GetInput : MonoBehaviour {
    private LogicalEngine engine;
    private static LogicalEngine staticengine;

    Database database;

    public APIInput api;
    private Direction lean_direction;
    private float camera_speed = 0.05f;
    private bool is_space;
    private bool is_walking;
    // Use this for initialization
    void Start()
    {
        database = Starter.GetDataBase();
        engine = Starter.GetEngine();
        is_space = false;
        is_walking = false;
        api = engine.apiinput;
        api.input = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (database.state == State.Idle)
        {
                // Absorb or Static Container or Undo Lean
                Get_Lean_Undo();
                // if (Input.GetKeyDown(KeyCode.A))
                //engine.Absorb(lean_direction);
                // else if (Input.GetKeyDown(KeyCode.Space))
                // engine.UseContainerBlockSwitch(lean_direction);

            
            if (is_space)
            {
                // Directional Abilities use
                if (Input.GetKeyUp(KeyCode.Space))
                    is_space = false;

            }
            else
            {
                if (!is_walking)
                    Get_Move();
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    if (!api.Action_Key())
                        is_space = true;
                }
                if (is_space)
                    Get_Space_Arrows();
                if (Input.GetKeyDown(KeyCode.W))
                    api.AbsorbRelease(Direction.Up);
                if (Input.GetKeyDown(KeyCode.S))
                    api.AbsorbRelease(Direction.Down);
                if (Input.GetKeyDown(KeyCode.A))
                    api.AbsorbRelease(Direction.Left);
                if (Input.GetKeyDown(KeyCode.D))
                    api.AbsorbRelease(Direction.Right);
            }

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

    public void Player_Move_Finished()
    {
        is_walking = false;
    }

    public void Player_Move_Started()
    {
        is_walking = true;
    }
}

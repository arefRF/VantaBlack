using UnityEngine;
using System.Collections.Generic;

public class Interface : MonoBehaviour {
    
    private LogicalEngine engine;
    private static LogicalEngine staticengine;
    
    Database database;

    public APIInput api;
    private Direction lean_direction;
    private float camera_speed = 0.05f;
    private bool is_lean;
    private bool is_space;
    private bool is_walking;
	// Use this for initialization
	void Start () {
        database = Starter.GetDataBase();
        engine = Starter.GetEngine();        
        is_lean = false;
        is_space = false;
        is_walking = false;
    }
	
	// Update is called once per frame
	void Update () {
        if (database.state == State.Idle)
        {
            if (is_lean)
            {
                // Absorb or Static Container or Undo Lean
                Get_Lean_Undo();
               // if (Input.GetKeyDown(KeyCode.A))
                    //engine.Absorb(lean_direction);
               // else if (Input.GetKeyDown(KeyCode.Space))
                   // engine.UseContainerBlockSwitch(lean_direction);

            }
            else if (is_space)
            {
                // Directional Abilities use
                if (Input.GetKeyUp(KeyCode.Space))
                    is_space = false;

            }
            else
            {
                if(!is_walking)
                    Get_Move();
                if (Input.GetKeyDown(KeyCode.Space))
                    //if (!engine.SpaceKeyPressed())
                        is_space = true;
            }
           
        }
    }

    // Get Arrows if Ability needs it
    private void Get_Space_Arrows()
    {
        /*
        if (Input.GetKeyDown(KeyCode.RightArrow))
            engine.SpaceKeyPressed(Direction.Right);
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
            engine.SpaceKeyPressed(Direction.Left);
        else if (Input.GetKeyDown(KeyCode.DownArrow))
            engine.SpaceKeyPressed(Direction.Down);
        else if (Input.GetKeyDown(KeyCode.UpArrow))
            engine.SpaceKeyPressed(Direction.Up);
            */
    }

    private void Get_Lean_Undo()
    {
        if (Input.GetKeyUp(KeyCode.RightArrow) && lean_direction == Direction.Right)
            Undo_Lean();
        else if (Input.GetKeyUp(KeyCode.LeftArrow) && lean_direction == Direction.Left)
            is_lean = false;
        else if (Input.GetKeyUp(KeyCode.DownArrow) && lean_direction == Direction.Down)
            is_lean = false;
        else if (Input.GetKeyUp(KeyCode.UpArrow) && lean_direction == Direction.Up)
            is_lean = false;
    }

    private void Undo_Lean()
    {
        is_lean = false;
    }
   
    private void Lean()
    {
        Debug.Log("Leaning");
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




    private bool _lean_action()
    {
        if(is_lean)
        {
            //engine.SwitchAction(lean_direction);
            return true;
        }
        return false;
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
    private bool _lean_absorb()
    {
        if(is_lean)
        {
           // engine.Absorb(lean_direction);
            return true;
        }
        return false;
    }
}

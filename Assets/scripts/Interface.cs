using UnityEngine;
using System.Collections.Generic;

public class Interface : MonoBehaviour {
    
    private LogicalEngine engine;
    private static LogicalEngine staticengine;
    
    Database database;
    
    private Direction lean_direction;
    private float camera_speed = 0.05f;
    private bool is_lean;
    private bool ability_use;
	// Use this for initialization
	void Start () {
        database = Starter.GetDataBase();
        engine = Starter.GetEngine();        
        is_lean = false;
        ability_use = false;
    }
	
	// Update is called once per frame
	void Update () {
        if (database.state == State.Idle)
        {
            if (!ability_use)
                Get_Arrows();
            else
                Get_Ability();
        }
    }



    private void Lean()
    {
        Debug.Log("Leaning");
    }

    private void Get_Ability()
    {

    }
    private void Get_Arrows()
    {

        //Take Arrows

        if (Input.GetKeyDown(KeyCode.RightArrow))
            if (!engine.ArrowKeyPressed(Direction.Right))
            {
                is_lean = true;
                lean_direction = Direction.Right;
                Lean();
            }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
            if (!engine.ArrowKeyPressed(Direction.Left))
            {
                is_lean = true;
                lean_direction = Direction.Left;
                Lean();
            }

        if (Input.GetKeyDown(KeyCode.DownArrow))
            if (!engine.ArrowKeyPressed(Direction.Down))
            {
                is_lean = true;
                lean_direction = Direction.Up;
                Lean();
            }
        if (Input.GetKeyDown(KeyCode.UpArrow))
            if (!engine.ArrowKeyPressed(Direction.Up))
            {
                is_lean = true;
                lean_direction = Direction.Up;
                Lean();
            }
    }




    private bool _lean_action()
    {
        if(is_lean)
        {
            engine.SwitchAction(lean);
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
    private bool _lean_absorb()
    {
        if(is_lean)
        {
            engine.Absorb(lean);
            return true;
        }
        return false;
    }
}

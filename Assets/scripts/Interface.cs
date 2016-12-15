using UnityEngine;
using System.Collections.Generic;

public class Interface : MonoBehaviour {
    
    private LogicalEngine engine;
    private static LogicalEngine staticengine;
    
    Database database;
    
    private Direction lean;
    private float camera_speed = 0.05f;
    private bool is_lean;
	// Use this for initialization
	void Start () {
        database = Starter.GetDataBase();
        engine = Starter.GetEngine();        
        is_lean = false;
    }
	
	// Update is called once per frame
	void Update () {
        if (database.state == State.Idle)
        {
            if (!Input.GetKeyDown(KeyCode.Space))
            {
                if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    if (!is_lean)
                    {
                        if (engine.ArrowKeyPressed(Direction.Right))
                        {
                            database.state = State.Idle;
                            return;
                        }
                        engine.Gengine._lean_right();
                        is_lean = true;
                        lean = Direction.Right;
                    }

                }
                else if (Input.GetKeyDown(KeyCode.LeftArrow))
                {

                    if (!is_lean)
                    {
                        if (engine.ArrowKeyPressed(Direction.Left))
                        {
                            database.state = State.Idle;
                            return;
                        }
                        engine.Gengine._lean_left();
                        is_lean = true;
                        lean = Direction.Left;
                    }
                }
                else if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    if(!is_lean)
                    if (true)//(new Vector2(0, -1)))
                        {
                        engine.move(Direction.Down);
                        engine.EndTurn();
                    }
                }
                else if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    if (!is_lean)
                    {
                        engine.Gengine._lean_top();
                        lean = Direction.Up;
                        is_lean = true;
                    }

                }

                else if (Input.GetKeyUp(KeyCode.UpArrow))
                {
                    if (is_lean && lean==Direction.Up)
                    {
                        engine.Gengine._lean_top_undo();
                        is_lean = false;
                    }
                }
                /// if released it should undo the lean
                else if (Input.GetKeyUp(KeyCode.RightArrow))
                {
                    if (is_lean && lean == Direction.Right)
                    {
                        engine.Gengine._lean_right_undo();
                        is_lean = false;
                    }
                }

                else if (Input.GetKeyUp(KeyCode.LeftArrow))
                    if (is_lean && lean==Direction.Left)
                    {
                        engine.Gengine._lean_left_undo();
                        is_lean = false;
                    }
            }
         
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                    engine.Act();
                    engine.EndTurn();
            }

            if (Input.GetKeyDown(KeyCode.A))
            {
               
                
            }
            else if (Input.GetKeyUp(KeyCode.R))
            {
                if (engine.snapshotunits.Count != 0)
                {
                    Wall.print("some changes not saved to snapshot");
                }
                engine.Undo();
            }
            else if (Input.GetKeyDown(KeyCode.M))
            {
                GameObject.Find("Map").GetComponent<MapController>()._click();
            }
            else if (Input.GetKey(KeyCode.L))
            {
                Set_Camera(new Vector2(camera_speed, 0));
            }
            else if (Input.GetKey(KeyCode.J))
            {
                Set_Camera(new Vector2(-camera_speed, 0));
            }
            else if (Input.GetKey(KeyCode.I))
            {
                Set_Camera(new Vector2(0, camera_speed));
            }
            else if (Input.GetKey(KeyCode.K))
            {
                Set_Camera(new Vector2(0, -camera_speed));
            }
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

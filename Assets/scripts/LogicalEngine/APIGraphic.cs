using UnityEngine;
using System.Collections;

public class APIGraphic{

    LogicalEngine logicalengine;
    GraphicalEngine graphicalengine;

    public APIGraphic(LogicalEngine logicalengine)
    {
        this.logicalengine = logicalengine;
        graphicalengine = GameObject.Find("Graphical").GetComponent<GraphicalEngine>();
    }

    //Ramp to Ramp
    public void MovePlayer_Ramp_1(Player player, Vector2 position,int ramptype)
    {
        Debug.Log("Ramp to Ramp");
        int x = 1, y = 1;
        if (ramptype == 3 || ramptype == 4)
            x = -1;
        if (ramptype == 2 || ramptype == 3)
            y = -1;
        position += new Vector2(x * 0.4f, y * 0.4f);
        player.GetComponent<PlayerPhysics>().Ramp_To_Ramp_Move(position);
    }
    
    // Ramp to Block
    public void MovePlayer_Ramp_2(Player player, Vector2 position)
    { 
        player.GetComponent<PlayerPhysics>().Simple_Move(position);
    }

    //Ramp to fall
    public void MovePlayer_Ramp_3(Player player, Vector2 position)
    {
        Debug.Log("Ramp to Fall");
        player.gameObject.GetComponent<PlayerPhysics>().Simple_Move(position);
    }

    // Ramp to corner
    public void MovePlayer_Ramp_4(Player player, Vector2 position,int type)
    {
        Debug.Log("ramp to corner");
        player.gameObject.GetComponent<PlayerPhysics>().Ramp_To_Corner_Move(position,type);
    }

    //Ramp to sharp
    public void MovePlayer_Ramp_5(Player player, Vector2 position,int type)
    {
        Debug.Log("Ramp to Sharp");
        player.gameObject.GetComponent<PlayerPhysics>().Ramp_To_Sharp_Move(position,type);
    }

    //  Block to Block
    public void MovePlayer_Simple_1(Player player, Vector2 position)
    {
        Debug.Log("simple1");
        //player.gameObject.GetComponent<PlayerGraphics>().Player_Move(player.gameObject, position);
        player.gameObject.GetComponent<PlayerPhysics>().Simple_Move(position);
    }

    // Block to Branch
    public void MovePlayer_Simple_2(Player player, Vector2 position)
    {
        Debug.Log("simple2");
        player.gameObject.GetComponent<PlayerGraphics>().Player_Move(player.gameObject, position);
    }

    // Block to Ramp
    public void MovePlayer_Simple_3(Player player, Vector2 position, int ramptype)
    {
        Debug.Log("Block to ramp simple 3");
        int x = 1, y = 1;
        if (ramptype == 3 || ramptype == 4)
            x = -1;
        if (ramptype == 2 || ramptype == 3)
            y = -1;
        position += new Vector2( x * 0.4f  , y * 0.4f);
        player.GetComponent<PlayerPhysics>().Block_To_Ramp_Move(position,ramptype);
        //player.gameObject.GetComponent<PlayerGraphics>().Player_Move(player.gameObject, position);
    }

    // Block to fall
    public void MovePlayer_Simple_4(Player player, Vector2 position)
    {
        Debug.Log("simple4");
        player.gameObject.GetComponent<PlayerPhysics>().Simple_Move(position);
    }

    // Block to Ramp (tekrari)
    public void MovePlayer_Simple_5(Player player, Vector2 position , int ramptype)
    {
       Debug.Log("Simple 5");
        int x = 1, y = 1;
        if (ramptype == 3 || ramptype == 4)
            x = -1;
        if (ramptype == 2 || ramptype == 3)
            y = -1;
        position += new Vector2(x * 0.3f, y * 0.3f);
        player.GetComponent<PlayerPhysics>().Block_To_Ramp_Move(position,ramptype);
        //player.gameObject.GetComponent<PlayerGraphics>().Player_Move(player.gameObject, position);
    }

    // Branch to Block
    public void MovePlayer_Branch_1(Player player, Vector2 position)
    {
        Debug.Log("branch1");
        player.gameObject.GetComponent<PlayerPhysics>().Simple_Move(position);
    }

    // Branch to Fall
    public void MovePlayer_Branch_2(Player player, Vector2 position)
    {
        Debug.Log("branch2");
        player.gameObject.GetComponent<PlayerPhysics>().Simple_Move(position);
    }

    // Branch to Ramp
    public void MovePlayer_Branch_3(Player player, Vector2 position, int ramptype)
    {
        Debug.Log("branch3");
        player.gameObject.GetComponent<PlayerPhysics>().Block_To_Ramp_Move(position,ramptype);
    }

    public void MovePlayerFinished(GameObject player_obj)
    {
        logicalengine.graphic_PlayerMoveAnimationFinished(player_obj.GetComponent<Player>());
    }
    
    public void Fall(Player player, Vector2 position)
    {
        player.gameObject.transform.position = position;
        logicalengine.graphic_FallFinished(player);
    }

    public void Land(Player player, Vector2 position, Unit fallonunit)
    {
        logicalengine.graphic_LandFinished(player);
    }

    public void LandOnRamp(Player player, Vector2 position, Unit fallonunit, int ramptype)
    {
        int x = 1, y = 1;
        if (ramptype == 3 || ramptype == 4)
            x = -1;
        if (ramptype == 2 || ramptype == 3)
            y = -1;
        logicalengine.graphic_LandFinished(player);
        
        player.gameObject.transform.position =  (Vector2)position + new Vector2(x * 0.3f, y * 0.3f);
    }

    public void MovePlayerToBranch(Player player, Vector2 position, bool isonramp)
    {
        player.gameObject.GetComponent<PlayerGraphics>().Player_Move(player.gameObject, position);
    }


    public void MoveGameObject(GameObject obj, Direction dir, Unit unit)
    {
        Vector2 pos = (Vector2) obj.transform.position + Toolkit.DirectiontoVector(dir);
        graphicalengine.Move_Object(obj,unit, pos);
       
    }

    public void MoveGameObjectFinished(GameObject obj, Unit unit)
    {
        logicalengine.graphic_GameObjectMoveAnimationFinished(obj, unit);
    }

    public void Jump(Player player, Vector2 position)
    {

    }


    public void MoveObject(Unit u)
    {

    }

    public void MovePlayerOnPlatform(Player player,Direction dir)
    {
        player.GetComponent<PlayerPhysics>().On_Platform_Move(dir);
    }
    public void Lean(Player player)
    {
           PlayerGraphics gl = player.GetComponent<PlayerGraphics>();
           switch (player.leandirection)
           {
               case Direction.Right: gl.Lean_Right(); break;
               case Direction.Left: gl.Lean_Left(); break;
               case Direction.Up: gl.Lean_Up(); break;
               case Direction.Down: gl.Lean_Down(); break;
           }
    }

    public void LeanFinished(Player player)
    {
        PlayerGraphics gl = player.GetComponent<PlayerGraphics>();
        gl.Lean_Finished();
    }

    public void Absorb(Player player, Container container)
    {

    }

    public void LeanStickMove(Player player,Direction dir)
    {

    }
    public void Release(Player player, Container container)
    {

    }

    public void PlayerChangeDirection(Player player, Direction olddirection, Direction newdirection)
    {
        player.GetComponent<PlayerGraphics>().Player_Change_Direction(player, newdirection);
    }

    public void PlayerChangeDirectionFinished(Player player)
    {
        logicalengine.graphic_PlayerChangeDirectionFinished(player);
    }

    public void UnitChangeSprite(Unit unit)
    {
        if (unit is SimpleContainer)
            graphicalengine.Simple_Container((SimpleContainer)unit);
        else if (unit is DynamicContainer)
            graphicalengine.Dynamic_Container((DynamicContainer) unit);
    }
}

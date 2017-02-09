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
        Debug.Log("Ramp To Ramp");
        player.GetComponent<PlayerPhysics>().Ramp_To_Ramp_Move(position,ramptype);
    }
    
    // Ramp to Block
    public void MovePlayer_Ramp_2(Player player, Vector2 position,int type)
    {
        Debug.Log("Ramp To Block");
        player.GetComponent<PlayerPhysics>().Ramp_To_Block_Move(position,type);
    }

    //Ramp to fall
    public void MovePlayer_Ramp_3(Player player, Vector2 position,int type)
    {
        Debug.Log("Ramp to Fall");
        player.gameObject.GetComponent<PlayerPhysics>().Ramp_To_Fall(position,type);
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
        player.GetComponent<PlayerPhysics>().Block_To_Ramp_Move(position,ramptype);
        //player.gameObject.GetComponent<PlayerGraphics>().Player_Move(player.gameObject, position);
    }

    // Block to fall
    public void MovePlayer_Simple_4(Player player, Vector2 position)
    {
        player.gameObject.GetComponent<PlayerPhysics>().Simple_Move(position);
    }
    
    // Block to Ramp (tekrari)
    public void MovePlayer_Simple_5(Player player, Vector2 position , int ramptype)
    {
        Debug.Log("Block to ramp tekrari");
        player.GetComponent<PlayerPhysics>().Block_To_Ramp_Move(position,ramptype);
        //player.gameObject.GetComponent<PlayerGraphics>().Player_Move(player.gameObject, position);
    }

    // Branch to Block
    public void MovePlayer_Branch_1(Player player, Vector2 position)
    {
        Debug.Log("branch1");
        player.gameObject.GetComponent<PlayerPhysics>().Simple_Move(position);
    }

    // Branch to allall
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
        Debug.Log("Fall");
        player.GetComponent<PlayerPhysics>().Fall(position);
        logicalengine.graphic_FallFinished(player);
    }

    public void Fall_Finish(Player player)
    {
        player.FallFinished();
    }

    public void Land(Player player, Vector2 position, Unit fallonunit)
    {
        Debug.Log("Land");
        player.GetComponent<PlayerPhysics>().Land(position);
        logicalengine.graphic_LandFinished(player);
        
    }
    
    public void LandOnRamp(Player player, Vector2 position, Unit fallonunit, int ramptype)
    {
        player.GetComponent<PlayerPhysics>().Land_On_Ramp(position,ramptype);
        Debug.Log("Land On Ramp");
        logicalengine.graphic_LandFinished(player);
    }

    public void MovePlayerToBranch(Player player, Vector2 position, bool isonramp)
    {
        player.gameObject.GetComponent<PlayerGraphics>().Player_Move(player.gameObject, position);
    }


    public void MoveGameObject(GameObject obj, Vector2 pos, Unit unit)
    {
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

    public void MovePlayerOnPlatform(Player player,Vector2 pos)
    {
        Debug.Log("MoveOnPlatForm");
        player.GetComponent<PlayerPhysics>().On_Platform_Move(pos);
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

    public void Move_Update(Player player,Vector2 pos)
    {
        player.GetComponent<PlayerPhysics>().Set_End(pos);
    }

    public void LeanStickMove(Player player,Vector2 pos)
    {
        Debug.Log("Lean Stick Move");
        player.GetComponent<PlayerPhysics>().Lean_Stick_Move(pos);
    }

    public void LeanStickStop(Player player)
    {
        Debug.Log("Lean Stop");
        player.GetComponent<PlayerPhysics>().Lean_Stick_Stop();
    }

    public void LeanStickFinished(Player player)
    {
        Debug.Log("Lean Stick Finish");
        logicalengine.graphic_LeanStickMoveFinished(player);
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
            graphicalengine.Dynamic_Container((DynamicContainer)unit);
        else if (unit is Gate)
            graphicalengine.Gate((Gate)unit);
    }
}

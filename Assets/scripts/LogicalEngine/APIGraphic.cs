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
    
        player.GetComponent<PlayerPhysics>().Ramp_To_Ramp_Move(position,ramptype);
    }
    
    // Ramp to Block
    public void MovePlayer_Ramp_2(Player player, Vector2 position,int type)
    {
    
        player.GetComponent<PlayerPhysics>().Ramp_To_Block_Move(position,type);
    }

    //Ramp to fall
    public void MovePlayer_Ramp_3(Player player, Vector2 position,int type)
    {
      
        player.gameObject.GetComponent<PlayerPhysics>().Ramp_To_Fall(position,type);
    }

  
    // Ramp to corner
    public void MovePlayer_Ramp_4(Player player, Vector2 position,int type)
    {
      
        player.gameObject.GetComponent<PlayerPhysics>().Ramp_To_Corner_Move(position,type);
    }

    //Ramp to sharp
    public void MovePlayer_Ramp_5(Player player, Vector2 position,int type)
    {
        
        player.gameObject.GetComponent<PlayerPhysics>().Ramp_To_Sharp_Move(position,type);
    }

    //ramp to branch
    public void MovePlayer_Ramp_Branch(Player player,Vector2 position,int type,Direction direction)
    {
        player.gameObject.GetComponent<PlayerPhysics>().Simple_Move(position);
    }

    //  Block to Block
    public void MovePlayer_Simple_1(Player player, Vector2 position)
    {
        //player.gameObject.GetComponent<PlayerGraphics>().Player_Move(player.gameObject, position);
        player.gameObject.GetComponent<PlayerPhysics>().Simple_Move(position);
        player.gameObject.GetComponent<PlayerGraphics>().Move_Animation(player.direction);
    }

    // Block to Branch
    public void MovePlayer_Simple_2(Player player, Vector2 position,Direction direction)
    {
        Lean(player,direction);
        player.gameObject.GetComponent<PlayerPhysics>().Simple_Move(position);
    }

    // Block to Ramp
    public void MovePlayer_Simple_3(Player player, Vector2 position, int ramptype)
    {

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

        player.GetComponent<PlayerPhysics>().Block_To_Ramp_Move(position,ramptype);
        //player.gameObject.GetComponent<PlayerGraphics>().Player_Move(player.gameObject, position);
    }

    // Branch to Block
    public void MovePlayer_Branch_1(Player player, Vector2 position)
    {

        player.gameObject.GetComponent<PlayerPhysics>().Simple_Move(position);
    }

    // Branch to allall
    public void MovePlayer_Branch_2(Player player, Vector2 position)
    {

        player.gameObject.GetComponent<PlayerPhysics>().Simple_Move(position);
    }

    // Branch to Ramp
    public void MovePlayer_Branch_3(Player player, Vector2 position, int ramptype)
    {

        player.gameObject.GetComponent<PlayerPhysics>().Block_To_Ramp_Move(position,ramptype);
    }


    public void MovePlayerFinished(GameObject player_obj)
    {
        logicalengine.graphic_PlayerMoveAnimationFinished(player_obj.GetComponent<Player>());
        player_obj.GetComponent<PlayerGraphics>().Move_Finished();
           
    }    
    public void Fall(Player player, Vector2 position)
    {
        player.GetComponent<PlayerPhysics>().Fall(position);
        logicalengine.graphic_FallFinished(player);
    }

    public void Fall_Finish(Player player)
    {
        player.FallFinished();
    }

    public void Land(Player player, Vector2 position, Unit fallonunit)
    {
        player.GetComponent<PlayerPhysics>().Land(position);
        logicalengine.graphic_LandFinished(player);
        
    }

    public void LandOnRamp(Player player, Vector2 position, Unit fallonunit, int ramptype)
    {
        player.GetComponent<PlayerPhysics>().Land_On_Ramp(position, ramptype);
        logicalengine.graphic_LandFinished(player);
    }

    public void MoveGameObject(GameObject obj, Vector2 pos, Unit unit)
    {
        graphicalengine.Move_Object(obj,unit, pos);
    }

    public void MoveGameObjectFinished(GameObject obj, Unit unit)
    {
        logicalengine.graphic_GameObjectMoveAnimationFinished(obj, unit);
    }

    public void Jump(Player player,Ability jump_ability, Vector2 position,Direction dir)
    {
        player.GetComponent<PlayerPhysics>().Jump(position, (Jump)jump_ability,dir);
    }

    public void Jump_Finish(Player player)
    {
        
    }
    public void Jumped_One(Ability abil)
    {
        
    }

    public void Jump_Hit(Player player,Direction dir,Jump ability)
    {
        Jump_Hit_Finish(player,ability);
    }

    public void Jump_Hit_Finish(Player player,Jump ability)
    {
        ability.JumpHitFinished(player);
    }

    public void MovePlayerOnPlatform(Player player,Vector2 pos)
    {
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
    public void Lean(Player player,Direction dir)
    {
        PlayerGraphics gl = player.GetComponent<PlayerGraphics>();
        switch (dir)
        {
            case Direction.Right: gl.Lean_Right(); break;
            case Direction.Left: gl.Lean_Left(); break;
            case Direction.Up: gl.Lean_Up(); break;
            case Direction.Down: gl.Lean_Down(); break;
        }

    }

    public void Camera_AutoMove()
    {
        Camera.main.GetComponent<CameraController>().AutoMove();
    }

    public void Fake_Lean(Player player,Direction dir)
    {
         
    }
    public void LeanFinished(Player player)
    {
        PlayerGraphics gl = player.GetComponent<PlayerGraphics>();
        gl.Lean_Finished();
    }

    // Change Color of Player in absorb , release , swap
    public void Absorb(Player player, Container container)
    {
        Debug.Log("change Color");
        player.GetComponent<PlayerGraphics>().ChangeColor();
        GameObject.Find("HUD").transform.GetChild(0).GetComponent<HUD>().AbilityChanged(player);
    }


    // for stoping courtines of moving container and objects

    public void Move_Update(Player player,Vector2 pos)
    {
        player.GetComponent<PlayerPhysics>().Set_End(pos);
    }

    public void LeanStickMove(Player player,Vector2 pos)
    {

        player.GetComponent<PlayerPhysics>().Lean_Stick_Move(pos);
    }

    public void LeanStickStop(Player player)
    {

        player.GetComponent<PlayerPhysics>().Lean_Stick_Stop();
    }

    public void LeanStickFinished(Player player)
    {
  
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

    public void Undo_Player(Player player,Unit unit)
    {
        player.GetComponent<PlayerPhysics>().Player_Undo();
    }
    public void Undo_Objects()
    {
        graphicalengine.StopAllCoroutines();
    }

    public void Player_Co_Stop(Player player)
    {
        player.GetComponent<PlayerPhysics>().StopAllCoroutines();
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

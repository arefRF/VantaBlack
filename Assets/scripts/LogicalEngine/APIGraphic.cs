using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class APIGraphic{

    LogicalEngine logicalengine;
    GraphicalEngine graphicalengine;

    public APIGraphic(LogicalEngine logicalengine)
    {
        this.logicalengine = logicalengine;
        graphicalengine = GameObject.Find("Graphical").GetComponent<GraphicalEngine>();
     //   graphicalengine.lasergraphics = graphicalengine.gameObject.GetComponent<LaserGraphics>();
    }

    //Ramp to Ramp
    public void MovePlayer_Ramp_1(Player player, Vector2 position,int ramptype)
    {
            player.GetComponent<PlayerGraphics>().StopAllCoroutines();
            player.GetComponent<PlayerPhysics>().Ramp_To_Ramp_Move(position, ramptype);
        player.gameObject.GetComponent<PlayerGraphics>().Move_Animation(player.direction);
    }
    
    // Ramp to Block
    public void MovePlayer_Ramp_2(Player player, Vector2 position,int type)
    {

            player.GetComponent<PlayerGraphics>().StopAllCoroutines();
            player.GetComponent<PlayerPhysics>().Ramp_To_Block_Move(position, type);
        //player.gameObject.GetComponent<PlayerGraphics>().Ramp_To_Block_Animation(player.direction);
        player.gameObject.GetComponent<PlayerGraphics>().Move_Animation(player.direction);
    }

    //Ramp to fall
    public void MovePlayer_Ramp_3(Player player, Vector2 position,int type)
    {
            player.GetComponent<PlayerGraphics>().StopAllCoroutines();
            player.gameObject.GetComponent<PlayerPhysics>().Ramp_To_Fall(position, type);
        player.gameObject.GetComponent<PlayerGraphics>().Move_Animation(player.direction);
    }

  
    // Ramp to corner
    public void MovePlayer_Ramp_4(Player player, Vector2 position,int type)
    {
        player.GetComponent<PlayerGraphics>().StopAllCoroutines();
        player.gameObject.GetComponent<PlayerPhysics>().Ramp_To_Corner_Move(position,type);
        player.gameObject.GetComponent<PlayerGraphics>().Move_Animation(player.direction);
    }

    //Ramp to sharp
    public void MovePlayer_Ramp_5(Player player, Vector2 position,int type)
    {

        player.GetComponent<PlayerGraphics>().StopAllCoroutines();
        player.gameObject.GetComponent<PlayerPhysics>().Ramp_To_Sharp_Move(position,type);
        player.gameObject.GetComponent<PlayerGraphics>().Move_Animation(player.direction);
    }

    //ramp to branch
    public void MovePlayer_Ramp_Branch(Player player,Vector2 position,int type,Direction direction)
    {
        Debug.Log("ramp to branch");
        player.GetComponent<PlayerGraphics>().StopAllCoroutines();
        player.gameObject.GetComponent<PlayerPhysics>().StopAllCoroutines();
        player.GetComponent<PlayerGraphics>().MoveToBranch(direction);
        player.gameObject.GetComponent<PlayerGraphics>().Move_Animation(player.direction);
    }

    //  Block to Block
    public void MovePlayer_Simple_1(Player player, Vector2 position)
    {
        //player.gameObject.GetComponent<PlayerGraphics>().Player_Move(player.gameObject, position);
        player.GetComponent<PlayerGraphics>().StopAllCoroutines();
        player.gameObject.GetComponent<PlayerGraphics>().Move_Animation(player.direction);
        player.gameObject.GetComponent<PlayerPhysics>().Simple_Move(position);
        
    }


    // Branch to Branch
    public void MovePlayer_Branch_Branch(Player player,Vector2 pos)
    {
        Debug.Log("branch to branch");
        player.GetComponent<PlayerGraphics>().StopAllCoroutines();
        player.gameObject.GetComponent<PlayerPhysics>().Branch_Branch(pos);
        player.gameObject.GetComponent<PlayerGraphics>().Move_Animation(player.direction);
    }

    public void BranchLight(bool on,Branch branch,Player player)
    {
        graphicalengine.BranchLight(branch, on,player);
    }

    public void LaserHitDynamic(DynamicContainer container)
    {
        graphicalengine.LaserHit(container);
    }

    public void LaserUnHitDynamic(DynamicContainer container)
    {
        graphicalengine.LaserUnhit(container);
    }
    // Block to Branch
    public void MovePlayer_Simple_2(Player player, Vector2 position,Direction direction)
    {
        player.GetComponent<PlayerGraphics>().StopAllCoroutines();
        player.gameObject.GetComponent<PlayerPhysics>().StopAllCoroutines();
        player.GetComponent<PlayerGraphics>().MoveToBranch(direction);
       // player.gameObject.GetComponent<PlayerGraphics>().Move_Animation(player.direction);
    }

    // Block to Ramp
    public void MovePlayer_Simple_3(Player player, Vector2 position, int ramptype)
    {
        player.GetComponent<PlayerGraphics>().StopAllCoroutines();
        player.GetComponent<PlayerPhysics>().Block_To_Ramp_Move(position,ramptype);
        //player.gameObject.GetComponent<PlayerGraphics>().Ramp_Animation(player.direction, ramptype);
        player.gameObject.GetComponent<PlayerGraphics>().Move_Animation(player.direction);

    }

    public void Drain(Player player,Drainer drainer)
    {
        player.GetComponent<PlayerGraphics>().Drain();
    }

    // Block to fall
    public void MovePlayer_Simple_4(Player player, Vector2 position)
    {
        player.gameObject.GetComponent<PlayerGraphics>().BlockToFallAnimation();
        player.GetComponent<PlayerGraphics>().StopAllCoroutines();
        player.gameObject.GetComponent<PlayerGraphics>().Move_Animation(player.direction);
        player.gameObject.GetComponent<PlayerPhysics>().Simple_Move(position);
       
    }
    
    // Block to Ramp (tekrari)
    public void MovePlayer_Simple_5(Player player, Vector2 position , int ramptype)
    {
        player.GetComponent<PlayerGraphics>().StopAllCoroutines();
        player.GetComponent<PlayerPhysics>().Block_To_Ramp_Move(position,ramptype);
        // player.gameObject.GetComponent<PlayerGraphics>().Ramp_Animation(player.direction, ramptype);
        player.gameObject.GetComponent<PlayerGraphics>().Move_Animation(player.direction);

    }

    // Branch to Block
    public void MovePlayer_Branch_1(Player player, Vector2 position,Direction dir)
    {
        player.GetComponent<PlayerGraphics>().StopAllCoroutines();
        player.GetComponent<PlayerPhysics>().StopAllCoroutines();
        player.GetComponent<PlayerGraphics>().BranchExit(dir,0);
        //player.gameObject.GetComponent<PlayerGraphics>().Move_Animation(player.direction);
   

    }

    // Branch to fall
    public void MovePlayer_Branch_2(Player player, Vector2 position,Direction dir)
    {
        Debug.Log("branch to fall");
        player.GetComponent<PlayerPhysics>().StopAllCoroutines();
        player.GetComponent<PlayerGraphics>().StopAllCoroutines();
        player.GetComponent<PlayerGraphics>().BranchExit(dir,0);
        player.gameObject.GetComponent<PlayerGraphics>().Move_Animation(player.direction);
        //player.gameObject.GetComponent<PlayerPhysics>().Simple_Move(position);
    }

    // Branch to Ramp
    public void MovePlayer_Branch_3(Player player, Vector2 position, int ramptype,Direction dir)
    {
        Debug.Log("branch to ramp");
        player.GetComponent<PlayerGraphics>().StopAllCoroutines();
        player.GetComponent<PlayerPhysics>().StopAllCoroutines();
        player.GetComponent<PlayerGraphics>().BranchExit(dir,ramptype);
        player.gameObject.GetComponent<PlayerGraphics>().Move_Animation(player.direction);
        //player.gameObject.GetComponent<PlayerPhysics>().Block_To_Ramp_Move(position,ramptype);
    }


    public void MovePlayerFinished(GameObject player_obj)
    {
        logicalengine.graphic_PlayerMoveAnimationFinished(player_obj.GetComponent<Player>());
        player_obj.GetComponent<PlayerGraphics>().Move_Finished();   
    }  
      
    public void Fall(Player player, Vector2 position)
    {   
        player.GetComponent<PlayerGraphics>().StopAllCoroutines();
        player.GetComponent<PlayerGraphics>().FallAnimation();
        player.GetComponent<PlayerPhysics>().Fall(position);
    }


    public void FallGraphics(Player player,Vector2 position,int height)
    {
        Debug.Log(height);
    }

    public void FallAfterOneJump(Player player,Vector2 position)
    {

    }

    public void Fall_Finish(Player player)
    {
        player.FallFinished();
    }

    public void Land(Player player, Vector2 position, Unit fallonunit)
    {
        player.GetComponent<PlayerGraphics>().StopAllCoroutines();
        player.GetComponent<PlayerPhysics>().Land(position);
        player.GetComponent<PlayerGraphics>().LandAnimation();
        
    }

    public void LandFinished(Player player)
    {
        logicalengine.graphic_LandFinished(player);
    }
    public void LandOnRamp(Player player, Vector2 position, Unit fallonunit, int ramptype)
    {
        player.GetComponent<PlayerGraphics>().StopAllCoroutines();
        player.GetComponent<PlayerPhysics>().Land_On_Ramp(position, ramptype);
        
    }

    // object move
    public void MoveGameObject(GameObject obj, Vector2 pos, Unit unit)
    {
        graphicalengine.Move_Object(obj,unit, pos);
    }
    public void MoveGameObjectFinished_90percent(GameObject obj, Unit unit)
    {
        logicalengine.graphic_GameObjectMoveAnimationFinished_90percent(obj, unit);
    }

    public void MoveGameObjectFinished_50percent(GameObject obj, Unit unit)
    {
        //logicalengine.lasercontroller.SetLasers();
    }

    public void Jump(Player player,Ability jump_ability, Vector2 position,Direction dir)
    {
        //player.GetComponent<PlayerGraphics>().Jump(dir);
        //player.transform.GetChild(0).GetComponent<AnimationEvents>().SetJumpCordinates(position, (Jump)jump_ability, dir, false);
        player.GetComponent<PlayerPhysics>().Jump(position, (Jump)jump_ability,dir,false);
    }

    public void Jump_Finish(Player player, Vector2 finalpos, Jump jump)
    {
        jump.JumpFinished(player);
    }

    public void Jump_Hit(Player player,Direction dir,Jump ability,Vector2 pos)
    {
        // uses a boolean to detemine jump hit
        player.GetComponent<PlayerPhysics>().Jump(pos, ability, dir,true);
    }

    public void Jump_Hit_Finish(Player player,Jump ability,Vector2 finalpos)
    {
        ability.JumpHitFinished(player);
    }

    public void MovePlayerOnPlatform(Player player,Vector2 pos)
    {
        player.GetComponent<PlayerGraphics>().StopAllCoroutines();
        player.GetComponent<PlayerPhysics>().On_Platform_Move(pos);
    }
    public void Lean(Player player)
    {
           PlayerGraphics gl = player.GetComponent<PlayerGraphics>();
           switch (player.leandirection)
           {
               case Direction.Right: gl.Lean_Right(false); break;
               case Direction.Left: gl.Lean_Left(false); break;
               case Direction.Up: gl.Lean_Up(false); break;
               case Direction.Down: gl.Lean_Down(false); break;
           }
    }

    public void Lean_On_Air(Player player)
    {
        Debug.Log("On Air");
        PlayerGraphics gl = player.GetComponent<PlayerGraphics>();
        switch (player.leandirection)
        {
            case Direction.Right: gl.Lean_Right(false); break;
            case Direction.Left: gl.Lean_Left(false); break;
            case Direction.Up: gl.Lean_Up(false); break;
            case Direction.Down: gl.Lean_Down(false); break;
        }
    }
    public void Camera_AutoMove()
    {
       // Camera.main.GetComponent<CameraController>().AutoMove();
    }

    public void Fake_Lean(Player player,Direction dir)
    {
        PlayerGraphics gl = player.GetComponent<PlayerGraphics>();
        switch (player.leandirection)
        {
            case Direction.Right: gl.FakeLean_Right(); break;
            case Direction.Left: gl.FakeLean_Left(); break;
            case Direction.Up: gl.FakeLean_Up(); break;
            case Direction.Down: gl.FakeLean_Down(); break;
        }
    }

    public LineRenderer AddLaserLine(Vector2 pos1, Vector2 pos2, GameObject parent,LineRenderer lineR)
    {
        return graphicalengine.GetComponent<LaserGraphics>().AddLaserLine(pos1, pos2, parent,lineR);
    }

    public void DestroyLasers()
    {
        graphicalengine.GetComponent<LaserGraphics>().DestroyLasers();
    }
    public void LeanFinished(Player player)
    {
        PlayerGraphics gl = player.GetComponent<PlayerGraphics>();
        gl.Lean_Finished();
    }

    // Change Color of Player in absorb , release , swap
    public void Absorb(Player player, Container container)
    {
        player.GetComponent<PlayerGraphics>().ChangeColor();
        try
        {
            //GameObject.Find("HUD").transform.GetChild(0).GetComponent<HUD>().AbilityChanged(player);
        }
        catch
        {
            //GameObject.Find("UI").GetComponent<Get>().hud.SetActive(true);
            GameObject.Find("HUD").transform.GetChild(0).GetComponent<HUD>().AbilityChanged(player);
        }
        
    }

    public void Transition(Player player)
    {

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
        player.GetComponent<PlayerGraphics>().ChangeColor();
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
        player.GetComponent<PlayerPhysics>().Player_Undo(unit);
    }
    public void Undo_Unit(Unit unit)
    {
        if(unit is Gate)
        {
            //unit.transform.GetChild(6).GetComponent<SpriteRenderer> = (Sprite)Resources.Load("")
        }
    }

    public void Roll(Player player,Vector2 pos)
    {
        player.GetComponent<PlayerPhysics>().Roll(pos);
    } 
    public void EnterPortalMode(List<Unit> portals,Container container)
    {
        graphicalengine.EnterPortalMode(portals,container);
    }

    public void QuitPortalMode(List<Unit> portals)
    {
        graphicalengine.QuitPortalMode(portals);
    }
    public void ProtalHighlight(Unit current,Unit pre)
    {
        graphicalengine.PortalHighlighter(current, pre);
    }
    public void Teleport(Player player,Vector2 pos)
    {
        player.GetComponent<PlayerGraphics>().Teleport(pos);
    }

    // Portal calls this
    public void Port(Player player,Vector2 pos)
    {
        player.GetComponent<PlayerGraphics>().Portal(pos);
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
        else if (unit is StaticContainer)
            graphicalengine.StaticContainer((StaticContainer)unit);
        else if (unit is Gate)
            graphicalengine.Gate((Gate)unit);
        else if (unit is Branch)
            graphicalengine.Branch((Branch)unit);
        else if (unit is Fountain)
            graphicalengine.Fountatin((Fountain)unit);
    }

    public void Fall_Player_Died(Player player)
    {
        player.GetComponent<PlayerPhysics>().Fall_Die(new Vector2(player.position.x, -10));
        Debug.Log("fall player died");
        GameObject.Find("UI").GetComponent<Get>().inMenu_Show();
    }

    public void Crush_Player_Died(Player player)
    {
        graphicalengine.StartCoroutine(LeanWait(0.15f, player));
        player.SetState(PlayerState.Gir);
        //GameObject.Find("UI").GetComponent<Get>().inMenu_Show();
    }


    private IEnumerator LeanWait(float f, Player player)
    {
        yield return new WaitForSeconds(f);
        graphicalengine.LaserDieAnimation(player);
    }

    public void Fake_Lean_Undo(Player player)
    {
        PlayerGraphics gl = player.GetComponent<PlayerGraphics>();
        gl.FakeLean_Finished();
    }
    public void AddLaser(Vector2 pos1,Vector2 pos2,Direction dir, GameObject parent)
    {
        if(graphicalengine.lasergraphics != null)
            graphicalengine.lasergraphics.AddLaser(pos1, pos2, dir, parent);
    }

    public void AddPartialLaser(Vector2 pos, Direction dir, GameObject parent)
    {
        if (graphicalengine.lasergraphics != null)
            graphicalengine.lasergraphics.AddPartialLaser(pos, dir, parent);
    }

    public void RemoveLaser()
    {
        if(graphicalengine.lasergraphics != null)
            graphicalengine.lasergraphics.RemoveLasers();
    }

    public void UnlockBranchLaser(Branch branch)
    {
        graphicalengine.UnlockBranchLaser(branch);
    }

    public void AdjustPlayer(Player player, Vector2 pos, Direction direction, System.Action<Player, Direction> passingmethod)
    {
        player.GetComponent<PlayerPhysics>().Adjust(pos, direction, passingmethod);
    }

    public void Laser_Player_Died(Player player)
    {
        Debug.Log("laser player died");
        graphicalengine.LaserDieAnimation(player);
    }
}

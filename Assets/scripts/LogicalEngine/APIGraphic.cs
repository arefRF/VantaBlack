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
    public void MovePlayer_Ramp_1(Player player, Vector2 position)
    {
        Debug.Log("ramp1");
        player.gameObject.GetComponent<PlayerGraphics>().Player_Move(player.gameObject, position);
    }
    public void MovePlayer_Ramp_2(Player player, Vector2 position)
    {
        Debug.Log("ramp2");
        player.gameObject.GetComponent<PlayerGraphics>().Player_Move(player.gameObject, position);
    }
    public void MovePlayer_Ramp_3(Player player, Vector2 position)
    {
        Debug.Log("ramp3");
        player.gameObject.GetComponent<PlayerGraphics>().Player_Move(player.gameObject, position);
    }
    public void MovePlayer_Ramp_4(Player player, Vector2 position)
    {
        Debug.Log("ramp4");
        player.gameObject.GetComponent<PlayerGraphics>().Player_Move(player.gameObject, position);
    }
    public void MovePlayer_Ramp_5(Player player, Vector2 position)
    {
        Debug.Log("ramp5");
        player.gameObject.GetComponent<PlayerGraphics>().Player_Move(player.gameObject, position);
    }
    public void MovePlayer_Simple_1(Player player, Vector2 position)
    {
        Debug.Log("simple1");
        player.gameObject.GetComponent<PlayerGraphics>().Player_Move(player.gameObject, position);
    }
    public void MovePlayer_Simple_2(Player player, Vector2 position)
    {
        Debug.Log("simple2");
        player.gameObject.GetComponent<PlayerGraphics>().Player_Move(player.gameObject, position);
    }
    public void MovePlayer_Simple_3(Player player, Vector2 position, int ramptype)
    {
        Debug.Log("simple3");
        player.gameObject.GetComponent<PlayerGraphics>().Player_Move(player.gameObject, position);
    }
    public void MovePlayer_Simple_4(Player player, Vector2 position)
    {
        Debug.Log("simple4");
        player.gameObject.GetComponent<PlayerGraphics>().Player_Move(player.gameObject, position);
    }
    public void MovePlayer_Simple_5(Player player, Vector2 position , int ramptype)
    {
        Debug.Log("simple5");
        player.gameObject.GetComponent<PlayerGraphics>().Player_Move(player.gameObject, position);
    }
    public void MovePlayer_Branch_1(Player player, Vector2 position)
    {
        Debug.Log("branch1");
        player.gameObject.GetComponent<PlayerGraphics>().Player_Move(player.gameObject, position);
    }
    public void MovePlayer_Branch_2(Player player, Vector2 position)
    {
        Debug.Log("branch2");
        player.gameObject.GetComponent<PlayerGraphics>().Player_Move(player.gameObject, position);
    }
    public void MovePlayer_Branch_3(Player player, Vector2 position, int ramptype)
    {
        Debug.Log("branch3");
        player.gameObject.GetComponent<PlayerGraphics>().Player_Move(player.gameObject, position);
    }

    public void MovePlayer(Player player, Vector2 position, bool wasonramp)
    {
        player.gameObject.GetComponent<PlayerGraphics>().Player_Move(player.gameObject, position);
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

    public void LandOnRamp(Player player, Vector2 position, Unit fallonunit, int landtype)
    {
        logicalengine.graphic_LandFinished(player);
        player.gameObject.transform.position = position;
    }

    public void MovePlayerToBranch(Player player, Vector2 position, bool isonramp)
    {
        player.gameObject.GetComponent<PlayerGraphics>().Player_Move(player.gameObject, position);
    }

    public void MovePlayerOnRamp(Player player, Vector2 position, bool was_on_ramp,Direction dir, int type)
    {
        if (was_on_ramp)
            player.GetComponent<PlayerGraphics>().Ramp_To_Ramp_Move(position);
        else
            player.GetComponent<PlayerGraphics>().Block_To_Ramp_Move(position,dir,type);
    }

    public void MoveGameObject(GameObject obj, Vector2 pos)
    {
        obj.transform.position = pos;
        logicalengine.graphic_GameObjectMoveAnimationFinished(obj);
    }

    public void MoveGameObjectFinished(GameObject obj)
    {

    }

    public void Jump(Player player, Vector2 position)
    {

    }


    public void MoveObject(Unit u)
    {

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
        switch (player.leandirection)
        {
            case Direction.Right: gl.Lean_Right_Finished(); break;
            case Direction.Left: gl.Lean_Left_Finished(); break;
            case Direction.Up: gl.Lean_Up_Finished(); break;
            case Direction.Down: gl.Lean_Down_Finished(); break;
        }
    }

    public void Absorb(Player player, Container container)
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
}

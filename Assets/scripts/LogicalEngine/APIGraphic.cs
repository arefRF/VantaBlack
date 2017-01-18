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

    public void MovePlayer(Player player, Vector2 position, bool isonramp)
    {
        player.gameObject.GetComponent<PlayerGraphics>().Player_Move(player.gameObject, position);
    }

    public void MovePlayerFinished(GameObject player_obj)
    {
        logicalengine.graphic_MoveAnimationFinished(player_obj.GetComponent<Player>());
    }
    
    public void MovePlayerToBranch(Player player, Vector2 position, bool isonramp)
    {

    }

    public void MovePlayerOnRamp(Player player, Vector2 position, bool isonramp)
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

           }
    }

    public void LeanFinished(Player player)
    {
        PlayerGraphics gl = player.GetComponent<PlayerGraphics>();
        switch (player.leandirection)
        {
            case Direction.Right: gl.Lean_Right_Finished(); break;
            case Direction.Left: gl.Lean_Left_Finished(); break;
        }
    }

}

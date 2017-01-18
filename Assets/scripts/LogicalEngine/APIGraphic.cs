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
        Debug.Log("move move moe");
        graphicalengine._move(player, position);
    }

    public void MovePlayerFinished(GameObject player_obj)
    {

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

    }

    public void LeanFinished(Player player)
    {

    }

}

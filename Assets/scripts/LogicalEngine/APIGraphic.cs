using UnityEngine;
using System.Collections;

public class APIGraphic{

    LogicalEngine logicalengine;
    GraphicalEngine graphicalengine;


    public void MovePlayer(Player player, Vector2 position)
    {
        graphicalengine._move(player, position);
    }
    
    public void MovePlayerOnRamp(Player player, Vector2 position)
    {

    }

    public void MovePlayerOnRampFromRamp(Player player, Vector2 postition)
    {

    }

    public void Jump(Player player, Vector2 position)
    {

    }


    public void MoveObject(Unit u)
    {

    }

}

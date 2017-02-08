using UnityEngine;
using System.Collections;

public class InputController {

    LogicalEngine engine;

    public InputController(LogicalEngine engine)
    {
        this.engine = engine;
    }

    public void PlayerMoveAction(Player player, Direction direction)
    {
        if (player.state == PlayerState.Idle)
        {
            IdlePLayerMove(player, direction);
        }
        else if(player.state == PlayerState.Moving)
        {
            MovingPlayerMove(player, direction);
        }
    }

    private void IdlePLayerMove(Player player, Direction direction)
    {
        if (!player.lean)
        {
            if (player.Can_Move_Direction(direction))
            {
                if (player.Should_Change_Direction(direction))
                {
                    Direction olddir = player.direction;
                    player.direction = direction;
                    engine.apigraphic.PlayerChangeDirection(player, olddir, player.direction);
                }
                else if (!player.Move(direction))
                {
                    engine.Lean(player, direction);
                }
                else
                {
                    player.state = PlayerState.Moving;
                }
            }
            else if (player.Can_Lean(direction))
            {
                engine.Lean(player, direction);
            }
        }
    }

    private void MovingPlayerMove(Player player, Direction direction)
    {
        //Debug.Log(player.direction);
        //Debug.Log(direction);
        if(player.direction == direction)
        {
            //Debug.Log("calling graphicals");
            if (player.movepercentage == 90)
            {
                Debug.Log("calling graphicals");
                if (!player.Move(direction))
                {
                    engine.Lean(player, direction);
                    
                }
                else
                {
                player.movepercentage = 91;
                    player.state = PlayerState.Moving;
                /*engine.apiunit.RemoveFromDatabase(player);
                player.position += Toolkit.DirectiontoVector(direction);
                engine.apiunit.AddToDatabase(player);
                Debug.Log("calling graphicals");
                engine.apigraphic.Move_Update(player, player.position);*/
                }
            }
        }
        else
        {
            Direction olddir = player.direction;
            player.direction = direction;
            engine.apigraphic.PlayerChangeDirection(player, olddir, player.direction);
            if (!player.Move(direction))
            {
                engine.Lean(player, direction);
            }
            else
            {
                player.state = PlayerState.Moving;
            }
        }
    }
}

using UnityEngine;
using System.Collections;

public class Jump : Ability {

    public int number;
    private int shouldjump, jumped;
    LogicalEngine engine;
	public Jump()
    {
        abilitytype = AbilityType.Jump;
    }

    public void Action(Player player, Direction direction)
    {
        Debug.Log("jump action begin");
        shouldjump = GetShouldJump(player.position, direction);
        Debug.Log("shouldjump: " + shouldjump);
        Vector2 finalpos = player.position + shouldjump * Toolkit.DirectiontoVector(direction);
        Starter.GetEngine().apigraphic.Jump(player, this, finalpos);
    }

    public void JumpedOnce(Player player, Direction direction)
    {
        Debug.Log("jumped once");
        engine.apiunit.RemoveFromDatabase(player);
        player.position += Toolkit.DirectiontoVector(direction);
        engine.apiunit.AddToDatabase(player);
        if (shouldjump == jumped && number != shouldjump)
            engine.apigraphic.Jump_Hit(player, direction);
        else
            engine.Applygravity();
    }

    private int GetShouldJump(Vector2 position, Direction direction)
    {
        int num = 0;
        for (int i = 0; i < number; i++)
        {
            position = Toolkit.VectorSum(position, direction);
            if (Toolkit.IsEmpty(position))
            {
                num++;
            }
            else if (Toolkit.HasRamp(position) && !Toolkit.IsdoubleRamp(position))
            {
                if (Toolkit.GetRamp(position).IsOnRampSide(Toolkit.ReverseDirection(direction)))
                    num++;
                else
                    break;
            }
            else
            {
                break;
            }
        }
        return num;
    }
}

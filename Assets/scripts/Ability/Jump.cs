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

    public Jump(int number)
    {
        abilitytype = AbilityType.Jump;
        this.number = number;
    }

    public void Action(Player player, Direction direction)
    {
        jumped = 0;
        if (engine == null)
            engine = Starter.GetEngine();
        shouldjump = GetShouldJump(player.position, direction);
        Vector2 finalpos = player.position + shouldjump * Toolkit.DirectiontoVector(direction);
        engine.apiunit.AddToSnapshot(player);
        player.state = PlayerState.Jumping;
        engine.inputcontroller.LeanUndo(player, player.leandirection);
        player.jumpdirection = direction;
        engine.apigraphic.Jump(player, this, finalpos, direction);
        
    }

    public void JumpedOnce(Player player, Direction direction)
    {

        jumped++;
        engine.apiunit.RemoveFromDatabase(player);
        player.position += Toolkit.DirectiontoVector(direction);
        engine.apiunit.AddToDatabase(player);
        if (shouldjump == jumped) {
            if (number != shouldjump)
                engine.apigraphic.Jump_Hit(player, direction, this);
            else
            {
                player.state = PlayerState.Idle;
                engine.Applygravity();
            }
        }
    }

    public void JumpHitFinished(Player player)
    {
        player.state = PlayerState.Idle;
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

    public override Ability ConvertContainerAbilityToPlayer()
    {
        number = 2;
        return this;
    }

    public override Ability ConvertPlayerAbilityToContainer()
    {
        number = 4;
        return this;
    }
}

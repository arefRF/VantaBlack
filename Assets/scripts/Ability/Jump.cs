using UnityEngine;
using System.Collections;

public class Jump : Ability {

    public int number;
    private int maxJump;
    LogicalEngine engine;
    public Coroutine coroutine;
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
        Debug.Log("here");
        Starter.GetDataBase().StopTimer();
        player.state = PlayerState.Busy;
        player.currentAbility = this;
        if (engine == null)
            engine = Starter.GetEngine();
        Vector2 finalpos = player.position + number * Toolkit.DirectiontoVector(direction);
        maxJump = GetShouldJump(finalpos, direction);
        engine.apiunit.AddToSnapshot(player);
        engine.inputcontroller.LeanUndo(player, player.leandirection, PlayerState.Busy);
        player.jumpdirection = direction;

        if (number <= maxJump)
            engine.apigraphic.Jump(player, this, finalpos, direction);
        else
            engine.apigraphic.Jump_Hit(player, direction, this, finalpos);
        
    }

    public void JumpFinished(Player player, Vector2 finalpos)
    {
        player.state = PlayerState.Jumping;
        engine.apiunit.RemoveFromDatabase(player);
        player.position = finalpos;
        engine.apiunit.AddToDatabase(player);
        coroutine = GameObject.Find("GetInput").GetComponent<GetInput>().StartCoroutine(JumpWait(0.5f,player));
    }

    public void StartTimer(Player player,Direction direction)
    {
        Vector2 maxPosition = player.position + 4 * Toolkit.DirectiontoVector(direction);
        maxJump = GetShouldJump(maxPosition,direction);
        number = 2;
        Starter.GetDataBase().timer =  GameObject.Find("GetInput").GetComponent<GetInput>().StartCoroutine(Timer());
    }
    private IEnumerator Timer()
    {
        yield return new WaitForSeconds(0.75f);
        number++;
        number %= 5;
        if (number == 0)
            number = 2;
        Debug.Log(number);
        Starter.GetDataBase().timer = GameObject.Find("GetInput").GetComponent<GetInput>().StartCoroutine(Timer());

    }

    private IEnumerator JumpWait(float f,Player player)
    {
        yield return new WaitForSeconds(f);
        player.ApplyGravity();

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

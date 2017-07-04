using UnityEngine;
using System.Collections.Generic;

public class Drainer : Unit {

    public override void Run()
    {
        base.Run();
    }

    public bool Check(Player player)
    {
        if (player.position == position)
        {
            Drain(player);
            return true;
        }
        return false;
    }

    public void Drain(Player player)
    {
        player.SetState(PlayerState.Busy);
        api.engine.apigraphic.Player_Co_Stop(player);
        player.GetComponent<PlayerGraphics>().ResetStates();
        api.engine.apigraphic.Drain(player, this);
    }

    public override bool PlayerMoveInto(Direction dir)
    {
        return true;
    }
}

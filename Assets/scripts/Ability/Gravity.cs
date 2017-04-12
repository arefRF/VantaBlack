using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Gravity : Ability {
    private List<Player> players;
    private LogicalEngine engine;
    public Gravity()
    {
        abilitytype = AbilityType.Gravity;
    }

    private void SetVariables()
    {
        players = Starter.GetDataBase().player;
        engine = Starter.GetEngine();
    }
    public void Action(Player player,Direction direction)
    {
        player.state = PlayerState.Busy;
        player.SetGravity(direction);
        player.state = PlayerState.Idle;
        player.ApplyGravity();
    }

    public void Action_Container(FunctionalContainer container)
    {
        if (players == null)
            SetVariables();
        for (int i = 0; i < players.Count; i++)
            players[i].SetGravity(container.direction);
        engine.pipecontroller.CheckPipes();
    }
}

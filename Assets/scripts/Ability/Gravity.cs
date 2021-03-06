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
        player.SetState(PlayerState.Busy);
        player.currentAbility = this;
        player.SetGravity(direction);
        player.UseAbility(this);
        player.currentAbility = null;
        player.SetState(PlayerState.Idle);
        player.ApplyGravity();
    }

    public void Action_Container(FunctionalContainer container)
    {
        if (players == null)
            SetVariables();
        engine.database.SetGravity(container.direction);
        for (int i = 0; i < players.Count; i++)
            players[i].SetGravity(container.direction);
        engine.pipecontroller.CheckPipes();
    }
}

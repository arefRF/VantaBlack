using UnityEngine;
using System.Collections;

public class Fountain : Unit {

    public AbilityType ability;
    public int count;

    public override void Run()
    {

        base.Run();
    }

    public void Action(Player player)
    {
        if(player.abilities.Count == 0)
        {
            for (int i = 0; i < count; i++)
                player.abilities.Add(Ability.GetAbilityInstance(ability).ConvertContainerAbilityToPlayer());
            player._setability();
            api.engine.apigraphic.Absorb(player, null);
        }
        else
        {
            if (player.abilities[0].abilitytype == ability)
            {
                if(player.abilities.Count < count)
                {
                    while(player.abilities.Count < count)
                    {
                        player.abilities.Add(Ability.GetAbilityInstance(ability).ConvertContainerAbilityToPlayer());
                    }
                    player._setability();
                    api.engine.apigraphic.Absorb(player, null);
                }
            }
        }
    }
}

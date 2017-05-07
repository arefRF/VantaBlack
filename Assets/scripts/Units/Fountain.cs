using UnityEngine;
using System.Collections.Generic;

public class Fountain : Unit {

    public AbilityType ability;
    public int count;
    List<Ability> abilities;
    public override void Run()
    {
        abilities = new List<Ability>();
        base.Run();
    }

    public void Action(Player player)
    {
        
        if(player.abilities.Count == 0)
        {
            UndoAbilities(player);
            for (int i = 0; i < count; i++)
            {
                Ability temp = Ability.GetAbilityInstance(ability).ConvertContainerAbilityToPlayer(player);
                abilities.Add(temp);
                player.abilities.Add(temp);
            }
            player._setability();
            api.engine.apigraphic.Absorb(player, null);
        }
        else
        {
            if (player.abilities[0].abilitytype == ability)
            {
                if(player.abilities.Count < count)
                {
                    UndoAbilities(player);
                    while(player.abilities.Count < count)
                    {
                        Ability temp = Ability.GetAbilityInstance(ability).ConvertContainerAbilityToPlayer(player);
                        abilities.Add(temp);
                        player.abilities.Add(temp);
                    }
                    player._setability();
                    api.engine.apigraphic.Absorb(player, null);
                }
            }
        }
    }

    private void UndoAbilities(Player player)
    {
        for (int i = 0; i < abilities.Count; i++)
        {
            if (abilities[i].owner is Player)
            {
                ((Player)abilities[i].owner).abilities.Remove(abilities[i]);
                ((Player)abilities[i].owner)._setability();
                api.engine.apigraphic.Absorb(((Player)abilities[i].owner), null);
            }
            else if (abilities[i].owner is Container)
            {
                ((Container)abilities[i].owner).abilities.Remove(abilities[i]);
                ((Container)abilities[i].owner)._setability(null);

                api.ChangeSprite(abilities[i].owner);
            }

        }
        for (int i = 0; i < abilities.Count; i++)
        {
            if (abilities[i].owner is Container)
                if (abilities[i].owner is FunctionalContainer)
                    if (ability == AbilityType.Fuel)
                    {
                        ((FunctionalContainer)abilities[i].owner).SetOnorOff();
                        ((FunctionalContainer)abilities[i].owner).firstmove = true; ;
                        ((FunctionalContainer)abilities[i].owner).Action_Fuel();
                    }
        }
    }
}

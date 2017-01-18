using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Container : ParentContainer {
    public List<Ability> abilities;
    public int capacity = 4;


    void Start()
    {
        abilities = new List<Ability>();
    }


    public void PlayerAbsorb(Player player)
    {
        if (abilities.Count == 0)
            return;
        else if(player.abilities.Count == 0)
            PlayerAbsorbAbilities(player);
        else
        {
            if (abilities[0].abilitytype == player.abilities[0].abilitytype)
                PlayerAbsorbAbilities(player);
            else
                Swap(player);
        }
    }

    private void Swap(Player player)
    {
        List<Ability> temp = abilities;
        abilities = player.abilities;
        player.abilities = temp;
    }

    private void PlayerAbsorbAbilities(Player player)
    {
            if(player.abilities.Count<4)
            {
                player.abilities.Add(abilities[0]);
                abilities.RemoveAt(0);
            }
            
    }

    private void PlayerReleaseAbilities(Player player)
    {
            if(abilities.Count<4)
            {
                abilities.Add(player.abilities[0]);
                player.abilities.RemoveAt(0);
            }
   }

    public void PlayerRelease(Player player)
    {
        if (player.abilities.Count == 0)
            return;
        else if (abilities.Count == 0)
            PlayerReleaseAbilities(player);
        else
        {
            if (player.abilities[0].abilitytype == abilities[0].abilitytype)
                PlayerReleaseAbilities(player);
            else
                Swap(player);
        }
    }
    public override bool PlayerMoveInto(Direction dir)
    {
        return false;
    }
}

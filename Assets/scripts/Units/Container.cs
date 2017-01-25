using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Container : ParentContainer {
    public List<AbilityType> abilities;
    public int capacity = 4;


    void Start()
    {
        
    }


    public virtual void PlayerAbsorb(Player player)
    {
        if (abilities.Count == 0)
            return;
        else if(player.abilities.Count == 0)
            PlayerAbsorbAbilities(player);
        else
        {
            if (abilities[0] == player.abilities[0])
                PlayerAbsorbAbilities(player);
            else
                Swap(player);
        }
    }

    private void Swap(Player player)
    {
        List<AbilityType> temp = abilities;
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

    public virtual void PlayerReleaseAbilities(Player player)
    {
        if(abilities.Count<4)
        {
            abilities.Add(player.abilities[0]);
            player.abilities.RemoveAt(0);
        }
   }

    public virtual void PlayerRelease(Player player)
    {
        if (player.abilities.Count == 0)
            return;
        else if (abilities.Count == 0)
            PlayerReleaseAbilities(player);
        else
        {
            if (player.abilities[0] == abilities[0])
                PlayerReleaseAbilities(player);
            else
                Swap(player);
        }
    }
    public override bool PlayerMoveInto(Direction dir)
    {
        return false;
    }

    public override void Action(Player player, Direction dir)
    {
        return;
    }
}

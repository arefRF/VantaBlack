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

    private void Swap(Player player)
    {
        List<AbilityType> temp = abilities;
        abilities = null;
        ContainerAbilityChanged(false, 0);
        abilities = player.abilities;
        ContainerAbilityChanged(true, abilities.Count);
        player.abilities = temp;
    }

    private void PlayerAbsorbAbilities(Player player)
    {
        if(player.abilities.Count<4)
        {
            player.abilities.Add(abilities[0]);
            abilities.RemoveAt(0);
            ContainerAbilityChanged(false, abilities.Count);
            api.ChangeSprite(this);
        }
    }

    public virtual void PlayerReleaseAbilities(Player player)
    {
        if(abilities.Count<4)
        {
            abilities.Add(player.abilities[0]);
            player.abilities.RemoveAt(0);
            ContainerAbilityChanged(true, abilities.Count);
            api.ChangeSprite(this);
        }

   }
    public virtual void PlayerAbsorb(Player player)
    {
        if (abilities.Count == 0)
            return;
        else if (player.abilities.Count == 0)
            PlayerAbsorbAbilities(player);
        else
        {
            if (abilities[0] == player.abilities[0])
                PlayerAbsorbAbilities(player);
            else
                Swap(player);
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

    public virtual void PlayerAbsorbHold(Player player)
    {
        if (abilities.Count == 0)
            return;
        else if (player.abilities.Count == 0)
        {
            while (abilities.Count != 0 && player.abilities.Count < 4)
                PlayerAbsorbAbilitiesHold(player);
        }
        else
        {
            if (abilities[0] == player.abilities[0])
            {
                while (abilities.Count != 0 && player.abilities.Count < 4)
                    PlayerAbsorbAbilitiesHold(player);
            }
            /*else
                Swap(player);*/
        }
        api.ChangeSprite(this);
        if(this is FunctionalContainer && ((FunctionalContainer)this).on)
            CheckReservedList();
    }

    public virtual void PlayerReleaseHold(Player player)
    {
        if (player.abilities.Count == 0)
            return;
        else if (abilities.Count == 0)
        {
            while(player.abilities.Count != 0 && abilities.Count < 4)
                PlayerReleaseAbilitiesHold(player);
        }
        else
        {
            if (player.abilities[0] == abilities[0])
            {
                while (player.abilities.Count != 0 && abilities.Count < 4)
                    PlayerReleaseAbilitiesHold(player);
            }
            /*else
                Swap(player);*/
        }
        api.ChangeSprite(this);
        if (this is FunctionalContainer && ((FunctionalContainer)this).on)
            CheckReservedList();
    }
    public virtual void PlayerReleaseAbilitiesHold(Player player)
    {
        if (abilities.Count < 4)
        {
            abilities.Add(player.abilities[0]);
            player.abilities.RemoveAt(0);
            AddToReservedMove(true, abilities.Count);
        }
    }
    private void PlayerAbsorbAbilitiesHold(Player player)
    {
        if (player.abilities.Count < 4)
        {
            player.abilities.Add(abilities[0]);
            abilities.RemoveAt(0);
            AddToReservedMove(false, abilities.Count);
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

    protected virtual void ContainerAbilityChanged(bool increased, int count)
    {
        return;
    }

    protected virtual void AddToReservedMove(bool increased, int count)
    {
        return;
    }

    public virtual void CheckReservedList()
    {
        return;
    }
}

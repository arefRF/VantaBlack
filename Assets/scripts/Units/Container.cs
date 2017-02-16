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
        List<AbilityType> temp = new List<AbilityType>();
        for (int i = 0; i < abilities.Count; i++)
            temp.Add(abilities[i]);
        abilities.Clear();
        abilities = player.abilities;
        player.abilities = temp;
        api.ChangeSprite(this);
        if (this is FunctionalContainer) {
            if ((player.abilities.Count != 0 && player.abilities[0] == AbilityType.Fuel) || (abilities.Count != 0 && abilities[0] == AbilityType.Fuel))
                if(((FunctionalContainer)this).on)
                    ((FunctionalContainer)this).Action_Fuel(true);
        }
            
    }

    private void PlayerAbsorbAbilities(Player player)
    {
        api.AddToSnapshot(this);
        if (this is FunctionalContainer)
            api.AddToSnapshot(ConnectedUnits);
        api.AddToSnapshot(player);
        //api.TakeSnapshot();
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
        api.AddToSnapshot(this);
        if (this is FunctionalContainer)
            api.AddToSnapshot(ConnectedUnits);
        api.AddToSnapshot(player);
        //api.TakeSnapshot();
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
        api.AddToSnapshot(this);
        if (this is FunctionalContainer)
            api.AddToSnapshot(ConnectedUnits);
        api.AddToSnapshot(player);
        if (player.abilities.Count == 0)
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
        api.AddToSnapshot(this);
        api.AddToSnapshot(ConnectedUnits);
        api.AddToSnapshot(player);
        if (abilities.Count == 0)
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

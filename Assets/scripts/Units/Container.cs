﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Container : ParentContainer {
    public AbilityType abilitytype;
    public int abilitycount;
    public List<Ability> abilities;
    public int capacity = 4;

    public override void SetInitialSprite()
    {
        bool[] notconnected = Toolkit.GetConnectedSides(this);
        if (notconnected[0] && notconnected[1] && notconnected[2] && notconnected[3])
            gameObject.GetComponent<SpriteRenderer>().sprite = api.engine.initializer.sprite_Container[1];
        else if (notconnected[0] && notconnected[1] && notconnected[2])
            gameObject.GetComponent<SpriteRenderer>().sprite = api.engine.initializer.sprite_Container[2];
        else if (notconnected[0] && notconnected[2] && notconnected[3])
            gameObject.GetComponent<SpriteRenderer>().sprite = api.engine.initializer.sprite_Container[3];
        else if (notconnected[0] && notconnected[1] && notconnected[3])
            gameObject.GetComponent<SpriteRenderer>().sprite = api.engine.initializer.sprite_Container[4];
        else if (notconnected[1] && notconnected[2] && notconnected[3])
            gameObject.GetComponent<SpriteRenderer>().sprite = api.engine.initializer.sprite_Container[5];
        else if (notconnected[0] && notconnected[2])
            gameObject.GetComponent<SpriteRenderer>().sprite = api.engine.initializer.sprite_Container[6];
        else if (notconnected[0] && notconnected[1])
            gameObject.GetComponent<SpriteRenderer>().sprite = api.engine.initializer.sprite_Container[7];
        else if (notconnected[1] && notconnected[3])
            gameObject.GetComponent<SpriteRenderer>().sprite = api.engine.initializer.sprite_Container[8];
        else if (notconnected[0] && notconnected[3])
            gameObject.GetComponent<SpriteRenderer>().sprite = api.engine.initializer.sprite_Container[9];
        else if (notconnected[2] && notconnected[3])
            gameObject.GetComponent<SpriteRenderer>().sprite = api.engine.initializer.sprite_Container[10];
        else if (notconnected[1] && notconnected[2])
            gameObject.GetComponent<SpriteRenderer>().sprite = api.engine.initializer.sprite_Container[11];
        else if (notconnected[0])
            gameObject.GetComponent<SpriteRenderer>().sprite = api.engine.initializer.sprite_Container[12];
        else if (notconnected[1])
            gameObject.GetComponent<SpriteRenderer>().sprite = api.engine.initializer.sprite_Container[13];
        else if (notconnected[2])
            gameObject.GetComponent<SpriteRenderer>().sprite = api.engine.initializer.sprite_Container[14];
        else if (notconnected[3])
            gameObject.GetComponent<SpriteRenderer>().sprite = api.engine.initializer.sprite_Container[15];
        api.ChangeSprite(this);
    }

    void Start()
    {
        
    }

    private void Swap(Player player)
    {
        List<Ability> temp = new List<Ability>();
        for (int i = 0; i < abilities.Count; i++)
            temp.Add(abilities[i]);
        abilities.Clear();
        abilities = player.abilities;
        player.abilities = temp;
        api.ChangeSprite(this);
        if (this is FunctionalContainer) {
            if ((player.abilities.Count != 0 && player.abilities[0].abilitytype == AbilityType.Fuel))
            {
                if (((FunctionalContainer)this).on)
                    ((FunctionalContainer)this).Action_Fuel(false);
                else
                {
                    for(int i=0; i<player.abilities.Count; i++)
                    {
                        ContainerAbilityChanged(false, player.abilities.Count - i);
                    }
                }
            }
            else if((abilities.Count != 0 && abilities[0].abilitytype == AbilityType.Fuel))
            {
                if (((FunctionalContainer)this).on)
                    ((FunctionalContainer)this).Action_Fuel(false);
                else
                {
                    for (int i = 0; i < abilities.Count; i++)
                        ContainerAbilityChanged(true, i+1);
                }
            }
        }
            
    }

    private void PlayerAbsorbAbilities(Player player)
    {
        if(player.abilities.Count<4)
        {
            player.abilities.Add(abilities[0]);
            abilities.RemoveAt(0);
            ContainerAbilityChanged(false, abilities.Count);
            api.ChangeSprite(this);
            _setability(player);
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
            _setability(player);
        }

   }


    public virtual void PlayerRelease(Player player)
    {
        api.AddToSnapshot(this);
        if (this is FunctionalContainer)
            api.AddToSnapshot(ConnectedUnits);
        api.AddToSnapshot(player);
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
        _setability(player);
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
    private void _setability(Player player)
    {
        abilitycount = abilities.Count;
        player.abilitycount = player.abilities.Count;
        if (abilities.Count != 0)
            abilitytype = abilities[0].abilitytype;
        if (player.abilities.Count != 0)
            player.abilitytype = player.abilities[0].abilitytype;
    }
    public virtual void PlayerAbsorb(Player player)
    {
        api.AddToSnapshot(this);
        if (this is FunctionalContainer)
            api.AddToSnapshot(ConnectedUnits);
        api.AddToSnapshot(player);
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
}

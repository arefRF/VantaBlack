using UnityEngine;
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
        bool[] notconnected = Toolkit.GetConnectedSidesForContainer(this);
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
            temp.Add(abilities[i].ConvertContainerAbilityToPlayer());
        abilities.Clear();
        for (int i = 0; i < player.abilities.Count; i++)
            abilities.Add(player.abilities[i].ConvertPlayerAbilityToContainer());
        player.abilities = temp;
        api.ChangeSprite(this);
        _setability(player);
        api.engine.apigraphic.Absorb(player, this);
        if (this is FunctionalContainer) {
            if ((player.abilities.Count != 0 && player.abilities[0].abilitytype == AbilityType.Fuel))
            {
                for(int i=0; i<player.abilities.Count; i++)
                {
                    AddToReservedMove(false, player.abilities.Count - i);
                }
                if (((FunctionalContainer)this).on)
                {
                    CheckReservedList();
                }
            }
            else if((abilities.Count != 0 && abilities[0].abilitytype == AbilityType.Fuel))
            {
                for (int i = 0; i < player.abilities.Count; i++)
                {
                    AddToReservedMove(false, i);
                }
                if (((FunctionalContainer)this).on)
                {
                    CheckReservedList();
                }
            }
        }
    }

    private void PlayerAbsorbAbilities(Player player)
    {
        if(player.abilities.Count<4)
        {
            player.abilities.Add(abilities[0].ConvertContainerAbilityToPlayer());
            abilities.RemoveAt(0);
            api.ChangeSprite(this);
            _setability(player);
            api.engine.apigraphic.Absorb(player, this);
            ContainerAbilityChanged(false, abilities.Count);
        }
    }

    public virtual void PlayerReleaseAbilities(Player player)
    {
        if(abilities.Count<4)
        {
            abilities.Add(player.abilities[0].ConvertPlayerAbilityToContainer());
            player.abilities.RemoveAt(0);
            api.ChangeSprite(this);
            _setability(player);
            api.engine.apigraphic.Absorb(player, this);
            ContainerAbilityChanged(true, abilities.Count);
        }

   }

    public virtual void PlayerAbsorb(Player player)
    {
        if (gameObject.transform.parent.gameObject.GetComponent<ParentScript>().movelock)
            return;
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
            if (abilities[0].abilitytype == player.abilities[0].abilitytype)
                PlayerAbsorbAbilities(player);
            else
                Swap(player);
        }

    }

    public virtual void PlayerRelease(Player player)
    {
        if (gameObject.transform.parent.gameObject.GetComponent<ParentScript>().movelock)
            return;
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
            if (player.abilities[0].abilitytype == abilities[0].abilitytype)
                PlayerReleaseAbilities(player);
            else
                Swap(player);
        }
        
    }

    public virtual void PlayerAbsorbHold(Player player)
    {
        if (gameObject.transform.parent.gameObject.GetComponent<ParentScript>().movelock)
            return;
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
        if (gameObject.transform.parent.gameObject.GetComponent<ParentScript>().movelock)
            return;
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
            abilities.Add(player.abilities[0].ConvertPlayerAbilityToContainer());
            player.abilities.RemoveAt(0);
            api.engine.apigraphic.Absorb(player, this);
            AddToReservedMove(true, abilities.Count);
        }
    }
    private void PlayerAbsorbAbilitiesHold(Player player)
    {
        if (player.abilities.Count < 4)
        {
            player.abilities.Add(abilities[0].ConvertContainerAbilityToPlayer());
            abilities.RemoveAt(0);
            api.engine.apigraphic.Absorb(player, this);
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
    protected void _setability(Player player)
    {
        abilitycount = abilities.Count;
        player.abilitycount = player.abilities.Count;
        if (abilities.Count != 0)
            abilitytype = abilities[0].abilitytype;
        if (player.abilities.Count != 0)
            player.abilitytype = player.abilities[0].abilitytype;
    }
    
}

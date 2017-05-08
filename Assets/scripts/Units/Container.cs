using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Container : ParentContainer {
    public AbilityType abilitytype;
    public int abilitycount;
    public List<Ability> abilities;
    public int capacity = 4;
    public List<Container> sameContainer;

    public override void SetInitialSprite()
    {/*
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
        api.ChangeSprite(this); */
    }

    void Start()
    {
        
    }

    private void Swap(Player player)
    {
        if (abilities.Count > 4)
            return;
        List<Ability> temp = new List<Ability>();
        for (int i = 0; i < abilities.Count; i++)
            temp.Add(abilities[i].ConvertContainerAbilityToPlayer(player));
        abilities.Clear();
        for (int i = 0; i < player.abilities.Count; i++)
            abilities.Add(player.abilities[i].ConvertPlayerAbilityToContainer(this));
        player.abilities = temp;
        api.ChangeSprite(this);
        _setability(player);
        api.engine.apigraphic.Absorb(player, this);
        SetNextState();
        if (this is FunctionalContainer && player.abilities.Count != 0 && player.abilities[0].abilitytype == AbilityType.Fuel && ((FunctionalContainer)this).on) { //shayad moshkel dashte bashe
            ((FunctionalContainer)this).SetOnorOff();
            ((FunctionalContainer)this).Action_Fuel();
        }
    }

    private void PlayerAbsorbAbilities(Player player)
    {
        if(player.abilities.Count<4)
        {
            player.abilities.Add(abilities[0].ConvertContainerAbilityToPlayer(player));
            abilities.RemoveAt(0);
            for(int i=0; i<sameContainer.Count; i++)
            {
                sameContainer[i].abilities.RemoveAt(0);
            }
            api.ChangeSprite(this);
            _setability(player);
            SetNextState();
            if(this is FunctionalContainer)
            {
                if (((FunctionalContainer)this).on)
                {
                    if (abilities.Count == 0)
                    {
                        ((FunctionalContainer)this).SetOnorOff();
                        ((FunctionalContainer)this).Action_Fuel();
                    }
                    else if (abilities[0].abilitytype == AbilityType.Fuel)
                    {
                        SetNextState();
                        ((FunctionalContainer)this).Action_Fuel();
                    }
                }
            }
        }
    }

    public virtual void PlayerReleaseAbilities(Player player)
    {
        if(abilities.Count<capacity)
        {
            abilities.Add(player.abilities[0].ConvertPlayerAbilityToContainer(this));
            for(int i=0; i<sameContainer.Count; i++)
            {
                sameContainer[i].abilities.Add(abilities[abilities.Count - 1]);
            }
            player.abilities.RemoveAt(0);
            api.ChangeSprite(this);
            _setability(player);
            if (this is FunctionalContainer)
            {
                if (((FunctionalContainer)this).on)
                {
                    if (abilities.Count != 0 && abilities[0].abilitytype == AbilityType.Fuel)
                    {
                        SetNextState();
                        ((FunctionalContainer)this).Action_Fuel();
                    }
                }
            }
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
            if (abilities[0].abilitytype == player.abilities[0].abilitytype)
            {
                while (abilities.Count != 0 && player.abilities.Count < 4)
                    PlayerAbsorbAbilitiesHold(player);
            }
            /*else
                Swap(player);*/
        }
        api.ChangeSprite(this);
        _setability(player);
        if (this is FunctionalContainer)
        {
            if (((FunctionalContainer)this).on)
            {
                if (abilities.Count == 0)
                {
                    ((FunctionalContainer)this).SetOnorOff();
                    ((FunctionalContainer)this).Action_Fuel();
                }
                else if (abilities[0].abilitytype == AbilityType.Fuel)
                {
                    SetNextState();
                    ((FunctionalContainer)this).Action_Fuel();
                }
            }
        }
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
            while(player.abilities.Count != 0 && abilities.Count < capacity)
                PlayerReleaseAbilitiesHold(player);
        }
        else
        {
            if (player.abilities[0].abilitytype == abilities[0].abilitytype)
            {
                while (player.abilities.Count != 0 && abilities.Count < capacity)
                    PlayerReleaseAbilitiesHold(player);
            }
            /*else
                Swap(player);*/
        }
        api.ChangeSprite(this);
        _setability(player);
        api.engine.apigraphic.Absorb(player, null);
        if (this is FunctionalContainer)
        {
            if (((FunctionalContainer)this).on)
            {
                if (abilities.Count != 0 && abilities[0].abilitytype == AbilityType.Fuel)
                {
                    SetNextState();
                    ((FunctionalContainer)this).Action_Fuel();
                }
            }
        }
    }
    public virtual void PlayerReleaseAbilitiesHold(Player player)
    {
        if (abilities.Count < capacity)
        {
            abilities.Add(player.abilities[0].ConvertPlayerAbilityToContainer(this));
            for (int i = 0; i < sameContainer.Count; i++)
            {
                sameContainer[i].abilities.Add(abilities[abilities.Count - 1]);
            }
            player.abilities.RemoveAt(0);
            api.engine.apigraphic.Absorb(player, this);
        }
    }
    private void PlayerAbsorbAbilitiesHold(Player player)
    {
        if (player.abilities.Count < 4)
        {
            player.abilities.Add(abilities[0].ConvertContainerAbilityToPlayer(player));
            abilities.RemoveAt(0);
            for (int i = 0; i < sameContainer.Count; i++)
            {
                sameContainer[i].abilities.RemoveAt(0);
            }
            api.engine.apigraphic.Absorb(player, this);
        }
    }

    public virtual void SetNextState()
    {

    }

    public override bool PlayerMoveInto(Direction dir)
    {
        return false;
    }

    public override void Action(Player player, Direction dir)
    {
        return;
    }

    public void _setability(Player player)
    {
        abilitycount = abilities.Count;
        for (int i = 0; i < sameContainer.Count; i++)
            sameContainer[i].abilitycount = abilitycount;
        if(player != null)
            player.abilitycount = player.abilities.Count;
        if (abilities.Count != 0)
        {
            abilitytype = abilities[0].abilitytype;
            for (int i = 0; i < sameContainer.Count; i++)
                sameContainer[i].abilitytype = abilitytype;
        }
        if (player != null)
        {
            if (player.abilities.Count != 0)
                player.abilitytype = player.abilities[0].abilitytype;
            api.engine.apigraphic.Absorb(player, null);
        }
    }
    

    public void PipeAbsorb(Container othercontainer)
    {
        int counter = 0;
        if (abilities.Count != 0 && othercontainer.abilities.Count == 0)
            return;
        for (int i = 0; i < othercontainer.abilities.Count && abilities.Count < capacity; i++)
        {
            abilities.Add(othercontainer.abilities[0]);
            counter++;
        }
        for (int i = 0; othercontainer.abilities.Count > 0 && i < counter; i++)
            othercontainer.abilities.RemoveAt(0);
        //othercontainer.abilities.Clear();
        _setability(null);
        othercontainer._setability(null);
        api.engine.apigraphic.UnitChangeSprite(this);
        api.engine.apigraphic.UnitChangeSprite(othercontainer);
        if(othercontainer is DynamicContainer)
        {
            if (((DynamicContainer)othercontainer).on && abilities.Count > 0 && abilities[0].abilitytype == AbilityType.Fuel)
            {
                ((DynamicContainer)othercontainer).SetOnorOff();
                ((DynamicContainer)othercontainer).Action_Fuel();
            }
        }
    }
}


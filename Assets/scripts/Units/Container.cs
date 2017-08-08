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
    {
        bool[] connected = Toolkit.GetConnectedSidesForContainer(this);
        int sidecount = 0;
        for (int i = 0; i < 4; i++)
            sidecount += System.Convert.ToInt32(connected[i]);
        switch (sidecount)
        {
            case 1: Connected_1(connected); break;
            case 2: Connected_2(connected); break;
            case 3: Connected_3(connected); break;
            case 4: Connected_4(connected); break;


        }
        SetCapacityLight();
    }

    private void Connected_1(bool[] connected)
    {
        Transform tr = Toolkit.GetObjectInChild(gameObject, "Connected").transform;
        tr.GetComponent<SpriteRenderer>().sprite = api.engine.initializer.sprite_Container[0];
        if (connected[1])
            tr.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 270);
        if (connected[2])
            tr.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 180);
        if (connected[3])
            tr.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 90);
    }
    private void Connected_2(bool[] connected)
    {
        Transform tr = Toolkit.GetObjectInChild(gameObject, "Connected").transform;
        if (connected[0] && connected[2])
            tr.GetComponent<SpriteRenderer>().sprite = api.engine.initializer.sprite_Container[2];
        else if (connected[1] && connected[3])
        {
            tr.GetComponent<SpriteRenderer>().sprite = api.engine.initializer.sprite_Container[2];
            tr.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 270);
        }
        else
        {
            tr.GetComponent<SpriteRenderer>().sprite = api.engine.initializer.sprite_Container[1];
            if (connected[1] && connected[2])
                tr.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 270);
            else if (connected[2] && connected[3])
                tr.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 180);
            else if (connected[3] && connected[0])
                tr.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 90);
        }
    }
    private void Connected_3(bool[] connected)
    {
        Transform tr = Toolkit.GetObjectInChild(gameObject, "Connected").transform;
        tr.GetComponent<SpriteRenderer>().sprite = api.engine.initializer.sprite_Container[3];
        if (!connected[0])
            tr.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 270);
        if (!connected[1])
            tr.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 180);
        if (!connected[2])
            tr.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 90);
    }
    private void Connected_4(bool[] connected)
    {
        Toolkit.GetObjectInChild(gameObject, "Connected").GetComponent<SpriteRenderer>().sprite = api.engine.initializer.sprite_Container[4];
    }
    void Start()
    {
        
    }

    private void Swap(Player player)
    {
        if (abilities.Count > 4)
            return;
        if (player.abilities.Count > capacity)
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

    public virtual void SetCapacityLight()
    {
        
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
        if (graphicalactions != null)
        {
            for (int i = 0; i < graphicalactions.Count; i++)
            {
                if (graphicalactions[i] is ActionUponAbsorb)
                {
                    graphicalactions[i].Action();
                }
            }
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
        if (graphicalactions != null)
        {
            for (int i = 0; i < graphicalactions.Count; i++)
            {
                if (graphicalactions[i] is ActionUponRelease)
                {
                    graphicalactions[i].Action();
                }
            }
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
        if (graphicalactions != null)
        {
            for (int i = 0; i < graphicalactions.Count; i++)
            {
                if (graphicalactions[i] is ActionUponAbsorb)
                {
                    graphicalactions[i].Action();
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
        if (graphicalactions != null)
        {
            for (int i = 0; i < graphicalactions.Count; i++)
            {
                if (graphicalactions[i] is ActionUponRelease)
                {
                    graphicalactions[i].Action();
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
            player._setability();
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


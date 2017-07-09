using UnityEngine;
using System.Collections.Generic;

public class Fountain : Unit {

    public AbilityType ability;
    public int count;
    public List<Ability> abilities;
    private Animator animator;
    public override void Run()
    {
        abilities = new List<Ability>();
        api.engine.apigraphic.UnitChangeSprite(this);
        base.Run();
    }


    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public override void SetInitialSprite()
    {
        bool[] notcoonected = Toolkit.GetConnectedSides(this);
        for(int i=0; i<4; i++)
        {
            if (!notcoonected[i])
                transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = api.engine.initializer.sprite_Fountain[i];
        }
    }

    public void Action(Player player)
    {
        
        if(player.abilities.Count == 0)
        {
            
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
        api.engine.apigraphic.UnitChangeSprite(this);
    }

    private bool UndoAbilities(Player player)
    {
        if (abilities.Count == 0)
            return false;
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
        abilities.Clear();

        return true;
    }

    public void PlayerLeaned(Player player, Direction direction)
    {
        player.LeanedTo = this;
        player.SetState(PlayerState.Lean);
        player.leandirection = direction;
        player.isonejumping = false;
        player.SetState(PlayerState.Lean);
        api.engine.apigraphic.Player_Co_Stop(player);
        player.currentAbility = null;
        api.engine.apiinput.leanlock = true;
        api.engine.apigraphic.Lean(player);
        if (UndoAbilities(player))
        {
            animator.SetBool("Open", false);
            api.engine.apigraphic.UnitChangeSprite(this);
        }
    }
    
    public void PlayerLeanUndo(Player player)
    {
        if(abilities.Count > 0 )
            animator.SetBool("Open", true);
    }
}

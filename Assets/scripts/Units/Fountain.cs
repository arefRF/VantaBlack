using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Fountain : Unit {

    public AbilityType ability;
    public int count;
    public List<Ability> abilities;
    private Animator animator;
    private bool[] connected;
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
        connected = Toolkit.GetConnectedSidesForLaser(this);
        for(int i=0; i<4; i++)
        {
            if (!connected[i])
                transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = api.engine.initializer.sprite_Fountain[i + 8];
            else
            {
                Vector2 pos = Toolkit.VectorSum(position, Toolkit.NumberToDirection(i + 1));
                if (Toolkit.HasBranch(pos) || Toolkit.HasLaser(pos))
                    transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = api.engine.initializer.sprite_Fountain[i + 4];
                else
                    transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = api.engine.initializer.sprite_Fountain[i];
            }
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

    private bool ResetFountatin(Player player)
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
                        if ((abilities[i].owner as FunctionalContainer).abilities.Count == 0)
                            ((FunctionalContainer)abilities[i].owner).SetOnorOff();
                        ((FunctionalContainer)abilities[i].owner).firstmove = true;
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
        api.engine.apigraphic.Lean(player);
        // Reseting the Abilities
        if (ResetFountatin(player))
        {
            CloseFountatin();
            api.engine.apigraphic.UnitChangeSprite(this);
        }
    }

    private void CloseFountatin()
    {
        for(int i = 0; i < 4; i++)
            switch (i)
            {
                case 0: StartCoroutine(SmoothMove(transform.GetChild(i), new Vector2(0, 1.25f))); break;
                case 1: StartCoroutine(SmoothMove(transform.GetChild(i), new Vector2(1.25f, 0f))); break;
                case 2: StartCoroutine(SmoothMove(transform.GetChild(i), new Vector2(0, -1.25f))); break;
                case 3: StartCoroutine(SmoothMove(transform.GetChild(i), new Vector2(-1.25f, 0))); break;
            }
    }
    public void PlayerLeanUndo(Player player)
    {
        if (abilities.Count > 0)
        {
            // To see what part of the Fountatin Should go up
            for (int i = 0; i < 4; i++)
                if (!connected[i])
                {
                    switch (i)
                    {
                        case 0: StartCoroutine(SmoothMove(transform.GetChild(i), new Vector2(0, 1.37f))); break;
                        case 1: StartCoroutine(SmoothMove(transform.GetChild(i), new Vector2(1.37f, 0f))); break;
                        case 2: StartCoroutine(SmoothMove(transform.GetChild(i), new Vector2(0, -1.37f))); break;
                        case 3: StartCoroutine(SmoothMove(transform.GetChild(i), new Vector2(-1.37f, 0))); break;
                    }
                }
        }
    }

    private IEnumerator SmoothMove(Transform obj,Vector2 target)
    {
        float remain = ((Vector2)obj.localPosition - target).sqrMagnitude;
        while (remain > float.Epsilon)
        {
            remain = ((Vector2)obj.localPosition - target).sqrMagnitude;
            obj.localPosition = Vector2.MoveTowards(obj.localPosition, target, Time.deltaTime / 5);
            yield return null;
        }

    }

}

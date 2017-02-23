using UnityEngine;
using System.Collections;

public class Ability{

    public AbilityType abilitytype;
    public bool used;
    public Ability()
    {
        used = false;
    }

    public static Ability GetAbilityInstance(AbilityType abilitytype)
    {
        switch (abilitytype)
        {
            case AbilityType.Fuel: return new Fuel();
            case AbilityType.Jump: return new Jump();
            case AbilityType.Key: return new Key();
            default: return null;
        }
    }


    public virtual Ability ConvertPlayerAbilityToContainer()
    {
        return this;
    }

    public virtual Ability ConvertContainerAbilityToPlayer()
    {
        return this;
    }
}

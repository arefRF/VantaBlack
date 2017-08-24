using UnityEngine;
using System.Collections;

public class Ability{

    public AbilityType abilitytype;
    public bool used;
    public Unit owner;
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
            case AbilityType.Teleport: return new Teleport();
            case AbilityType.Gravity: return new Gravity();
            case AbilityType.Rope: return new Rope();
            case AbilityType.Laser: return new LaserAbility();
            default: return null;
        }
    }


    public virtual Ability ConvertPlayerAbilityToContainer(Container container)
    {
        owner = container;
        return this;
    }

    public virtual Ability ConvertContainerAbilityToPlayer(Player player)
    {
        owner = player;
        return this;
    }
}

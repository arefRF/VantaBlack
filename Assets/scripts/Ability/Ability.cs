using UnityEngine;
using System.Collections;

public class Ability{

    public AbilityType abilitytype;
    public Ability()
    {

    }
	public void Action_Jump(Player player, int jumpnumber, Direction jumpdirection)
    {

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
}

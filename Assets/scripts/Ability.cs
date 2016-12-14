using UnityEngine;
using System.Collections.Generic;

public class Ability : MonoBehaviour{
    public AbilityType abilitytype;
    public List<Direction> direction; //for gravity and blink abilities
    public int numberofuse;
    public int number;
}

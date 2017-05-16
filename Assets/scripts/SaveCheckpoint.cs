using UnityEngine;
using System.Collections;

public class SaveCheckpoint {

    public Vector2 position;
    public int abilitycount;
    public AbilityType abilitytype;


    public void Save(Player player)
    {
        position = player.position;
        abilitycount = player.abilitycount;
        abilitytype = player.abilitytype;
    }
}

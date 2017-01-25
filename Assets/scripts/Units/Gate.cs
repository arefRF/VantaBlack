using UnityEngine;
using System.Collections;

public class Gate : Container {

    public string sceneName;

    public void Start()
    {
        capacity = 1;
    }

    public override bool PlayerMoveInto(Direction dir)
    {
        return false;
    }

    public override void PlayerRelease(Player player)
    {
        if (capacity == abilities.Count)
            return;
        if (player.abilities[0] != AbilityType.Key)
            return;
        PlayerReleaseAbilities(player);
    }

    public override void PlayerAbsorb(Player player)
    {
        return;
    }
}

using UnityEngine;
using System.Collections;

public class LeanableC1_RemoteOn : Leanable {

    public Bot bot;

    public override void Run()
    {
        abilities = new System.Collections.Generic.List<Ability>();
        base.Run();
    }

    // Use this for initialization
    void Start () {
        capacity = 1;
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public override void PlayerAbsorb(Player player)
    {
        return;
    }

    public override void PlayerRelease(Player player)
    {
        if (abilities.Count != 0)
            return;
        if (player.abilities.Count == 0)
            return;
        if (player.abilities[0].abilitytype != AbilityType.Fuel)
            return;
        abilities.Add(player.abilities[0]);
        player.abilities.RemoveAt(0);
        _setability(player);
    }

    public override void PlayerAbsorbHold(Player player)
    {
        return;
    }

    public override void PlayerReleaseHold(Player player)
    {
        PlayerRelease(player);
    }

    public override void LeanedAction(Player player, Direction direction)
    {
        if (abilities.Count == 1)
        {
            bot.TurnOn();
            api.engine.inputcontroller.LeanUndo(player, Toolkit.ReverseDirection(direction), PlayerState.Idle);
        }
    }
}

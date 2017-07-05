using UnityEngine;
using System.Collections;

public class LeanableC0_RemoteOn : Leanable {

    public Bot bot;
	// Use this for initialization
	void Start () {
        capacity = 0;
    }

    public override void LeanedAction(Player player, Direction direction)
    {
        bot.TurnOn();
    }
}

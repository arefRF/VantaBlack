using UnityEngine;
using System.Collections.Generic;

public class Drainer : Unit {

    List<Vector2> position_drain;

    public override void Run()
    {
        position_drain = new List<Vector2>();
        position_drain.Add(Toolkit.VectorSum(position, Direction.Up));
        position_drain.Add(Toolkit.VectorSum(position, Direction.Right));
        position_drain.Add(Toolkit.VectorSum(position, Direction.Down));
        position_drain.Add(Toolkit.VectorSum(position, Direction.Left));
        base.Run();
    }

    public void Check(Player player)
    {
        for (int i = 0; i < position_drain.Count; i++)
        {
            if (player.position == position_drain[i])
            {
                Drain(player);
            }
        }
    }

    public void Drain(Player player)
    {
        player.SetState(PlayerState.Busy);
        api.engine.apigraphic.Drain(player, this);
    }
}

using UnityEngine;
using System.Collections;

public class Teleport : Ability {

    int length;
    private int teleporttype; //1 teleport blink  2 portal
    LogicalEngine engine;
    public Teleport()
    {
        abilitytype = AbilityType.Teleport;
        length = 2;
        engine = Starter.GetEngine();
    }

    public void Action(Player player,Direction dir)
    {
        Debug.Log("action called");
        if(engine == null)
            engine = Starter.GetEngine();
        Vector2 pos = player.position + Toolkit.DirectiontoVector(dir) * length ;
        int x = (int)pos.x;
        int y = (int)pos.y;
        if(engine.database.units[x,y].Count == 0)
        {
            player.currentAbility = this;
            player.UseAbility(this);
            player.state = PlayerState.Busy;
            engine.apiunit.RemoveFromDatabase(player);
            player.position = pos;
            engine.apiunit.AddToDatabase(player);
            engine.apigraphic.Teleport(player, pos);
        }
    }
    private void Action_player(Player player, Direction dir)
    {
        /*Vector2 destpos = Toolkit.VectorSum(player.position, 2 * Toolkit.DirectiontoVector(dir));
        if (Toolkit.IsEmpty(destpos))
        {
            engine.apiunit.RemoveFromDatabase(player);
            player.position = 
        }*/
    }

    private void Action_Container(Player player, Direction dir, Container container)
    {

    }

    public override Ability ConvertContainerAbilityToPlayer()
    {
        teleporttype = 1;
        return this;
    }

    public override Ability ConvertPlayerAbilityToContainer()
    {
        teleporttype = 2;
        return this;
    }
}

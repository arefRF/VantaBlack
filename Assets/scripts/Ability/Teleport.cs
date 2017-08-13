using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Teleport : Ability {

    int length;
    private int teleporttype; //1 teleport blink  2 portal
    LogicalEngine engine;
    private List<FunctionalContainer> functionalContainers;
    APIGraphic api_graphic;
    APIInput api_input;
    APIUnit api_unit;
    private int current;
    private List<Unit> portals;
    private Direction dir;
    private Player player;
    public Teleport()
    {
        abilitytype = AbilityType.Teleport;
        length = 2;
        engine = Starter.GetEngine();
    }

    public void Action(Player player,Direction dir)
    {
        if(engine == null)
            engine = Starter.GetEngine();
        Vector2 pos = player.position + Toolkit.DirectiontoVector(dir) * length ;
        int x = (int)pos.x;
        int y = (int)pos.y;
        if(engine.database.units[x,y].Count == 0)
        {
            player.currentAbility = this;
            player.UseAbility(this);
            player.SetState(PlayerState.Busy);
            engine.apiunit.RemoveFromDatabase(player);
            player.position = pos;
            engine.apiunit.AddToDatabase(player);
            engine.apigraphic.Teleport(player, pos);
        }
    }
    private void SetFunctionalCons()
    {
        functionalContainers = Starter.GetDataBase().functionalCon;
        api_graphic = engine.apigraphic;
        api_input = engine.apiinput;
        api_unit = engine.apiunit;
    }

    public void Action_Container(Player the_player, Direction direction, Container container)
    {
        dir = Toolkit.ReverseDirection(direction);
        player = the_player;
        if (functionalContainers == null)
            SetFunctionalCons();
        portals = new List<Unit>();
        for(int i = 0; i < functionalContainers.Count; i++)
        {
            if (functionalContainers[i].abilities.Count != 0)
                if (functionalContainers[i].abilities[0].abilitytype == AbilityType.Teleport)
                    portals.Add(functionalContainers[i]);
        }
        portals = Toolkit.SortByDirectionNearest(portals, Direction.Left,container);
        for (int i = 0; i < portals.Count; i++)
            if (portals[i] == container)
                current = i;

        api_graphic.EnterPortalMode(portals,container);
        api_input.EnterPortalMode(this);

    }

    public void ArrowKeyPressed(Direction dir)
    {
        int pre = current;
        if (dir == Direction.Right || dir == Direction.Up)
            current++;
        else
            current--;
        current = current % portals.Count;
        if (current < 0)
            current = portals.Count + current;
        api_graphic.ProtalHighlight(portals[current], portals[pre]);
    }

    // Quit Portal Mode by Walking
    public void Detach()
    {
        if (player.state == PlayerState.Lean)
        {
            player.SetState(PlayerState.Idle);
            player.LeanUndoPortal();
            api_graphic.QuitPortalMode(portals);
        }
    }

    
    public void Port()
    {
        player.SetState(PlayerState.Busy);
        Vector2 pos = portals[current].position + Toolkit.DirectiontoVector(dir);
        int x = (int)pos.x;
        int y = (int)pos.y;
        if (engine.database.units[x, y].Count == 0)
        {
            api_unit.RemoveFromDatabase(player);
            player.position = portals[current].position + Toolkit.DirectiontoVector(dir);
            player.leandirection = Toolkit.ReverseDirection(dir);
            api_unit.AddToDatabase(player);
            api_graphic.Port(player, pos);
            api_graphic.QuitPortalMode(portals);
            api_input.QuitPortalMode();
        }
    }

    public override Ability ConvertContainerAbilityToPlayer(Player player)
    {
        teleporttype = 1;
        owner = player;
        return this;
    }

    public override Ability ConvertPlayerAbilityToContainer(Container container)
    {
        teleporttype = 2;
        owner = container;
        return this;
    }
}

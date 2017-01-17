using UnityEngine;
using System.Collections;

public class AandR {
    GraphicalEngine Gengine;
    Player player;
    Database database;
    LogicalEngine engine;

	public AandR(LogicalEngine engine)
    {
        this.Gengine = engine.Gengine;
        this.player = engine.player;
        this.database = engine.database;
        this.engine = engine;
    }
    

    public void Absorb(Direction dir)
    {
        Unit unit = GetBlockandContainer(Toolkit.VectorSum(player.transform.position, Toolkit.DirectiontoVector(dir)));
        if (unit != null)
        {
            if (unit.unitType == UnitType.Container)
            {
                DoContainer((Container)unit);
                Gengine._Container_Change_Sprite((Container)unit);
            }
        }
    }
    private void DoContainer(Container container)
    {
        if(container.ability == null)
        {
            _release(container);
            return;
        }
        else if(player.ability == null)
        {
            _absorb(container);
            return;
        }
        else if(container.ability.abilitytype == player.ability.abilitytype)
        {
            player.ability.numberofuse = Mathf.Min(4, player.ability.numberofuse + container.ability.numberofuse);
            container.ability.numberofuse = Mathf.Max(0, player.ability.numberofuse + container.ability.numberofuse - 4);
            return;
        }
        else
        {
            _swap(container);
            return;
        }
    }

    private void _absorb(Container container)
    {
        player.ability = container.ability;
        container.ability = null;       
    }

    private void _release(Container container)
    {
        container.ability = player.ability;
        player.ability = null;
    }

    private void _swap(Container container)
    {
        Ability abil = player.ability;
        player.ability = container.ability;
        container.ability = abil;
    }
   
    private Unit GetBlockandContainer(Vector2 position)
    {
        foreach (Unit u in database.units[(int)position.x, (int)position.y])
        {
            if (u.unitType == UnitType.Block || u.unitType == UnitType.Container)
                return u;
        }

        return null;
    }
}

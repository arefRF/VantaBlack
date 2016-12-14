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
        if (Toolkit.IsWallOnTheWay(player.transform.position, dir))
            return;
        Unit unit = null;
        switch (dir)
        {
            case Direction.Down: unit = GetBlockandContainer(Toolkit.VectorSum(player.transform.position, new Vector2(0, -1))); break;
            case Direction.Up: unit = GetBlockandContainer(Toolkit.VectorSum(player.transform.position, new Vector2(0, 1))); break;
            case Direction.Right: unit = GetBlockandContainer(Toolkit.VectorSum(player.transform.position, new Vector2(1, 0))); break;
            case Direction.Left: unit = GetBlockandContainer(Toolkit.VectorSum(player.transform.position, new Vector2(-1, 0))); break;
        }
        if (unit != null)
        {
            if (unit.unitType == UnitType.Block)
            {
                engine.AddToSnapshot(player);
                engine.AddToSnapshot(unit);
                _absorb(((Block)unit).GetComponent<Block>());
                engine.NextTurn();
            }
            else if (unit.unitType == UnitType.Container)
            {
                if (DoContainer(unit))
                {
                    Gengine._Container_Change_Sprite((Container)unit);
                    engine.NextTurn();
                }
            }
        }
    }
    
    public void Absorb()
    {
        if (Toolkit.IsWallOnTheWay(player.transform.position, database.gravity_direction))
            return;
        Unit unit = null;
        switch (database.gravity_direction)
        {
            case Direction.Down: unit = GetBlockandContainer(Toolkit.VectorSum(player.transform.position, new Vector2(0, -1)));  break;
            case Direction.Up: unit = GetBlockandContainer(Toolkit.VectorSum(player.transform.position, new Vector2(0, 1))); break;
            case Direction.Right: unit = GetBlockandContainer(Toolkit.VectorSum(player.transform.position, new Vector2(1, 0))); break;
            case Direction.Left: unit = GetBlockandContainer(Toolkit.VectorSum(player.transform.position, new Vector2(-1, 0))); break;
        }

        if (unit != null)
        {
            if (unit.unitType == UnitType.Block)
            {
                engine.AddToSnapshot(player);
                engine.AddToSnapshot(unit);
                _absorb(((Block)unit).GetComponent<Block>());
                engine.NextTurn();
            }
            else if (unit.unitType == UnitType.Container)
            {

                if (DoContainer(unit))
                {
                    Gengine._Container_Change_Sprite((Container)unit);
                    engine.NextTurn();
                }
            }
            
        }
    }

    private bool DoContainer(Unit unit)
    {
        
        return false;
    }

    private Ability _absorb(Container container)
    {
        Ability abil = player.ability;
        player.ability = container.ability;
        container.ability = null;       
        return abil;
    }

    private Ability _release(Container container)
    {
        Ability abil = container.ability;
        container.ability = player.ability;
        player.ability = null;
        return abil;
    }

    public void Drain()
    {
        player.ability = null;
    }
    /// <summary>
    /// Drains the Block :
    /// 1. logical
    /// </summary>
    /// <param name="block"></param>
    public void Drain(Block block)
    {
        block.ability = null;
    }

    //parivate functions

    private void Swap(Block block)
    {
        Ability block_ability = block.ability;
        block.ability = player.ability;
        player.ability = block_ability;

    }

    private void Release(Container container)
    {
        Wall.print("releasing");
        container.ability = player.ability;
        player.ability = null;
        container._lastAbility = null;
        container.forward = true;
        engine.action.RunContainer(container);
    }

    private void Swap(Container container)
    {
        
    }

    

    private void Absorb(Container container)
    {
        
    }

    private void SafeSwap(Container container)
    {
        
    }
    private void _absorb(Block block)
    {
        if (block.ability == null)
        {
            ReleaseAbility(block);
        }
        else if(player.ability == null)
        {
            Swap(block);
            block.CheckPipe();
        }
        else if (block.ability.abilitytype == player.ability.abilitytype)
        {
            if (block.ability.abilitytype == AbilityType.Fuel)
                Swap(block);
            else
            {
                //LevelUpAbility(block)
            }
            block.CheckPipe();
        }
        else
        {
            Swap(block);
        }
        Gengine._Block_Change_Sprite(block);
    }
    private void ReleaseAbility(Block block)
    {
        block.ability = player.ability;
        Drain();
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

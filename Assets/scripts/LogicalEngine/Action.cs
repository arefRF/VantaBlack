using UnityEngine;
using System.Collections.Generic;

public class Action{
    Player player;
    Database database;
    GraphicalEngine Gengine;
    Move move;
    LogicalEngine engine;
    public Action(LogicalEngine engine)
    {
        this.player = engine.player;
        this.database = engine.database;
        this.Gengine = engine.Gengine;
        this.move = engine.moveObject;
        this.engine = engine;
    }

    public void ChangeGravity(Direction direction)
    {
        for (int i = 0; i < player.ability.direction.Count; i++)
        {
            if (direction == player.ability.direction[i])
            {
                Direction temp = database.gravity_direction;
                database.gravity_direction = direction;
                player.ability.direction[i] = temp;
            }
        }
    }
    public void Teleport(Vector2 position)
    {
        database.player.transform.position = position;
        engine.NextTurn();
    }
    public void Rope()
    {
        database.timeLaps.Add(new TimeLaps(player.ability.number, database.player));
    }
    public void ChangeDirection()
    {
        engine.AddToSnapshot(player);
        switch (player.move_direction[0])
        {
            case Direction.Down: player.move_direction[0] = Direction.Up; break;
            case Direction.Up: player.move_direction[0] = Direction.Down; break;
            case Direction.Right: player.move_direction[0] = Direction.Left; break;
            case Direction.Left: player.move_direction[0] = Direction.Right; break;
        }
        engine.NextTurn();
    }
    private bool isvoid1(int x, int y)
    {
        foreach (Unit u in database.units[(int)player.position.x + x, (int)player.position.y + y])
        {
            if (u.unitType == UnitType.Block || u.unitType == UnitType.Container)
                return false;
        }
        return true;
    }
}

using UnityEngine;
using System.Collections.Generic;

public class LogicalEngine {

    APIGraphic apigraphic;
    APIInput apiinput;
    APIUnit apiunit;
    Database database;

    public LogicalEngine()
    {

    }

    public void MoveUnit(Unit unit, Vector2 position)
    {

    }

    public void MovePlayer(Player player, Vector2 position)
    {

    }

    public void Lean(Player player, Direction direction)
    {

    }

    public List<Unit> GetUnits(Vector2 position)
    {
        return database.units[(int)position.x, (int)position.y];
    }
}

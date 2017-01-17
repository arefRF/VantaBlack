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

    public void MovePlayer(Player player, Direction dir)
    {
        List<Unit> units = GetUnits(player.position);
        bool onramp = false;
        for(int i=0; i<units.Count; i++)
        {
            if(units[i] is Ramp)
            {
                onramp = true;
            }
        }
        if (onramp)
            units = GetUnits(Toolkit.VectorSum(Toolkit.VectorSum(player.position, Toolkit.DirectiontoVector(dir)), Toolkit.DirectiontoVector(database.gravity_direction)));
        else
            units = GetUnits(Toolkit.VectorSum(player.position, Toolkit.DirectiontoVector(dir)));
        for(int i=0; i<units.Count; i++)
        {
            if(units[i] is Ramp)
            {
                if (onramp)
                {
                    apigraphic.MovePlayerOnRampFromRamp(player, Toolkit.VectorSum(Toolkit.VectorSum(player.position, Toolkit.DirectiontoVector(dir)), Toolkit.DirectiontoVector(database.gravity_direction)));
                }
            }
        }
    }

    public void Lean(Player player, Direction direction)
    {

    }

    public List<Unit> GetUnits(Vector2 position)
    {
        return database.units[(int)position.x, (int)position.y];
    }
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Map
{
    Player player;
    LogicalEngine engine;
    public Map(LogicalEngine engine)
    {
        this.player = engine.player;
        this.engine = engine;
    }


    

    public void Fountain(int num)
    {
        player.ability.numberofuse += num;
    }
}

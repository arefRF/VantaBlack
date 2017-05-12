using UnityEngine;
using System.Collections.Generic;

public class DrainerController{

    List<Drainer> drainers = new List<Drainer>();

    public DrainerController(List<Drainer> drainers)
    {
        this.drainers = drainers;
    }


    public bool Check(Player player)
    {
        if (player.abilities.Count == 0)
            return false;
        for(int i=0; i<drainers.Count; i++)
        {
            if (drainers[i].Check(player))
                return true;
        }
        return false;
    }

}

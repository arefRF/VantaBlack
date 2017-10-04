using UnityEngine;
using System.Collections.Generic;

public class DrainerController{

    List<Drainer> drainers;
    List<Branch> branchDrainers;

    public DrainerController(List<Drainer> drainers, List<Branch> branchDrainers)
    {
        this.drainers = drainers;
        this.branchDrainers = branchDrainers;
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
        for(int i=0; i<branchDrainers.Count; i++)
        {
            if (branchDrainers[i].CheckDrain(player))
                return true;
        }
        return false;
    }

}

using UnityEngine;
using System.Collections.Generic;

public class DrainerController{

    List<Drainer> drainers = new List<Drainer>();

    public DrainerController(List<Drainer> drainers)
    {
        this.drainers = drainers;
    }


    public void Check(Player player)
    {
        for(int i=0; i<drainers.Count; i++)
        {
            drainers[i].Check(player);
        }
    }

}

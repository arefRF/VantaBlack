using UnityEngine;
using System.Collections.Generic;

public class LaserController{

    public List<Laser> lasers;
    public List<DynamicContainer> containers;
    LogicalEngine engine;

    public LaserController(List<Laser> lasers)
    {
        this.lasers = lasers;
        containers = new List<DynamicContainer>();
    }
    
    public void SetLasers()
    {
        return;
        if (engine == null)
            engine = Starter.GetEngine();
        engine.apigraphic.RemoveLaser();
        containers.Clear();
        for (int i = 0; i < lasers.Count; i++)
        {
            lasers[i].SetLaser();
        }
    }

    public bool CollisionCheck(Vector2 pos)
    {
        for (int i = 0; i < lasers.Count; i++)
        {
            if (lasers[i].CollideLaser(pos))
                return true;
        }
        return false;
    }
}

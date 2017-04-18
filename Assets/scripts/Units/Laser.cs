using UnityEngine;
using System.Collections.Generic;

public class Laser : Unit {

    LogicalEngine engine;
    public List<Vector2[]> beamPositions { get; set; }
    public override void Run()
    {
        engine = Starter.GetEngine();
        beamPositions = new List<Vector2[]>();
        base.Run();
    }

    public void SetLaser()
    {
        SetLaser(Direction.Right, position);
        SetLaser(Direction.Down, position);
        SetLaser(Direction.Left, position);
        SetLaser(Direction.Up, position);
        Debug.Log(beamPositions.Count);
        for (int i = 0; i < beamPositions.Count; i++)
        {
            api.engine.apigraphic.AddLaser(beamPositions[i][0], beamPositions[i][1]);
            Debug.Log(beamPositions[i][0] + " to " + beamPositions[i][1]);
        }
    }

    public void SetLaser(Direction direction, Vector2 startingpos)
    {
        if(direction == Direction.Right)
        {
            Vector2 finalpos = Toolkit.VectorSum(startingpos, direction);
            Vector2 pos = Toolkit.VectorSum(startingpos, direction);
            bool flag = false;
            while (finalpos.x < engine.sizeX && Toolkit.IsEmpty(finalpos))
            {
                flag = true;
                finalpos = Toolkit.VectorSum(finalpos, direction);
            }
            DynamicContainer container = (DynamicContainer)Toolkit.GetContainer(finalpos);
            if (container != null)
                SetLaser(container.direction, container.position);
            if (flag)
                beamPositions.Add(new Vector2[] { pos, finalpos });
        }
        else if (direction == Direction.Down)
        {
            Vector2 finalpos = Toolkit.VectorSum(startingpos, direction);
            Vector2 pos = Toolkit.VectorSum(startingpos, direction);
            bool flag = false;
            while (finalpos.y > 0 && Toolkit.IsEmpty(finalpos))
            {
                flag = true;
                finalpos = Toolkit.VectorSum(finalpos, direction);
            }
            DynamicContainer container = (DynamicContainer)Toolkit.GetContainer(finalpos);
            if (container != null)
                SetLaser(container.direction, container.position);
            if (flag)
                beamPositions.Add(new Vector2[] { pos, finalpos });
        }
        else if (direction == Direction.Left)
        {
            Vector2 finalpos = Toolkit.VectorSum(startingpos, direction);
            Vector2 pos = Toolkit.VectorSum(startingpos, direction);
            bool flag = false;
            while (finalpos.x > 0 && Toolkit.IsEmpty(finalpos))
            {
                flag = true;
                finalpos = Toolkit.VectorSum(finalpos, direction);
            }
            DynamicContainer container = (DynamicContainer)Toolkit.GetContainer(finalpos);
            if (container != null)
                SetLaser(container.direction, container.position);
            if (flag)
                beamPositions.Add(new Vector2[] { pos, finalpos });
        }
        else if (direction == Direction.Up)
        {
            Vector2 finalpos = Toolkit.VectorSum(startingpos, direction);
            Vector2 pos = Toolkit.VectorSum(startingpos, direction);
            bool flag = false;
            while ((finalpos.y < engine.sizeY && Toolkit.IsEmpty(finalpos)))
            {
                flag = true;
                finalpos = Toolkit.VectorSum(finalpos, direction);
            }
            DynamicContainer container = (DynamicContainer)Toolkit.GetContainer(finalpos);
            if (container != null)
                SetLaser(container.direction, container.position);
            if (flag)
                beamPositions.Add(new Vector2[] { pos, finalpos });
        }

    }

    public bool CollideLaser(Vector2 pos)
    {
        for(int i=0; i<beamPositions.Count; i++)
        {
            if(beamPositions[i][0].x == beamPositions[i][1].x)
            {
                if(pos.x == beamPositions[i][0].x)
                {
                    if(IsBetween(pos.y, beamPositions[i][0].y, beamPositions[i][1].y))
                    {
                        Debug.Log("beam collision");
                        return true;
                    }
                }
            }
            else if (beamPositions[i][0].y == beamPositions[i][1].y)
            {
                if (pos.y == beamPositions[i][0].y)
                {
                    if (IsBetween(pos.x, beamPositions[i][0].x, beamPositions[i][1].x))
                    {
                        Debug.Log("beam collision");
                        return true;
                    }
                }
            }
            else
            {
                Debug.Log("error in laser");
                return false;
            }
        }
        return false;
    }


    private bool IsBetween(float i, float s1, float s2)
    {
        if (i < s1 && i > s2)
            return true;
        if (i > s1 && i < s2)
            return true;
        return false;
    }
}

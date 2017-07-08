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


    public override void SetInitialSprite()
    {
        bool[] connected = Toolkit.GetConnectedSidesForLaser(this);
        for(int i=0; i<4; i++)
        {
            if (!connected[i])
                transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = null;
        }
    }
    public void SetLaser()
    {
        SetLaser(Direction.Right, position);
        SetLaser(Direction.Down, position);
        SetLaser(Direction.Left, position);
        SetLaser(Direction.Up, position);
    }

    public void SetLaser(Direction direction, Vector2 startingpos)
    {
        beamPositions.Clear();
        if(direction == Direction.Right)
        {
            Vector2 finalpos = Toolkit.VectorSum(startingpos, direction);
            Vector2 pos = Toolkit.VectorSum(startingpos, direction);
            bool flag = false;
            while (finalpos.x < engine.sizeX - 1 && Toolkit.IsEmpty(finalpos))
            {
                flag = true;
                finalpos = Toolkit.VectorSum(finalpos, direction);
            }
            DynamicContainer container = Toolkit.GetContainer(finalpos) as DynamicContainer;
            if (container != null && !api.engine.lasercontroller.containers.Contains(container))
            {
                api.engine.lasercontroller.containers.Add(container);
                SetLaser(container.direction, container.position);
            }
            else
            {
                Branch branch = Toolkit.GetBranch(finalpos) as Branch;
                if(branch != null && branch.islocked)
                {
                    branch.islocked = false;
                }
            }
            if (flag)
            {
                beamPositions.Add(new Vector2[] { pos, finalpos });
                api.engine.apigraphic.AddLaser(pos, finalpos, direction);
            }
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
            DynamicContainer container = Toolkit.GetContainer(finalpos) as DynamicContainer;
            if (container != null && !api.engine.lasercontroller.containers.Contains(container))
            {
                api.engine.lasercontroller.containers.Add(container);
                SetLaser(container.direction, container.position);
            }
            else
            {
                Branch branch = Toolkit.GetBranch(finalpos) as Branch;
                if (branch != null && branch.islocked)
                {
                    branch.islocked = false;
                }
            }
            if (flag)
            {
                beamPositions.Add(new Vector2[] { pos, finalpos });
                api.engine.apigraphic.AddLaser(pos, finalpos, direction);
            }
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
            DynamicContainer container = Toolkit.GetContainer(finalpos) as DynamicContainer;
            if (container != null && !api.engine.lasercontroller.containers.Contains(container))
            {
                api.engine.lasercontroller.containers.Add(container);
                SetLaser(container.direction, container.position);
            }
            else
            {
                Branch branch = Toolkit.GetBranch(finalpos) as Branch;
                if (branch != null && branch.islocked)
                {
                    branch.islocked = false;
                }
            }
            if (flag)
            {
                beamPositions.Add(new Vector2[] { pos, finalpos });
                api.engine.apigraphic.AddLaser(pos, finalpos, direction);
            }
        }
        else if (direction == Direction.Up)
        {
            Vector2 finalpos = Toolkit.VectorSum(startingpos, direction);
            Vector2 pos = Toolkit.VectorSum(startingpos, direction);
            bool flag = false;
            while ((finalpos.y < engine.sizeY - 1 && Toolkit.IsEmpty(finalpos)))
            {
                flag = true;
                finalpos = Toolkit.VectorSum(finalpos, direction);
            }
            DynamicContainer container = Toolkit.GetContainer(finalpos) as DynamicContainer;
            if (container != null && !api.engine.lasercontroller.containers.Contains(container))
            {
                api.engine.lasercontroller.containers.Add(container);
                SetLaser(container.direction, container.position);
            }
            else
            {
                Branch branch = Toolkit.GetBranch(finalpos) as Branch;
                if (branch != null && branch.islocked)
                {
                    branch.islocked = false;
                }
            }
            if (flag)
            {
                beamPositions.Add(new Vector2[] { pos, finalpos });
                api.engine.apigraphic.AddLaser(pos, finalpos, direction);
            }
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
                    if (IsBetween(pos.y, beamPositions[i][0].y, beamPositions[i][1].y))
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
        if (i <= s1 && i >= s2)
            return true;
        if (i >= s1 && i <= s2)
            return true;
        return false;
    }
}

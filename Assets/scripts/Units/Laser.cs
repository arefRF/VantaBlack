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

    public void Update()
    {

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
        beamPositions.Clear();
        SetLaserInDirection(Direction.Right, position);
        SetLaserInDirection(Direction.Down, position);
        SetLaserInDirection(Direction.Left, position);
        SetLaserInDirection(Direction.Up, position);
    }

    private void SetLaserInDirection(Direction direction, Vector2 startingpos)
    {
        if(direction == Direction.Right)
        {
            Vector2 pos = Toolkit.VectorSum(startingpos, Toolkit.DirectiontoVector(direction)/2);
            RaycastHit2D hit = Physics2D.Raycast(pos, Toolkit.DirectiontoVector(direction));
            Vector2 finalpos = hit.point;
            Debug.Log(pos);
            Debug.Log(finalpos);
            bool flag = false;
            while (finalpos.x < engine.sizeX - 1 && Toolkit.IsEmpty(finalpos))
            {
                flag = true;
                finalpos = Toolkit.VectorSum(finalpos, direction);
            }
            SetLaser(direction, pos, finalpos, flag);
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
            SetLaser(direction, pos, finalpos, flag);
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
            SetLaser(direction, pos, finalpos, flag);
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
            SetLaser(direction, pos, finalpos, flag);
        }

    }

    private void SetLaser(Direction direction, Vector2 pos, Vector2 finalpos, bool flag)
    {
        DynamicContainer container = Toolkit.GetContainer(finalpos) as DynamicContainer;
        Branch branch = Toolkit.GetBranch(finalpos) as Branch;
        Unit unit = Toolkit.GetUnitIncludingPlayer(finalpos);
        if (unit != null)
        {
            if (unit.isLeanable())
                api.engine.apigraphic.AddPartialLaser(finalpos, Toolkit.ReverseDirection(direction), unit.gameObject);
            else if (unit is Player)
            {
                engine.apigraphic.Laser_Player_Died(unit as Player);
                finalpos = Toolkit.VectorSum(finalpos, direction);
            }
        }
        finalpos = Toolkit.VectorSum(finalpos, Toolkit.ReverseDirection(direction));
        if (container != null && !api.engine.lasercontroller.containers.Contains(container))
        {
            if (!(container.ConnectedUnits.Contains(this) && Toolkit.AreNeighbours(container, this)))
            {
                api.engine.lasercontroller.containers.Add(container);
                api.engine.apigraphic.AddPartialLaser(container.position, container.direction, container.gameObject);
                api.engine.apigraphic.LaserHitDynamic(container);
                SetLaserInDirection(container.direction, container.position);
            }
        }
        else
        {
            if (branch != null && branch.islocked)
            {
                engine.apigraphic.UnlockBranchLaser(branch);
            }
        }
        if (flag)
        {
            beamPositions.Add(new Vector2[] { pos, finalpos });
            api.engine.apigraphic.AddLaser(pos, finalpos, direction, gameObject);
        }
    }

    public bool CollideLaser(Vector2 pos)
    {
        for (int i=0; i<beamPositions.Count; i++)
        {
            if(beamPositions[i][0].x == beamPositions[i][1].x)
            {
                if(pos.x == beamPositions[i][0].x)
                {
                    if (IsBetween(pos.y, beamPositions[i][0].y, beamPositions[i][1].y))
                    {
                        engine.apigraphic.Laser_Player_Died(Toolkit.GetPlayer(pos));
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
                        Player player = Toolkit.GetPlayer(pos);
                        if (player.transform.position.y != player.position.y)
                        {
                            player.SetState(PlayerState.Busy);  
                        }
                        else
                            engine.apigraphic.Laser_Player_Died(player);
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

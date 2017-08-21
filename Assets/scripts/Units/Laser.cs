using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Laser : Unit {

    LogicalEngine engine;
    public List<Vector2[]> beamPositions { get; set; }
    LineRenderer[] linerenderers;
    public override void Run()
    {
        engine = Starter.GetEngine();
        beamPositions = new List<Vector2[]>();
        base.Run();
        linerenderers = new LineRenderer[4];
        //StartCoroutine(SetLaserTimer(0.05f));
    }

    public void Update()
    {
        SetLaser();
        //SetLaser();
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
        
        SetLaserInDirection(Direction.Right, transform.position);
        SetLaserInDirection(Direction.Down, transform.position);
        SetLaserInDirection(Direction.Left, transform.position);
        SetLaserInDirection(Direction.Up, transform.position);
        //engine.apigraphic.DestroyLasers();
    }

    private void SetLaserInDirection(Direction direction, Vector2 startingpos)
    {
        
            Vector2 pos = Toolkit.VectorSum(startingpos, Toolkit.DirectiontoVector(direction)/1.9f);
            RaycastHit2D hit = Physics2D.Raycast(pos, Toolkit.DirectiontoVector(direction), Mathf.Max(engine.sizeX, engine.sizeY));
            //if(hit.point == new)
            Vector2 finalpos = hit.point;
            int num = Toolkit.DirectionToNumber(direction) - 1;
            linerenderers[num] =  engine.apigraphic.AddLaserLine(pos, finalpos, transform.parent.gameObject, linerenderers[num]);
            //SetLaser(direction, pos, finalpos);
        /*else if (direction == Direction.Down)
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
        */
    }

    private void SetLaser(Direction direction, Vector2 pos, Vector2 finalpos)
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
        /*if (flag)
        {
            beamPositions.Add(new Vector2[] { pos, finalpos });
            api.engine.apigraphic.AddLaser(pos, finalpos, direction, gameObject);
        }*/
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

    public IEnumerator SetLaserTimer(float time)
    {
        yield return new WaitForSeconds(time);
        SetLaser();
        StartCoroutine(SetLaserTimer(0.05f));
    }
}

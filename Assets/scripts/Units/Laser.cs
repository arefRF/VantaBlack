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
        
        SetLaserInDirection(Direction.Right, transform.position, linerenderers[1], this);
        SetLaserInDirection(Direction.Down, transform.position, linerenderers[2], this);
        SetLaserInDirection(Direction.Left, transform.position, linerenderers[3], this);
        SetLaserInDirection(Direction.Up, transform.position, linerenderers[0], this);
        //engine.apigraphic.DestroyLasers();
    }

    private void SetLaserInDirection(Direction direction, Vector2 startingpos, LineRenderer linerenderer, Unit LaserSource)
    {
        
        Vector2 pos = Toolkit.VectorSum(startingpos, Toolkit.DirectiontoVector(direction)/1.95f);
        RaycastHit2D hit = Physics2D.Raycast(pos, Toolkit.DirectiontoVector(direction), Mathf.Max(engine.sizeX, engine.sizeY));
        Vector2 finalpos = hit.point;
        if (LaserSource is DynamicContainer)
            pos -= Toolkit.DirectiontoVector((LaserSource as DynamicContainer).direction) / 5;
        if (hit.collider == null)
        {
            finalpos = pos + Toolkit.DirectiontoVector(direction) * Mathf.Max(engine.sizeX, engine.sizeY);
        }
        else {
            if(hit.collider.transform.gameObject.GetComponent<Unit>().isLeanable())
                finalpos += Toolkit.DirectiontoVector(direction) / 8;
            if (hit.collider.tag == "Dynamic Container")
            {
                DynamicContainer container = hit.collider.transform.gameObject.GetComponent<DynamicContainer>();
                if (!(container.ConnectedUnits.Contains(this) && Toolkit.AreNeighbours(container, this)))
                {
                    SetLaserInDirection(container.direction, container.transform.position, container.linerenderer, container);
                }
            }
            else if (hit.collider.tag == "Player")
            {
                Debug.Log("player shoud die");
            }
        }
        linerenderer =  engine.apigraphic.AddLaserLine(pos, finalpos, transform.parent.gameObject, linerenderer);
        if (LaserSource is DynamicContainer)
            (LaserSource as DynamicContainer).linerenderer = linerenderer;
        else
        {
            linerenderers[Toolkit.DirectionToNumber(direction) - 1] = linerenderer;
        }
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
                //SetLaserInDirection(container.direction, container.position);
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

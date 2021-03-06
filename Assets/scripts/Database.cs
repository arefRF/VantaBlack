using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Database {
    
    public Database(long i)
    {
        snapshots = new List<Snapshot>();
        pipes = new List<Unit>();
        turn = i;
        functionalCon = new List<FunctionalContainer>();
        drainers = new List<Drainer>();
        lasers = new List<Laser>();
        bots = new List<Bot>();
        LaserContainers = new List<DynamicContainer>();
        branchDrainers = new List<Branch>();
        branchGravityChangers = new List<Branch>();
    }
    public List<Bot> bots;
    public List<Unit> pipes;
    public List<Player> player;
    public readonly int numberOfSnapshot = 5;
    public int snapShotCount;
    public Direction gravity_direction { get; private set; }
    public List<Unit>[,] units;
    public long turn;
    public List<TimeLaps> timeLaps;
    public State state;
    public int[,] checkPointPositions;
    public int Ysize { get; set; }
    public int Xsize { get; set; }
    
    public List<Snapshot> snapshots;

    public static Direction GravityDirection;
    public Coroutine timer;

    // List of all Dynamic and Static Containers for Portal ability
    public List<FunctionalContainer> functionalCon;
    public List<Drainer> drainers;
    public List<Branch> branchDrainers;
    public List<Branch> branchGravityChangers;
    public List<Laser> lasers;
    public List<DynamicContainer> LaserContainers; //containers which project or have laser
    public List<Unit> GetUnits(Vector2 position)
    {
        return units[(int)position.x, (int)position.y];
    }

    public void SetGravity(Direction newdirection)
    {
        gravity_direction = newdirection;
        for (int i = 0; i < player.Count; i++)
        {
            if(player[i].gravitynum != 0)
            {
                player[i].SetGravity(Toolkit.NumberToDirection(player[i].gravitynum));
                player[i].gravitynum = 0;
                continue;
            }
            player[i].SetGravity(newdirection);
        }
    }

    public void StopTimer()
    {
        if(timer!=null)
            GameObject.Find("GetInput").GetComponent<GetInput>().StopCoroutine(timer);
    }
}





public class TimeLaps
{
    public long lifetime;
    public Vector2 position;
    public long time;
    public GameObject gameobject;
    public TimeLaps(int lifetime, GameObject gameobject)
    {
        this.lifetime = lifetime + Starter.GetDataBase().turn;
        this.gameobject = gameobject;
        position = gameobject.transform.position;
        time = Starter.GetDataBase().turn;
    }
}

public class ContainerLaser
{
    public DynamicContainer container;
    public bool ContainerTimeFinished;
    public Coroutine ContainerLaserBeginCoroutine;

    public ContainerLaser(DynamicContainer container)
    {
        this.container = container;
        ContainerTimeFinished = false;
    }
}
public class LaserBranchUnlocker
{
    public Branch branch;
    public Coroutine LaserUnlockWaitCoroutine;
    public bool LaserUnlockTimeFinished;
}
public enum State
{
    Idle, Busy, Busy_Moving
}
public enum Direction
{
    Right, Left, Up, Down
}

public enum AbilityType
{
     Fuel,Direction, Jump, Gravity, Teleport, Rope, Key, Laser
}

public enum PlayerState
{
    Idle, Falling, Jumping, Moving, Fakelean, Lean, Busy, Adjust, Gir
}

public enum LifeState
{
    Alive, Dead
}

public enum EnemyState
{
    Idle, Moving, Falling, Patrolling
}
public enum GateType
{
    Internal, InternalChangeScene, External
}

public enum GameMode
{
    Play, Portal, Menu, Real
}

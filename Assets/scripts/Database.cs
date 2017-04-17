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
    }
    public List<Unit> pipes;
    public List<Player> player;
    public readonly int numberOfSnapshot = 5;
    public int snapShotCount;
    public Direction gravity_direction { get; set; }
    public List<Unit>[,] units;
    public long turn;
    public List<TimeLaps> timeLaps;
    public State state;
    public int[,] checkPointPositions;
    public int Ysize { get; set; }
    public int Xsize { get; set; }
    
    public List<Snapshot> snapshots;

    public static Direction GravityDirection;

    // List of all Dynamic and Static Containers for Portal ability
    public List<FunctionalContainer> functionalCon;
    public List<Drainer> drainers;
    public List<Unit> GetUnits(Vector2 position)
    {
        return units[(int)position.x, (int)position.y];
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
     Fuel,Direction, Jump, Gravity, Teleport, Rope, Key
}

public enum PlayerState
{
    Idle, Falling, Jumping, Moving, Fakelean, Lean, Busy
}

public enum GateType
{
    Internal, InternalChangeScene, External
}

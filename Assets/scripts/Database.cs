using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Database {
    
    public Database(long i)
    {
        snapshots = new List<Snapshot>();
        turn = i;
    }

    public LogicalEngine logicalengine;
    public GameObject player;
    public readonly int numberOfSnapshot = 5;
    public int snapShotCount;
    public Direction gravity_direction;
    public List<Unit>[,] units;
    public List<Switch> AutomaticSwitches;
    public long turn;
    public List<TimeLaps> timeLaps;
    public State state;
    public int[,] checkPointPositions;
    public int Ysize { get; set; }
    public int Xsize { get; set; }

    private Direction GravityDirection;
    
    public List<Snapshot> snapshots;

    public bool ACTIVE_RED, ACTIVE_BLUE, ACTIVE_GREEN, ACTIVE_PURPLE, ACTIVE_LIGHTBLUE;


    public void Setsize(int x, int y)
    {
        Xsize = x;
        Ysize = y;
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
        this.lifetime = lifetime + Starter.GetEngine().database.turn;
        this.gameobject = gameobject;
        position = gameobject.transform.position;
        time = Starter.GetEngine().database.turn;
    }
}

[SerializeField]
public enum UnitType
{
    Block, Pipe, Box, Magnet, Switch, Wall, Container, Player, Rock, Door, BlockSwitch
}

public enum State
{
    Idle, Busy, Busy_Moving
}
[SerializeField]
public enum Direction
{
    Right, Left, Up, Down
}

[SerializeField]
public enum AbilityType
{
    Direction, Jump, Gravity, Blink, Rope, Fuel
}

[SerializeField]
public enum DoorType
{
    Internal, External
}


public enum PlayerState
{
    Steady, Falling, Hanging, Jumping
}

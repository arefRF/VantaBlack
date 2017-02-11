using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Database {
    
    public Database(long i)
    {
        snapshots = new List<Snapshot>();
        turn = i;
    }

    public List<Player> player;
    public readonly int numberOfSnapshot = 5;
    public int snapShotCount;
    public Direction gravity_direction;
    public List<Unit>[,] units;
    public long turn;
    public List<TimeLaps> timeLaps;
    public State state;
    public int[,] checkPointPositions;
    public int Ysize { get; set; }
    public int Xsize { get; set; }
    
    public List<Snapshot> snapshots;

    public static Direction GravityDirection;
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

public enum UnitType
{
    Block, Pipe, Box, Magnet, Switch, Wall, Container, Player, Rock, Door, BlockSwitch, Ramp
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
    Direction, Jump, Gravity, Blink, Rope, Fuel, Key
}

public enum PlayerState
{
    Idle, Falling, Jumping, Moving
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class LogicalEngine
{
    public Database database;
    public Player player;
    public GraphicalEngine Gengine;
    public PlayerGraphics playergraphics;
    public Move moveObject;
    int x, y;
    public Action action;
    AandR AR;
    Map map;
       
    SnapshotManager spManager;
    public Vector3 camerapos;
    public float camerasize;

    public List<CloneableUnit> snapshotunits;
    Snapshot currentSnapshot;
    public LogicalEngine(int x, int y)
    {
        database = Starter.GetDataBase();
        player = database.player.GetComponent<Player>();
        Gengine = GameObject.Find("Graphical").GetComponent<GraphicalEngine>();
        playergraphics = GameObject.Find("Graphical").GetComponent<PlayerGraphics>();
        spManager = new SnapshotManager();
        database.units = new List<Unit>[x, y];
        database.timeLaps = new List<TimeLaps>();
        database.Xsize = this.x;
        database.Ysize = this.y;
        this.x = x;
        this.y = y;
        init();
        moveObject = new Move(this);
        action = new Action(this);
        map = new Map(this);
        AR = new AandR(this);

        snapshotunits = new List<CloneableUnit>();
    }
    void init()
    {
        Unit.Code = 0;
        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                database.units[i, j] = new List<Unit>();
            }
        }
        List<GameObject> game_objs = new List<GameObject>();
        for (int i = 0; i < GameObject.Find("Objects").transform.childCount; i++)
        {
            GameObject room = GameObject.Find("Objects").transform.GetChild(i).gameObject;
            for (int j = 0; j < room.transform.childCount; j++)
            {
                GameObject g = room.transform.GetChild(j).gameObject;
                switch (g.tag)
                {
                    case "Wall":
                        {
                            Wall[] wall = g.GetComponents<Wall>();
                            wall[0].codeNumber = Unit.Code;
                            Unit.Code++;
                            wall[1].codeNumber = Unit.Code;
                            Unit.Code++;
                            if (wall[0].direction == Direction.Right)
                            {
                                database.units[(int)g.transform.position.x, (int)g.transform.position.y].Add(wall[0]);
                                database.units[(int)g.transform.position.x + 1, (int)g.transform.position.y].Add(wall[1]);
                                wall[0].x = (int)g.transform.position.x; wall[0].y = (int)g.transform.position.y;
                                wall[1].x = (int)g.transform.position.x + 1; wall[1].y = (int)g.transform.position.y;
                            }
                            else
                            {
                                database.units[(int)g.transform.position.x, (int)g.transform.position.y].Add(wall[0]);
                                database.units[(int)g.transform.position.x, (int)g.transform.position.y + 1].Add(wall[1]);
                                wall[0].x = (int)g.transform.position.x; wall[0].y = (int)g.transform.position.y;
                                wall[1].x = (int)g.transform.position.x; wall[1].y = (int)g.transform.position.y + 1;
                            }
                        }
                        break;

                    case "Block":
                        {
                            Block temp = g.GetComponent<Block>();
                            temp.codeNumber = Unit.Code;
                            Unit.Code++;
                            temp.unitType = UnitType.Block;
                            database.units[(int)g.transform.position.x, (int)g.transform.position.y].Add(temp);
                            temp.x = (int)g.transform.position.x; temp.y = (int)g.transform.position.y;
                        }
                        break;

                    case "Container":
                        {
                            Container temp = g.GetComponent<Container>();
                            temp.codeNumber = Unit.Code;
                            Unit.Code++;
                            database.units[(int)g.transform.position.x, (int)g.transform.position.y].Add(g.GetComponent<Container>());
                            temp.x = (int)g.transform.position.x; temp.y = (int)g.transform.position.y;
                        }
                        break;

                    case "Switch":
                        {
                            
                        }
                        break;

                    case "Player":
                        {
                            Player temp = g.GetComponent<Player>();
                            temp.codeNumber = Unit.Code;
                            Unit.Code++;
                            database.units[(int)g.transform.position.x, (int)g.transform.position.y].Add(g.GetComponent<Player>());
                            temp.x = (int)g.transform.position.x; temp.y = (int)g.transform.position.y;
                        }
                        break;

                    case "Rock":
                        {
                            Rock temp = g.GetComponent<Rock>();
                            temp.codeNumber = Unit.Code;
                            Unit.Code++;
                            database.units[(int)g.transform.position.x, (int)g.transform.position.y].Add(g.GetComponent<Rock>());
                            temp.x = (int)g.transform.position.x; temp.y = (int)g.transform.position.y;
                        }
                        break;

                    case "Door":
                        {
                            g.GetComponent<Door>().codeNumber = Unit.Code;
                            Unit.Code++;
                            database.units[(int)g.transform.position.x, (int)g.transform.position.y].Add(g.GetComponent<Door>());
                        }
                        break;

                    case "BlockSwitch":
                        {
                            BlockSwitch temp = g.GetComponent<BlockSwitch>();
                            temp.codeNumber = Unit.Code;
                            Unit.Code++;
                            database.units[(int)g.transform.position.x, (int)g.transform.position.y].Add(g.GetComponent<BlockSwitch>());
                            temp.x = (int)g.transform.position.x; temp.y = (int)g.transform.position.y;
                        }
                        break;
                }

            }
        }
        /*for (int i = 0; i < database.units.GetLength(0); i++)
        {
            for (int j = 0; j < database.units.GetLength(1); j++)
            {
                for (int k = 0; k < database.units[i, j].Count; k++)
                {
                    Wall.print(database.units[i, j][k]);
                    Wall.print(database.units[i, j][k].codeNumber);
                }
            }
        }*/

    }

    public void run()
    {
        CheckTimeLaps();
        database.state = State.Idle;
    }

    public void UseContainerBlockSwitch(Direction direction)
    {
        Vector2 pos = Toolkit.VectorSum(player.transform.position, Toolkit.DirectiontoVector(direction));
        foreach(Unit u in database.units[(int)pos.x, (int)pos.y])
        {
            if(u.unitType == UnitType.Container)
            {
                action.RunContainer((Container)u);
                return;
            }
            else if (u.unitType == UnitType.BlockSwitch)
            {
                action.RunBlockSwitch((BlockSwitch)u);
                return;
            }
        }
    }

    public bool SpaceKeyPressed()
    {
        if (player.ability == null)
        {
            database.state = State.Idle;
            return true;
        }
        switch (player.ability.abilitytype)
        {
            case AbilityType.Direction: ChangeDirection(); return false;
            case AbilityType.Jump: Jump(); return false;
        }
        return false;
    }


    private void Jump()
    {
        if (player.ability.numberofuse > 0)
        {
            player.ability.numberofuse--;
            player.state = PlayerState.Jumping;
            moveObject.Jump();
            player.state = PlayerState.Falling;
            moveObject.FallPlayer();
        }

    }
    public void SpaceKeyPressed(Direction direction)
    {
        if (player.ability == null)
        {
            database.state = State.Idle;
            return;
        }
        switch (player.ability.abilitytype)
        {
            case AbilityType.Direction: return;
            case AbilityType.Jump: return;
            case AbilityType.Blink: Blink(direction); return;
        }
    }

    public bool ArrowKeyPressed(Direction direction)
    {
        bool flag = false;
        foreach (Direction d in player.move_direction)
        {
            if(d == direction)
            {
                flag = true;
                break;
            }
        }
        if (flag)
        {
            if (player.direction != direction)
            {
                player.direction = direction;
                playergraphics.Player_Change_Direction(player.gameObject, player.direction);
                return true;
            }
            if (Toolkit.IsEmptySpace(player.position, direction))
            {
                if (move(direction))
                    return true;
                return false;
            }
            return false;
        }
        return true;
    }

    public bool move(Direction direction)
    {
        bool flag = moveObject.move(direction);       
        ApplyGravity();
        NextTurn();
        return flag;
    }

    public void DoneMoving()
    {
        Wall.print("done moving");
        database.state = State.Busy;
        
    }

    /*public void Absorb()
    {
        AR.Absorb();
        Gengine._Player_Change_Ability();
    }*/

    public void Absorb(Direction direcion)
    {
        AR.Absorb(direcion);
        Gengine._Player_Change_Ability();

    }

    public void NextTurn()
    {
        spManager.takesnapshot(snapshotunits, camerapos, camerasize);
        //snapshotunits.Clear();
        /*for(int i=0; i < database.snapSshots[database.snapShotCount-1].units.GetLength(0); i++)
        {
            for(int j=0; j< database.snapshots[database.snapShotCount - 1].units.GetLength(1); j++)
            {
                for(int k=0; k< database.snapshots[database.snapShotCount - 1].units[i,j].Count; k++)
                {
                    if(database.snapshots[database.snapShotCount - 1].units[i,j][k].unitType == UnitType.Player)
                        Wall.print(database.snapshots[database.snapShotCount - 1].units[i, j][k].transform.position);
                }
            }
        }*/
        database.turn++;
        snapshotunits.Clear();
        camerapos = Camera.main.transform.position;
        camerasize = Camera.main.orthographicSize;
        database.state = State.Idle;
    }

    public void Undo()
    {
        database.state = State.Busy;
        Snapshot snapshot = spManager.Revese();
        currentSnapshot = snapshot;
        if (snapshot == null)
        {
            database.state = State.Idle;
            return;
        }
        for (int l = 0; l < snapshot.units.Count; l++)
        {
            Gengine.Refresh(_undo(snapshot.units[l]));
        }
        //Camera.main.transform.position = snapshot.cameraPosition;
        //Camera.main.orthographicSize = snapshot.cameraSize;
        Refresh();
    }

    private Unit _undo(CloneableUnit u)
    {
        for (int i = 0; i < database.units.GetLength(0); i++)
        {
            for (int j = 0; j < database.units.GetLength(1); j++)
            {
                for (int k = 0; k < database.units[i, j].Count; k++)
                {
                    if (database.units[i, j][k].codeNumber == u.codeNumber)
                    {
                        Unit utemp = database.units[i, j][k];
                        Toolkit.ClonableUnitToUnit(u, database.units[i, j][k]);
                        database.units[u.x, u.y].Add(database.units[i, j][k]);
                        u.position = new Vector2(u.x, u.y);
                        database.units[i, j].Remove(utemp);
                        if(utemp.unitType == UnitType.Wall)
                        {
                            if(((Wall)utemp).direction == Direction.Right)
                            {
                                database.units[i + 1, j].Remove(utemp.obj.GetComponents<Wall>()[1]);
                                database.units[u.x + 1, u.y].Add(utemp.obj.GetComponents<Wall>()[1]);
                            }
                            else
                            {
                                database.units[i, j + 1].Remove(utemp.obj.GetComponents<Wall>()[1]);
                                database.units[u.x, u.y + 1].Add(utemp.obj.GetComponents<Wall>()[1]);
                            }
                        }
                        return utemp;
                    }
                }
            }
        }
    return null;
    }

    public void SwitchAction()
    {
       
        //action.SwitchActionPressed();
        ApplyGravity();
    }

    public void SwitchAction(Direction d)
    {
        //action.SwitchActionPressed(d);
        ApplyGravity();
    }

    public void Refresh()
    {
        //Gengine.Refresh();
        database.state = State.Idle;
    }

    public void AddToSnapshot(Unit u)
    {
        if (u.unitType == UnitType.Wall && (((Wall)u).direction == Direction.Left || ((Wall)u).direction == Direction.Down))
            return;
        for (int i=0; i<snapshotunits.Count; i++)
        {
            if (u.codeNumber == snapshotunits[i].codeNumber)
                return;
        }
        switch (u.unitType)
        {
            case UnitType.Block: snapshotunits.Add(((Block)u).Clone()); break;
            case UnitType.BlockSwitch: snapshotunits.Add(((BlockSwitch)u).Clone()); break;
            case UnitType.Box: snapshotunits.Add(((Box)u).Clone()); break;
            case UnitType.Container: snapshotunits.Add(((Container)u).Clone()); break;
            case UnitType.Door: snapshotunits.Add(((Door)u).Clone()); break;
            case UnitType.Magnet: break;
            case UnitType.Pipe: break;
            case UnitType.Player: snapshotunits.Add(player.Clone()); break;
            case UnitType.Rock: snapshotunits.Add(((Rock)u).Clone()); break;
            case UnitType.Switch: snapshotunits.Add(((Switch)u).Clone()); break;
            case UnitType.Wall: snapshotunits.Add(((Wall)u).Clone()); break;
        }
    }
    public void RemoveFromSnapshot(Unit u)
    {
        for(int i=0; i<snapshotunits.Count; i++)
        {
            if(snapshotunits[i].codeNumber == u.codeNumber)
            {
                snapshotunits.RemoveAt(i);
                return;
            }
        }
    }
    public int MoveObjects(Unit unit, Direction d, int distance)
    {
        int i = moveObject.MoveObjects(unit, d, distance);
        if (unit.unitType == UnitType.Player && i != 0)
        {

            moveObject.MoveObjects(unit, d, distance);
        }
        return i;
    }

    private void CheckTimeLaps()
    {
        foreach (TimeLaps t in database.timeLaps)
        {
            t.time++;
            if (t.time == t.lifetime)
            {
                database.timeLaps.Remove(t);
                action.Teleport(t.position);
            }
        }
    }

    public void ApplyGravity()
    {
        for(int i=0; i<database.units.GetLength(0); i++)
        {
            for(int j=0; j<database.units.GetLength(1); j++)
            {
                for(int k=0; k<database.units[i,j].Count; k++)
                {
                    Unit u = database.units[i,j][k];
                    if (u.CanBeMoved)
                    {
                        ApplyGravity(u);
                    }
                }
            }
        }
        
    }

    private bool ApplyGravity(Unit unit)
    {
        bool flag = false;
        int counter = 0;
        Vector2 pos1 = unit.obj.transform.position;
        Vector2 pos2 = Toolkit.DirectiontoVector(database.gravity_direction);
        for (int i = 0; i < database.units[(int)unit.obj.transform.position.x, (int)unit.obj.transform.position.y].Count; i++)
        {
            Unit u = database.units[(int)unit.obj.transform.position.x, (int)unit.obj.transform.position.y][i];
            if (u.unitType == UnitType.Door)
            {

                if (((Door)u).direction == database.gravity_direction && !((Door)u).isOpen)
                    return false;
            }
        }
        try
        {
            
            for (int i = 0; i < database.units[(int)Toolkit.VectorSum(pos1, pos2).x, (int)Toolkit.VectorSum(pos1, pos2).y].Count; i++)
            {
                Unit u = database.units[(int)Toolkit.VectorSum(pos1, pos2).x, (int)Toolkit.VectorSum(pos1, pos2).y][i];
                if (u.unitType == UnitType.Door)
                {
                    return false;
                }
            }
        }
        catch { }

        while (true)
        {
            if (Toolkit.IsEmptySpace(Toolkit.VectorSum(pos1, pos2), database.gravity_direction))
            {
                flag = true;
                counter++;
                pos1 = Toolkit.VectorSum(pos1, pos2);
            }
            
            
            else
                break;
        }
        if (counter != 0)
            AddToSnapshot(unit);
        database.units[(int)unit.obj.transform.position.x, (int)unit.obj.transform.position.y].Remove(unit);
        for (int i = 0; i < counter; i++)
            Gengine._Move_Object(unit.obj, Toolkit.VectorSum(unit.obj.transform.position, Toolkit.DirectiontoVector(database.gravity_direction)));
        unit.position = unit.obj.transform.position;
        database.units[(int)unit.obj.transform.position.x, (int)unit.obj.transform.position.y].Add(unit);
        
        try 
        {
            for (int i = 0; i < ((Rock)unit).connectedUnits.Count; i++)
            {
                //engine.reserved.Add(unit);
                database.units[(int)((Rock)unit).connectedUnits[i].obj.transform.position.x, (int)((Rock)unit).connectedUnits[i].obj.transform.position.y].Remove(((Rock)unit).connectedUnits[i]);
                Vector2 temppos = Toolkit.VectorSum((Toolkit.DirectiontoVector(Toolkit.ReverseDirection(((Switch)((Rock)unit).connectedUnits[i]).direction))), unit.gameObject.transform.position);
                GraphicalEngine.MoveObject(((Rock)unit).connectedUnits[i].obj, temppos);
                database.units[(int)temppos.x, (int)temppos.y].Add((Switch)((Rock)unit).connectedUnits[i]);
            }
        }
        catch
        {

        }
        //action.CheckAutomaticSwitch(pos1);
        return flag;
    }
    
    public Snapshot GetCurrentSnapshot()
    {
        return currentSnapshot;
    }

    public void ChangeDirection()
    {
        if(player.ability.numberofuse > 0)
        {
            player.ability.numberofuse--;
            action.ChangeDirection();
        }
    }
    public void Blink(Direction direction)
    {
        if(player.ability.numberofuse > 0)
        {
            player.ability.numberofuse--;
            moveObject.Blink(direction);
            player.state = PlayerState.Falling;
            return;
        }
    }
    public void ContainerBlink(Container container)
    {
        for(int i=0; i< x; i++)
        {
            for(int j=0; j< y; j++)
            {
                for(int k=0; k<database.units[i,j].Count; k++)
                {
                    if(database.units[i,j][k].unitType == UnitType.Container)
                    {
                        if (((Container)database.units[i, j][k]).codeNumber == container.codeNumber)
                            continue;
                        if(((Container)database.units[i, j][k]).ability != null && ((Container)database.units[i, j][k]).ability.abilitytype == AbilityType.Blink)
                        {
                            moveObject.ContainerBlink(Toolkit.VectorSum(container.position, Toolkit.DirectiontoVector(((Container)database.units[i, j][k]).direction)));
                            player.state = PlayerState.Falling;
                            moveObject.FallPlayer();
                            return;
                        }
                    }
                }
            }
        }
    }
    public void ContainerJump(int distance)
    {
        player.state = PlayerState.Jumping;
        moveObject.Jump();
        player.state = PlayerState.Falling;
        moveObject.FallPlayer();
    }

    public void RollOnRamp(Unit unit)
    {
        int counter = 0;
        if(unit.unitType == UnitType.Player)
        {
            Ramp ramp = null;
            player.state = PlayerState.Rolling;
            while (true)
            {
                ramp = Toolkit.GetRamp(player);
                if (ramp == null)
                    break;
                if (!Toolkit.IsEmptySpace(unit.position, Toolkit.ReverseDirection(ramp.direction)))
                    break;
                counter++;
            }
            if(counter != 0)
                moveObject.RollPlayer(ramp.direction, counter);
            player.state = PlayerState.Steady;
        }
    }
}


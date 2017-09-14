using UnityEngine;
using System.Collections.Generic;
using System;

public class LogicalEngine {

    public APIGraphic apigraphic;
    public APIInput apiinput;
    public APIUnit apiunit;
    public Database database;
    public SubEngine_Initializer initializer;
    public InputController inputcontroller;
    public int sizeX, sizeY;
    public List<Unit> stuckedunits;
    public SnapshotManager snpmanager;
    public PipeController pipecontroller;
    public DrainerController drainercontroller;
    public LaserController lasercontroller;

    public List<Unit> leanmove;
    public List<Unit> shouldmove;

    public SaveSerialize saveserialize;
    public SaveCheckpoint savecheckpoint;

    public LockCombination lockcombination;
    public LogicalEngine(int x, int y)
    {
        sizeX = x;
        sizeY = y;
        stuckedunits = new List<Unit>();
        apigraphic = new APIGraphic(this);
        apiinput = new APIInput(this);
        apiunit = new APIUnit(this);
        database = Starter.GetDataBase();
        leanmove = new List<Unit>();
        snpmanager = new SnapshotManager(this);
        initializer = new SubEngine_Initializer(x,y, this);
        pipecontroller = new PipeController(this);
        lasercontroller = new LaserController(database.lasers);
        saveserialize = new SaveSerialize();
        savecheckpoint = new SaveCheckpoint();
    }

    public void Run()
    {
        database.units = initializer.init();
        database.state = State.Idle;
        for (int i = 0; i < sizeX; i++)
            for (int j = 0; j < sizeY; j++)
                for (int k = 0; k < database.units[i, j].Count; k++)
                    database.units[i, j][k].Run();
        inputcontroller = new InputController(this);
        for(int i=0; i<database.player.Count; i++)
            snpmanager.AddToSnapShot(database.player[i]);
        snpmanager.takesnapshot();
        drainercontroller = new DrainerController(database.drainers);
        pipecontroller.CheckPipes();
        lasercontroller.SetLasers();
        //Applygravity();
    }

    public bool MoveUnit(Unit unit, Direction dir)
    {
        shouldmove = new List<Unit>();
        leanmove = new List<Unit>();
        List<Unit> playerinbranchmove = new List<Unit>();
        if (!(unit is Box))
        {
            snpmanager.CloneStuckList();
            if (!unit.CanMove(dir, unit.transform.parent.gameObject))
                return false;
            
            for (int i = 0; i < unit.ConnectedUnits.Count; i++)
                if (!unit.ConnectedUnits[i].CanMove(dir, unit.transform.parent.gameObject))
                    return false;
            //friction
            shouldmove.AddRange(unit.EffectedUnits(Toolkit.ReverseDirection(Starter.GetDataBase().gravity_direction)));
            shouldmove.AddRange(unit.players);

            int bound = unit.players.Count;
            for (int i = 0; i < bound; i++)
            {
                if (Toolkit.HasBranch(Toolkit.VectorSum(unit.players[i].position, dir)))
                {
                    if(Toolkit.GetBranch(Toolkit.VectorSum(unit.players[i].position, dir)).PlayerForcePushIntoBranch(unit.players[i] as Player, dir))
                    {
                        while (shouldmove.IndexOf(unit.players[i]) != -1)
                            shouldmove.Remove(unit.players[i]);
                        unit.players.RemoveAt(i);
                        i--;
                        bound--;
                        continue;
                    }
                }
                else if (!unit.players[i].CanMove(dir, unit.transform.parent.gameObject))
                {
                    if (Toolkit.HasBranch(Toolkit.VectorSum(unit.players[i].position, Toolkit.ReverseDirection(dir))))
                    {
                        if (Toolkit.GetBranch(Toolkit.VectorSum(unit.players[i].position, Toolkit.ReverseDirection(dir))).PlayerForcePushIntoBranch(unit.players[i] as Player, Toolkit.ReverseDirection(dir)))
                        {
                            while (shouldmove.IndexOf(unit.players[i]) != -1)
                                shouldmove.Remove(unit.players[i]);
                            unit.players.RemoveAt(i);
                            i--;
                            bound--;
                            continue;
                        }

                    }
                    else
                        apigraphic.Crush_Player_Died(unit.players[i] as Player);
                }
                unit.players.AddRange(unit.players[i].players);

            }
            for (int i = 0; i < unit.ConnectedUnits.Count; i++)
            {
                if(unit.ConnectedUnits[i] is Branch)
                {
                    Player tempplayer = Toolkit.GetPlayer(unit.ConnectedUnits[i].position);
                    if (tempplayer != null)
                        playerinbranchmove.Add(tempplayer);
                }
                bound = unit.ConnectedUnits[i].players.Count;
                shouldmove.AddRange(unit.ConnectedUnits[i].EffectedUnits(Toolkit.ReverseDirection(Starter.GetDataBase().gravity_direction)));
                shouldmove.AddRange(unit.ConnectedUnits[i].players);
                
                for (int j = 0; j < bound; j++)
                {
                    if (Toolkit.HasBranch(Toolkit.VectorSum(unit.ConnectedUnits[i].players[j].position, dir)))
                    {
                        if (Toolkit.GetBranch(Toolkit.VectorSum(unit.ConnectedUnits[i].players[j].position, dir)).PlayerForcePushIntoBranch(unit.ConnectedUnits[i].players[j] as Player, dir))
                        {
                            while (shouldmove.IndexOf(unit.ConnectedUnits[i].players[j]) != -1)
                            {
                                shouldmove.Remove(unit.ConnectedUnits[i].players[j]);
                            }
                            unit.ConnectedUnits[i].players.RemoveAt(j);
                            j--;
                            bound--;
                            continue;
                        }
                    }
                    if (!unit.ConnectedUnits[i].players[j].CanMove(dir, unit.ConnectedUnits[i].transform.parent.gameObject))
                    {
                        if (Toolkit.HasBranch(Toolkit.VectorSum(unit.ConnectedUnits[i].players[j].position, Toolkit.ReverseDirection(dir))))
                        {
                            if (Toolkit.GetBranch(Toolkit.VectorSum(unit.ConnectedUnits[i].players[j].position, Toolkit.ReverseDirection(dir))).PlayerForcePushIntoBranch(unit.ConnectedUnits[i].players[j] as Player, Toolkit.ReverseDirection(dir)))
                            {
                                while (shouldmove.IndexOf(unit.ConnectedUnits[i].players[j]) != -1)
                                    shouldmove.Remove(unit.ConnectedUnits[i].players[j]);
                                unit.ConnectedUnits[i].players.RemoveAt(j);
                                j--;
                                bound--;
                                continue;
                            }

                        }
                        else
                            apigraphic.Crush_Player_Died(unit.ConnectedUnits[i].players[j] as Player);
                    }
                    unit.ConnectedUnits[i].players.AddRange(unit.ConnectedUnits[i].players[j].players);
                }
            }
            for (int i = 0; i < unit.ConnectedUnits.Count; i++)
            {
                Unit u = unit.ConnectedUnits[i];
                apiunit.RemoveFromDatabase(u);
                u.position = Toolkit.VectorSum(u.position, Toolkit.DirectiontoVector(dir));
                apiunit.AddToDatabase(u);
               
            }
            leanmove.AddRange(GetRelatedLeanedPlayers(unit.gameObject.transform.parent.gameObject));
            Vector2 tempposition = unit.position - (Vector2)unit.gameObject.transform.localPosition;
            apiunit.RemoveFromDatabase(unit);
            unit.position = Toolkit.VectorSum(unit.position, Toolkit.DirectiontoVector(dir));
            apiunit.AddToDatabase(unit);
            for(int i=0; i<playerinbranchmove.Count; i++)
            {
                leanmove.Remove(playerinbranchmove[i]);
                shouldmove.Remove(playerinbranchmove[i]);
                apiunit.RemoveFromDatabase(playerinbranchmove[i]);
                playerinbranchmove[i].position = Toolkit.VectorSum(playerinbranchmove[i].position, Toolkit.DirectiontoVector(dir)); ;
                apiunit.AddToDatabase(playerinbranchmove[i]);
                apigraphic.Player_Co_Stop((Player)playerinbranchmove[i]);
                apigraphic.MovePlayerOnPlatform((Player)playerinbranchmove[i], ((Player)playerinbranchmove[i]).position);
            }
            for (int i=0; i<leanmove.Count; i++)
            {
                bool flag = false;
                for(int j=0; j<shouldmove.Count;j++)
                {
                    if(leanmove[i] == shouldmove[j])
                    {
                        flag = true;
                    }
                }
                if (!flag)
                {
                    Player tempplayer = leanmove[i] as Player;
                    if (tempplayer.CanMove(dir, unit.transform.parent.gameObject))
                    {
                        if(tempplayer.state == PlayerState.Lean)
                        {
                            if(tempplayer.LeanedTo.transform.parent != unit.transform.parent)
                            {
                                inputcontroller.LeanUndo(tempplayer, tempplayer.leandirection, PlayerState.Idle);
                            }
                        }
                        if (((FunctionalContainer)unit).firstmove)
                            snpmanager.AddToSnapShot(tempplayer);
                        apiunit.RemoveFromDatabase(tempplayer);
                        tempplayer.position = Toolkit.VectorSum(tempplayer.position, Toolkit.DirectiontoVector(dir));
                        apiunit.AddToDatabase(tempplayer);
                        //((Player)leanmove[i]).nextpos = Toolkit.VectorSum(leanmove[i].position, Toolkit.DirectiontoVector(dir));
                        apigraphic.Player_Co_Stop(tempplayer);
                        apigraphic.LeanStickMove(tempplayer, tempplayer.position);
                    }
                    else
                    {
                        inputcontroller.LeanUndo(tempplayer, tempplayer.leandirection, PlayerState.Idle);
                        inputcontroller.FakeLeanUndo(tempplayer, tempplayer.leandirection);
                    }
                }
            }
            for (int i = 0; i < shouldmove.Count; i++)
            {
                for (int j = i + 1; j < shouldmove.Count; j++)
                {
                    if (shouldmove[i] == shouldmove[j])
                    {
                        shouldmove.RemoveAt(j);
                        j--;
                    }
                }
                if (shouldmove[i].CanMove(dir, unit.transform.parent.gameObject))
                {
                    Player tempplayer = shouldmove[i] as Player;
                    if (tempplayer.state == PlayerState.Lean)
                    {
                        if (tempplayer.LeanedTo.transform.parent != unit.transform.parent)
                        {
                            inputcontroller.LeanUndo(tempplayer, tempplayer.leandirection, PlayerState.Idle);
                        }
                    }
                    if (((FunctionalContainer)unit).firstmove)
                        snpmanager.AddToSnapShot(shouldmove[i]);
                    apiunit.RemoveFromDatabase(shouldmove[i]);
                    shouldmove[i].position = Toolkit.VectorSum(shouldmove[i].position, Toolkit.DirectiontoVector(dir)); ;
                    apiunit.AddToDatabase(shouldmove[i]);
                    apigraphic.Player_Co_Stop((Player)shouldmove[i]);
                    apigraphic.MovePlayerOnPlatform((Player)shouldmove[i], ((Player)shouldmove[i]).position);
                }
                else
                {
                    Player tempplayer = shouldmove[i] as Player;
                    inputcontroller.LeanUndo(tempplayer, tempplayer.leandirection, PlayerState.Idle);
                    inputcontroller.FakeLeanUndo(tempplayer, tempplayer.leandirection);
                }
            }
            if (((FunctionalContainer)unit).firstmove)
                snpmanager.takesnapshot();
            else
            {
                snpmanager.MergeSnapshot();
            }
            apigraphic.MoveGameObject(unit.transform.parent.gameObject, Toolkit.VectorSum(tempposition, dir), unit);
        }
        else
        {

        }
        return true;
    }

    public List<Unit> GetRelatedLeanedPlayers(GameObject parent)
    {
        List<Unit> players = new List<Unit>();
        for(int i=0; i<database.player.Count; i++)
        {
            if (database.player[i].state == PlayerState.Lean)
            {
                if (database.player[i].LeanedTo.transform.parent.gameObject == parent)
                    players.Add(database.player[i]);
            }
        }
        return players;
    }

    public void MovePlayer(Player player, Direction dir)
    {
        //try {
        Vector2 nextpos;
            //snpmanager.AddToSnapShot(player);
            if (player.onramp && player.state != PlayerState.Jumping) //ramp move
            {
                List<Unit> units = GetUnits_ExcludingPlayer(player.position);
                Ramp ramp;
                bool goingup = true;
                if (units[0] is Ramp)
                    ramp = (Ramp)units[0];
                else
                    ramp = (Ramp)units[1];
                switch (database.gravity_direction)
                {
                    case Direction.Down:
                        switch (dir)
                        {
                            case Direction.Right: if (ramp.type == 1) goingup = false; break;
                            case Direction.Left: if (ramp.type == 4) goingup = false; break;
                        }
                        break;
                    case Direction.Right:
                        switch (dir)
                        {
                            case Direction.Up: if (ramp.type == 4) goingup = false; break;
                            case Direction.Down: if (ramp.type == 3) goingup = false; break;
                        }
                        break;
                    case Direction.Up:
                        switch (dir)
                        {
                            case Direction.Right: if (ramp.type == 2) goingup = false; break;
                            case Direction.Left: if (ramp.type == 3) goingup = false; break;
                        }
                        break;
                    case Direction.Left:
                        switch (dir)
                        {
                            case Direction.Up: if (ramp.type == 1) goingup = false; break;
                            case Direction.Down: if (ramp.type == 2) goingup = false; break;
                        }
                        break;
                }
                if (goingup)
                {
                    Vector2 temppos = Toolkit.VectorSum(player.position, dir);
                    if (Toolkit.HasBranch(temppos))
                    {
                        database.units[(int)player.position.x, (int)player.position.y].Remove(player);
                        player.position = Toolkit.VectorSum(temppos, Toolkit.ReverseDirection(database.gravity_direction));
                        database.units[(int)player.position.x, (int)player.position.y].Add(player);
                        apigraphic.MovePlayer_Ramp_2(player, player.position, ramp.type);
                    }
                    else if (Toolkit.HasRamp(temppos))
                    {
                        Ramp ramptemp = Toolkit.GetRamp(temppos);
                        nextpos = Toolkit.VectorSum(player.position, Toolkit.VectorSum(Toolkit.DirectiontoVector(Toolkit.ReverseDirection(database.gravity_direction)), Toolkit.DirectiontoVector(dir)));
                        if (Toolkit.HasBranch(nextpos))
                        {
                            database.units[(int)player.position.x, (int)player.position.y].Remove(player);
                            player.position = nextpos;
                            database.units[(int)player.position.x, (int)player.position.y].Add(player);
                            apigraphic.MovePlayer_Ramp_2(player, player.position, ramp.type);
                        }
                        else if (Toolkit.HasRamp(nextpos) && Toolkit.CanplayerGoOnRampSideFromRamp(Toolkit.GetRamp(nextpos), database.gravity_direction, dir))
                        {
                            database.units[(int)player.position.x, (int)player.position.y].Remove(player);
                            player.position = nextpos;
                            database.units[(int)player.position.x, (int)player.position.y].Add(player);
                            apigraphic.MovePlayer_Ramp_1(player, nextpos, ((Ramp)units[0]).type);
                        }
                        else if (ramptemp.type + ramp.type == 5)
                        {
                            apiunit.RemoveFromDatabase(player);
                            player.position = temppos;
                            apiunit.AddToDatabase(player);
                            apigraphic.MovePlayer_Ramp_5(player, player.position, ramptemp.type);
                        }
                        else
                        {
                            database.units[(int)player.position.x, (int)player.position.y].Remove(player);
                            player.position = nextpos;
                            database.units[(int)player.position.x, (int)player.position.y].Add(player);
                            apigraphic.MovePlayer_Ramp_2(player, nextpos, ramp.type);
                        }
                    }
                    else
                    {
                        nextpos = Toolkit.VectorSum(player.position, Toolkit.VectorSum(Toolkit.DirectiontoVector(Toolkit.ReverseDirection(database.gravity_direction)), Toolkit.DirectiontoVector(dir)));
                        units = GetUnits_ExcludingPlayer(nextpos);
                        if (units.Count == 0)
                        {
                            units = GetUnits_ExcludingPlayer(Toolkit.VectorSum(nextpos, Toolkit.DirectiontoVector(database.gravity_direction)));
                            if (units.Count == 0)
                            {
                                database.units[(int)player.position.x, (int)player.position.y].Remove(player);
                                player.position = nextpos;
                                database.units[(int)player.position.x, (int)player.position.y].Add(player);
                                apigraphic.MovePlayer_Ramp_3(player, nextpos, ramp.type);

                            }
                            else
                            {
                                if (units[0] is Ramp)
                                {
                                    database.units[(int)player.position.x, (int)player.position.y].Remove(player);
                                    if (Toolkit.IsdoubleRamp(units[0].position))
                                    {
                                        int type = Toolkit.GetRamp(player.position).type;
                                        player.position = nextpos;
                                        database.units[(int)player.position.x, (int)player.position.y].Add(player);
                                        apigraphic.MovePlayer_Ramp_2(player, player.position, type);
                                    }
                                    else if (((Ramp)units[0]).IsOnRampSide(Toolkit.ReverseDirection(database.gravity_direction)))
                                    {
                                        player.position = Toolkit.VectorSum(player.position, Toolkit.DirectiontoVector(dir));
                                        database.units[(int)player.position.x, (int)player.position.y].Add(player);
                                        apigraphic.MovePlayer_Ramp_5(player, Toolkit.VectorSum(nextpos, Toolkit.DirectiontoVector(database.gravity_direction)), ((Ramp)units[0]).type);
                                    }
                                    else
                                    {
                                        int type = Toolkit.GetRamp(player.position).type;
                                        player.position = nextpos;
                                        database.units[(int)player.position.x, (int)player.position.y].Add(player);
                                        apigraphic.MovePlayer_Ramp_2(player, player.position, type);
                                    }
                                }
                                else
                                {
                                    database.units[(int)player.position.x, (int)player.position.y].Remove(player);
                                    player.position = nextpos;
                                    database.units[(int)player.position.x, (int)player.position.y].Add(player);
                                    apigraphic.MovePlayer_Ramp_2(player, nextpos, ramp.type);
                                }
                            }
                        }
                        else
                        {
                            if (Toolkit.HasBranch(nextpos))
                            {
                                database.units[(int)player.position.x, (int)player.position.y].Remove(player);
                                player.position = nextpos;
                                database.units[(int)player.position.x, (int)player.position.y].Add(player);
                                apigraphic.MovePlayer_Ramp_2(player, player.position, ramp.type);
                            }
                            else if (Toolkit.HasDrainer(nextpos))
                            {
                                database.units[(int)player.position.x, (int)player.position.y].Remove(player);
                                player.position = nextpos;
                                database.units[(int)player.position.x, (int)player.position.y].Add(player);
                                apigraphic.MovePlayer_Simple_1(player, player.position);
                            }
                            else if (Toolkit.CanplayerGoOnRampSideFromRamp(Toolkit.GetRamp(nextpos), database.gravity_direction, dir))
                            {
                                database.units[(int)player.position.x, (int)player.position.y].Remove(player);
                                player.position = nextpos;
                                database.units[(int)player.position.x, (int)player.position.y].Add(player);
                                apigraphic.MovePlayer_Ramp_1(player, nextpos, ((Ramp)units[0]).type);
                            }
                            else if (Toolkit.GetRamp(nextpos).IsOnRampSide(Toolkit.ReverseDirection(dir))) //zir pelle az ramp be zir pelle going up
                            {
                                database.units[(int)player.position.x, (int)player.position.y].Remove(player);
                                player.position = nextpos;
                                database.units[(int)player.position.x, (int)player.position.y].Add(player);
                                apigraphic.MovePlayer_Ramp_2(player, nextpos, ramp.type);
                            }
                            else
                            {
                                database.units[(int)player.position.x, (int)player.position.y].Remove(player);
                                player.position = Toolkit.VectorSum(player.position, dir);
                                database.units[(int)player.position.x, (int)player.position.y].Add(player);
                                apigraphic.MovePlayer_Ramp_5(player, player.position, Toolkit.GetRamp(player.position).type);
                            }
                        }
                    }
                }
                else
                {
                    nextpos = Toolkit.VectorSum(player.position, Toolkit.DirectiontoVector(dir));
                    units = GetUnits_ExcludingPlayer(nextpos);
                    if (Toolkit.HasRamp(nextpos) && Toolkit.GetRamp(nextpos).ComingOnRampSide(player.position))
                    {
                        database.units[(int)player.position.x, (int)player.position.y].Remove(player);
                        player.position = nextpos;
                        database.units[(int)player.position.x, (int)player.position.y].Add(player);
                        apigraphic.MovePlayer_Ramp_4(player, nextpos, ((Ramp)units[0]).type);
                    }
                    else
                    {
                        units = GetUnits_ExcludingPlayer(Toolkit.VectorSum(nextpos, Toolkit.DirectiontoVector(database.gravity_direction)));
                        database.units[(int)player.position.x, (int)player.position.y].Remove(player);
                        if (Toolkit.HasRamp(Toolkit.VectorSum(nextpos, Toolkit.DirectiontoVector(database.gravity_direction))))
                        {
                            if (Toolkit.IsdoubleRamp(Toolkit.VectorSum(nextpos, Toolkit.DirectiontoVector(database.gravity_direction))))
                            {
                                player.position = nextpos;
                                database.units[(int)player.position.x, (int)player.position.y].Add(player);
                                apigraphic.MovePlayer_Ramp_2(player, player.position, ramp.type);
                            }
                            else if (!Toolkit.GetRamp(Toolkit.VectorSum(nextpos, Toolkit.DirectiontoVector(database.gravity_direction))).IsOnRampSide(Toolkit.ReverseDirection(database.gravity_direction)))
                            {
                                player.position = nextpos;
                                database.units[(int)player.position.x, (int)player.position.y].Add(player);
                                apigraphic.MovePlayer_Ramp_2(player, player.position, ramp.type);
                            }
                            else
                            {
                                ramp = Toolkit.GetRamp(Toolkit.VectorSum(nextpos, Toolkit.DirectiontoVector(database.gravity_direction)));
                                player.position = Toolkit.VectorSum(nextpos, Toolkit.DirectiontoVector(database.gravity_direction));
                                database.units[(int)player.position.x, (int)player.position.y].Add(player);
                                apigraphic.MovePlayer_Ramp_1(player, Toolkit.VectorSum(nextpos, Toolkit.DirectiontoVector(database.gravity_direction)), ramp.type);
                            }
                        }
                        else if (!Toolkit.IsEmpty(Toolkit.VectorSum(nextpos, Toolkit.DirectiontoVector(database.gravity_direction))))
                        {
                            player.position = nextpos;
                            database.units[(int)player.position.x, (int)player.position.y].Add(player);
                            apigraphic.MovePlayer_Ramp_2(player, nextpos, ramp.type);
                        }
                        else
                        {
                            player.position = nextpos;
                            database.units[(int)player.position.x, (int)player.position.y].Add(player);
                            apigraphic.MovePlayer_Ramp_3(player, nextpos, ramp.type);
                        }
                    }
                }
            }
            else //not on ramp
            {
                if (Toolkit.IsInsideBranch(player) && player.state != PlayerState.Jumping) //branch move
                {
                    apigraphic.BranchLight(false, Toolkit.GetBranch(player.position),player);
                    nextpos = Toolkit.VectorSum(player.position, Toolkit.DirectiontoVector(dir));
                    Vector2 temp = Toolkit.VectorSum(nextpos, Toolkit.DirectiontoVector(database.gravity_direction));
                    List<Unit> units = GetUnits_ExcludingPlayer(nextpos);
                    if (units.Count != 0)
                    {
                        if(units[0] is Branch)
                        {
                            database.units[(int)player.position.x, (int)player.position.y].Remove(player);
                            player.position = nextpos;
                            database.units[(int)player.position.x, (int)player.position.y].Add(player);
                            apigraphic.MovePlayer_Branch_Branch(player, player.position);
                        }
                        else if (units[0] is Ramp)
                        {
                            if (Toolkit.IsdoubleRamp(nextpos))
                                return;
                            else if (((Ramp)units[0]).IsOnRampSide(Toolkit.ReverseDirection(dir)))
                            {
                                database.units[(int)player.position.x, (int)player.position.y].Remove(player);
                                player.position = nextpos;
                                database.units[(int)player.position.x, (int)player.position.y].Add(player);
                                apigraphic.MovePlayer_Branch_3(player, nextpos, ((Ramp)units[0]).type,dir);
                                player.SetState(PlayerState.Busy);
                            }
                        }
                        else
                        {
                            database.units[(int)player.position.x, (int)player.position.y].Remove(player);
                            player.position = nextpos;
                            database.units[(int)player.position.x, (int)player.position.y].Add(player);
                            apigraphic.MovePlayer_Branch_1(player, nextpos,dir);
                            player.SetState(PlayerState.Busy);
                        }
                    }
                    else
                    {
                        if (Toolkit.HasRamp(temp) && !Toolkit.IsdoubleRamp(temp))
                        {
                            Ramp ramp = Toolkit.GetRamp(temp);
                            if (ramp.IsOnRampSide(Toolkit.ReverseDirection(database.gravity_direction)))
                            {
                                database.units[(int)player.position.x, (int)player.position.y].Remove(player);
                                player.position = temp;
                                database.units[(int)player.position.x, (int)player.position.y].Add(player);
                                apigraphic.MovePlayer_Branch_3(player, player.position, ramp.type,dir);
                                player.SetState(PlayerState.Busy);
                            }
                            else
                            {
                                database.units[(int)player.position.x, (int)player.position.y].Remove(player);
                                player.position = nextpos;
                                database.units[(int)player.position.x, (int)player.position.y].Add(player);
                                apigraphic.MovePlayer_Branch_1(player, player.position, dir);
                                player.SetState(PlayerState.Busy);
                            }
                        }
                        else
                        {
                            database.units[(int)player.position.x, (int)player.position.y].Remove(player);
                            player.position = nextpos;
                            database.units[(int)player.position.x, (int)player.position.y].Add(player);
                            apigraphic.MovePlayer_Branch_1(player, nextpos,dir);
                            player.SetState(PlayerState.Busy);
                        }
                    }
                }
                else //simple move
                {
                    nextpos = Toolkit.VectorSum(player.position, Toolkit.DirectiontoVector(dir));
                    Vector2 temp = Toolkit.VectorSum(nextpos, Toolkit.DirectiontoVector(database.gravity_direction));
                    List<Unit> units = GetUnits_ExcludingPlayer(nextpos);
                    if (units.Count != 0)
                    {
                        if(units[0] is Drainer)
                        {
                            database.units[(int)player.position.x, (int)player.position.y].Remove(player);
                            player.position = nextpos;
                            database.units[(int)player.position.x, (int)player.position.y].Add(player);
                            apigraphic.MovePlayer_Simple_1(player, nextpos);
                        }

                        if (units[0] is Branch)
                        {
                            database.units[(int)player.position.x, (int)player.position.y].Remove(player);
                            player.position = nextpos;
                            database.units[(int)player.position.x, (int)player.position.y].Add(player);
                            apigraphic.MovePlayer_Simple_2(player, nextpos, dir);
                        }
                        else if (units[0] is Ramp)
                        {
                            database.units[(int)player.position.x, (int)player.position.y].Remove(player);
                            if (Toolkit.CanplayerGoOnRampSideFromFromNoneRamp((Ramp)units[0], database.gravity_direction, dir))
                            {
                                player.position = nextpos;
                                database.units[(int)player.position.x, (int)player.position.y].Add(player);
                                apigraphic.MovePlayer_Simple_3(player, nextpos, Toolkit.GetRamp(nextpos).type);
                            }
                            else if (Toolkit.HasRamp(temp))
                            {
                                if (Toolkit.CanplayerGoOnRampSideFromFromNoneRamp(Toolkit.GetRamp(temp), database.gravity_direction, Toolkit.ReverseDirection(dir)))
                                {
                                    player.position = temp;
                                    database.units[(int)player.position.x, (int)player.position.y].Add(player);
                                    apigraphic.MovePlayer_Simple_3(player, temp, Toolkit.GetRamp(temp).type);
                                }
                                else
                                {
                                    player.position = nextpos;
                                    database.units[(int)player.position.x, (int)player.position.y].Add(player);
                                    apigraphic.MovePlayer_Simple_4(player, nextpos);
                                }
                            }
                            else
                            {
                                player.position = nextpos;
                                database.units[(int)player.position.x, (int)player.position.y].Add(player);
                                apigraphic.MovePlayer_Simple_4(player, player.position);
                            }
                        }
                    }
                    else
                    {
                        units = GetUnits_ExcludingPlayer(temp);
                        if (units.Count != 0)
                        {
                            if (units[0] is Ramp)
                            {
                                database.units[(int)player.position.x, (int)player.position.y].Remove(player);
                                if (Toolkit.IsdoubleRamp(temp))
                                {
                                    player.position = nextpos;
                                    database.units[(int)player.position.x, (int)player.position.y].Add(player);
                                    apigraphic.MovePlayer_Simple_1(player, nextpos);
                                }

                                else if (Toolkit.CanplayerGoOnRampSideFromFromNoneRamp(units[0] as Ramp, database.gravity_direction, Toolkit.ReverseDirection(dir)))
                                {
                                    player.position = temp;
                                    database.units[(int)player.position.x, (int)player.position.y].Add(player);
                                    apigraphic.MovePlayer_Simple_5(player, temp, ((Ramp)units[0]).type);
                                }
                                else
                                {
                                    player.position = nextpos;
                                    database.units[(int)player.position.x, (int)player.position.y].Add(player);
                                    apigraphic.MovePlayer_Simple_4(player, nextpos);
                                }

                            }
                            else
                            {
                                database.units[(int)player.position.x, (int)player.position.y].Remove(player);
                                player.position = nextpos;
                                database.units[(int)player.position.x, (int)player.position.y].Add(player);
                                apigraphic.MovePlayer_Simple_1(player, nextpos);
                            }
                        }
                        else
                        {
                            database.units[(int)player.position.x, (int)player.position.y].Remove(player);
                            player.position = nextpos;
                            database.units[(int)player.position.x, (int)player.position.y].Add(player);
                            apigraphic.MovePlayer_Simple_4(player, player.position);
                        }
                    }
                }
            }
            snpmanager.takesnapshot();
        /*}
        catch
        {
            Debug.Log("cant move!!!");
        }*/
    }

    public void Undo()
    {
        apigraphic.Undo_Objects();
        snpmanager.MergeSnapshot();
        snpmanager.Undo();
    }

    public void Applygravity()
    {
        for (int i=0; i<database.player.Count; i++)
        {
            database.player[i].ApplyGravity();
        }
        //apiinput.PlayerMoveFinished();
    }

    public void graphic_LandFinished(Player player)
    {
        player.movepercentage = 0;
        player.SetState(PlayerState.Idle);
    }

    
    public List<Unit> GetUnits(Vector2 position)
    {
        try {
            return database.units[(int)position.x, (int)position.y];
        }
        catch
        {
            Debug.Log("position out of range");
            return null;
        }
    }

    public List<Unit> GetUnits_ExcludingPlayer(Vector2 position)
    {
        try
        {
            List<Unit> tempunit = database.units[(int)position.x, (int)position.y];
            for (int i = 0; i < tempunit.Count; i++)
            {
                if (tempunit[i] is Player)
                {
                    tempunit.RemoveAt(i);
                    i--;
                }
            }
            return tempunit;
        }
        catch
        {
            Debug.Log("position out of range");
            return null;
        }
    }

    public void Input_Move(Direction direction)
    {
        for(int i=0; i<database.player.Count; i++)
        {
            inputcontroller.PlayerMoveAction(database.player[i], direction);
        }
        if (direction == Direction.Up || direction == Direction.Down) {
            for (int i = 0; i < database.bots.Count; i++)
            {
                if(database.bots[i] is MockingBot)
                {
                    (database.bots[i] as MockingBot).FakeLean(direction);
                }
            }
        }
    }

    

    public void ActionKeyPressed(bool KeyUp)
    {
        for(int i=0; i<database.player.Count; i++)
        {
            if (database.player[i].state == PlayerState.Idle || database.player[i].state == PlayerState.Lean || database.player[i].state == PlayerState.Gir)
            {
                if (!Toolkit.IsInsideBranch(database.player[i]))
                {
                    if (database.player[i].state == PlayerState.Lean)
                    {
                        /*//Vector2 newpos = Toolkit.VectorSum(database.player[i].position, Toolkit.DirectiontoVector(database.player[i].leandirection));
                        Vector2 newpos = Toolkit.GetNearestUnit(database.player[i].transform.position, database.player[i].leandirection);
                        List<Unit> units = GetUnits(newpos);
                        for (int j = 0; j < units.Count; j++)
                        {
                            if (units[i] is ParentContainer)
                            {
                                if(KeyUp)
                                    ((ParentContainer)units[i]).Action(database.player[i], Toolkit.ReverseDirection(database.player[i].leandirection));
                                else if(units[i] is FunctionalContainer)
                                    ((FunctionalContainer)units[i]).ActionKeyDown(database.player[i], Toolkit.ReverseDirection(database.player[i].leandirection));
                            }
                            else if (units[i] is Fountain)
                            {
                                ((Fountain)units[i]).Action(database.player[i]);
                            }
                        }*/
                        if (database.player[i].LeanedTo is ParentContainer)
                        {
                            if (KeyUp)
                                ((ParentContainer)database.player[i].LeanedTo).Action(database.player[i], Toolkit.ReverseDirection(database.player[i].leandirection));
                            else if (database.player[i].LeanedTo is FunctionalContainer)
                                ((FunctionalContainer)database.player[i].LeanedTo).ActionKeyDown(database.player[i], Toolkit.ReverseDirection(database.player[i].leandirection));
                            else if (database.player[i].LeanedTo is Leanable)
                            {
                                ((Leanable)database.player[i].LeanedTo).LeanedAction(database.player[i], Toolkit.ReverseDirection(database.player[i].leandirection));
                            }
                        }
                        else if(database.player[i].LeanedTo is Fountain)
                        {
                            if (!KeyUp)
                            {
                                (database.player[i].LeanedTo as Fountain).Reset(database.player[i]);
                            }
                        }
                        else if(database.player[i].LeanedTo is Enemy)
                        {
                            if (KeyUp)
                            {
                                (database.player[i].LeanedTo as Enemy).OnOff();

                            }
                        }
                    }
                    else
                    {
                        database.player[i].Action();
                    }
                }
            }
        }
    }

    public void ActionKeyPressed(Direction dir)
    {
        for (int i=0; i<database.player.Count; i++)
        {
            if (database.player[i].state != PlayerState.Idle)
                continue;
            if (database.player[i].state == PlayerState.Lean)
                continue;
            database.player[i].Action(dir);
        }
    }

    public void JumpKeyprssed()
    {
        for (int i = 0; i < database.player.Count; i++)
        {
            if(database.player[i].CanJump)
                inputcontroller.Jump(database.player[i]);
        }
    }

    public void graphic_PlayerMoveAnimationFinished(Player player)
    {
        player.movepercentage = 0;
        if (player.state == PlayerState.Lean)
            return;
        if (player.state == PlayerState.Gir)
            return;
        if (Toolkit.IsInsideBranch(player))
            return;
        if (!player.ApplyGravity())
            player.SetState(PlayerState.Idle);

    }

    public void graphic_LeanStickMoveFinished(Player player)
    {
        /*apiunit.RemoveFromDatabase(player);
        player.position = player.nextpos;
        apiunit.AddToDatabase(player);*/
        if (player.state == PlayerState.Gir)
            return;
        Applygravity();
        if (player.state != PlayerState.Lean)
            player.SetState(PlayerState.Idle);
    }

    public void graphic_GameObjectMoveAnimationFinished_90percent(GameObject gameobject, Unit unit)
    {
        if (unit is Player)
        {
            if (((Player)unit).state == PlayerState.Gir)
                return;
        }
        if (unit == null)
            return;
        //unit.gameObject.transform.parent.gameObject.GetComponent<ParentScript>().movelock = false;
        /*apiunit.RemoveFromDatabase(unit);
        unit.position = unit.next_pos;
        apiunit.AddToDatabase(unit);*/
        /*for(int i=0; i<unit.ConnectedUnits.Count; i++)
        {
            apiunit.RemoveFromDatabase(unit.ConnectedUnits[i]);
            unit.ConnectedUnits[i].position = unit.ConnectedUnits[i].next_pos;
            apiunit.AddToDatabase(unit.ConnectedUnits[i]);
        }*/
        if(unit is FunctionalContainer)
        {
            if (lockcombination != null)
                lockcombination.Check();
            apiunit.GameObjectAnimationFinished((FunctionalContainer)unit);
            lasercontroller.SetLasers();
            for(int i=0; i<database.player.Count; i++)
            {
                lasercontroller.CollisionCheck(database.player[i].position);
            }
            (unit as FunctionalContainer).CheckNextMove();
        }
        //CheckStuckedUnit(unit);
    }
    public void graphic_AbsorbReleaseFinished(Player player)
    {
    }

    public void graphic_PlayerChangeDirectionFinished(Player player)
    {
        //player.state = PlayerState.Idle;
    }
    public void UnitToGraphic_Land(Unit unit, Unit landingunit,Vector2 landingposition)
    {
        database.units[(int)landingposition.x, (int)landingposition.y].Remove(landingunit);
        landingunit.position = landingposition;
        database.units[(int)landingposition.x, (int)landingposition.y].Add(landingunit);
        apigraphic.Land((Player)unit, landingposition, landingunit);
    }

    public void UnitToGraphic_LandOnRamp(Unit unit, Ramp landingunit, Vector2 landingposition, int landingtype)
    {
        database.units[(int)landingposition.x, (int)landingposition.y].Remove(landingunit);
        landingunit.position = landingposition;
        database.units[(int)landingposition.x, (int)landingposition.y].Add(landingunit);
        apigraphic.LandOnRamp((Player)unit, landingposition, landingunit, landingtype);
    }

    public void CheckStuckedUnit()
    {
        for (int i = 0; i < stuckedunits.Count; i++)
        {
            Unit u = stuckedunits[i];
                
            if (u is FunctionalContainer)
            {
                if (!u.gameObject.transform.parent.gameObject.GetComponent<ParentScript>().movelock)
                {
                    stuckedunits.RemoveAt(i);
                    ((FunctionalContainer)u).firstmove = false;
                    snpmanager.AddToSnapShot(u);
                    snpmanager.AddToSnapShot(u.ConnectedUnits);
                    ((FunctionalContainer)u).Action_Fuel();
                }
            }
        }
    }

    public void CheckStuckedUnit(Unit ExceptThis)
    {
        for (int i = 0; i < stuckedunits.Count; i++)
        {
            Unit u = stuckedunits[i];
            if (u == ExceptThis)
            {
                return;
            }
            if (u is FunctionalContainer)
            {
                if (!u.gameObject.transform.parent.gameObject.GetComponent<ParentScript>().movelock)
                {
                    stuckedunits.RemoveAt(i);
                    ((FunctionalContainer)u).firstmove = false;
                    snpmanager.AddToSnapShot(u);
                    snpmanager.AddToSnapShot(u.ConnectedUnits);
                    ((FunctionalContainer)u).Action_Fuel();
                }
            }
        }
    }
    public bool AdjustPlayer(Player player,Direction direction , Action<Player, Direction> passingmethod)
    {
        return false;
        if (player.transform.position.x == player.position.x && player.transform.position.y == player.position.y)
            return false;
        if (Toolkit.HasRamp(player.position) && !Toolkit.IsdoubleRamp(player.position) && Toolkit.GetRamp(player.position).IsOnRampSide(Toolkit.ReverseDirection(database.gravity_direction)))
            return false;
        apigraphic.Player_Co_Stop(player);
        player.SetState(PlayerState.Adjust);
        if (passingmethod == MovePlayerToDirection)
        {
            if (direction != player.direction)
            {
                Direction olddir = player.direction;
                player.direction = direction;
                player.GetComponent<PlayerGraphics>().Move_Animation(direction);
            }
        }
        apigraphic.AdjustPlayer(player, player.position, direction, passingmethod);
        return true;
    }

    public void JumpToDirection(Player player, Direction direction)
    {
        inputcontroller.Jump(player);
    }
    
    public void MovePlayerToDirection(Player player, Direction direction)
    {

    }

    public void ShowHologram()
    {
        for(int i=0; i<database.player.Count; i++)
        {
            database.player[i].GetComponent<PlayerGraphics>().ShowHologram();
        }
    }

    public void HideHologram()
    {
        for (int i = 0; i < database.player.Count; i++)
        {
            database.player[i].GetComponent<PlayerGraphics>().HideHologram();
        }
    }
}

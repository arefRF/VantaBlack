using UnityEngine;
using System.Collections.Generic;

public class LogicalEngine {

    public APIGraphic apigraphic;
    public APIInput apiinput;
    public APIUnit apiunit;
    public Database database;
    SubEngine_Initializer initializer;
    InputController inputcontroller;
    int sizeX, sizeY;
    public List<Unit> stuckedunits;
    Object lock_move;


    private List<Unit> leanmove;
    public LogicalEngine(int x, int y)
    {
        lock_move = new Object();
        sizeX = x;
        sizeY = y;
        stuckedunits = new List<Unit>();
        apigraphic = new APIGraphic(this);
        apiinput = new APIInput(this);
        apiunit = new APIUnit(this);
        database = Starter.GetDataBase();
        leanmove = new List<Unit>();
        initializer = new SubEngine_Initializer(x,y, this);
    }

    public void Run()
    {
        database.units = initializer.init();
        database.state = State.Idle;
        inputcontroller = new InputController(this);
        //Applygravity();
    }

    public bool MoveUnit(Unit unit, Direction dir)
    {
        List<Unit> shouldmove = new List<Unit>();
        leanmove = new List<Unit>();
        if (!(unit is Box))
        {
            if (!unit.CanMove(dir, unit.transform.parent.gameObject))
                return false;
            shouldmove.AddRange(unit.players);
            for (int i = 0; i < unit.ConnectedUnits.Count; i++)
            {
                if (!unit.ConnectedUnits[i].CanMove(dir, unit.transform.parent.gameObject))
                {
                    return false;
                }
                shouldmove.AddRange(unit.ConnectedUnits[i].players);
            }
            for (int i = 0; i < unit.ConnectedUnits.Count; i++)
            {
                Unit u = unit.ConnectedUnits[i];
                Vector2 newpos = Toolkit.VectorSum(u.position, Toolkit.DirectiontoVector(dir));
                database.units[(int)u.position.x, (int)u.position.y].Remove(u);
                u.position = newpos;
                database.units[(int)u.position.x, (int)u.position.y].Add(u);
            }
            leanmove.AddRange(GetRelatedLeanedPlayers(unit.gameObject.transform.parent.gameObject));
            database.units[(int)unit.position.x, (int)unit.position.y].Remove(unit);
            unit.position = Toolkit.VectorSum(unit.position, Toolkit.DirectiontoVector(dir));
            database.units[(int)unit.position.x, (int)unit.position.y].Add(unit);
            for(int i=0; i<leanmove.Count; i++)
            {
                bool flag = false;
                for(int j=0; j<shouldmove.Count;j++)
                {
                    if(leanmove[i] == shouldmove[j])
                    {
                        Debug.Log(leanmove[i]);
                        Debug.Log(shouldmove[j]);
                        
                        flag = true;
                    }
                }
                if (!flag && leanmove[i].CanMove(dir, unit.transform.parent.gameObject))
                {
                    database.units[(int)leanmove[i].position.x, (int)leanmove[i].position.y].Remove(leanmove[i]);
                    ((Player)leanmove[i]).nextpos = Toolkit.VectorSum(leanmove[i].position, Toolkit.DirectiontoVector(dir));
                    apigraphic.LeanStickMove((Player)leanmove[i], dir);
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
                    database.units[(int)shouldmove[i].position.x, (int)shouldmove[i].position.y].Remove(shouldmove[i]);
                    shouldmove[i].position = Toolkit.VectorSum(shouldmove[i].position, Toolkit.DirectiontoVector(dir));
                    database.units[(int)shouldmove[i].position.x, (int)shouldmove[i].position.y].Add(shouldmove[i]);
                    apigraphic.MovePlayerOnPlatform((Player)shouldmove[i], dir);
                }
            }

            apigraphic.MoveGameObject(unit.transform.parent.gameObject, dir, unit);
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
            if (database.player[i].lean)
            {
                if (database.player[i].IsRelatedLean(parent))
                    players.Add(database.player[i]);
            }
        }
        return players;
    }

    public void MovePlayer(Player player, Direction dir)
    {
        Vector2 nextpos;
        if (player.onramp)
        {
            List<Unit> units = GetUnits(player.position);
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
                nextpos = Toolkit.VectorSum(player.position, Toolkit.VectorSum(Toolkit.DirectiontoVector(Toolkit.ReverseDirection(database.gravity_direction)), Toolkit.DirectiontoVector(dir)));
                units = GetUnits(nextpos);
                if (units.Count == 0)
                {
                    units = GetUnits(Toolkit.VectorSum(nextpos, Toolkit.DirectiontoVector(database.gravity_direction)));
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
                            Vector2 temp = Toolkit.VectorSum(player.position, Toolkit.DirectiontoVector(dir));
                            player.position = temp;
                            database.units[(int)player.position.x, (int)player.position.y].Add(player);
                            apigraphic.MovePlayer_Ramp_5(player, Toolkit.VectorSum(nextpos, Toolkit.DirectiontoVector(database.gravity_direction)),((Ramp)units[0]).type);
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
                    if (Toolkit.CanplayerGoOnRampSideFromRamp(Toolkit.GetRamp(nextpos), database.gravity_direction, dir))
                    {
                        database.units[(int)player.position.x, (int)player.position.y].Remove(player);
                        player.position = nextpos;
                        database.units[(int)player.position.x, (int)player.position.y].Add(player);
                        apigraphic.MovePlayer_Ramp_1(player, nextpos, ((Ramp)units[0]).type);
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
            else
            {
                nextpos = Toolkit.VectorSum(player.position, Toolkit.DirectiontoVector(dir));
                units = GetUnits(nextpos);
                if(Toolkit.HasRamp(nextpos) && Toolkit.GetRamp(nextpos).ComingOnRampSide(player.position))
                {
                    database.units[(int)player.position.x, (int)player.position.y].Remove(player);
                    player.position = nextpos;
                    database.units[(int)player.position.x, (int)player.position.y].Add(player);
                    apigraphic.MovePlayer_Ramp_4(player, nextpos, ((Ramp)units[0]).type);
                }
                else
                {
                    units = GetUnits(Toolkit.VectorSum(nextpos, Toolkit.DirectiontoVector(database.gravity_direction)));
                    database.units[(int)player.position.x, (int)player.position.y].Remove(player);
                    if (Toolkit.HasRamp(Toolkit.VectorSum(nextpos, Toolkit.DirectiontoVector(database.gravity_direction))))
                    {
                        ramp = Toolkit.GetRamp(Toolkit.VectorSum(nextpos, Toolkit.DirectiontoVector(database.gravity_direction)));
                        player.position = Toolkit.VectorSum(nextpos, Toolkit.DirectiontoVector(database.gravity_direction));
                        database.units[(int)player.position.x, (int)player.position.y].Add(player);
                        apigraphic.MovePlayer_Ramp_1(player, Toolkit.VectorSum(nextpos, Toolkit.DirectiontoVector(database.gravity_direction)),ramp.type);
                        
                    }
                    else if(!Toolkit.IsEmpty(Toolkit.VectorSum(nextpos, Toolkit.DirectiontoVector(database.gravity_direction))))
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
            if (Toolkit.IsInsideBranch(player))
            {
                nextpos = Toolkit.VectorSum(player.position, Toolkit.DirectiontoVector(dir));
                Vector2 temp = Toolkit.VectorSum(nextpos, Toolkit.DirectiontoVector(database.gravity_direction));
                List<Unit> units = GetUnits(temp);
                if(units.Count != 0)
                {
                    if(units[0] is Ramp)
                    {
                        database.units[(int)player.position.x, (int)player.position.y].Remove(player);
                        player.position = nextpos;
                        database.units[(int)player.position.x, (int)player.position.y].Add(player);
                        apigraphic.MovePlayer_Branch_3(player, temp, ((Ramp)units[0]).type);
                    }
                    else
                    {
                        database.units[(int)player.position.x, (int)player.position.y].Remove(player);
                        player.position = nextpos;
                        database.units[(int)player.position.x, (int)player.position.y].Add(player);
                        apigraphic.MovePlayer_Branch_1(player, nextpos);
                    }
                }
                else
                {
                    database.units[(int)player.position.x, (int)player.position.y].Remove(player);
                    player.position = nextpos;
                    database.units[(int)player.position.x, (int)player.position.y].Add(player);
                    apigraphic.MovePlayer_Branch_2(player, nextpos);
                }
            }
            else //simple move
            {
                nextpos = Toolkit.VectorSum(player.position, Toolkit.DirectiontoVector(dir));
                Vector2 temp = Toolkit.VectorSum(nextpos, Toolkit.DirectiontoVector(database.gravity_direction));
                List<Unit> units = GetUnits(nextpos);
                if (units.Count != 0)
                {
                    Debug.Log(units.Count);
                    if(units[0] is Branch)
                    {
                        database.units[(int)player.position.x, (int)player.position.y].Remove(player);
                        player.position = nextpos;
                        database.units[(int)player.position.x, (int)player.position.y].Add(player);
                        apigraphic.MovePlayer_Simple_2(player, nextpos);
                    }
                    else if(units[0] is Ramp)
                    {
                        database.units[(int)player.position.x, (int)player.position.y].Remove(player);
                        if(Toolkit.CanplayerGoOnRampSideFromFromNoneRamp((Ramp)units[0], database.gravity_direction, dir))
                        {
                            player.position = nextpos;
                            database.units[(int)player.position.x, (int)player.position.y].Add(player);
                            apigraphic.MovePlayer_Simple_3(player, nextpos, Toolkit.GetRamp(nextpos).type);
                        }
                        else if(Toolkit.HasRamp(temp))
                        {
                            if(Toolkit.CanplayerGoOnRampSideFromFromNoneRamp(Toolkit.GetRamp(temp), database.gravity_direction, Toolkit.ReverseDirection(dir)))
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
                            database.units[(int)player.position.x, (int)player.position.y].Add(player);
                            graphic_PlayerMoveAnimationFinished(player);
                            return;
                        }
                    }
                }
                else
                {
                    units = GetUnits(temp);
                    if(units.Count != 0)
                    {
                        if(units[0] is Ramp)
                        {
                            database.units[(int)player.position.x, (int)player.position.y].Remove(player);
                            if (Toolkit.IsdoubleRamp(units[0].position))
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
                        apigraphic.MovePlayer_Simple_4(player, nextpos);
                    }
                }
            }
        }
    }
    public void Applygravity()
    {
        for(int i=0; i<database.player.Count; i++)
        {
            database.player[i].ApplyGravity(database.gravity_direction, database.units);
        }
        //apiinput.PlayerMoveFinished();
    }

    public void graphic_FallFinished(Player player)
    {
        bool isonunit = true;
        if (GetUnits(Toolkit.VectorSum(Toolkit.DirectiontoVector(database.gravity_direction), player.position)).Count == 0)
            isonunit = false;
        if (isonunit)
        {
            player.movepercentage = 0;
        }
        else
            player.ApplyGravity(database.gravity_direction, database.units);
    }

    public void graphic_LandFinished(Player player)
    {
        player.movepercentage = 0;
        player.state = PlayerState.Idle;
    }

    public void Lean(Player player, Direction direction)
    {
        if (!player.lean)
        {
            player.lean = true;
            player.leandirection = direction;
            apigraphic.Lean(player);
        }
    }
    public List<Unit> GetUnits(Vector2 position)
    {
        return database.units[(int)position.x, (int)position.y];
    }

    public void Input_Move(Direction direction)
    {
        for(int i=0; i<database.player.Count; i++)
        {
            inputcontroller.PlayerMoveAction(database.player[i], direction);
        }
    }

    public void Input_AbsorbRlease(Direction dir)
    {
        for(int i=0; i<database.player.Count; i++)
        {
            if (database.player[i].lean) //for absorb
            {
                if (database.player[i].leandirection == dir)
                {
                    List<Unit> units = GetUnits(Toolkit.VectorSum(database.player[i].position, Toolkit.DirectiontoVector(dir)));
                    for (int j = 0; j < units.Count; j++)
                    {
                        if (units[j] is Container)
                        {
                            database.player[i].Release((Container)units[j]);
                            break;
                        }
                    }
                }
                else // for release
                {
                    List<Unit> units = GetUnits(Toolkit.VectorSum(database.player[i].position, Toolkit.DirectiontoVector(Toolkit.ReverseDirection(dir))));
                    for (int j = 0; j < units.Count; j++)
                    {
                        if (units[j] is Container)
                        {
                            database.player[i].Absorb((Container)units[j]);
                            break;
                        }
                    }
                }
            }
        }
    }
    public void Input_AbsorbRleaseHold(Direction dir)
    {
        for (int i = 0; i < database.player.Count; i++)
        {
            if (database.player[i].lean) //for absorb
            {
                if (database.player[i].leandirection == dir)
                {
                    List<Unit> units = GetUnits(Toolkit.VectorSum(database.player[i].position, Toolkit.DirectiontoVector(dir)));
                    for (int j = 0; j < units.Count; j++)
                    {
                        if (units[j] is Container)
                        {
                            database.player[i].ReleaseHold((Container)units[j]);
                            break;
                        }
                    }
                }
                else // for release
                {
                    List<Unit> units = GetUnits(Toolkit.VectorSum(database.player[i].position, Toolkit.DirectiontoVector(Toolkit.ReverseDirection(dir))));
                    for (int j = 0; j < units.Count; j++)
                    {
                        if (units[j] is Container)
                        {
                            database.player[i].AbsorbHold((Container)units[j]);
                            break;
                        }
                    }
                }
            }
        }
    }
    public void ArrowkeyReleased(Direction direction)
    {
        for(int i=0; i<database.player.Count; i++)
        {
            if (database.player[i].lean)
            {
                Debug.Log("player lean stopig");
                database.player[i].lean = false;
                apigraphic.LeanFinished(database.player[i]);
                for(int j=0; j<leanmove.Count; j++)
                {
                    if(database.player[i] == leanmove[j])
                    {
                        Debug.Log("stoping lean stick");
                        apiunit.AddToDatabase(database.player[i]);
                        apigraphic.LeanStickStop(database.player[i]);
                    }
                }
                database.player[i].ApplyGravity(database.gravity_direction, database.units);
            }
        }
        //Applygravity();
    }

    public void ActionKeyPressed()
    {
        for(int i=0; i<database.player.Count; i++)
        {
            if (database.player[i].lean)
            {
                Vector2 newpos = Toolkit.VectorSum(database.player[i].position, Toolkit.DirectiontoVector(database.player[i].leandirection));
                List<Unit> units = GetUnits(newpos);
                for(int j=0; j<units.Count; j++)
                {
                    if (units[i] is ParentContainer)
                        ((ParentContainer)units[i]).Action(database.player[i], Toolkit.ReverseDirection(database.player[i].leandirection));
                }
            }
            else
            {
                database.player[i].Action();
            }
        }
    }

    public void ActionKeyPressed(Direction dir)
    {
        for(int i=0; i<database.player.Count; i++)
        {
            if (database.player[i].lean)
                continue;
            database.player[i].Action(dir);
        }
    }

    public void graphic_PlayerMoveAnimationFinished(Player player)
    {
        player.movepercentage = 0;
        Applygravity();
        player.state = PlayerState.Idle;
    }

    public void graphic_LeanStickMoveFinished(Player player)
    {
        player.position = player.nextpos;
        apiunit.AddToDatabase(player);
        Applygravity();
        player.state = PlayerState.Idle;
    }

    public void graphic_GameObjectMoveAnimationFinished(GameObject gameobject, Unit unit)
    {
        if (unit == null)
            return;
        //unit.gameObject.transform.parent.gameObject.GetComponent<ParentScript>().movelock = false;
        
        if(unit is FunctionalContainer)
        {
            apiunit.GameObjectAnimationFinished((FunctionalContainer)unit);
        }
        CheckStuckedUnit(unit);
    }
    public void graphic_AbsorbReleaseFinished(Player player)
    {

    }

    public void graphic_PlayerChangeDirectionFinished(Player player)
    {
        player.state = PlayerState.Idle;
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
                Debug.Log(u.gameObject.transform.parent.gameObject.GetComponent<ParentScript>().movelock);
                if (!u.gameObject.transform.parent.gameObject.GetComponent<ParentScript>().movelock)
                {
                    ((FunctionalContainer)u).ResetStuckLevel();
                    stuckedunits.RemoveAt(i);
                    ((FunctionalContainer)u).Action_Fuel(false);
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
                Debug.Log(u.gameObject.transform.parent.gameObject.GetComponent<ParentScript>().movelock);
                if (!u.gameObject.transform.parent.gameObject.GetComponent<ParentScript>().movelock)
                {
                    ((FunctionalContainer)u).ResetStuckLevel();
                    stuckedunits.RemoveAt(i);
                    ((FunctionalContainer)u).Action_Fuel(false);
                }
            }
        }
    }
}

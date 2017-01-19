using UnityEngine;
using System.Collections.Generic;

public class LogicalEngine {

    public APIGraphic apigraphic;
    public APIInput apiinput;
    public APIUnit apiunit;
    public Database database;
    SubEngine_Initializer initializer;
    int sizeX, sizeY;
    public LogicalEngine(int x, int y)
    {
        sizeX = x;
        sizeY = y;
        apigraphic = new APIGraphic(this);
        apiinput = new APIInput(this);
        apiunit = new APIUnit(this);
        database = Starter.GetDataBase();
        initializer = new SubEngine_Initializer(x,y, apiunit);
    }

    public void Run()
    {
        database.units = initializer.init();
        database.state = State.Idle;
    }

    public bool MoveUnit(Unit unit, Vector2 position)
    {
        if(!(unit is Box))
        {
            
        }
        else
        {
            
        }
        return false;
    }

    public void MovePlayer(Player player, Direction dir)
    {   
        bool simplemove = true;
        apiinput.PlayerMoveStarted();
        List<Unit> units = GetUnits(player.position);
        bool onramp = false;
        Vector2 newpos = Toolkit.VectorSum(Toolkit.DirectiontoVector(dir), player.position);
        for (int i=0; i<units.Count; i++)
        {
            if(units[i] is Ramp)
            {
                onramp = true;
            }
        }
        if (onramp)
            units = GetUnits(Toolkit.VectorSum(Toolkit.VectorSum(player.position, Toolkit.DirectiontoVector(dir)), Toolkit.DirectiontoVector(database.gravity_direction)));
        else
            units = GetUnits(Toolkit.VectorSum(player.position, Toolkit.DirectiontoVector(dir)));
        for(int i=0; i<units.Count; i++)
        {
            if(units[i] is Ramp)
            {
                if (onramp) 
                    newpos = Toolkit.VectorSum(Toolkit.VectorSum(player.position, Toolkit.DirectiontoVector(dir)), Toolkit.DirectiontoVector(database.gravity_direction));
                else
                    newpos = units[i].position;
                simplemove = false;
                apigraphic.MovePlayerOnRamp(player, newpos, onramp, dir, ((Ramp)units[i]).type);
                break;
            }
            if(units[i] is Branch)
            {
                simplemove = false;
                apigraphic.MovePlayerToBranch(player, newpos, onramp);
                break;
            }
        }
        if (simplemove)
            apigraphic.MovePlayer(player, newpos, onramp);
        database.units[(int)player.position.x, (int)player.position.y].Remove(player);
        database.units[(int)newpos.x, (int)newpos.y].Add(player);
        player.position = newpos;
    }

    private void Applygravity()
    {
        for(int i=0; i<database.player.Count; i++)
        {
            ApplyGravity_Player(database.player[i]);
        }
        //apiinput.PlayerMoveFinished();
    }
    public void ApplyGravity_Player(Player player)
    {
        List<Unit> units;
        Vector2 nextpos;
        nextpos = Toolkit.VectorSum(player.position, Toolkit.DirectiontoVector(database.gravity_direction));
        units = GetUnits(nextpos);
        if (units.Count != 0)
        {
            if (units.Count == 1)
            {
                units[0].fallOn(player, Toolkit.ReverseDirection(database.gravity_direction));
                
            }
            else
            {
                Unit fallonunit = Toolkit.GetUnitToFallOn(units, Toolkit.ReverseDirection(database.gravity_direction));
                fallonunit.fallOn(player, Toolkit.ReverseDirection(database.gravity_direction));
                
            }
        }
        else
        {
            database.units[(int)player.position.x, (int)player.position.y].Remove(player);
            database.units[(int)nextpos.x, (int)nextpos.y].Add(player);
            player.position = nextpos;
            apigraphic.Fall(player, nextpos);
            
        }
    }

    public void graphic_FallFinished(Player player)
    {
        bool isonunit = true;
        if (GetUnits(Toolkit.VectorSum(Toolkit.DirectiontoVector(database.gravity_direction), player.position)).Count == 0)
            isonunit = false;
        if (isonunit)
        {
            player.state = PlayerState.Steady;
        }
        else
            ApplyGravity_Player(player);
    }

    public void graphic_LandFinished(Player player)
    {
        player.state = PlayerState.Steady;
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
            if (!database.player[i].lean)
            {
                for(int j=0; j<database.player[i].move_direction.Count; j++)
                {
                    if(database.player[i].move_direction[j] == direction)
                    {
                        if(direction == database.player[i].direction)
                        {
                            if (!database.player[i].Move(direction))
                            {
                                Lean(database.player[i], direction);
                                apiinput.PlayerMoveFinished();
                            }
                        }
                        else
                        {
                            Direction olddir = database.player[i].direction;
                            database.player[i].direction = direction;
                            apiinput.PlayerMoveStarted();
                            apigraphic.PlayerChangeDirection(database.player[i], olddir, database.player[i].direction);
                        }
                    }
                }
            }
        }
    }

    public void Input_AbsorbRlease(Direction dir)
    {
        for(int i=0; i<database.player.Count; i++)
        {
            if (database.player[i].lean)
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
                else
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

    public void ArrowkeyReleased(Direction direction)
    {
        for(int i=0; i<database.player.Count; i++)
        {
            if (database.player[i].lean)
            {
                database.player[i].lean = false;
                apigraphic.LeanFinished(database.player[i]);
            }
        }
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

    public void graphic_MoveAnimationFinished(Player player)
    {
        Applygravity();
        apiinput.PlayerMoveFinished();
    }

    public void graphic_AbsorbReleaseFinished(Player player)
    {

    }

    public void graphic_PlayerChangeDirectionFinished(Player player)
    {
        apiinput.PlayerMoveFinished();
    }

    public void UnitToGraphic_Land(Unit unit, Unit landingunit,Vector2 landingposition)
    {
        apigraphic.Land((Player)unit, landingposition, landingunit);
    }

    public void UnitToGraphic_LandOnRamp(Unit unit, Ramp landingunit, Vector2 landingposition, int landingtype)
    {
        apigraphic.LandOnRamp((Player)unit, landingposition, landingunit, landingtype);
    }

    public void UnitToGraphic_Absorb(Player player, Container container, AbilityType ability)
    {

    }

    public void UnitToGraphic_Release(Player player, Container container, AbilityType ability)
    {

    }
    public void UnitToGraphic_Swap(Player player, Container container, AbilityType ability)
    {

    }
}

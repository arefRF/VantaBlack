using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Player : Unit
{
    public List<AbilityType> abilities;
    public List<Direction> move_direction;
    public Direction direction { get; set; }
    public int movepercentage { get; set; }
    public PlayerState state { get; set; }
    public Direction leandirection { get; set; }
    public bool lean { get; set; }

    public bool onramp { get; set; }
    public Direction gravity { get; set; }

    public Vector2 nextpos { get; set; }

    public void Awake()
    {
        direction = move_direction[0];
    }

    public bool Should_Change_Direction(Direction dir)
    {
        for (int i = 0; i < move_direction.Count; i++)
            if (dir == move_direction[i])
                if (dir != direction)
                    return true;
        return false;
    }

    public bool Can_Move_Direction(Direction dir)
    {
        if (Can_Lean(dir))
        {
            Vector2 pos = Toolkit.VectorSum(position, Toolkit.DirectiontoVector(dir));
            if (Toolkit.IsInsideBranch(pos))
                return true;
        }
        for (int i = 0; i < move_direction.Count; i++)
            if (dir == move_direction[i])
                return true;
        return false;
    }
    public bool Can_Lean(Direction dir)
    {
        if (dir == Direction.Up || dir == Direction.Down)
            return true;
        else
            return false;
    }

    public override bool Move(Direction dir)
    {
        Ramp ramp = null;
        List<Unit> units = api.engine_GetUnits(this, dir);
        onramp = false;
        List<Unit> temp = api.engine_GetUnits(position);
        bool goingup = true;
        for (int i = 0; i < temp.Count; i++)
        {
            if (temp[i] is Ramp)
            {
                ramp = (Ramp)temp[i];
                if (ramp.IsOnRampSide(Toolkit.ReverseDirection(Starter.GetGravityDirection())))
                {
                    onramp = true;
                }
            }
        }
        if (onramp)
        {
            Direction gravitydirection = Starter.GetDataBase().gravity_direction;
            switch (gravitydirection)
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
                units = api.engine_GetUnits(Toolkit.VectorSum(Toolkit.VectorSum(Toolkit.DirectiontoVector(Toolkit.ReverseDirection(gravitydirection)), Toolkit.DirectiontoVector(dir)), position));
        }
        for (int i = 0; i < units.Count; i++) {
            if (!units[i].PlayerMoveInto(Toolkit.ReverseDirection(dir)))
                return false;
        }
        api.engine_Move(this, dir);
        return true;
    }

    public override bool CanMove(Direction dir, GameObject parent)
    {
        List<Unit> units = api.engine_GetUnits(this, dir);
        players = new List<Unit>();
        for (int i = 0; i < units.Count; i++)
        {
            if (units[i] is Player)
            {
                players.Add(units[i]);
                continue;
            }
            else if (units[i] is Branch)
            {
                continue;
            }
            else if (units[i] is Ramp)
            {
                return false;
                // baadan age bekhaym player moghe harekate game object ziresh bere ru ramp ino barmidarim
                /*Ramp ramp = (Ramp)units[i];
                switch (dir)
                {
                    case Direction.Up: if (ramp.type != 2 && ramp.type != 3) return false; break;
                    case Direction.Right: if (ramp.type != 3 && ramp.type != 4) return false; break;
                    case Direction.Left: if (ramp.type != 1 && ramp.type != 2) return false; break;
                    case Direction.Down: if (ramp.type != 1 && ramp.type != 4) return false; break;
                }*/
            }
            else if (units[i].transform.parent.gameObject != parent)
                return false;
        }
        for (int i = 0; i < players.Count; i++)
        {
            if (!players[i].CanMove(dir, parent))
            {
                Debug.Log(players[i]);
                return false;
            }
            
        }
        int bound = players.Count;
        for(int i = 0; i< bound; i++)
        {
            players.AddRange(players[i].players);
        }
        return true;
    }

    public void Absorb(Container container)
    {
        container.PlayerAbsorb(this);
    }
    public void Release(Container container)
    {
        container.PlayerRelease(this);
    }

    public void AbsorbHold(Container container)
    {
        container.PlayerAbsorbHold(this);
    }
    public void ReleaseHold(Container container)
    {
        container.PlayerReleaseHold(this);
    }
    public override bool PlayerMoveInto(Direction dir)
    {
        return false;
    }

    public override bool ApplyGravity(Direction gravitydirection, List<Unit>[,] units)
    {
        if (lean)
            return false;
        if (Stand_On_Ramp(position) || Toolkit.HasBranch(position))
        {
            return false;
        }
        Vector2 pos = Toolkit.VectorSum(position, gravitydirection);
        if (!Fall(pos))
            return false;
        while (Fall(pos))
        {
            /*if (pos.y <= 0 || pos.x <= 0)
                break;*/
            api.RemoveFromDatabase(this);
            position = pos;
            api.AddToDatabase(this);
            pos = Toolkit.VectorSum(position, gravitydirection);
        }
        state = PlayerState.Falling;
        api.graphicalengine_Fall(this, position);
        return true;
    }

    public void FallFinished()
    {
        Vector2 pos = Toolkit.VectorSum(position, Starter.GetGravityDirection());
        if (pos.x <= 0 || pos.y <= 0)
            return;
        if (Toolkit.HasRamp(pos)) //ramp
        {
            if (Toolkit.IsdoubleRamp(pos))
            {
                api.graphicalengine_Land(this, position);
            }
            else
            {
                Vector2 temp = Toolkit.GetRamp(pos).fallOn(this, Toolkit.ReverseDirection(Starter.GetGravityDirection()));
                Debug.Log(temp);
                Debug.Log(position);
                if (temp == position)
                {
                    api.graphicalengine_Land(this, position);
                }
                else
                {
                    api.RemoveFromDatabase(this);
                    position = temp;
                    api.AddToDatabase(this);
                    api.graphicalengine_LandOnRamp(this, position);
                }
            }
        }
        else //Block
        {
            api.graphicalengine_Land(this, position);
        }
    }

    public bool IsRelatedLean(GameObject parent)
    {
        List<Unit> units = api.engine_GetUnits(this, leandirection);    
        for(int i=0; i<units.Count; i++)
        {
            if (parent == units[i].gameObject.transform.parent.gameObject)
                return true;
        }
        return false;
    }
    public bool Action()
    {
        if (abilities.Count == 0)
            return false;
        switch (abilities[0])
        {
            case AbilityType.Fuel: return false;
            case AbilityType.Direction: return true;
            case AbilityType.Jump: return true;
            case AbilityType.Blink: return false;
            case AbilityType.Gravity: return false;
            case AbilityType.Rope: return false;
            default: return false;
        }
    }

    public bool Action(Direction dir)
    {
        switch (abilities[0])
        {
            case AbilityType.Fuel: return true;
            case AbilityType.Direction: return false;
            case AbilityType.Jump: return false;
            case AbilityType.Blink: return true;
            case AbilityType.Gravity: return true;
            case AbilityType.Rope: return true;
            default: return false;
        }
    }


    private bool Stand_On_Ramp(Vector2 position)
    {
        List<Unit>[,] units = Starter.GetDataBase().units;
        for (int i = 0; i < units[(int)position.x, (int)position.y].Count; i++)
        {
            if (units[(int)position.x, (int)position.y][i] is Ramp)
            {
                Ramp ramp = (Ramp)units[(int)position.x, (int)position.y][i];
                //if player can move to it , it should not fall
                return ramp.PlayerMoveInto(Direction.Up);
            }
        }
        return false;
    }

    private bool Fall(Vector2 position)
    {
        List<Unit>[,] units = Starter.GetDataBase().units;
        if (units[(int)position.x, (int)position.y].Count != 0)
        {
            for (int i = 0; i < units[(int)position.x, (int)position.y].Count; i++)
            {
                Unit unit = units[(int)position.x, (int)position.y][i];
                if (unit is Ramp)
                {
                    Ramp ramp = (Ramp)unit;
                    // Land On Ramp should be called
                    return false;
                }

            }
            // There is Some Object and fall should stop
            return false;
        }
        else
        {
            return true;
        }
    }

    public override CloneableUnit Clone()
    {
        return new CloneablePlayer(this);
    }
}

public class CloneablePlayer : CloneableUnit
{
    public List<AbilityType> abilities;
    public List<Direction> move_direction;
    public Direction direction;
    public int movepercentage;
    public PlayerState state;
    public Direction leandirection;
    public bool lean;
    public bool onramp;
    public Direction gravity;
    public Vector2 nextpos;
    public CloneablePlayer(Player player) : base(player.position)
    {
        original = player;
        abilities = new List<AbilityType>();
        for (int i = 0; i < player.abilities.Count; i++)
            abilities.Add(player.abilities[i]);
        move_direction = new List<Direction>();
        for (int i = 0; i < player.move_direction.Count; i++)
            move_direction.Add(player.move_direction[i]);
        direction = player.direction;
        movepercentage = player.movepercentage;
        state = player.state;
        leandirection = player.leandirection;
        lean = player.lean;
        onramp = player.onramp;
        gravity = player.gravity;
        nextpos = new Vector2(player.nextpos.x, player.nextpos.y);
    }

    public override void Undo()
    {
        Player original = (Player)base.original;
        original.api.RemoveFromDatabase(original);
        original.position = position;
        original.api.AddToDatabase(original);
        original.abilities = new List<AbilityType>();
        for (int i = 0; i < abilities.Count; i++)
            original.abilities.Add(abilities[i]);
        move_direction = new List<Direction>();
        for (int i = 0; i < move_direction.Count; i++)
            move_direction.Add(move_direction[i]);
        original.direction = direction;
        original.movepercentage = movepercentage;
        original.state = state;
        original.leandirection = leandirection;
        original.lean = lean;
        original.onramp = onramp;
        original.gravity = gravity;
        original.nextpos = new Vector2(nextpos.x, nextpos.y);

        SetPosition();
    }
}


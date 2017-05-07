using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Player : Unit
{
    public AbilityType abilitytype;
    public int abilitycount;
    public List<Ability> abilities;
    public List<Direction> move_direction;
    public Direction direction { get; set; }
    public int movepercentage { get; set; }
    public PlayerState state { get; set; }
    public Direction leandirection { get; set; }
    public bool lean { get; set; }

    public bool onramp { get; set; }
    private Direction gravity { get; set; }

    public Vector2 nextpos { get; set; }

    public Direction jumpdirection { get; set; }

    public Jump oneJump { get; set; }
    public bool isonejumping { get; set; }

    public Ability currentAbility { get; set; }
    private List<Unit>[,] units;
    private int x_bound;
    private int y_bound;
    public GameMode mode;

    public Coroutine leancoroutine;
    public void Awake()
    {
        abilities = new List<Ability>();
        for (int i = 0; i < abilitycount; i++)
        {
            abilities.Add(Ability.GetAbilityInstance(abilitytype));
            if (abilitytype == AbilityType.Jump)
                ((Jump)abilities[i]).number = 2;
        }
        direction = move_direction[0];
        oneJump = new Jump(1);
        state = PlayerState.Idle;

    }
    public Direction GetGravity(){
        return gravity;
    }

    public void SetGravity(Direction dir)
    {
        gravity = dir;
        Update_Move_Direction();
    }

    private void Update_Move_Direction()
    {
        if (gravity == Direction.Down || gravity == Direction.Up)
        {
            move_direction[0] = Direction.Right;
            move_direction[1] = Direction.Left;
        }
        else
        {
            move_direction[0] = Direction.Up;
            move_direction[1] = Direction.Down;
        }
            
    }

    void Start()
    {
        gravity = Starter.GetGravityDirection();
        units = Starter.GetDataBase().units;
        x_bound = GameObject.Find("Starter").GetComponent<Starter>().x;
        y_bound = GameObject.Find("Starter").GetComponent<Starter>().y;
        api.engine.apiinput.SetMode(mode);
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
        if (Toolkit.HasBranch(Toolkit.VectorSum(position, dir)))
            return true;
        if (Can_Lean(dir))
        {
            Vector2 pos = Toolkit.VectorSum(position, Toolkit.DirectiontoVector(dir));
            if (Toolkit.IsInsideBranch(pos))
                return true;
        }
        for (int i = 0; i < move_direction.Count; i++)
            if (dir == move_direction[i])
                return true;

        //if inside branch go anywhere
        if (Toolkit.IsInsideBranch(position))
            return true;
        return false;
    }
    public bool Can_Lean(Direction dir)
    {
        if (Toolkit.HasBranch(position))
            return false;
        List<Unit> units = api.engine_GetUnits(Toolkit.VectorSum(position, dir));
        for (int i = 0; i < units.Count; i++)
        {
            if (units[i] is Container || units[i] is Fountain || (units[i] is Branch && ((Branch)units[i]).islocked) || (units[i] is Rock && ((Rock)units[i]).isleanable))
                return true;
        }
        return false;
    }

    public bool Can_Lean(Vector2 pos)
    {
        if (Toolkit.HasBranch(position))
            return false;
        List<Unit> units = api.engine.database.GetUnits(pos);
        for (int i = 0; i < units.Count; i++)
        {
            if (units[i] is Container || units[i] is Fountain || (units[i] is Branch && ((Branch)units[i]).islocked) || (units[i] is Rock && ((Rock)units[i]).isleanable))
                return true;
        }
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
                if (ramp.IsOnRampSide(Toolkit.ReverseDirection(GetGravity())))
                {
                    onramp = true;
                }
            }
        }
        if (onramp)
        {
            Direction gravitydirection = GetGravity();
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
            {
                return false;
            }
        }
        api.engine_Move(this, dir);
        return true;
    }

    public bool JumpingMove(Direction dir)
    {
        Ramp ramp = null;
        List<Unit> units = api.engine_GetUnits(Toolkit.VectorSum(position, dir));
        onramp = false;
        List<Unit> temp = api.engine_GetUnits(position);
        bool goingup = true;
        for (int i = 0; i < temp.Count; i++)
        {
            if (temp[i] is Ramp)
            {
                ramp = (Ramp)temp[i];
                if (ramp.IsOnRampSide(Toolkit.ReverseDirection(GetGravity())))
                {
                    onramp = true;
                }
            }
        }
        if (onramp)
        {
            Direction gravitydirection = GetGravity();
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
                units = api.engine_GetUnits(Toolkit.VectorSum(Toolkit.VectorSum(Toolkit.DirectiontoVector(Toolkit.ReverseDirection(GetGravity())), Toolkit.DirectiontoVector(dir)), position));
        }
        for (int i = 0; i < units.Count; i++)
        {
            if (!units[i].PlayerMoveInto(Toolkit.ReverseDirection(dir)))
            // Add Container Lean Code
                return false;
        }
        api.engine.apigraphic.Player_Co_Stop(this);
        if (!isonejumping && state == PlayerState.Jumping && abilities.Count > 0)
        {
            UseAbility(abilities[0]);
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
            if (units[i] is Pipe)
                continue;
            if (units[i] is Player)
            {
                players.Add(units[i]);
                continue;
            }
            /*else if (units[i] is Branch)
            {
                continue;
            }*/
            else if (units[i] is Ramp && units[i].transform.parent.gameObject != parent)
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
            {
                return false;
            }
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
        for (int i = 0; i < bound; i++)
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

    public override bool ApplyGravity()
    {
        if (mode == GameMode.Real)
            return false;
        isonejumping = false;
        api.engine.drainercontroller.Check(this);
        state = PlayerState.Idle;
        this.GetComponent<PlayerGraphics>().ResetStates();
        api.engine.lasercontroller.CollisionCheck(position);

        // to avoid exception
        if (position.x <= 0 || position.y <= 0)
            return false;
        if (state == PlayerState.Falling)
            return false;
        if (lean)
            return false;

        if (Stand_On_Ramp(position) || Toolkit.HasBranch(position))
        {
            return false;
        }

        Vector2 pos = Toolkit.VectorSum(position, gravity);
        if (Toolkit.IsdoubleRamp(pos))
            return false;
        if (units[(int)pos.x, (int)pos.y].Count != 0)
        {
            if (!Stand_On_Ramp(pos))
                return false;
        }

        if (!NewFall())
            return false;
        api.engine.drainercontroller.Check(this);
        api.engine.lasercontroller.CollisionCheck(position);
        while (IsInBound(position) && NewFall())
        {
            api.engine.drainercontroller.Check(this);
            api.RemoveFromDatabase(this);
            position = Toolkit.VectorSum(position,gravity);
            api.engine.lasercontroller.CollisionCheck(position);
            api.AddToDatabase(this);
        }
        state = PlayerState.Falling;
        api.graphicalengine_Fall(this, FallPos());
        return true;
          
    }

    private bool IsInBound(Vector2 pos)
    {
        if (pos.x == 1 || pos.y == 1)
            return false;
        if (pos.x == x_bound-1 || pos.y == y_bound-1)
            return false;
        return true;
    }

    private Vector2 FallPos()
    {
        if (gravity == Direction.Down || gravity == Direction.Up)
            return new Vector2(transform.position.x, position.y);
        else
            return new Vector2(position.x, transform.position.y);
    }

    private bool IsOnObject(Unit obj)
    {
        if (obj is Ramp)
        {
            if (Mathf.Abs(obj.transform.position.x - transform.position.x) < 0.5)
                return !Stand_On_Ramp(position);
            else
                return false;
        }
        else if (obj is Pipe)
            return false;
        else
        {
            if (gravity == Direction.Down || gravity == Direction.Up)
            {
                return Mathf.Abs(obj.transform.position.x - transform.position.x) < 0.5;
            }
            else
                return Mathf.Abs(obj.transform.position.y - transform.position.y) < 0.5;
        }
    }
    private List<Unit> GetUnderUnits()
    {
        List<Unit> units = new List<Unit>();
        int x = (int)position.x;
        int y = (int)position.y;
        Database db = Starter.GetDataBase();
        if (gravity == Direction.Down)
        {
            units.AddRange(db.units[x, y - 1]);
            units.AddRange(db.units[x - 1, y - 1]);
            units.AddRange(db.units[x + 1, y - 1]);
        }
        else if(gravity == Direction.Up)
        {
            units.AddRange(db.units[x, y + 1]);
            units.AddRange(db.units[x - 1, y + 1]);
            units.AddRange(db.units[x + 1, y + 1]);
        }
        else if(gravity == Direction.Right)
        {
            units.AddRange(db.units[x + 1, y]);
            units.AddRange(db.units[x + 1, y - 1]);
            units.AddRange(db.units[x + 1, y + 1]);
        }
        else if(gravity == Direction.Left)
        {
            units.AddRange(db.units[x - 1, y]);
            units.AddRange(db.units[x - 1, y - 1]);
            units.AddRange(db.units[x - 1, y + 1]);
        }

        return units;
    }

    private bool NewFall()
    {
        List<Unit> under = GetUnderUnits();
        for (int i = 0; i < under.Count; i++)
        {
            if (IsOnObject(under[i]))
            {
                return false;
            }
        }
        return true;
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

    public void TeleportFinished()
    {
        state = PlayerState.Idle;
        currentAbility = null;
        ApplyGravity();
    }
    public void FallFinished()
    {
        if(position.x <= 0 || position.y <= 0)
        {
            api.engine.apigraphic.Fall_Player_Died(this);
            return;
        }
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
                    onramp = true;
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

    public void UseAbility(Ability ability)
    {
        if (currentAbility == ability)
        {
            abilities.Remove(ability);
            abilitycount = abilities.Count;
            api.engine.apigraphic.Absorb(this, null);
        }
    }
    public bool Action()
    {
        if (abilities.Count == 0)
            return false;
        switch (abilities[0].abilitytype)
        {
            case AbilityType.Fuel: return false;
            case AbilityType.Direction: return true;
            case AbilityType.Jump:
                if (!Toolkit.IsInsideBranch(this))
                    ((Jump)abilities[0]).Action(this, Toolkit.ReverseDirection(api.engine.database.gravity_direction));
                return true;
            case AbilityType.Teleport: return false;
            case AbilityType.Gravity: return false;
            case AbilityType.Rope: ((Rope)abilities[0]).Action(this); return true;
            default: return false;
        }
    }

    public bool Action(Direction dir)
    {
        if (abilities.Count == 0)
            return false;
        switch (abilities[0].abilitytype)
        {
            case AbilityType.Fuel: return true;
            case AbilityType.Direction: return false;
            case AbilityType.Jump: return false;
            case AbilityType.Teleport:((Teleport)abilities[0]).Action(this,dir); return true;
            case AbilityType.Gravity:((Gravity)abilities[0]).Action(this, dir); return true;
            case AbilityType.Rope: return true;
            default: return false;
        }
    }


    private bool Stand_On_Ramp(Vector2 pos)
    {
        List<Unit>[,] units = Starter.GetDataBase().units;
        for (int i = 0; i < units[(int)pos.x, (int)pos.y].Count; i++)
        {
            if (units[(int)pos.x, (int)pos.y][i] is Ramp)
            {
                Ramp ramp = (Ramp)units[(int)pos.x, (int)pos.y][i];
                //if player can move to it , it should  fall
                return ramp.PlayerMoveInto(Toolkit.ReverseDirection(gravity));
            }
        }
        return false;
    }

    public void _setability()
    {
        if (abilities.Count != 0)
        {
            abilitycount = abilities.Count;
            abilitytype = abilities[0].abilitytype;
        }
        else
            abilitycount = 0;
        api.engine.apigraphic.Absorb(this, null);
    }

    public override CloneableUnit Clone()
    {
        return new CloneablePlayer(this);
    }
}

public class CloneablePlayer : CloneableUnit
{
    public List<Ability> abilities;
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
        abilities = new List<Ability>();
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
        gravity = player.GetGravity();
        nextpos = new Vector2(player.nextpos.x, player.nextpos.y);
    }

    public override void Undo()
    {
        Player original = (Player)base.original;
        original.api.RemoveFromDatabase(original);
        original.position = position;
        original.api.StopPlayerCoroutine(original);
        original.api.AddToDatabase(original);
        original.abilities = new List<Ability>();
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
        original.SetGravity(gravity);
        original.nextpos = new Vector2(nextpos.x, nextpos.y);
        original.lean = false;
        original.api.engine.apigraphic.Absorb(original, null);
        original.abilitycount = original.abilities.Count;
        if (original.abilitycount != 0)
            original.abilitytype = original.abilities[0].abilitytype;
        SetPosition();
        if (!Toolkit.HasBranch(original.position))
        {
            //Debug.Log(Resources.Load<Sprite>("Player\\1"));
            original.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Player\\1");
            Debug.Log(original.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite);
            Debug.Log(original.transform.GetChild(1).name);
            original.transform.GetChild(1).GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        }
        original.state = PlayerState.Idle;
    }
}


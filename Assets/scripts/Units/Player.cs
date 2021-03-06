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
    public LifeState lifestate { get; set; }

    //Skill locks
    public bool CanJump;

    public Direction direction { get; set; }
    public int movepercentage { get; set; }
    public PlayerState state { get; private set; }
    public Direction leandirection { get; set; }
    public Unit LeanedTo { get; set; }
    public bool onramp { get; set; }
    public Direction gravity { get; private set; }

    public Vector2 nextpos { get; set; }

    public Direction jumpdirection { get; set; }

    public Jump oneJump { get; set; }
    public bool isonejumping { get; set; }

    public Ability currentAbility { get; set; }
    private List<Unit>[,] units;
    private int x_bound;
    private int y_bound;
    private bool onthesameramp;
    public GameMode mode;

    private PlayerState tempstate;
    public PlayerState LeanUndoNextState { get; set; }

    public Coroutine leancoroutine { get; set;}

    public  int gravitynum = 0;

    public Container movedbycontainer { get; set; }
    public void Awake()
    {
        abilities = new List<Ability>();
        for (int i = 0; i < abilitycount; i++)
        {
            abilities.Add(Ability.GetAbilityInstance(abilitytype));
            if (abilitytype == AbilityType.Jump)
                ((Jump)abilities[i]).number = 1;
        }
        direction = move_direction[0];
        oneJump = new Jump(1);
        state = PlayerState.Idle;
        tempstate = state;
        lifestate = LifeState.Alive;
    }

    public void Update()
    {
        if(state != tempstate)
        {
            //Debug.Log(state);
            tempstate = state;
        }
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
        units = Starter.GetDataBase().units;
        x_bound = GameObject.Find("Starter").GetComponent<Starter>().x;
        y_bound = GameObject.Find("Starter").GetComponent<Starter>().y;
        api.engine.apiinput.SetMode(mode);
    }

    public void SetState(PlayerState state)
    {
        this.state = state;
        /*if (state == PlayerState.Transition)
        {
            GetComponent<PlayerGraphics>().TransitionAnimation();
        }*/
        /*Debug.Log(state);
        System.Diagnostics.StackTrace stackTrace = new System.Diagnostics.StackTrace();
        Debug.Log(stackTrace.GetFrame(1).GetMethod().Name);*/
        if (state == PlayerState.Idle)
        {
            api.engine.drainercontroller.Check(this);
            for(int i=0; i<api.engine.database.branchGravityChangers.Count; i++)
            {
                api.engine.database.branchGravityChangers[i].CheckGravity(this);
            }
            GetComponent<PlayerGraphics>().ResetStates();
            movepercentage = 0;
        }
        else if (state == PlayerState.Lean)
        {
            //api.engine.apigraphic.Lean(this);
        }
        else if (state == PlayerState.Gir)
        {
            state = PlayerState.Gir;
        }
    }

    // After TP Mode cancelation this works to stop player being moved while it is undoing lean
    // Preventing Slide Bug
    public void LeanUndoPortal()
    {
        SetState(PlayerState.Busy);
        StartCoroutine(WaitForLeanUndo());
  
    }

    public IEnumerator WaitForLeanUndo()
    {
        yield return new WaitForSeconds(0.3f);
        SetState(PlayerState.Idle);
        api.engine.apiinput.QuitPortalMode();
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
            if (units[i].isLeanable())
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
            if (units[i].isLeanable())
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
                if (ramp.IsOnRampSide(Toolkit.ReverseDirection(gravity)))
                {
                    onramp = true;
                }
            }
        }
        if (onramp)
        {
            Direction gravitydirection = gravity;
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
            if (units[i] is Player)
            {
                if ((units[i] as Player).CanPlayerMoveInDirection(direction))
                    break;
                return false;

            }
            else if (!units[i].PlayerMoveInto(Toolkit.ReverseDirection(dir)))
            {
                
                return false;
            }
        }
        api.engine_Move(this, dir);
        return true;
    }

    public bool CanPlayerMoveInDirection(Direction dir)
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
                if (ramp.IsOnRampSide(Toolkit.ReverseDirection(gravity)))
                {
                    onramp = true;
                }
            }
        }
        if (onramp)
        {
            Direction gravitydirection = gravity;
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
        for (int i = 0; i < units.Count; i++)
        {
            if (units[i] is Player)
            {
                if ((units[i] as Player).CanPlayerMoveInDirection(direction))
                    return true;
                return false;

            }
            else if (!units[i].PlayerMoveInto(Toolkit.ReverseDirection(dir)))
            {

                return false;
            }
        }
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
                if (ramp.IsOnRampSide(Toolkit.ReverseDirection(gravity)))
                {
                    onramp = true;
                }
            }
        }
        if (onramp)
        {
            Direction gravitydirection = gravity;
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
                units = api.engine_GetUnits(Toolkit.VectorSum(Toolkit.VectorSum(Toolkit.DirectiontoVector(Toolkit.ReverseDirection(gravity)), Toolkit.DirectiontoVector(dir)), position));
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
        direction = dir;
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
        if (gravity == Direction.Down || gravity == Direction.Up)
            transform.position.Set(transform.position.x, position.y, transform.position.z);
        else
            transform.position.Set(position.x, transform.position.y, transform.position.z);

        if (state == PlayerState.Gir)
            return false;
        if (mode == GameMode.Real)
            return false;
        if (state == PlayerState.Lean)
            return false;
        isonejumping = false;
        if (api.engine.drainercontroller.Check(this))
            return false;
        if (api.engine.lasercontroller.CollisionCheck(position))
        {
            if ((Vector2)transform.position != position)
            {
                SetState(PlayerState.Falling);
                int h = Math.Abs((int)(transform.position.y - position.y));
                if (Toolkit.isHorizontal(gravity))
                    h = Math.Abs((int)(transform.position.x - position.x));
                api.graphicalengine_Fall(this, FallPos(), h);

                return true;
            }
        }

        // to avoid exception
        if (position.x <= 0 || position.y <= 0)
            return false;
        if (state == PlayerState.Falling)
            return false;
        if (state == PlayerState.Lean)
            return false;
        if (Toolkit.HasBranch(position) || Toolkit.HasRamp(position))
            return false;
        
        Vector2 pos = Toolkit.VectorSum(position, gravity);
        onthesameramp = false;

        if (Stand_On_Ramp(pos))
        {
            api.RemoveFromDatabase(this);
            position = pos;
            api.AddToDatabase(this);
            onthesameramp = true;
            api.engine.apigraphic.LandOnRamp(this, pos, Toolkit.GetRamp(pos), Toolkit.GetRamp(pos).type);
            return false;
        }
        if (Toolkit.IsdoubleRamp(pos))
            return false;
        if (units[(int)pos.x, (int)pos.y].Count != 0)
        {
            if (!Stand_On_Ramp(pos))
                return false;
        }
        if (!NewFall())
        {
            SetState(PlayerState.Idle);
            return false;
        }
        if (api.engine.drainercontroller.Check(this))
            return false;
        if (api.engine.lasercontroller.CollisionCheck(position))
        {
            if ((Vector2)transform.position != position)
            {
                SetState(PlayerState.Falling);
                int h = Math.Abs((int)(transform.position.y - position.y));
                if (Toolkit.isHorizontal(gravity))
                    h = Math.Abs((int)(transform.position.x - position.x));
                api.graphicalengine_Fall(this, FallPos(), h);

                return true;
            }
        }
        SetState(PlayerState.Idle);
        int height = 1;
        while (IsInBound(position) && NewFall())
        {
            if (api.engine.drainercontroller.Check(this))
                return false;
            api.RemoveFromDatabase(this);
            position = Toolkit.VectorSum(position,gravity);
            if (api.engine.lasercontroller.CollisionCheck(position))
            {
                if ((Vector2)transform.position != position)
                {
                    SetState(PlayerState.Falling);
                    api.graphicalengine_Fall(this, FallPos(), height);

                    return true;    
                }
            }
            
            api.AddToDatabase(this);
            height++;
        }
        SetState(PlayerState.Falling);
        if (Toolkit.HasPlayer(Toolkit.VectorSum(position, gravity)) && Toolkit.GetPlayerNumberInDatabase(this) < Toolkit.GetPlayerNumberInDatabase(Toolkit.GetPlayer(Toolkit.VectorSum(position, gravity))))
        {
            Debug.Log("this part should be changed after demo");
            height /= 2;
            api.RemoveFromDatabase(this);
            for (int i = 0; i < height; i++)
            {
                position = Toolkit.VectorSum(position, Toolkit.ReverseDirection(gravity));
            }
            api.AddToDatabase(this);
        }
        api.graphicalengine_Fall(this, FallPos(),height);

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
        else if (obj is BotPart)
            return true;
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

    public void PortalFinished()
    {
        SetState(PlayerState.Lean);
        api.engine.apigraphic.Lean(this);
        currentAbility = null;
        ApplyGravity();
    }

    public void TeleportFinished()
    {
        SetState(PlayerState.Idle);
        currentAbility = null;
        ApplyGravity();
    }
    public void FallFinished()
    {
        if (position.x <= 0 || position.y <= 0)
        {
            api.engine.apigraphic.Fall_Player_Died(this);
            return;
        }
        if (api.engine.lasercontroller.CollisionCheck(position))
        {
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
                /*if (temp == position)
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
                }*/
            }
        }
        else //Block
        {
            api.graphicalengine_Land(this, position);
        }
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
            case AbilityType.Jump: return true;
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
        GetComponent<PlayerGraphics>().UpdateHologram();
        api.engine.apigraphic.Absorb(this, null);
    }

    public void LandOnRampFinished()
    {
        if (onthesameramp)
        {
            state = PlayerState.Idle;
            onthesameramp = false;
            return;
        }
        
        Vector2 newpos = Toolkit.VectorSum(position, api.engine.database.gravity_direction);
        Ramp ramp = Toolkit.GetRamp(newpos);
        if (ramp == null)
        {
            ApplyGravity();
            return;
        }
        int ramptype = ramp.type;
        int x = (int)newpos.x, y = (int)newpos.y;
        Vector2 temppos = new Vector2(x, y);
        while (true)
        {
            switch (api.engine.database.gravity_direction)
            {
                case Direction.Down:
                    y--;
                    if (ramptype == 1) x++;
                    else if (ramptype == 4) x--;
                    else return;
                    break;
                case Direction.Left:
                    x--;
                    if (ramptype == 1) y++;
                    else if (ramptype == 2) y--;
                    else return;
                    break;
                case Direction.Up:
                    y++;
                    if (ramptype == 2) x++;
                    else if (ramptype == 3) x--;
                    else return;
                    break;
                case Direction.Right:
                    x++;
                    if (ramptype == 3) y--;
                    else if (ramptype == 4) y++;
                    else return;
                    break;
            }

            Vector2 temppos1 = new Vector2(x, y);
            temppos = temppos1;
            if (Toolkit.HasRamp(temppos1) && !Toolkit.IsdoubleRamp(temppos1))
            {
                if (Toolkit.GetRamp(temppos1).type == ramptype)
                {
                    continue;
                }
            }
            else
                state = PlayerState.Idle;
            break;
        }
        
        if(!Toolkit.IsEmpty(temppos))
            temppos = Toolkit.VectorSum(temppos, Toolkit.ReverseDirection(api.engine.database.gravity_direction));
        if (newpos.x != temppos.x || newpos.y != temppos.y)
        {
            api.RemoveFromDatabase(this);
            position = temppos;
            api.AddToDatabase(this);
            //api.engine.apigraphic.MovePlayerOnPlatform(this, temppos);
            Debug.Log(position);
            state = PlayerState.Busy;
            api.engine.apigraphic.Roll(this, position);
            //ApplyGravity();
        }
    }

    public void RollingFinished()
    {
        ApplyGravity();
    }

    public void DrainFinished()
    {

        GameObject.Find("UI").GetComponent<Get>().DrainShow();
        Debug.Log("drain UI commented");
        //GameObject.Find("DrainUI").GetComponent<DrainPoints>().DrainPoint(abilities.Count, api.engine.saveserialize.draincount);
        api.engine.saveserialize.draincount += abilities.Count;
        abilities.Clear();
        _setability();
        api.engine.apigraphic.Absorb(this, null);
        ApplyGravity();
    }


    public void MoveToBranchFinished()
    {
        /*int counter = 0, emptycounter = 0;
        for(int i = 0; i < 4; i++)
        {
            Direction dir = Toolkit.NumberToDirection(i + 1);
            if (Toolkit.HasBranch(Toolkit.VectorSum(position, dir)))
            {
                counter++;
            }
            if (Toolkit.IsEmptySameParent(Toolkit.GetBranch(position).gameObject, Toolkit.VectorSum(position, dir)))
                emptycounter++;
        }
        if (counter != 1 || emptycounter > 1)
        {
            api.engine.apigraphic.BranchLight(true, Toolkit.GetBranch(position));
            SetState(PlayerState.Idle);
            return;
        }
        for (int i = 0; i < 4; i++)
        {
            Direction dir = Toolkit.NumberToDirection(i + 1);
            if (Toolkit.HasBranch(Toolkit.VectorSum(position, dir)))
            {
                SetState(PlayerState.Busy);
                api.engine.apigraphic.BranchLight(false, Toolkit.GetBranch(position));
                Toolkit.GetBranch(Toolkit.VectorSum(position, dir)).PlayerMove(Toolkit.ReverseDirection(dir), this);
                return;
            }
        }*/
        api.engine.apigraphic.BranchLight(true, Toolkit.GetBranch(position),this);
        SetState(PlayerState.Idle);
    }

    public void MoveOutOfBranchFinished()
    {
        SetState(PlayerState.Idle);
        ApplyGravity();
    }

    public void LeanUndoFinished()
    {
        if (state == PlayerState.Lean)
            return;
        SetState(LeanUndoNextState);
        if (LeanUndoNextState == PlayerState.Idle)
            ApplyGravity();
        if(LeanUndoNextState == PlayerState.Busy)
        {
            ApplyGravity();
            return;
            if (!Toolkit.IsEmpty(Toolkit.VectorSum(position, gravity)))
            {
                SetState(PlayerState.Idle);
            }
        }
    }

    public void AdjustPlayerFinshed(Direction direction, Action<Player, Direction> passingmethod)
    {
        SetState(PlayerState.Idle);
        if(!(currentAbility is Jump))
            ApplyGravity();
        passingmethod(this, direction);
    }

    public bool ShouldAdjust(Direction dir)
    {
        if(direction == dir)
        {
            switch (direction)
            {
                case Direction.Right: return transform.position.x < position.x;
                case Direction.Left: return transform.position.x > position.x;
                case Direction.Up: return transform.position.y < position.y;
                case Direction.Down: return transform.position.y > position.y;
            }
        }
        else
        {
            switch (direction)
            {
                case Direction.Right: return transform.position.x > position.x;
                case Direction.Left: return transform.position.x < position.x;
                case Direction.Up: return transform.position.y > position.y;
                case Direction.Down: return transform.position.y < position.y;
            }
        }
        return false;
    }

    public bool HasAbility(AbilityType abilitytype)
    {
        if (abilities.Count != 0 && abilities[0].abilitytype == abilitytype)
            return true;
        return false;
    }

    public override bool isLeanable()
    {
        return false;
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
        onramp = player.onramp;
        gravity = player.gravity;
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
        original.SetState(state);
        original.leandirection = leandirection;
        original.onramp = onramp;
        original.SetGravity(gravity);
        original.nextpos = new Vector2(nextpos.x, nextpos.y);
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
        original.SetState(PlayerState.Idle);
    }
}


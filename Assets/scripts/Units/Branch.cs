using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Branch : Unit {

    public bool islocked = false;
    public bool isblocked = false;
    public bool isExternal = false;
    public bool isDrainer = false;
    public bool isGravityChanger = false;
    public Direction GravityDirection;
    public Circle circle { get; set; }

    private Coroutine BranchUnlockCoroutine;
    public override void SetInitialSprite()
    {
        bool[] connected = Toolkit.GetConnectedSidesForBranch(this);
        int rot = 0;
        int i = 0;
        if (isExternal)
            i = 1;
        Sprite body = null;
        if (connected[0] && connected[1] && connected[2] && connected[3])
        {
            body = api.engine.initializer.sprite_Branch[i, 3];
        }
        // 3 way
        else if(connected[0] && connected[1] && connected[2])
        {
            body = api.engine.initializer.sprite_Branch[i, 2];
           //transform.rotation = Quaternion.Euler(0, 0, 270);
            rot = 270;
        }
        // tekrari badan pak kon
        else if (connected[0] && connected[1] && connected[2])
        {
            body = api.engine.initializer.sprite_Branch[i, 2];
           // transform.rotation = Quaternion.Euler(0, 0, 270);
            rot = 270;
        }
        else if (connected[0] && connected[1] && connected[3])
        {
            body = api.engine.initializer.sprite_Branch[i, 2];
           //transform.rotation = Quaternion.Euler(0, 0, 0);
            rot = 0;
        }
        else if (connected[0] && connected[2] && connected[3])
        {
            body = api.engine.initializer.sprite_Branch[i, 2];
           //transform.rotation = Quaternion.Euler(0, 0, 90);
            rot = 90;
        }
        else if (connected[1] && connected[2] && connected[3])
        {
            body = api.engine.initializer.sprite_Branch[i, 2];
           //transform.rotation = Quaternion.Euler(0, 0, 180);
            rot = 180;
        }

        // 2 way top bot
        else if (connected[0] && connected[2])
        {
            body = api.engine.initializer.sprite_Branch[i, 0];
          // transform.rotation = Quaternion.Euler(0, 0, 90);
            rot = 90;
        }

        // 2 way left right
        else if (connected[1] && connected[3])
        {
            body = api.engine.initializer.sprite_Branch[i, 0];
          // transform.rotation = Quaternion.Euler(0, 0, 0);
            rot = 0;
        }

        // 2 way  top right
        else if (connected[0] && connected[1])
        {
            body = api.engine.initializer.sprite_Branch[i, 1];
           //transform.rotation = Quaternion.Euler(0, 0, 270);
            rot = 270;
        }

        // 2 way  right bot
        else if (connected[1] && connected[2])
        {
            body = api.engine.initializer.sprite_Branch[i, 1];
           //transform.rotation = Quaternion.Euler(0, 0, 180);
            rot = 180;
        }
        // 2 way  bot left
        else if (connected[2] && connected[3])
        {
            body = api.engine.initializer.sprite_Branch[i, 1];
           //transform.rotation = Quaternion.Euler(0, 0, 90);
            rot = 90;
        }

        // 2 way left top
        else if (connected[3] && connected[0])
        {
            body = api.engine.initializer.sprite_Branch[i, 1];
          // transform.rotation = Quaternion.Euler(0, 0, 0);
            rot = 0;
        }

        // 1 way top
        else if (connected[0])
        {
            body = api.engine.initializer.sprite_Branch[i, 3];
           //transform.rotation = Quaternion.Euler(0, 0, 90);
            rot = 90;
        }
        else if (connected[1])
        {
            body = api.engine.initializer.sprite_Branch[i, 3];
        }
        else if (connected[2])
        {
            body = api.engine.initializer.sprite_Branch[i, 3];
          // transform.rotation = Quaternion.Euler(0, 0, 270);
            rot = 270;
        }
        else if (connected[3])
        {
            body = api.engine.initializer.sprite_Branch[i, 3];
           //transform.rotation = Quaternion.Euler(0, 0, 180);
            rot = 180;
        }
        else
        {
            body = api.engine.initializer.sprite_Branch[i, 3];
        }

        Toolkit.GetObjectInChild(this.gameObject,"BranchBody").transform.rotation = Quaternion.Euler(0, 0, rot);
        Toolkit.GetObjectInChild(this.gameObject, "BranchBody").GetComponent<SpriteRenderer>().sprite = body;
        SetentranceOrJoin(connected);

        // SetJointOrEntrance(Direction.Up);
        //SetJointOrEntrance(Direction.Right);
        //  SetJointOrEntrance(Direction.Down);
        //  SetJointOrEntrance(Direction.Left);
        api.engine.apigraphic.UnitChangeSprite(this);
        if (isblocked)
        {
            GameObject obj = Toolkit.GetObjectInChild(gameObject, "Icon").transform.GetChild(0).gameObject;
            obj.SetActive(true);
            obj.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Branch\\Light Glass-DONT");
            //Debug.Log(obj.GetComponent<SpriteRenderer>().sprite);
            //obj.GetComponent<SpriteRenderer>().color = new Color(0.27f, 0, 0, 1f);
        }
        if (isDrainer)
        {
            api.engine.database.branchDrainers.Add(this);
        }
        if (isGravityChanger)
        {
            api.engine.database.branchGravityChangers.Add(this);
        }
    }

    private void SetentranceOrJoin(bool[] connected)
    {
        bool[] isEmptySides = Toolkit.GetEmptySidesSameParent(this);
        for(int i = 0; i < 4; i++)
        {
            if (isEmptySides[i])
            {
                Toolkit.GetObjectInChild(this.gameObject, "Icon").SetActive(true);
                Toolkit.GetObjectInChild(this.gameObject, "Entrances").transform.GetChild(i).gameObject.SetActive(true);
            }
        }
        int counter = 0;
        for(int i = 0; i < 4; i++)
        {
            if (connected[i])
                counter++;
        }
        if(counter>2)
            Toolkit.GetObjectInChild(this.gameObject, "Icon").SetActive(true);
        //new code for corner test
        if (counter == 2) {
            if (!(connected[0] && connected[2]) && (connected[0] || connected[2]))
                Toolkit.GetObjectInChild(this.gameObject, "Icon").SetActive(true);
            if (!(connected[1] && connected[3]) && (connected[1] || connected[3]))
                Toolkit.GetObjectInChild(this.gameObject, "Icon").SetActive(true);
        }
    }
    private void SetJointOrEntrance(Direction direction)
    { /*
        if(Toolkit.IsConnectedFromPosition(this, Toolkit.VectorSum(position, direction)))
        {
            if (!Toolkit.HasBranch(Toolkit.VectorSum(position, direction)))
            {
                switch (direction)
                {
                    case Direction.Up:
                        transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = api.engine.initializer.sprite_BranchHolder;
                        transform.GetChild(1).GetComponent<SpriteRenderer>().flipX = false; return;
                    case Direction.Right:
                        transform.GetChild(2).GetComponent<SpriteRenderer>().sprite = api.engine.initializer.sprite_BranchHolder;
                        transform.GetChild(2).GetComponent<SpriteRenderer>().flipX = true; return;
                    case Direction.Down:
                        transform.GetChild(3).GetComponent<SpriteRenderer>().sprite = api.engine.initializer.sprite_BranchHolder;
                        transform.GetChild(3).GetComponent<SpriteRenderer>().flipX = true; return;
                    case Direction.Left:
                        transform.GetChild(4).GetComponent<SpriteRenderer>().sprite = api.engine.initializer.sprite_BranchHolder;
                        transform.GetChild(4).GetComponent<SpriteRenderer>().flipX = true; return;
                }
            }
        }
        else
        {
            switch (direction)
            {
                case Direction.Up:
                    transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = api.engine.initializer.sprite_BranchEntrance;
                    return;
                case Direction.Right:
                    transform.GetChild(2).GetComponent<SpriteRenderer>().sprite = api.engine.initializer.sprite_BranchEntrance;
                    return;
                case Direction.Down:
                    transform.GetChild(3).GetComponent<SpriteRenderer>().sprite = api.engine.initializer.sprite_BranchEntrance;
                    return;
                case Direction.Left:
                    transform.GetChild(4).GetComponent<SpriteRenderer>().sprite = api.engine.initializer.sprite_BranchEntrance;
                    return;
            }
        } */
    }

    public override bool PlayerMoveInto(Direction dir)
    {
        return !islocked && !isblocked;
    }

    public override CloneableUnit Clone()
    {
        return new CloneableBranch(this);
    }

    public void PlayerLeaned(Player player, Direction direction)
    {
        if (islocked)
        {
            if (player.abilities.Count != 0 && player.abilities[0] is Key)
            {
                UnlockBranch(player);
                //api.engine.inputcontroller.PlayerMoveAction(player, direction);
            }
            else
            {
                return;
                player.SetState(PlayerState.Lean);
                //player.transform.position = player.position;
                player.LeanedTo = this;
                player.isonejumping = false;
                api.engine.apigraphic.Player_Co_Stop(player);
                player.SetState(PlayerState.Lean);
                player.leandirection = direction;
                player.currentAbility = null;
                if (Toolkit.IsEmpty(Toolkit.VectorSum(player.position, player.gravity)))
                    api.engine.apigraphic.Lean_On_Air(player);
                else
                    api.engine.apigraphic.Lean(player);
            }
        }
    }
    public bool CheckGravity(Player player)
    {
        if (player.position == position)
        {
            player.SetGravity(GravityDirection);
            int zrot = 0;
            if (player.gravity == Direction.Up)
                zrot = 180;
            else if (player.gravity == Direction.Right)
                zrot = 90;
            else if (player.gravity == Direction.Left)
                zrot = 270;

            GameObject obj = Toolkit.GetObjectInChild(player.gameObject, "Sprite Holder");
            obj.transform.rotation = Quaternion.Euler(obj.transform.rotation.x, obj.transform.rotation.y, zrot);
            return true;
        }
        return false;
    }

    public bool CheckDrain(Player player)
    {
        if (player.position == position)
        {
            Drain(player);
            return true;
        }
        return false;
    }

    public void Drain(Player player)
    {
        Debug.Log("dariner has some commented code! check");
        player.SetState(PlayerState.Busy);
        player.abilities.Clear();
        api.engine.apigraphic.Player_Co_Stop(player);
        player.GetComponent<PlayerGraphics>().ResetStates();
        api.engine.apigraphic.BranchLight(true, this, player);
        api.engine.apigraphic.Absorb(player, null);
        player.SetState(PlayerState.Idle);
        //api.engine.apigraphic.Drain(player, this);
    }

    public void PlayerMove(Direction CameFrom, Player player)
    {
        if(Toolkit.GetObjectInChild(gameObject, "Icon").activeSelf == true)
        {
            api.RemoveFromDatabase(player);
            player.position = position;
            player.transform.position = position;
            api.AddToDatabase(player);
            api.engine.apigraphic.BranchLight(true, Toolkit.GetBranch(player.position),player);
            StartCoroutine(Wait(0.3f, player));
            return;
        }
        bool[] hastbranch = new bool[4];
        int branchcounter = 0;
        int counter = 0;
        for(int i=0; i<4; i++)
        {
            Vector2 temppos = Toolkit.VectorSum(position, Toolkit.NumberToDirection(i+1));
            if (Toolkit.HasBranch(temppos))
            {
                hastbranch[i] = true;
                branchcounter++;
            }
            if (Toolkit.IsEmpty(temppos))
            {
                counter++;
            }

        }
        if (branchcounter == 0)  //fucked up
        {
            Debug.Log("fucked up    ");
            if (player.Move(Toolkit.ReverseDirection(CameFrom)))
            {
                player.SetState(PlayerState.Moving);
            }
            return;
        }
        else if(branchcounter == 1 || branchcounter == 3 || branchcounter == 4 || (branchcounter == 2 && (counter == 2 || counter == 1)))
        {
            api.RemoveFromDatabase(player);
            player.position = position;
            player.transform.position = position;
            api.AddToDatabase(player);
            api.engine.apigraphic.BranchLight(true, Toolkit.GetBranch(player.position),player);
            StartCoroutine(Wait(0.3f, player));
            return;
        }
        else if(branchcounter == 2)
        {
            for (int i=0; i<4; i++)
            {
                if(hastbranch[i] && Toolkit.DirectionToNumber(CameFrom)-1 != i)
                {
                    Branch tempbranch = Toolkit.GetBranch(Toolkit.VectorSum(position, Toolkit.NumberToDirection(i + 1)));
                    if (tempbranch.islocked)
                    {
                        Debug.Log("locked");
                        api.engine.apigraphic.BranchLight(true, Toolkit.GetBranch(player.position),player);
                        player.SetState(PlayerState.Idle);
                        return;
                    }
                    tempbranch.PlayerMove(Toolkit.ReverseDirection(Toolkit.NumberToDirection(i + 1)), player);
                    return;
                }
            }
        }
    }

    public bool PlayerForcePushIntoBranch(Player player, Direction direction)
    {
        if(islocked)
        {
            Debug.Log("player with key cant be pushed into branch in new version");
            return false;
            if (player.abilities.Count == 0 || player.abilities[0].abilitytype != AbilityType.Key)
                return false;
            (player.abilities[0] as Key).branch = this;
            player.abilities.RemoveAt(0);
            islocked = false;
            api.engine.apigraphic.UnitChangeSprite(player);
            api.engine.apigraphic.UnitChangeSprite(this);
        }
        if(player.state == PlayerState.Lean)
            api.engine.inputcontroller.LeanUndo(player, player.leandirection, PlayerState.Idle);
        player.SetState(PlayerState.Gir);
        api.engine.MovePlayer(player, direction);
        return true;
    }


    public void UnlockBranch(Player player)
    {
        if (BranchUnlockCoroutine != null)
            return;
        BranchUnlockCoroutine = StartCoroutine(unlock_branch(player));
        StartCoroutine(CheckPlayerHoldingKey(player));
    }

    private void UnlockBranchFinished(Player player)
    {
        islocked = false;
        (player.abilities[0] as Key).branch = this;
        player.abilities.RemoveAt(0);
        player._setability();
        api.engine.apigraphic.Absorb(player, null);
    }

    private IEnumerator CheckPlayerHoldingKey(Player player)
    {
        while (!IsCircleFinished())
        {
            Direction tempdir = Toolkit.VectorToDirection(position - player.position);
            if (!api.engine.apiinput.isMoveKeyDown(tempdir))
            {
                if (BranchUnlockCoroutine != null)
                {
                    StopCoroutine(BranchUnlockCoroutine);
                    BranchUnlockCoroutine = null;
                    circle.StopCircle(new Color(1, 1, 1));
                }
                break;
            }
            yield return null;
        }
    }

    private IEnumerator unlock_branch(Player player)
    {
        circle.StartCircleForLaser();
        yield return new WaitUntil(new System.Func<bool>(() => IsCircleFinished()));
        UnlockBranchFinished(player);
        //api.engine.apigraphic.UnitChangeSprite(this);
    }

    public void BranchUnlockAnimationFinished()
    {
        islocked = false;
    }

    public override bool isLeanable()
    {
        //return false;
        return islocked || isblocked;
    }

    private IEnumerator Wait(float f, Player player)
    {
        yield return new WaitForSeconds(f);
        player.SetState(PlayerState.Idle);
    }

    public bool IsCircleFinished()
    {
        return circle.CircleFinished;
    }
}

public class CloneableBranch : CloneableUnit
{
    public CloneableBranch(Branch branch) : base(branch.position)
    {
        original = branch;
    }
}

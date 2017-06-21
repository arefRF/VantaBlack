using UnityEngine;
using System.Collections;

public class Branch : Unit {

    public bool islocked = false;
    public bool blocked = false;
    public override void SetInitialSprite()
    {
        bool[] connected = Toolkit.GetConnectedSidesForBranch(this);
        if (connected[0] && connected[1] && connected[2] && connected[3])
        {
            GetComponent<SpriteRenderer>().sprite = api.engine.initializer.sprite_Branch[3];
        }
        // 3 way
        else if(connected[0] && connected[1] && connected[2])
        {
           GetComponent<SpriteRenderer>().sprite = api.engine.initializer.sprite_Branch[2];
           transform.rotation = Quaternion.Euler(0, 0, 270);
        }
        // tekrari badan pak kon
        else if (connected[0] && connected[1] && connected[2])
        {
            GetComponent<SpriteRenderer>().sprite = api.engine.initializer.sprite_Branch[2];
            transform.rotation = Quaternion.Euler(0, 0, 270);
        }
        else if (connected[0] && connected[1] && connected[3])
        {
            GetComponent<SpriteRenderer>().sprite = api.engine.initializer.sprite_Branch[2];
           transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (connected[0] && connected[2] && connected[3])
        {
           GetComponent<SpriteRenderer>().sprite = api.engine.initializer.sprite_Branch[2];
           transform.rotation = Quaternion.Euler(0, 0, 90);
        }
        else if (connected[1] && connected[2] && connected[3])
        {
           GetComponent<SpriteRenderer>().sprite = api.engine.initializer.sprite_Branch[2];
           transform.rotation = Quaternion.Euler(0, 0, 180);
        }

        // 2 way top bot
        else if (connected[0] && connected[2])
        {
           Debug.Log("Top bot");
           GetComponent<SpriteRenderer>().sprite = api.engine.initializer.sprite_Branch[0];
           transform.rotation = Quaternion.Euler(0, 0, 90);
        }

        // 2 way left right
        else if (connected[1] && connected[3])
        {
           GetComponent<SpriteRenderer>().sprite = api.engine.initializer.sprite_Branch[0];
           transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        // 2 way  top right
        else if (connected[0] && connected[1])
        {
           GetComponent<SpriteRenderer>().sprite = api.engine.initializer.sprite_Branch[1];
           transform.rotation = Quaternion.Euler(0, 0, 270);
        }

        // 2 way  right bot
        else if (connected[1] && connected[2])
        {
            GetComponent<SpriteRenderer>().sprite = api.engine.initializer.sprite_Branch[1];
           transform.rotation = Quaternion.Euler(0, 0, 180);
        }
        // 2 way  bot left
        else if (connected[2] && connected[3])
        {
            GetComponent<SpriteRenderer>().sprite = api.engine.initializer.sprite_Branch[1];
           transform.rotation = Quaternion.Euler(0, 0, 90);
        }

        // 2 way left top
        else if (connected[3] && connected[0])
        {
           GetComponent<SpriteRenderer>().sprite = api.engine.initializer.sprite_Branch[1];
           transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        // 1 way top
        else if (connected[0])
        {
           GetComponent<SpriteRenderer>().sprite = api.engine.initializer.sprite_Branch[3];
           transform.rotation = Quaternion.Euler(0, 0, 90);
        }
        else if (connected[1])
        {
            GetComponent<SpriteRenderer>().sprite = api.engine.initializer.sprite_Branch[3];
        }
        else if (connected[2])
        {
           GetComponent<SpriteRenderer>().sprite = api.engine.initializer.sprite_Branch[3];
           transform.rotation = Quaternion.Euler(0, 0, 270);
        }
        else if (connected[3])
        {
           GetComponent<SpriteRenderer>().sprite = api.engine.initializer.sprite_Branch[3];
           transform.rotation = Quaternion.Euler(0, 0, 180);
        }
        else
        {
           GetComponent<SpriteRenderer>().sprite = api.engine.initializer.sprite_Branch[3];
        }

       // SetJointOrEntrance(Direction.Up);
        //SetJointOrEntrance(Direction.Right);
      //  SetJointOrEntrance(Direction.Down);
      //  SetJointOrEntrance(Direction.Left);
        api.engine.apigraphic.UnitChangeSprite(this);
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
        return !islocked;
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
                islocked = false;
                player.abilities.Clear();
                player._setability();
                api.engine.apigraphic.Absorb(player, null);
                api.engine.apigraphic.UnitChangeSprite(this);
                api.engine.inputcontroller.PlayerMoveAction(player, direction);
            }
            else
            {
                if (player.currentAbility is Jump)
                {
                    GameObject.Find("GetInput").GetComponent<GetInput>().StopCoroutine(((Jump)player.currentAbility).coroutine);
                }
                player.SetState(PlayerState.Lean);
                //player.transform.position = player.position;
                player.LeanedTo = this;
                player.isonejumping = false;
                api.engine.apigraphic.Player_Co_Stop(player);
                player.SetState(PlayerState.Lean);
                player.leandirection = direction;
                player.currentAbility = null;
                api.engine.apiinput.leanlock = true;
                api.engine.apigraphic.Lean(player);
            }
        }
    }
    public void PlayerMove(Direction CameFrom, Player player)
    {
        bool[] hastbranch = new bool[4];
        int branchcounter = 0;
        if (Toolkit.HasBranch(Toolkit.VectorSum(position, Direction.Up)))
        {
            hastbranch[0] = true;
            branchcounter++;
        }
        if (Toolkit.HasBranch(Toolkit.VectorSum(position, Direction.Right)))
        {
            hastbranch[1] = true;
            branchcounter++;
        }
        if (Toolkit.HasBranch(Toolkit.VectorSum(position, Direction.Down)))
        {
            hastbranch[2] = true;
            branchcounter++;
        }
        if (Toolkit.HasBranch(Toolkit.VectorSum(position, Direction.Left)))
        {
            hastbranch[3] = true;
            branchcounter++;
        }

        if(branchcounter == 0)  //fucked up
        {
            Debug.Log("fucked up    ");
            if (player.Move(Toolkit.ReverseDirection(CameFrom)))
            {
                player.SetState(PlayerState.Moving);
            }
            return;
        }
        else if(branchcounter == 1 || branchcounter == 3 || branchcounter == 4)
        {
            api.RemoveFromDatabase(player);
            player.position = position;
            player.transform.position = position;
            api.AddToDatabase(player);
            return;
        }
        else if(branchcounter == 2)
        {
            for(int i=0; i<4; i++)
            {
                if(hastbranch[i] && Toolkit.DirectionToNumber(CameFrom)-1 != i)
                {
                    Toolkit.GetBranch(Toolkit.VectorSum(position, Toolkit.NumberToDirection(i + 1))).PlayerMove(Toolkit.ReverseDirection(Toolkit.NumberToDirection(i + 1)), player);
                    return;
                }
            }
        }
    }

    public override bool isLeanable()
    {
        return true;
    }
}

public class CloneableBranch : CloneableUnit
{
    public CloneableBranch(Branch branch) : base(branch.position)
    {
        original = branch;
    }
}

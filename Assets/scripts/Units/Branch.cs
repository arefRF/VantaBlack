using UnityEngine;
using System.Collections;

public class Branch : Unit {

    public bool islocked = false;
    public bool blocked = false;
    public override void SetInitialSprite()
    {
        /*bool[] notconnected = Toolkit.GetConnectedSidesForBranch(this);

        if (notconnected[0] && notconnected[1] && notconnected[2] && notconnected[3])
        {
            transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = api.engine.initializer.sprite_Branch[5];
        }
        else if(notconnected[0] && notconnected[1] && notconnected[2])
        {
            transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = api.engine.initializer.sprite_Branch[0];
            transform.GetChild(0).transform.rotation = Quaternion.Euler(0, 0, 180);
        }
        else if (notconnected[0] && notconnected[1] && notconnected[2])
        {
            transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = api.engine.initializer.sprite_Branch[0];
            transform.GetChild(0).transform.rotation = Quaternion.Euler(0, 0, 180);
        }
        else if (notconnected[0] && notconnected[1] && notconnected[3])
        {
            transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = api.engine.initializer.sprite_Branch[0];
            transform.GetChild(0).transform.rotation = Quaternion.Euler(0, 0, 270);
        }
        else if (notconnected[0] && notconnected[2] && notconnected[3])
        {
            transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = api.engine.initializer.sprite_Branch[0];
        }
        else if (notconnected[1] && notconnected[2] && notconnected[3])
        {
            transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = api.engine.initializer.sprite_Branch[0];
            transform.GetChild(0).transform.rotation = Quaternion.Euler(0, 0, 90);
        }
        else if (notconnected[0] && notconnected[2])
        {
            transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = api.engine.initializer.sprite_Branch[4];
        }
        else if (notconnected[1] && notconnected[3])
        {
            transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = api.engine.initializer.sprite_Branch[4];
            transform.GetChild(0).transform.rotation = Quaternion.Euler(0, 0, 90);
        }
        else if (notconnected[0] && notconnected[1])
        {
            transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = api.engine.initializer.sprite_Branch[1];
            transform.GetChild(0).transform.rotation = Quaternion.Euler(0, 0, 90);
        }
        else if (notconnected[1] && notconnected[2])
        {
            transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = api.engine.initializer.sprite_Branch[1];
        }
        else if (notconnected[2] && notconnected[3])
        {
            transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = api.engine.initializer.sprite_Branch[1];
            transform.GetChild(0).transform.rotation = Quaternion.Euler(0, 0, 270);
        }
        else if (notconnected[3] && notconnected[0])
        {
            transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = api.engine.initializer.sprite_Branch[1];
            transform.GetChild(0).transform.rotation = Quaternion.Euler(0, 0, 180);
        }
        else if (notconnected[0])
        {
            transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = api.engine.initializer.sprite_Branch[2];
            transform.GetChild(0).transform.rotation = Quaternion.Euler(0, 0, 90);
        }
        else if (notconnected[1])
        {
            transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = api.engine.initializer.sprite_Branch[2];
        }
        else if (notconnected[2])
        {
            transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = api.engine.initializer.sprite_Branch[2];
            transform.GetChild(0).transform.rotation = Quaternion.Euler(0, 0, 270);
        }
        else if (notconnected[3])
        {
            transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = api.engine.initializer.sprite_Branch[2];
            transform.GetChild(0).transform.rotation = Quaternion.Euler(0, 0, 180);
        }
        else
        {
            transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = api.engine.initializer.sprite_Branch[3];
        }*/
        SetJointOrEntrance(Direction.Up);
        SetJointOrEntrance(Direction.Right);
        SetJointOrEntrance(Direction.Down);
        SetJointOrEntrance(Direction.Left);
        api.engine.apigraphic.UnitChangeSprite(this);
    }

    private void SetJointOrEntrance(Direction direction)
    {
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
        }
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
                player.lean = true;
                player.leandirection = direction;
                player.currentAbility = null;
                api.engine.apigraphic.Lean(player);
            }
        }
    }

}

public class CloneableBranch : CloneableUnit
{
    public CloneableBranch(Branch branch) : base(branch.position)
    {
        original = branch;
    }
}

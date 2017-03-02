using UnityEngine;
using System.Collections;

public class Branch : Unit {

    public override void SetInitialSprite()
    {
        bool[] notconnected = Toolkit.GetConnectedSidesForBranch(this);
        Debug.Log(this);
        Debug.Log("not connected to branch from UP: " + notconnected[0]);
        Debug.Log("not connected to branch from Right: " + notconnected[1]);
        Debug.Log("not connected to branch from Down: " + notconnected[2]);
        Debug.Log("not connected to branch from Left: " + notconnected[3]);

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
            transform.GetChild(0).transform.rotation = Quaternion.Euler(0, 0, 270);
        }
        else if (notconnected[0] && notconnected[2])
        {
            transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = api.engine.initializer.sprite_Branch[4];
            transform.GetChild(0).transform.rotation = Quaternion.Euler(0, 0, 90);
        }
        else if (notconnected[1] && notconnected[3])
        {
            transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = api.engine.initializer.sprite_Branch[4];
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
        }
        SetJointOrEntrance(Direction.Up);
        SetJointOrEntrance(Direction.Right);
        SetJointOrEntrance(Direction.Down);
        SetJointOrEntrance(Direction.Left);
    }

    private void SetJointOrEntrance(Direction direction)
    {
        Debug.Log("fuck you : " + direction);
        if(Toolkit.IsConnectedFromPosition(this, Toolkit.VectorSum(position, direction)))
        {
            if (!Toolkit.HasBranch(Toolkit.VectorSum(position, direction)))
            {
                switch (direction)
                {
                    case Direction.Up:
                        transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = api.engine.initializer.sprite_BranchJoint;
                        transform.GetChild(1).GetComponent<SpriteRenderer>().flipX = false; return;
                    case Direction.Right:
                        transform.GetChild(2).GetComponent<SpriteRenderer>().sprite = api.engine.initializer.sprite_BranchJoint;
                        transform.GetChild(2).GetComponent<SpriteRenderer>().flipX = true; return;
                    case Direction.Down:
                        transform.GetChild(3).GetComponent<SpriteRenderer>().sprite = api.engine.initializer.sprite_BranchJoint;
                        transform.GetChild(3).GetComponent<SpriteRenderer>().flipX = true; return;
                    case Direction.Left:
                        transform.GetChild(4).GetComponent<SpriteRenderer>().sprite = api.engine.initializer.sprite_BranchJoint;
                        transform.GetChild(4).GetComponent<SpriteRenderer>().flipX = true; return;
                }
            }
        }
        if(Toolkit.IsConnectedFromPositionToBranch(this, direction))
        {
            switch (direction)
            {
                case Direction.Up:
                    transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = null;
                    return;
                case Direction.Right:
                    transform.GetChild(2).GetComponent<SpriteRenderer>().sprite = null;
                    return;
                case Direction.Down:
                    transform.GetChild(3).GetComponent<SpriteRenderer>().sprite = null;
                    return;
                case Direction.Left:
                    transform.GetChild(4).GetComponent<SpriteRenderer>().sprite = null;
                    return;
            }
        }
    }

    public override bool PlayerMoveInto(Direction dir)
    {
        return true;
    }

    public override CloneableUnit Clone()
    {
        return new CloneableBranch(this);
    }

}

public class CloneableBranch : CloneableUnit
{
    public CloneableBranch(Branch branch) : base(branch.position)
    {
        original = branch;
    }
}

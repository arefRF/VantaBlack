using UnityEngine;
using System.Collections.Generic;
using System;

public class Rock : Unit
{
    public bool isleanable = false;

    public override void SetInitialSprite()
    {
        if (Starter.Blockinvis)
            gameObject.GetComponent<SpriteRenderer>().sprite = null;
        else
        {
            bool[] notconnected = Toolkit.GetConnectedSides(this);
            if (notconnected[0] && notconnected[1] && notconnected[2] && notconnected[3])
                return;
            else if(notconnected[0] && notconnected[1] && notconnected[2])
                transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = api.engine.initializer.sprite_Rock[8];
            else if (notconnected[1] && notconnected[2] && notconnected[3])
                transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = api.engine.initializer.sprite_Rock[9];
            else if (notconnected[2] && notconnected[3] && notconnected[0]) 
                transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = api.engine.initializer.sprite_Rock[10];
            else if (notconnected[3] && notconnected[0] && notconnected[1])
                transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = api.engine.initializer.sprite_Rock[11];
            else if (notconnected[0] && notconnected[1])
                transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = api.engine.initializer.sprite_Rock[7];
            else if (notconnected[1] && notconnected[2])
                transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = api.engine.initializer.sprite_Rock[5];
            else if (notconnected[2] && notconnected[3])
                transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = api.engine.initializer.sprite_Rock[4];
            else if (notconnected[3] && notconnected[0])
                transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = api.engine.initializer.sprite_Rock[6];
            else if (notconnected[0] && notconnected[2])
                transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = api.engine.initializer.sprite_Rock[13];
            else if (notconnected[1] && notconnected[3])
                transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = api.engine.initializer.sprite_Rock[12];
            else if (notconnected[0])
                transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = api.engine.initializer.sprite_Rock[3];
            else if (notconnected[1])
                transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = api.engine.initializer.sprite_Rock[1];
            else if (notconnected[2])
                transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = api.engine.initializer.sprite_Rock[0];
            else if (notconnected[3])
                transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = api.engine.initializer.sprite_Rock[2];
            else
                transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = api.engine.initializer.sprite_Rock[14];
        }

    }

    private void Connected_4(bool[] connected)
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = api.engine.initializer.sprite_Rock[0];
    }
    private void Connected_3(bool[] connected)
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = api.engine.initializer.sprite_Rock[4];
        if(!connected[1])
            transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 270);
        else if (!connected[2])
            transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 180);
        else if (!connected[3])
            transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 90);
    }
    private void Connected_2(bool[] connected)
    {
        if (connected[1] && connected[3])
            gameObject.GetComponent<SpriteRenderer>().sprite = api.engine.initializer.sprite_Rock[3];
        else if (connected[0] && connected[2])
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = api.engine.initializer.sprite_Rock[3];
            transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 90);
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = api.engine.initializer.sprite_Rock[5];
            if(connected[0] && connected[1])
                transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 180);
            else if(connected[1] && connected[2])
                transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 90);
            else if(connected[3] && connected[0])
                transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 270);

        }
    }
    private void Connected_1(bool[] connected)
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = api.engine.initializer.sprite_Rock[2];
        if (connected[0])
            transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 270);
        else if (connected[1])
            transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 180);
        else if (connected[2])
            transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 90);
    }
    private void Connected_0(bool[] connected)
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = api.engine.initializer.sprite_Rock[1];
    }

    public override CloneableUnit Clone()
    {
        return new CloneableRock(this);
    }

    public override bool isLeanable()
    {
        return isleanable;
    }
}


public class CloneableRock : CloneableUnit
{
    public CloneableRock(Rock rock) : base(rock.position)
    {
        original = rock;
    }
}
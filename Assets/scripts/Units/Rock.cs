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
            bool[] connected = Toolkit.GetConnectedSides(this);
            int sidecount = 0;
            for (int i = 0; i < 4; i++)
            {
                Debug.Log(connected[i]);
                sidecount += Convert.ToInt32(connected[i]);
            }
            Debug.Log(sidecount);
            switch (sidecount)
            {
                case 0: Connected_0(connected); break;
                case 1: Connected_1(connected); break;
                case 2: Connected_2(connected); break;
                case 3: Connected_3(connected); break;
                case 4: Connected_4(connected); break;
            }
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
        Debug.Log(connected[1]);
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
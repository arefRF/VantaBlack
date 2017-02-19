using UnityEngine;
using System.Collections.Generic;
using System;

public class Rock : Unit
{
    public override void SetInitialSprite()
    {
        bool[] notconnected = Toolkit.GetConnectedSides(this);
        if (notconnected[0] && notconnected[1] && notconnected[2] && notconnected[3])
            gameObject.GetComponent<SpriteRenderer>().sprite = api.engine.initializer.sprite_Rock[1];
        else if (notconnected[0] && notconnected[1] && notconnected[2])
            gameObject.GetComponent<SpriteRenderer>().sprite = api.engine.initializer.sprite_Rock[2];
        else if (notconnected[0] && notconnected[2] && notconnected[3])
            gameObject.GetComponent<SpriteRenderer>().sprite = api.engine.initializer.sprite_Rock[3];
        else if (notconnected[0] && notconnected[1] && notconnected[3])
            gameObject.GetComponent<SpriteRenderer>().sprite = api.engine.initializer.sprite_Rock[4];
        else if (notconnected[1] && notconnected[2] && notconnected[3])
            gameObject.GetComponent<SpriteRenderer>().sprite = api.engine.initializer.sprite_Rock[5];
        else if (notconnected[0] && notconnected[2])
            gameObject.GetComponent<SpriteRenderer>().sprite = api.engine.initializer.sprite_Rock[6];
        else if (notconnected[1] && notconnected[3])
            gameObject.GetComponent<SpriteRenderer>().sprite = api.engine.initializer.sprite_Rock[7];
        else if (notconnected[0] && notconnected[1])
            gameObject.GetComponent<SpriteRenderer>().sprite = api.engine.initializer.sprite_Rock[8];
        else if (notconnected[0] && notconnected[3])
            gameObject.GetComponent<SpriteRenderer>().sprite = api.engine.initializer.sprite_Rock[9];
        else if (notconnected[1] && notconnected[2])
            gameObject.GetComponent<SpriteRenderer>().sprite = api.engine.initializer.sprite_Rock[10];
        else if (notconnected[2] && notconnected[3])
            gameObject.GetComponent<SpriteRenderer>().sprite = api.engine.initializer.sprite_Rock[11];
        else if (notconnected[0])
            gameObject.GetComponent<SpriteRenderer>().sprite = api.engine.initializer.sprite_Rock[12];
        else if (notconnected[1])
            gameObject.GetComponent<SpriteRenderer>().sprite = api.engine.initializer.sprite_Rock[13];
        else if (notconnected[2])
            gameObject.GetComponent<SpriteRenderer>().sprite = api.engine.initializer.sprite_Rock[14];
        else if (notconnected[3])
            gameObject.GetComponent<SpriteRenderer>().sprite = api.engine.initializer.sprite_Rock[15];
        else
            gameObject.GetComponent<SpriteRenderer>().sprite = api.engine.initializer.sprite_Rock[0];
    }
    public override CloneableUnit Clone()
    {
        return new CloneableRock(this);
    }
}


public class CloneableRock : CloneableUnit
{
    public CloneableRock(Rock rock) : base(rock.position)
    {
        original = rock;
    }
}
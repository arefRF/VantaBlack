using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Block : Unit
{

    public Ability ability;
    public Block Pipedto, Pipedfrom;


    // Use this for initialization
    void Awake()
    {
        unitType = UnitType.Block;
        obj = this.gameObject;
        position = gameObject.transform.position;
        movable = true;
        layer = 1;
    }



    // Update is called once per frame
    void Update()
    {

    }

    public void CheckPipe()
    {
        if (Pipedfrom == null)
            return;
        if (ability == null && Pipedfrom.ability != null)
        {
            Pomp();
            Pipedfrom.CheckPipe();
        }
    }

    private void Pomp()
    {
        ability = Pipedfrom.ability;
        Pipedfrom.ClearBlock();
    }

    public void ClearBlock()
    {
        ability = null;
    }

    public override bool CanMove(UnitType unittype)
    {
        if (unittype == UnitType.Box || unittype == UnitType.Player)
            return true;
        return false;
    }

    public CloneableBlock Clone()
    {
        return CloneableBlock.Clone(this);
    }
}

public class CloneableBlock : CloneableUnit
{
    public Ability ability;
    public static CloneableBlock Clone(Block block)
    {
        CloneableBlock b = new CloneableBlock();
        CloneableUnit.init(block, b);
        b.ability = block.ability;
        return b;
    }
}



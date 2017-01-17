using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Player : Unit
{
    public Ability ability;
    public List<Direction> move_direction;
    public Direction direction { get; set; }
    public PlayerState state { get; set; }

    //public bool CanUseRed;
    // Use this for initialization
    void Awake()
    {
        direction = move_direction[0];
        unitType = UnitType.Player;
        obj = this.gameObject;
        position = gameObject.transform.position;
        movable = true;
        layer = 1;
    }

    // Update is called once per frame
    void Update()
    {

    }


    public override bool MoveInto(Direction dir)
    {
        return false;
    }

    public CloneablePlayer Clone()
    {
        return CloneablePlayer.Clone(this);
    }
}

public class CloneablePlayer : CloneableUnit
{
    public Ability ability;
    public List<Direction> move_direction;
    public Direction direction;
    //public bool CanUseRed;
    public static CloneablePlayer Clone(Player player)
    {
        CloneablePlayer p = new CloneablePlayer();
        CloneableUnit.init(player, p);
        p.ability = player.ability;
        p.move_direction = new List<Direction>();
        p.direction = player.direction;
        foreach (Direction d in player.move_direction)
        {
            switch (d)
            {
                case Direction.Down: p.move_direction.Add(Direction.Down); break;
                case Direction.Up: p.move_direction.Add(Direction.Left); break;
                case Direction.Right: p.move_direction.Add(Direction.Right); break;
                case Direction.Left: p.move_direction.Add(Direction.Left); break;
            }
        }
        //p.CanUseRed = player.CanUseRed;
        return p;
    }
}


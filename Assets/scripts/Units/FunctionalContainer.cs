using UnityEngine;
using System.Collections.Generic;
using System;

public class FunctionalContainer : Container
{
    public Direction direction;
    public bool on;
    public int currentState { get; set; }
    public int nextState { get; set; }
    public bool firstmove { get; set; }

    public override bool PlayerMoveInto(Direction dir)
    {
        return false;
    }

    public override void Action(Player player, Direction dir)
    {
        if (gameObject.transform.parent.gameObject.GetComponent<ParentScript>().movelock)
            return;
        if (abilities.Count == 0)
            return;
        api.AddToSnapshot(this);
        api.AddToSnapshot(ConnectedUnits);
        api.AddToSnapshot(player);
        //api.TakeSnapshot();
        switch (abilities[0].abilitytype)
        {
            case AbilityType.Fuel: SetOnorOff(); firstmove = true; Action_Fuel(); break;
            case AbilityType.Jump: ((Jump)abilities[0]).Action(player, dir); break;
        }
    }

    public void SetOnorOff()
    {
        if (abilities.Count == 0)
            on = false;
        else
            on = !on;
        api.engine.apigraphic.UnitChangeSprite(this);
        if (!on)
            nextState = 0;
        else
            nextState = abilities.Count;
        for(int i=0; i<sameContainer.Count; i++)
        {
            ((FunctionalContainer)sameContainer[i]).on = on;
            ((FunctionalContainer)sameContainer[i]).nextState = nextState;
        }
    }

    public void Action_Fuel()
    {
        api.engine.Applygravity();
        if (currentState == nextState)
        {
            api.engine.pipecontroller.CheckPipes();
            return;
        }
        if (!MoveContainer(GetMoveDirection()))
        {
            api.AddToStuckList(this);
            api.engine.pipecontroller.CheckPipes();
            return;
        }
        api.CheckstuckedList();
        SetCurrentState();
        if (firstmove)
            firstmove = false;
    }

    private void SetCurrentState()
    {
        if (currentState > nextState)
            currentState--;
        else if (currentState < nextState)
            currentState++;
        else
        {
            throw new Exception("current state = next state");
        }
        for (int i = 0; i < sameContainer.Count; i++)
        {
            ((FunctionalContainer)sameContainer[i]).currentState = currentState;
        }
    }

    private Direction GetMoveDirection()
    {
        if (currentState > nextState)
            return Toolkit.ReverseDirection(direction);
        else if (currentState < nextState)
            return direction;
        else
        {
            throw new Exception("current state = next state");
        }
    }

    public bool MoveContainer(Direction dir)
    {
            return api.MoveUnit(this, dir);
    }

    public override void SetNextState()
    {
        if (abilities.Count == 0)
            nextState = 0;
        else if (abilities[0].abilitytype != AbilityType.Fuel)
            nextState = 0;
        else
            nextState = abilities.Count;
        for (int i = 0; i < sameContainer.Count; i++)
        {
            ((FunctionalContainer)sameContainer[i]).nextState = nextState;
        }
    }
}
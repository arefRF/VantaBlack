using UnityEngine;
using System.Collections.Generic;
using System;
using System.Runtime.CompilerServices;
using System.Collections;

public class FunctionalContainer : Container
{
    public Direction direction;
    public bool on;
    public int currentState { get; set; }
    public int nextState { get; set; }
    public bool firstmove { get; set; }
    private AudioSource audio_source;
    private bool actionlock = false;
    private bool IsOnMove; //if container is moving this will be true
    public override bool PlayerMoveInto(Direction dir)
    {
        return false;
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public override void Action(Player player, Direction dir)
    {
        if (gameObject.transform.parent.gameObject.GetComponent<ParentScript>().movelock)
            return;
        if (actionlock)
            return;
        if (abilities.Count == 0)
            return;
        if (player.state == PlayerState.Gir)
            player.SetState(PlayerState.Lean);
        switch (abilities[0].abilitytype)
        {
            case AbilityType.Fuel: SetOnorOff(); firstmove = true; Action_Fuel(); break;
            case AbilityType.Jump: ((Jump)abilities[0]).Action(player, dir); break;
            case AbilityType.Teleport: ((Teleport)abilities[0]).Action_Container(player,dir,this);break;
            case AbilityType.Gravity: ((Gravity)abilities[0]).Action_Container(this);break;
            case AbilityType.Laser: SetOnorOff(); break;
        }
        actionlock = false;
    }

    public void ActionKeyDown(Player player,Direction dir)
    {
        if (gameObject.transform.parent.gameObject.GetComponent<ParentScript>().movelock)
            return;
        if (abilities.Count == 0)
            return;
        switch (abilities[0].abilitytype)
        {
            case AbilityType.Jump: ((Jump)abilities[0]).StartTimer(player, dir); break;
        }
    }

    public void SetOnorOff()
    {
        if (abilities.Count == 0)
            on = false;
        else
            on = !on;

        if (audio_source == null)
            audio_source = GetComponent<AudioSource>();

        // for Sound Effect
       // if (on)
           // audio_source.Play();

        api.engine.apigraphic.UnitChangeSprite(this);
        if (!on)
            nextState = 0;
        else
            nextState = abilities.Count;
       // api.RemoveFromStuckList(this);
        for(int i=0; i<sameContainer.Count; i++)
        {
            ((FunctionalContainer)sameContainer[i]).on = on;
            ((FunctionalContainer)sameContainer[i]).nextState = nextState;
        }
    }

    public void Action_Fuel()
    {
        if (actionlock)
            return;
        actionlock = true;
        //api.engine.Applygravity();
        if (currentState == nextState)
        {
            api.engine.pipecontroller.CheckPipes();
            actionlock = false;
            IsOnMove = false;
            return;
        }
        IsOnMove = true;
        if (!MoveContainer(GetMoveDirection()))
        {
            Debug.Log(this);
            IsOnMove = false;
            api.AddToStuckList(this);
            api.engine.pipecontroller.CheckPipes();
            actionlock = false;
            return;
        }
        api.CheckstuckedList(this);
        SetCurrentState();
        if (firstmove)
            firstmove = false;
        actionlock = false;
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

    public void CheckNextMove()
    {
        if (currentState != nextState && !IsOnMove)
        {
            Action_Fuel();
        }
    }
    public bool MoveContainer(Direction dir)
    {
        return api.MoveUnit(this, dir);
    }


    [MethodImpl(MethodImplOptions.Synchronized)]
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

    private IEnumerator _ActionLockWait(float time)
    {
        yield return new WaitForSeconds(time);
        actionlock = false;
    }
}
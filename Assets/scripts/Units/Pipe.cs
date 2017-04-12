using UnityEngine;
using System.Collections.Generic;

public class Pipe : Unit {

    public List<Unit> PipedTo;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


    public void Action()
    {
        PipedTo = Toolkit.SortByDirection(PipedTo, api.engine.database.gravity_direction);
        for (int i = 0; i < PipedTo.Count; i++)
        {
            if (CheckGravitywise(PipedTo[i] as Pipe))
            {
                Container sink = ((Pipe)PipedTo[i]).CheckPipeAction();
                Container source = CheckAvailableContainer();
                if (sink != null && source != null) {
                    if (source.abilities.Count < source.capacity && sink.abilities.Count != 0)
                    {
                        if(source.abilities.Count == 0 || source.abilities[0].abilitytype == sink.abilities[0].abilitytype)
                            Pomp(PipedTo[i] as Pipe);
                    }
                }
            }
        }
    }


    private bool CheckGravitywise(Pipe pipe)
    {
        if (Starter.GetGravityDirection() == Direction.Up)
        {
            if (pipe.position.y < position.y)
                return true;
        }
        else if (Starter.GetGravityDirection() == Direction.Down)
        {
            if (pipe.position.y > position.y)
                return true;
        }
        else if (Starter.GetGravityDirection() == Direction.Right)
        {
            if (pipe.position.x < position.x)
                return true;
        }
        else if (Starter.GetGravityDirection() == Direction.Left)
        {
            if (pipe.position.x > position.x)
                return true;
        }
        return false;
    }

    private Container CheckAvailableContainer()
    {
        foreach (Unit c in api.engine_GetUnits(position))
        {
            if (c is SimpleContainer || c is DynamicContainer)
                return c as Container;
        }
        return null;
    }

    private Container CheckPipeAction()
    {
        foreach (Unit c in api.engine_GetUnits(position))
        {
            if (c is SimpleContainer || c is DynamicContainer)
            {
                if (((Container)c).abilities.Count != 0)
                    return c as Container;
                return null;
            }
        }
        return null;

    }

    private void Pomp(Pipe pipe)
    {
        Container thiscontainer = null;
        foreach (Unit c in api.engine_GetUnits(position))
            if (c is SimpleContainer || c is DynamicContainer)
            {
                thiscontainer = c as Container;
            }
        foreach (Unit c in api.engine_GetUnits(pipe.position))
            if (c is SimpleContainer || c is DynamicContainer)
            {
                Debug.Log("pomping " + c + " to " + thiscontainer);
                (thiscontainer).PipeAbsorb((Container)c);
            }
        api.engine.pipecontroller.CheckPipes();
    }
}

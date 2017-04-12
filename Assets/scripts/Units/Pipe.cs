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
        Toolkit.SortByDirection(PipedTo, api.engine.database.gravity_direction);
        for (int i = 0; i < PipedTo.Count; i++)
        {
            if (CheckGravitywise(PipedTo[i] as Pipe))
                if (CheckAvailableContainer())
                    if (((Pipe)PipedTo[i]).CheckPipeAction())
                        Pomp(PipedTo[i] as Pipe);
        }
    }


    private bool CheckGravitywise(Pipe pipe)
    {
        if (Starter.GetGravityDirection() == Direction.Up)
        {
            if (pipe.position.y > position.y)
                return true;
        }
        else if (Starter.GetGravityDirection() == Direction.Down)
        {
            if (pipe.position.y < position.y)
                return true;
        }
        else if (Starter.GetGravityDirection() == Direction.Right)
        {
            if (pipe.position.x > position.x)
                return true;
        }
        else if (Starter.GetGravityDirection() == Direction.Left)
        {
            if (pipe.position.x < position.x)
                return true;
        }
        return false;
    }

    private bool CheckAvailableContainer()
    {
        foreach (Unit c in api.engine_GetUnits(position))
        {
            if (c is SimpleContainer || c is DynamicContainer)
            {

                if (((Container)c).abilities.Count != 0)
                    return true;
                return false;
            }
        }
        return false;
    }

    private bool CheckPipeAction()
    {
        foreach (Unit c in api.engine_GetUnits(position))
        {
            if (c is SimpleContainer || c is DynamicContainer)
            {
                if (((Container)c).abilities.Count == 0)
                    return true;
                return false;
            }
        }
        return false;

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
                Debug.Log("pomping " + thiscontainer + " to " + c);
                ((Container)c).PipeAbsorb(thiscontainer);
            }
        //api.engine.pipecontroller.CheckPipes();
    }
}

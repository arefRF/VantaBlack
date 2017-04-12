using UnityEngine;
using System.Collections;

public class Pipe : Unit {

    public Pipe PipedTo;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


    public void Action()
    {
        if (CheckGravitywise())
            if(CheckAvailableContainer())
                if (PipedTo.CheckPipeAction())
                    Pomp();
    }


    private bool CheckGravitywise()
    {
        if (Starter.GetGravityDirection() == Direction.Up)
        {
            if (PipedTo.position.y > position.y)
                return true;
        }
        else if (Starter.GetGravityDirection() == Direction.Down)
        {
            if (PipedTo.position.y < position.y)
                return true;
        }
        else if (Starter.GetGravityDirection() == Direction.Right)
        {
            if (PipedTo.position.x > position.x)
                return true;
        }
        else if (Starter.GetGravityDirection() == Direction.Left)
        {
            if (PipedTo.position.x < position.x)
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

    private void Pomp()
    {
        Container thiscontainer = null;
        foreach (Unit c in api.engine_GetUnits(position))
            if (c is SimpleContainer || c is DynamicContainer)
            {
                thiscontainer = c as Container;
            }
        foreach (Unit c in api.engine_GetUnits(PipedTo.position))
            if (c is SimpleContainer || c is DynamicContainer)
            {
                Debug.Log("pomping " + thiscontainer + " to " + c);
                ((Container)c).PipeAbsorb(thiscontainer);
            }
    }
}

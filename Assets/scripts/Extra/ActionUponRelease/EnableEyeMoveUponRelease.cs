using UnityEngine;
using System.Collections.Generic;
using System;

public class EnableEyeMoveUponRelease : ActionUponRelease {

    public List<GameObject> Eyes;
    Container container;

    // Use this for initialization
    void Start()
    {
        container = GetComponent<Container>();
        if (container.graphicalactions == null)
            container.graphicalactions = new List<GraphicalActions>();
        container.graphicalactions.Add(this);
    }

    // Update is called once per frame
    void Update () {
	    
	}

    public override void Action()
    {
        for(int i=0; i<Eyes.Count; i++)
        {
            Eyes[i].GetComponent<EyeMove>().EyeLock = false;
        }
    }
}

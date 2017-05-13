using UnityEngine;
using System.Collections.Generic;

public class CloseEyesUponRelease : ActionUponRelease {

    public GameObject Eyepart;
    Container container;

    // Use this for initialization
    void Start () {
        container = GetComponent<Container>();
        if (container.graphicalactions == null)
            container.graphicalactions = new List<GraphicalActions>();
        container.graphicalactions.Add(this);
    }
	
	

    public override void Action()
    {
        Eyepart.GetComponent<Animator>().SetBool("Open", false);
    }
}

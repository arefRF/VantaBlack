using UnityEngine;
using System.Collections.Generic;

public class OpenEyesUponRelease : ActionUponRelease {

    public GameObject Eyepart;
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
        Eyepart.GetComponent<SpriteRenderer>().enabled = false;
    }
}

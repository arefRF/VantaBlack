using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class LockCombination : MonoBehaviour {

    public Branch branch;
    public List<DynamicContainer> containers;
    public string code;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		for(int i=0; i<containers.Count; i++)
        {
            if (!(containers[i].on && Convert.ToInt32(code[i] + "") == containers[i].abilities.Count))
                return;
        }
        Unlock();
	}

    private void Unlock()
    {
        branch.islocked = false;
        branch.api.engine.apigraphic.UnitChangeSprite(branch);
    }
}

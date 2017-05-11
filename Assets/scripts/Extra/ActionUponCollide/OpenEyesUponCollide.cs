using UnityEngine;
using System.Collections.Generic;
using System;

public class OpenEyesUponCollide : MonoBehaviour{
    public List<GameObject> parentEye;
    bool opened;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D col)
    {
        if (!opened && col.tag == "Player")
        {
            opened = true;
            for(int i = 0; i < parentEye.Count; i++)
            {
                StartCoroutine(parentEye[i].transform.GetChild(0).GetComponent<EyeMove>().EyeSmoothMove());
            }
            /*


            for (int i = 0; i < eyepart.Count; i++)
                eyepart[i].GetComponent<SpriteRenderer>().enabled = false; */
        }
    }

    public void UnlockPArent()
    {
       GetComponent<ParentScript>().movelock = true;
    }
}

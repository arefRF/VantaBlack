using UnityEngine;
using System.Collections;

public class ParentScript : MonoBehaviour {

    public bool movelock { get; set; }
    private bool temp;
	// Use this for initialization
	void Start () {
        movelock = false;
        temp = movelock;
	}
}

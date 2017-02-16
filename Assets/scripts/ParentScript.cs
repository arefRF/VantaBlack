using UnityEngine;
using System.Collections;

public class ParentScript : MonoBehaviour {

    public bool movelock { get; set; }
	// Use this for initialization
	void Start () {
        movelock = false;
	}
}

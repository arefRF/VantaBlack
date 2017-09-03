using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torch : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GetComponent<Animator>().Play("Torch Light", 0, Random.Range(0,1.1f));
	}
	

}

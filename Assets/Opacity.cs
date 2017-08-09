using UnityEngine;
using System.Collections;

public class Opacity : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GetComponent<Renderer>().material.SetFloat("_Cutoff", 0.5f);
	}
	
	// Update is called once per frame

}

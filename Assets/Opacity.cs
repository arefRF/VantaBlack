using UnityEngine;
using System.Collections;

public class Opacity : MonoBehaviour {
    float opa = 1;
	// Use this for initialization
	void Start () {
        GetComponent<Renderer>().material.SetFloat("_Cutoff",1f);
   
	}
	
    public void LaserHit()
    {
        StartCoroutine(fill());
    }
    IEnumerator fill()
    {
        while (opa > 0)
        {
            GetComponent<Renderer>().material.SetFloat("_Cutoff", opa);
            opa = opa - 0.005f;
            yield return null;
        }
    }
	// Update is called once per frame

}

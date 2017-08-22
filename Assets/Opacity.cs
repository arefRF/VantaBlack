using UnityEngine;
using System.Collections;

public class Opacity : MonoBehaviour {
    float opa = 1;
    private Coroutine coroutine;
    private bool unhit;
	// Use this for initialization
	void Start () {
        GetComponent<Renderer>().material.SetFloat("_Cutoff",1f);
   
	}
	
    public void LaserHit()
    {
        Debug.Log("Lasda");
        coroutine = StartCoroutine(fill());
        unhit = false;
    }

    public void LaserUnhit()
    {
        Debug.Log("f;kjvnfjhkbgfjhbnljfbnlgfnblngljhng ");
        unhit = true;
        Debug.Log(unhit);
        opa = 1;
        GetComponent<Renderer>().material.SetFloat("_Cutoff", opa);
    }
    IEnumerator fill()
    {
        while (opa > 0)
        {
            //Debug.Log(unhit);
            if (unhit)
            {
                Debug.Log(":::::::::");
                opa = 1;
                GetComponent<Renderer>().material.SetFloat("_Cutoff", opa);
                break;
            }
            GetComponent<Renderer>().material.SetFloat("_Cutoff", opa);
            opa = opa - 0.005f;
            yield return null;
        }
    }

}

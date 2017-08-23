using UnityEngine;
using System.Collections;
using System.Runtime.CompilerServices;

public class Opacity : MonoBehaviour {
    float opa = 1;
    private Coroutine coroutine;
    private bool hit;
	// Use this for initialization
	void Start () {
        GetComponent<Renderer>().material.SetFloat("_Cutoff",1f);
   
	}
	
    public void LaserHit()
    {
        opa = 1;
        GetComponent<Renderer>().material.SetFloat("_Cutoff", opa);
        ChangeHitState(true);
        coroutine = StartCoroutine(fill());
    }

    public void LaserUnhit()
    {
        ChangeHitState(false);
        opa = 1;
        GetComponent<Renderer>().material.SetFloat("_Cutoff", opa);
    }
    IEnumerator fill()
    {
        while (opa > 0)
        {
            if (!hit)
            {
                opa = 1;
                GetComponent<Renderer>().material.SetFloat("_Cutoff", opa);
                break;
            }
            GetComponent<Renderer>().material.SetFloat("_Cutoff", opa);
            opa = opa - 0.005f;
            yield return null;
        }
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    private void ChangeHitState(bool hitstate)
    {
        hit = hitstate;
    }
}

using UnityEngine;
using System.Collections;

public class ObjectShow : MonoBehaviour {
    public string name;
    private GameObject tutorial;
    private GameObject text_obj;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            
            GameObject.Find("TutorialAnimation").GetComponent<Animator>().SetBool(name,true);
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if(col.gameObject.tag == "Player")
        {
            GameObject.Find("TutorialAnimation").GetComponent<Animator>().SetBool(name,false);
        }
    }
}

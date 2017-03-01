using UnityEngine;
using System.Collections;

public class ObjectShow : MonoBehaviour {
    public string parent_name;
    public string object_name;
    private GameObject tutorial;
    private GameObject text_obj;

    void Start()
    {
        GameObject tutorial = GameObject.Find(parent_name);
        for (int i = 0; i < tutorial.transform.childCount; i++)
        {
            GameObject text = tutorial.transform.GetChild(i).gameObject;
            if (text.name == object_name)
                text_obj = text;
        }
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == "Player")
        {
            text_obj.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if(col.gameObject.tag == "Player")
        {
            text_obj.SetActive(false);
        }
    }
}

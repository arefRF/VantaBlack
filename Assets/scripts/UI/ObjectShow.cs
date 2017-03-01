using UnityEngine;
using System.Collections;

public class ObjectShow : MonoBehaviour {
    public string parent_name;
    public string object_name;
    private GameObject tutorial;
    private GameObject text_obj;

    void Start()
    {
        Set_Object();

    }
    void Set_Object()
    {
        if (text_obj != null)
            text_obj.SetActive(false);
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
        if (col.gameObject.tag == "Player")
        {
            Show_Object();
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if(col.gameObject.tag == "Player")
        {
            text_obj.SetActive(false);
        }
    }

    public void Show_Object()
    {
        Set_Object();
        text_obj.SetActive(true);
    }

    public void Hide_Object()
    {
        text_obj.SetActive(false);
    }
}

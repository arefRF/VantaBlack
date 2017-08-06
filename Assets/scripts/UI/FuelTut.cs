using UnityEngine;
using System.Collections;

public class FuelTut : MonoBehaviour {
    private bool show;
    private string par_name;
    private GameObject space;

    void Start()
    {
        space = GameObject.Find("space");
    }

    void OnTriggerStay2D(Collider2D col)
    {

        if (col.gameObject.tag == "Dynamic Container")
        {
            if (col.GetComponent<DynamicContainer>().abilities.Count != 0)
                show = true;
            else
                show = false;
            space.SetActive(show);
        }
        if(col.gameObject.tag == "Player")
        {
            space.SetActive(show);
        }
    }
    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            space.SetActive(false);
        }
    }
}

using UnityEngine;
using System.Collections;

public class DisableTutEvent : MonoBehaviour {
    public string event_name;
    void OnTriggerStay2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            for(int i= 0; i < transform.parent.childCount; i++)
            {
                if (transform.parent.GetChild(i).name == event_name)
                {
                    transform.parent.GetChild(i).gameObject.SetActive(false);
                }
            }
        }
    }
}

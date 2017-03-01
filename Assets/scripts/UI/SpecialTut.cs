using UnityEngine;
using System.Collections;

public class SpecialTut : MonoBehaviour {
    void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("enter");
        if(col.tag == "Player")
        {
            Player player = col.GetComponent<Player>();
            if(player.abilities.Count!=0)
            {
                GetComponent<ObjectShow>().object_name = "Release";

            }

        }
    }
}

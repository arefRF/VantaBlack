using UnityEngine;
using System.Collections;

public class SpecialTut : MonoBehaviour
{
    private bool got_key;
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            Player player = col.GetComponent<Player>();
            if (player.abilities.Count != 0)
            {
                GetComponent<ObjectShow>().object_name = "Release";
                got_key = true;
            }

        }
    }
    void OnTriggerStay2D(Collider2D col)
    {

        if (col.tag == "Player")
        {
            Player player = col.GetComponent<Player>();
            if (got_key && player.abilities.Count == 0)
            {
                GetComponent<ObjectShow>().object_name = "Space";
                GetComponent<ObjectShow>().Show_Object();
            }

        }

    }
}

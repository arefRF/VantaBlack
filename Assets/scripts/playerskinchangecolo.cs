using UnityEngine;
using System.Collections;

public class playerskinchangecolo : MonoBehaviour {
    public Player player;
    public bool white;
    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "Player")
        {
            if(white)
                player.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
            else
                player.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().color = new Color(109f/255f, 175f/255f, 188f/255f, 1);
        }
    }
}

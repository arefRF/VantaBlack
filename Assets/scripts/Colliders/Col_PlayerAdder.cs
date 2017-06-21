using UnityEngine;
using System.Collections;

public class Col_PlayerAdder : MonoBehaviour {

    public Player player;

    void OnTriggerStay2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            if ((Vector2)col.transform.position == col.gameObject.GetComponent<Unit>().position)
            {
                Starter.GetDataBase().player.Add(player);
                player.ApplyGravity();
                GetComponent<BoxCollider2D>().isTrigger = false;
            }
        }
    }
}

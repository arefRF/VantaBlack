using UnityEngine;
using System.Collections;

public class DIE : MonoBehaviour {
    public Player player;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            player.api.RemoveFromDatabase(player);
            player.position = new Vector2(6, 9);
            player.api.AddToDatabase(player);
            player.transform.position = new Vector3(6, 9, 1);
            player.SetState(PlayerState.Idle);
            player.ApplyGravity();
        }
    }
}

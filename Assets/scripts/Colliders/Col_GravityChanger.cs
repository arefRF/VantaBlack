using UnityEngine;
using System.Collections;

public class Col_GravityChanger : MonoBehaviour {

    public Direction GravityDirection;

    void OnTriggerStay2D(Collider2D col)
    {
        if(col.tag == "Player")
        {
            if ((Vector2)col.transform.position == col.gameObject.GetComponent<Unit>().position)
            {
                Player player = col.gameObject.GetComponent<Player>();
                Debug.Log("asd");
                player.transform.rotation = Quaternion.Euler(player.transform.rotation.x, player.transform.rotation.y, 180);
                Starter.GetDataBase().SetGravity(GravityDirection);
                Starter.GetEngine().Applygravity();
            }
        }
    }
}

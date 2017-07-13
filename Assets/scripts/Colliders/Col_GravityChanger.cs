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
                Starter.GetDataBase().SetGravity(GravityDirection);
                int zrot = 0;
                if (player.GetGravity() == Direction.Up)
                    zrot = 180;
                player.transform.rotation = Quaternion.Euler(player.transform.rotation.x, player.transform.rotation.y, zrot);
                Starter.GetEngine().Applygravity();
            }
        }
    }
}

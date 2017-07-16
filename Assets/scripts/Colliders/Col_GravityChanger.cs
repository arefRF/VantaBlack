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
                else if (player.GetGravity() == Direction.Right)
                    zrot = 90;
                else if (player.GetGravity() == Direction.Left)
                    zrot = 270;
                GameObject obj = Toolkit.GetObjectInChild(player.gameObject, "Sprite Holder");
                obj.transform.rotation = Quaternion.Euler(obj.transform.rotation.x, obj.transform.rotation.y, zrot);
                Starter.GetEngine().Applygravity();
            }
        }
    }
}

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
                Starter.GetDataBase().SetGravity(GravityDirection);
                Starter.GetEngine().Applygravity();
            }
        }
    }
}

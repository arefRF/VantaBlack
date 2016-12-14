using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class CheckPoint : MonoBehaviour {
    private bool is_used = false;
    public void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "Player" && !is_used)
        {
            is_used = true;
             Database.database.snapshots.Clear();
        }
    }
}

using UnityEngine;
using System.Collections;

public class Checkpoint : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log(col);
        if (col.tag == "Player")
        {
            Player player = Starter.GetDataBase().player[0];
            Starter.GetEngine().saveserialize.serialize(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name, player.position, player.abilitytype, player.abilitycount);
        }
    }
}
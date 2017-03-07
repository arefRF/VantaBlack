using UnityEngine;
using System.Collections;

public class SpecialTut : MonoBehaviour
{
    public string name;
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            Player player = col.GetComponent<Player>();
            if (player.abilities.Count == 0)
            {
                Debug.Log("false");
                GameObject.Find("TutorialAnimation").GetComponent<Animator>().SetBool(name, false);
            }

        }
    }
}

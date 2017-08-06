using UnityEngine;
using System.Collections;

public class SpecialTut : MonoBehaviour
{
    public int number;
    private Animator animator;

    void Start()
    {
        animator = GameObject.Find("Keyboard Arrow Tutorial").GetComponent<Animator>();
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            Player player = col.GetComponent<Player>();
            Debug.Log(player.state);
            if (player.abilities.Count == 0 && player.state == PlayerState.Lean)
            {
                animator.SetInteger("Tut", number);
                //GameObject.Find("TutorialAnimation").GetComponent<Animator>().SetBool(name, false);
            }

        }
    }
    void OnTriggerExit2D(Collider2D col)
    {
        if(col.tag== "Player")
        {
            animator.SetInteger("Tut", 0);
        }
    }
}

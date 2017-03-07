using UnityEngine;
using System.Collections;

public class FuelTut : MonoBehaviour {
    private bool show;
    private string par_name;
    private Animator animator;

    void Start()
    {
        animator = GameObject.Find("TutorialAnimation").GetComponent<Animator>();
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.tag == "Dynamic Container")
        {
            if (col.GetComponent<DynamicContainer>().abilities.Count != 0)
                show = true;
            else
                show = false;
        }
        else if(col.gameObject.tag == "Player")
        {
            if(show)
                animator.SetBool("Space", true);
            else
                animator.SetBool("Space", false);
        }
    }
    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
          animator.SetBool("Space", false);
        }
    }
}

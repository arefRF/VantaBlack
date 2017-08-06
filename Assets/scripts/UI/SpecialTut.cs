using UnityEngine;
using System.Collections;

public class SpecialTut : MonoBehaviour
{
    public string name;
    public string number;
    private GameObject arrow;

    void Start()
    {
        GameObject arrow_holder = GameObject.Find("Arrows Tutorial "+number);
        arrow = Toolkit.GetObjectInChild(arrow_holder, name);
    }

    void OnTriggerStay2D(Collider2D col)
    {
        Debug.Log("collision");
        if (col.tag == "Player")
        {
            Player player = col.GetComponent<Player>();
            Debug.Log(player.state);
            if (player.abilities.Count == 0 && player.state == PlayerState.Lean)
            {
                Debug.Log(arrow);
                arrow.transform.GetChild(0).gameObject.SetActive(true);
                //GameObject.Find("TutorialAnimation").GetComponent<Animator>().SetBool(name, false);
            }
            else
                arrow.transform.GetChild(0).gameObject.SetActive(false);

        }
    }
}

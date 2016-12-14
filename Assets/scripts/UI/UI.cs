using UnityEngine;
using System.Collections;

public class UI : MonoBehaviour {

    public Sprite right_on;
    public Sprite right_off;
    public Sprite left_on;
    public Sprite left_off;

	// Use this for initialization
    public void _right_light()
    {
        GameObject.Find("Right").GetComponent<SpriteRenderer>().sprite = right_on;
    }
    public void _right_light_off()
    {
        GameObject.Find("Right").GetComponent<SpriteRenderer>().sprite = right_off;
    }

    public void _left_light_on()
    {
        GameObject.Find("Left").GetComponent<SpriteRenderer>().sprite = left_on;
    }
    public void _left_light_off()
    {
        GameObject.Find("Left").GetComponent<SpriteRenderer>().sprite = left_off;
    }

}

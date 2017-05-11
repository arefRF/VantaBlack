using UnityEngine;
using System.Collections;

public class EyeMove : MonoBehaviour {

    private Vector2 center_position;
    private float radius;
    private float cos,sin, constant;
    Player player;
	// Use this for initialization
	void Start () {
        if (player == null)
            player = Starter.GetDataBase().player[0];
        center_position = transform.parent.transform.position;
        radius = Mathf.Sqrt(Mathf.Pow(Mathf.Abs(transform.position.x - center_position.x), 2) + Mathf.Pow(Mathf.Abs(transform.position.y - center_position.y), 2));
    }
	
	// Update is called once per frame
	void Update () {
        if (!transform.parent.parent.GetComponent<ParentScript>().movelock)
            return;
        constant = Mathf.Sqrt(Mathf.Pow(Mathf.Abs(player.transform.position.x - center_position.x), 2) + Mathf.Pow(Mathf.Abs(player.transform.position.y - center_position.y), 2));
        cos = (player.transform.position.x - center_position.x)/constant;
        sin = (player.transform.position.y - center_position.y)/constant;
        transform.position = new Vector3(center_position.x + radius * cos, center_position.y + radius * sin, 0);
    }
}

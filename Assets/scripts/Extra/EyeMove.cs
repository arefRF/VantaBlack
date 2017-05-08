using UnityEngine;
using System.Collections;

public class EyeMove : MonoBehaviour {

    public Vector2 center_position;
    private float cos,sin,constant;
    Player player;
	// Use this for initialization
	void Start () {
        if (player == null)
            player = Starter.GetDataBase().player[0];
	}
	
	// Update is called once per frame
	void Update () {
        constant = Mathf.Sqrt(Mathf.Pow(Mathf.Abs(player.transform.position.x - center_position.x), 2) + Mathf.Pow(Mathf.Abs(player.transform.position.y - center_position.y), 2));
        cos = (player.transform.position.x - center_position.x)/constant;
        sin = (player.transform.position.y - center_position.y)/constant;
        transform.position = new Vector3(center_position.x + 0.13f * cos, center_position.y + 0.13f * sin, 0);
    }
}

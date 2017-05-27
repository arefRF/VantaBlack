using UnityEngine;
using System.Collections;

public class EyeMove : MonoBehaviour {

    private Vector2 center_position;
    private float radius;
    private float cos,sin, constant;
    public bool EyeLock = true;
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
        if (EyeLock)
            return;
        center_position = transform.parent.transform.position;
        constant = Mathf.Sqrt(Mathf.Pow(Mathf.Abs(player.transform.position.x - center_position.x), 2) + Mathf.Pow(Mathf.Abs(player.transform.position.y - center_position.y), 2));
        cos = (player.transform.position.x - center_position.x)/constant;
        sin = (player.transform.position.y - center_position.y)/constant;
        StartCoroutine(EyeSmoothMove());
        //transform.position = new Vector3(center_position.x + radius * cos, center_position.y + radius * sin, 0);
    }


    public IEnumerator EyeSmoothMove()
    {

        constant = Mathf.Sqrt(Mathf.Pow(Mathf.Abs(player.transform.position.x - center_position.x), 2) + Mathf.Pow(Mathf.Abs(player.transform.position.y - center_position.y), 2));
        cos = (player.transform.position.x - center_position.x) / constant;
        sin = (player.transform.position.y - center_position.y) / constant;
        Vector3 target = new Vector3(center_position.x + radius * cos, center_position.y + radius * sin, 0);
        float remain = (transform.position - target).sqrMagnitude;
        while(remain > float.Epsilon)
        {
            constant = Mathf.Sqrt(Mathf.Pow(Mathf.Abs(player.transform.position.x - center_position.x), 2) + Mathf.Pow(Mathf.Abs(player.transform.position.y - center_position.y), 2));
            cos = (player.transform.position.x - center_position.x) / constant;
            sin = (player.transform.position.y - center_position.y) / constant;
            target = new Vector3(center_position.x + radius * cos, center_position.y + radius * sin, 0);
            remain = (transform.position - target).sqrMagnitude;
            transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime/3f);
            yield return null;
        }
        
        EyeLock = false;
    }
}

using UnityEngine;
using System.Collections;

public class EyeMoveMouse : MonoBehaviour {
    private Vector2 center_position;
    private float max_radius;
    private float radius;
    private float cos, sin, constant;
    private Vector3 target;
    // Use this for initialization
    void Start () {
        center_position = transform.parent.transform.position;
        max_radius = Mathf.Sqrt(Mathf.Pow(Mathf.Abs(transform.position.x - center_position.x), 2) + Mathf.Pow(Mathf.Abs(transform.position.y - center_position.y), 2));
        radius = max_radius;
    }
	
	// Update is called once per frame
	void Update () {
        if ((Input.mousePosition - transform.parent.transform.position).sqrMagnitude < 50)
            radius = max_radius / 4;
        else
            radius = max_radius;
        center_position = transform.parent.transform.position;
        constant = Mathf.Sqrt(Mathf.Pow(Mathf.Abs(Input.mousePosition.x - center_position.x), 2) + Mathf.Pow(Mathf.Abs(Input.mousePosition.y - center_position.y), 2));
        cos = (Input.mousePosition.x - center_position.x) / constant;
        sin = (Input.mousePosition.y - center_position.y) / constant;
        transform.position = new Vector3(center_position.x + radius * cos, center_position.y + radius * sin, 0);
    }
}

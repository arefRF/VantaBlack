using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
    public Player player;
    public float left_bound, right_bound, upper_bound, lower_bound;
    private Transform p_transform;
    private float vert_view;
    private float horz_view;

	// Use this for initialization
	void Start () {
        p_transform = player.transform;
        Camera_Bounds_Calculate();
    }


    private void Camera_Bounds_Calculate()
    {
        vert_view = Camera.main.orthographicSize;
        horz_view = vert_view * Screen.width / Screen.height;
    }
	
	// Update is called once per frame
	void Update () {
        Vector3 pos = new Vector3(p_transform.position.x,p_transform.position.y,transform.position.z);
        pos.x = Mathf.Clamp(pos.x, left_bound, right_bound);
        pos.y = Mathf.Clamp(pos.y, lower_bound, upper_bound);
        transform.position = pos;
	}
}

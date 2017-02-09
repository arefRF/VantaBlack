using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
    public Player player;
    public bool auto_move = true;
    public float left_bound, right_bound, upper_bound, lower_bound;
    private Transform p_transform;
    private float vert_view;
    private float horz_view;
    private Vector3 pos;

	// Use this for initialization
	void Start () {
        p_transform = player.transform;
        Camera_Bounds_Calculate();
        pos = new Vector3(p_transform.position.x, p_transform.position.y, transform.position.z);
    }


    private void Camera_Bounds_Calculate()
    {
        vert_view = Camera.main.orthographicSize;
        horz_view = vert_view * Screen.width / Screen.height;
    }
	
	// Update is called once per frame
	void Update () {
        if (auto_move)
        {
            pos = new Vector3(p_transform.position.x, p_transform.position.y, transform.position.z);
            pos.x = Mathf.Clamp(pos.x, left_bound, right_bound);
            pos.y = Mathf.Clamp(pos.y, lower_bound, upper_bound);
            transform.position = pos;
        }
	}

    
    public void Camera_Move(Vector2 pos,float move_time)
    {

    }

    private IEnumerator Move_Couroutine(Vector3 end,float move_time)
    {
        float remain = (transform.position - end).sqrMagnitude;
        while(remain > float.Epsilon)
        {
            remain = (transform.position - end).sqrMagnitude;
            transform.position = Vector3.MoveTowards(transform.position, end, Time.deltaTime * move_time);
            yield return null;
        }
    }
    public void Camera_Offset_Change(float left, float right, float lower, float upper)
    {
        right_bound = right;
        left_bound = left;
        upper_bound = upper;
        lower_bound = lower;
    }
}

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
    public float move_time = 0.5f;

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
            transform.position = Vector3.MoveTowards(transform.position, end, Time.deltaTime / move_time);
            yield return null;
        }
    }
    public void Camera_Offset_Change(float left, float right, float lower, float upper)
    {
        auto_move = false;
        if(right!=0)
            right_bound = right;
        if(left!=0)
            left_bound = left;
        if(upper!=0)
            upper_bound = upper;
        if(lower!= 0)
            lower_bound = lower;
        Vector3 pos = new Vector3(p_transform.position.x, p_transform.position.y,Camera.main.transform.position.z);
        pos.x = Mathf.Clamp(pos.x, left_bound, right_bound);
        pos.y = Mathf.Clamp(pos.y, lower_bound, upper_bound);
        StartCoroutine(Camera_Move(pos,move_time,true));

    }

    public void Camera_Size_Change(float zoom )
    {
        StartCoroutine(Zoom(zoom,1));
    }

    public void Camera_Rotation_Change(float rot, float move_time)
    {
        StartCoroutine(Rotation(rot,move_time));
    }
    private IEnumerator Zoom(float zoom,float move_time)
    {
        float remain = Mathf.Abs( Camera.main.orthographicSize - zoom);
        while (remain> float.Epsilon)
        {
            remain = Mathf.Abs(Camera.main.orthographicSize - zoom);
            Camera.main.orthographicSize = Mathf.MoveTowards(Camera.main.orthographicSize, zoom, Time.smoothDeltaTime / move_time);
            yield return null; 
        }
    }

    private IEnumerator Rotation(float rot, float move_time)
    {
        float remain = Mathf.Abs(Camera.main.transform.rotation.z - rot);
        float rotation = Camera.main.transform.rotation.z;
        while(remain > float.Epsilon)
        {
            remain = Mathf.Abs(Camera.main.transform.rotation.z - rot);
            rotation = Mathf.MoveTowards(rotation, rot, Time.deltaTime / move_time);
            Camera.main.transform.rotation = Quaternion.Euler(new Vector3(0, 0, rotation));
            yield return null;

        }
        
    }
    private IEnumerator Camera_Move(Vector3 end,float move_time,bool auto)
    {
        float remain = (Camera.main.transform.position - end).sqrMagnitude;
        while (remain > float.Epsilon)
        {
            Vector3 pos = new Vector3(p_transform.position.x, p_transform.position.y, Camera.main.transform.position.z);
            pos.x = Mathf.Clamp(pos.x, left_bound, right_bound);
            pos.y = Mathf.Clamp(pos.y, lower_bound, upper_bound);
            remain  = (Camera.main.transform.position - pos).sqrMagnitude;
            pos = Vector3.MoveTowards(Camera.main.transform.position, pos, Time.smoothDeltaTime * move_time);
            Camera.main.transform.position =pos;
            yield return null;
        }
        
        auto_move = auto;
        
    }
}

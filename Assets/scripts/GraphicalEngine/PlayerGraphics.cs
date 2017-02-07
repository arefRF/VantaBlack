using UnityEngine;
using System.Collections;

public class PlayerGraphics : MonoBehaviour {
    public float move_time = 0.5f;
    private APIGraphic api;
    private LogicalEngine engine;
    private Vector2 unmoved_pos;
    public Vector2 camera_offset = new Vector2(3, 3);
    void Start()
    {
        unmoved_pos = transform.position;
        engine = Starter.GetEngine();
        api = engine.apigraphic;
    }


    public void Lean_Right()
    {
        transform.GetChild(0).localPosition +=  new Vector3(0.2f,0,0);
    }

    public void Lean_Left()
    {
        transform.GetChild(0).localPosition += new Vector3(-0.2f, 0,0);
    }

    public void Lean_Up()
    {
        transform.GetChild(0).localPosition += new Vector3(0, 0.3f,0);
    }

    public void Lean_Down()
    {
        transform.GetChild(0).localPosition +=  new Vector3( 0, -0.3f,0);
    }

    public void Lean_Finished()
    {
        transform.GetChild(0).localPosition = new Vector2(0, 0);
    }
    public void Player_Move(GameObject player,Vector2 end)
    {
        StartCoroutine(Player_Move_Coroutine(end,true));
    }
    
    public void Check_Camera()
    {/*
        if(Mathf.Abs(unmoved_pos.x - transform.position.x)> camera_offset.x)
        {  
            StartCoroutine(Smooth_Move_Camera(new Vector3( Camera_Pos().x, Camera.main.transform.position.y, -15)));
            unmoved_pos = transform.position;
        }
        
        if(Mathf.Abs(unmoved_pos.y - transform.position.y)> camera_offset.y)
        {
            StartCoroutine(Smooth_Move_Camera(new Vector3(Camera.main.transform.position.x, Camera_Pos().y , -15)));
            unmoved_pos = transform.position;
        }*/
    }

    private Vector2 Camera_Pos()
    {
        return (Vector2)Camera.main.transform.position + ((Vector2)transform.position - unmoved_pos);
    }


    private IEnumerator Smooth_Move_Camera(Vector3 end)
    {

        float sqrRemainingDistance = (Camera.main.transform.position - end).sqrMagnitude;

        while (sqrRemainingDistance > float.Epsilon)
        {

            sqrRemainingDistance = (Camera.main.transform.position - end).sqrMagnitude;
            Vector3 newPostion = Vector3.MoveTowards(Camera.main.transform.position, end,  2 * Time.deltaTime);
            Camera.main.transform.position = newPostion;
            yield return null;
        }
 


    }
    private IEnumerator Move_Camera_Coroutine_X(float pos)
    {
        Debug.Log("Move Camera coroutine");
        Debug.Log(pos);
        Camera main_camera = Camera.main;
        float remain = Mathf.Abs(main_camera.transform.position.x - pos);
        Vector3 end = new Vector3(pos, main_camera.transform.position.y, -15);
        while ( remain > float.Epsilon)
        {
            remain = Mathf.Abs(main_camera.transform.position.x - pos);
            main_camera.transform.position = Vector3.MoveTowards(main_camera.transform.position,end ,Time.deltaTime * 1/10);
        }
        yield return null;
    }

    private IEnumerator Move_Camera_Coroutine_Y(float pos)
    {
        Camera main_camera = Camera.main;
        float remain = Mathf.Abs(main_camera.transform.position.y - pos);
        while (remain > float.Epsilon)
        {
            remain = Mathf.Abs(main_camera.transform.position.y - pos);
            main_camera.transform.position = Vector3.MoveTowards(main_camera.transform.position, new Vector2( main_camera.transform.position.x, pos), Time.deltaTime);
        }
        yield return null;
    }
    public void Player_Change_Direction(Player player,Direction dir)
    {
        if (dir == Direction.Right)
            transform.GetChild(0).rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        else if (dir == Direction.Left)
            transform.GetChild(0).rotation = Quaternion.Euler(new Vector3(0, 180, 0));
        api.PlayerChangeDirectionFinished(gameObject.GetComponent<Player>());

    }  

    private void Change_Direction_Finished()
    {
        api.PlayerChangeDirectionFinished(gameObject.GetComponent<Player>());
    }
   
    private IEnumerator Player_Move_Coroutine(Vector2 end,bool call_finish)
    {
        Player player = gameObject.GetComponent<Player>();
        float remain_distance = ((Vector2)transform.position - end).sqrMagnitude;
        while(remain_distance > float.Epsilon)
        {
            remain_distance = ((Vector2)transform.position - end).sqrMagnitude;
            Vector2 new_pos = Vector2.MoveTowards(transform.position, end, Time.deltaTime * 1 / move_time);
            transform.position = new_pos;
            yield return null;
        }
        if (call_finish)
            api.MovePlayerFinished(gameObject);

    }

    private IEnumerator Ramp_Move_Coroutine(Vector2 end, Vector2 end2)
    {
        Player player = gameObject.GetComponent<Player>();
        Debug.Log("Ramp Co");
        float remain_distance = ((Vector2)transform.position - end).sqrMagnitude;
        while (remain_distance > float.Epsilon)
        {
            remain_distance = ((Vector2)transform.position - end).sqrMagnitude;
            Vector2 new_pos = Vector2.MoveTowards(transform.position, end, Time.deltaTime * 1 / move_time);
            transform.position = new_pos;
            yield return null;
        }
        Player_Move(gameObject,end2);
    }

}

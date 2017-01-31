using UnityEngine;
using System.Collections;

public class CameraMove : MonoBehaviour {
    public float x;
    public float y;
    private static int moving;
    private static bool is_moving;
    public float moveTime = 0.1f;
    private float inverseMoveTime;
    public float zoom;
    public float zoomTime;
    private float inverseZoomTime;
    void Start()
    {
        inverseZoomTime = 1f / zoomTime;
        inverseMoveTime = 1f / moveTime;
        moving = 0;
        if (zoom == 0)
        {
            zoom = Camera.main.orthographicSize;
        }
        if(x ==0 && y == 0)
        {
            x = Camera.main.transform.position.x;
            y = Camera.main.transform.position.y;
        }
    }
    private void _Set_camera()
    {
        Camera.main.transform.position = new Vector3(x, y, -10);
    }

    public void OnTriggerEnter2D(Collider2D col)
    {

        if (col.tag == "Player")
        {
            if (!is_moving)
            {
                    //Starter.GetEngine().camerapos = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, Camera.main.transform.position.z);
                    //Starter.GetEngine().camerasize = Camera.main.orthographicSize;
                    is_moving = true;
                    StartCoroutine(Smooth_Move(new Vector3(x, y, -10)));
                    StartCoroutine(Smooth_Zoom(zoom));
            }

        }
    }



    protected IEnumerator Smooth_Zoom(float zoom)
    {
        float sqrRemainingDistance = Mathf.Abs( Camera.main.orthographicSize - zoom);
        while(sqrRemainingDistance > 0.2)
        {
            sqrRemainingDistance = Mathf.Abs(Camera.main.orthographicSize - zoom);
            float new_size = Mathf.MoveTowards(Camera.main.orthographicSize,zoom,Time.deltaTime * inverseZoomTime);
            Camera.main.orthographicSize = new_size;
            yield return null;
        }    
    }

    protected IEnumerator Smooth_Move(Vector3 end)
    {
        
        float sqrRemainingDistance = (Camera.main.transform.position - end).sqrMagnitude;

        while (sqrRemainingDistance > float.Epsilon)
        {
            sqrRemainingDistance = (Camera.main.transform.position - end).sqrMagnitude;
            Vector3 newPostion = Vector3.MoveTowards(Camera.main.transform.position, end, inverseMoveTime * Time.deltaTime);


            Camera.main.transform.position =newPostion;


            yield return null;
        }
        is_moving = false;


}







}

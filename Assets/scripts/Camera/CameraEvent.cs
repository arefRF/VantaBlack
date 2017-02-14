using UnityEngine;
using System.Collections;

public class CameraEvent : MonoBehaviour {
    public float left, right, lower, upper;
    public float zoom;
    public float rotation = 360;
    public float rotation_move_time = 0.5f;
    void OnTriggerEnter2D(Collider2D col)
    {
        Camera.main.GetComponent<CameraController>().Camera_Offset_Change(left, right, lower, upper);
        if (zoom != 0)
            Camera.main.GetComponent<CameraController>().Camera_Size_Change(zoom);
        if (rotation != 360)
            Camera.main.GetComponent<CameraController>().Camera_Rotation_Change(rotation,rotation_move_time);
            
    }



   

    
}

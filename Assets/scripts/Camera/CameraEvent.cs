using UnityEngine;
using System.Collections;

public class CameraEvent : MonoBehaviour {
    public float left, right, lower, upper;
    public float zoom, zoomtime;
    public float rotation = 360;
    public float rotation_move_time = 0.5f;
    public float move_time = 0;
    public bool manual = false;
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            Camera.main.GetComponent<SmoothCamera>().ChangeOffset(left, right, lower, upper,manual);
            if (zoom != 0)
                Camera.main.GetComponent<CameraController>().Camera_Size_Change(zoom, zoomtime);
            if (rotation != 360)
                Camera.main.GetComponent<CameraController>().Camera_Rotation_Change(rotation, rotation_move_time);
            if (move_time != 0)
                Camera.main.GetComponent<CameraController>().move_time = move_time;

        }  
    }


}

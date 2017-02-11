using UnityEngine;
using System.Collections;

public class CameraEvent : MonoBehaviour {
    public float left, right, lower, upper;
    void OnTriggerEnter2D(Collider2D col)
    {
        Camera.main.GetComponent<CameraController>().Camera_Offset_Change(left, right, lower, upper);
            
    }

   

    
}

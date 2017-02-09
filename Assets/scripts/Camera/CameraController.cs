using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
    public Player player;
    public Vector2 x_limit;
    public Vector2 y_limit;
    private Transform p_transform;


	// Use this for initialization
	void Start () {
        p_transform = player.transform;
	}
	
	// Update is called once per frame
	void Update () {
        
	}
}

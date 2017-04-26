using UnityEngine;
using System.Collections;

public class UnityPhysics : MonoBehaviour {
    private Rigidbody2D rb;
    private float hoverHeight = 0.5f;
    private float hoverForce = 50;
    private float powerInput;
    private float speed = 50;
    private Player player;
    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody2D>();
        player = GetComponent<Player>();
    }
	
	// Update is called once per frame
	void Update () {
        
    }


    void FixedUpdate()
    {
        if(player.mode == GameMode.Real)
        {

        }
    }

    void OnCollisionStay2D(Collision2D col)
    {

    }
    void OnCollisionExit2D(Collision2D col)
    {
        
    }
}

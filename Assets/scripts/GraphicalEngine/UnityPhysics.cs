using UnityEngine;
using System.Collections;

public class UnityPhysics : MonoBehaviour {
    private Rigidbody2D rb;
    private float hoverHeight =1;
    private float hoverForce = 50;
    private float powerInput;
    private float speed =50;
    private Player player;
    private Transform player_transform;
    public float move_time = 0.5f;
    private float remain = 0 ;
    private Coroutine co;
    private Direction move_dir = Direction.Up;
    public PhysicState state = PhysicState.Idle;
    private Vector2 adj_pos;
    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody2D>();
        player = GetComponent<Player>();
        player_transform = transform;
    }

    // Update is called once per frame
    void Update() {
        if (player.mode == GameMode.Real && player.state == PlayerState.Idle)
        {
            powerInput = Input.GetAxis("Horizontal");
            if (powerInput > 0)
                move_dir = Direction.Right;
            else if (powerInput < 0)
                move_dir = Direction.Left;
        }

            if (Mathf.Abs(rb.velocity.x) < 3)
                {
                    rb.AddRelativeForce(new Vector2(powerInput * speed, 0));
                }
        
    }


    void FixedUpdate()
    {
        if(player.mode == GameMode.Real)
        {
            if(state != PhysicState.Falling)
            {
                RaycastHit2D hit = Physics2D.Raycast(transform.position + new Vector3(0, 0.5f, 0), -transform.up);
                if (hit)
                {
                    float proportionalHeight = (hoverHeight - hit.distance) / hoverHeight;
                    Vector2 force = hit.normal;
                    Vector3 appliedHoverForce = force * proportionalHeight * hoverForce;
                    rb.AddForce(appliedHoverForce, ForceMode2D.Force);
                }
            }

        }
    }

    public void Move(Vector2 pos)
    {
       co = StartCoroutine(Move(pos, move_time));
    }

    private IEnumerator Move(Vector2 end, float move_time)
    {
        state = PhysicState.Falling;
        float remain_distance = Mathf.Abs( transform.position.x - end.x);
        
        while (remain_distance > float.Epsilon)
        {
            remain_distance = Mathf.Abs(transform.position.x - end.x);
            transform.position = new Vector2(Mathf.MoveTowards(transform.position.x, end.x, Time.smoothDeltaTime / move_time),transform.position.y);
            yield return new WaitForSeconds(0.001f);
        }
        state = PhysicState.Falling;
        rb.gravityScale = 10;
        player.api.engine.inputcontroller.RealModePlayerTransitionMoveDone(player);
        
        // if it needs Call Finished Move of API
    }
    void OnCollisionStay2D()
    {
        if(state == PhysicState.Falling)
        {
            rb.gravityScale = 2;
            state = PhysicState.Idle;
        }
    }
}


public enum PhysicState
{
    Adjust,Idle,JumpMove,Falling
}

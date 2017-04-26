using UnityEngine;
using System.Collections;

public class UnityPhysics : MonoBehaviour {
    private Rigidbody2D rb;
    private float hoverHeight =1;
    private float hoverForce = 50;
    private float powerInput;
    private float speed = 50;
    private Player player;
    private Transform player_transform;
    public float move_time = 0.5f;
    private float remain = 0 ;
    private Coroutine co;
    private Direction move_dir = Direction.Up;
    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody2D>();
        player = GetComponent<Player>();
        player_transform = transform;
    }
	
	// Update is called once per frame
	void Update () {
        if (player.mode == GameMode.Real)
        {
            powerInput = Input.GetAxis("Horizontal");
            if (powerInput > 0)
                move_dir = Direction.Right;
            else if (powerInput < 0)
                move_dir = Direction.Left;
            if (powerInput == 0)
            {
                Vector2 pos = (Vector2)transform.position;
                if (move_dir == Direction.Right)
                    pos = new Vector2(Mathf.Ceil(transform.position.x), transform.position.y);
                else if (move_dir == Direction.Left)
                    pos = new Vector2(Mathf.Floor(transform.position.x), transform.position.y);

                if ((Vector2)transform.position != pos)
                {
                    rb.AddRelativeForce(new Vector2((Mathf.Abs(pos.x) / pos.x * speed, 0));
                }
                move_dir = Direction.Up;
                if (transform.position.x - pos.x < 0.1f)
                {
                    transform.position = new Vector2(pos.x, transform.position.y);
                    rb.velocity = new Vector2(0, 0);
                }
            }

            if (rb.velocity.x < 3)
                rb.AddRelativeForce(new Vector2(powerInput * speed, 0));

            


        }
    }


    void FixedUpdate()
    {
        if(player.mode == GameMode.Real)
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

    public void Move(Direction dir)
    {
      /*  if (remain == 0)
        {
            if (co != null)
                StopCoroutine(co);
            Vector2 end = Toolkit.VectorSum(player.transform.position, dir);
            co = StartCoroutine(Constant_Move(end, move_time));
        }*/

    }

    private IEnumerator Constant_Move(Vector2 end, float move_time)
    {
        float remain_distance = Mathf.Abs( transform.position.x - end.x);
        
        while (remain_distance > float.Epsilon)
        {
            remain_distance = Mathf.Abs(transform.position.x - end.x);
            remain = remain_distance;
            transform.position = new Vector2(Mathf.MoveTowards(transform.position.x, end.x, Time.smoothDeltaTime / move_time),transform.position.y);
            yield return new WaitForSeconds(0.001f);
        }

        remain = 0;
        // if it needs Call Finished Move of API
    }

}

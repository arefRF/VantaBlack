using UnityEngine;
using System.Collections;

public class UnityPhysics : MonoBehaviour {
    private Rigidbody2D rb;
    private float hoverHeight = 0.5f;
    private float hoverForce = 50;
    private float powerInput;
    private float speed = 50;
    private Player player;
    private Transform player_transform;
    public float move_time = 0.5f;
    private float remain;
    private Coroutine co;
    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody2D>();
        player = GetComponent<Player>();
        player_transform = transform;
    }
	
	// Update is called once per frame
	void Update () {
        
    }


    void FixedUpdate()
    {
        if(player.mode == GameMode.Real)
        {
            RaycastHit2D ray = Physics2D.Raycast(transform.position + new Vector3(0, 0.5f, 0), -transform.up);
            if (ray)
            {
                if (ray.distance > 1)
                    transform.position = transform.position - new Vector3(0, 0.1f, 0);   
            }
        }
    }

    public void Move(Direction dir)
    {
        if (remain == 0)
        {
            if (co != null)
                StopCoroutine(co);
            Vector2 end = Toolkit.VectorSum(player.transform.position, dir);
            co = StartCoroutine(Constant_Move(end, move_time));
        }

    }

    private IEnumerator Constant_Move(Vector2 end, float move_time)
    {

        float remain_distance = ((Vector2)player_transform.position - end).sqrMagnitude;
        remain = remain_distance;
        while (remain_distance > float.Epsilon)
        {
            remain_distance = ((Vector2)player_transform.position - end).sqrMagnitude;
            remain = remain_distance;
            transform.position = Vector2.MoveTowards(transform.position, end, Time.smoothDeltaTime / move_time);
            yield return new WaitForSeconds(0.001f);
        }
        remain = 0;
        // if it needs Call Finished Move of API
    }

}

using UnityEngine;
using System.Collections;

public class PlayerGraphics : MonoBehaviour {
    public float move_time = 0.5f;
    private Animator animation;
    private APIGraphic api;
    private LogicalEngine engine;
    void Start()
    {
        engine = Starter.GetEngine();
        animation = GetComponent<Animator>();
        api = engine.apigraphic;
    }


    public void Lean_Right()
    {
        gameObject.transform.position = (Vector2)gameObject.transform.position + new Vector2(0.2f,0);
    }

    public void Lean_Left()
    {
        gameObject.transform.position = (Vector2)gameObject.transform.position + new Vector2(-0.2f, 0);
    }

    public void Lean_Up()
    {
        gameObject.transform.position = (Vector2)gameObject.transform.position + new Vector2(0, 0.2f);
    }

    public void Lean_Down()
    {
        gameObject.transform.position = (Vector2)gameObject.transform.position + new Vector2( 0, -0.2f);
    }

    public void Lean_Down_Finished()
    {
        gameObject.transform.position = (Vector2)gameObject.transform.position + new Vector2(0, 0.2f);
    }

    public void Lean_Up_Finished()
    {
        gameObject.transform.position = (Vector2)gameObject.transform.position + new Vector2(0, -0.2f);
    }
    public void Lean_Left_Finished()
    {
        // animation.SetInteger("State", 0);
        // gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
        gameObject.transform.position = (Vector2)gameObject.transform.position + new Vector2(0.2f, 0);
    }

    public void Lean_Right_Finished()
    {
       // animation.SetInteger("State", 0);
        gameObject.transform.position = (Vector2)gameObject.transform.position + new Vector2(-0.2f, 0);
    }
    public void Player_Move(GameObject player,Vector2 end)
    {
        StartCoroutine(Player_Move_Coroutine(end,true));
    }

    public void Player_Change_Direction(Player player,Direction dir)
    {

        if (dir == Direction.Right)
            animation.SetInteger("State", 1);
        else if (dir == Direction.Left)
            animation.SetInteger("State", -1);
    }
    
    public void Ramp_To_Block_Move()
    {

    }

    public void Ramp_To_Fall()
    {

    }

    public void Ramp_To_Corner(Vector2 position)
    {
        float x1 = position.x - transform.position.x;
        position += new Vector2(-x1 / 2, 0.4f);
        Player_Move(gameObject,position);
    }

    public void Ramp_To_Sharp(Vector2 position,int ramptype)
    {
        Debug.Log("Ramp Type" + ramptype);
        int x = 1, y = 1;
        if (ramptype == 3 || ramptype == 4)

            x = -1;
        if (ramptype == 2 || ramptype == 3)
            y = -1;

        Vector2 end = position + new Vector2(x * 0.4f, y * 0.4f);
        float x1 = position.x - transform.position.x;
        position += new Vector2(-x1/2, 1);
        Debug.Log(position);
        StartCoroutine(Ramp_Move_Coroutine(position,end));
        
    }

    public void Branch_To_Block_Move()
    {

    }

    public void Branch_To_Fall()
    {

    }

    public void Branch_To_Ramp_Move()
    {
        
    }



    private void Change_Direction_Finished()
    {
        animation.SetInteger("State", 0);
        api.PlayerChangeDirectionFinished(gameObject.GetComponent<Player>());
    }

    public void Player_Roll(GameObject player,Direction dir,int number)
    {

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

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
        transform.GetChild(0).localPosition =  new Vector2(0.2f,0);
    }

    public void Lean_Left()
    {
        transform.GetChild(0).localPosition = new Vector2(-0.2f, 0);
    }

    public void Lean_Up()
    {
        transform.GetChild(0).localPosition = new Vector2(0, 0.3f);
    }

    public void Lean_Down()
    {
        transform.GetChild(0).localPosition =  new Vector2( 0, -0.3f);
    }

    public void Lean_Finished()
    {
        transform.GetChild(0).localPosition = new Vector2(0, 0);
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

    private void Change_Direction_Finished()
    {
        animation.SetInteger("State", 0);
        api.PlayerChangeDirectionFinished(gameObject.GetComponent<Player>());
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

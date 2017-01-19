using UnityEngine;
using System.Collections;

public class PlayerGraphics : MonoBehaviour {
    public float move_time = 1;
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
        StartCoroutine(Player_Move_Coroutine(player,end));
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

    public void Player_Roll(GameObject player,Direction dir,int number)
    {

    }

    private IEnumerator Player_Move_Coroutine(GameObject obj , Vector2 end)
    {
        float remain_distance = ((Vector2)obj.transform.position - end).sqrMagnitude;
        while(remain_distance > float.Epsilon)
        {
            remain_distance = ((Vector2)obj.transform.position - end).sqrMagnitude;
            Vector3 new_pos = Vector3.MoveTowards(obj.transform.position, end, Time.deltaTime * 1 / move_time);
            obj.transform.position = new_pos;
            yield return null;
        }
        api.MovePlayerFinished(obj);
        
        
    }

}

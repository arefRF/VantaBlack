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
        StartCoroutine(Player_Move_Coroutine(end,true,0));
    }

    public void Player_Change_Direction(Player player,Direction dir)
    {

        if (dir == Direction.Right)
            animation.SetInteger("State", 1);
        else if (dir == Direction.Left)
            animation.SetInteger("State", -1);
    }

    public void Ramp_To_Ramp_Move(Vector2 position)
    {
        position += new Vector2(0.5f,0.5f);
        StartCoroutine(Player_Move_Coroutine(position,true,0));
    }

    public void Block_To_Ramp_Move(Vector2 position,Direction dir,int type)
    {
        Vector2 plus = Toolkit.DirectiontoVector(dir);
        plus = new Vector2(0.5f * plus.x, 0.5f * plus.y);
        position += plus;
        StartCoroutine(Player_Move_Coroutine(position, false, type));
    }

    private void Block_To_Ramp_Move_Part2(Vector2 position,int type)
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

    private IEnumerator Player_Move_Coroutine(Vector2 end,bool call_finish, int type)
    {
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
        else
            Block_To_Ramp_Move_Part2(end,type);

    }

}

using UnityEngine;
using System.Collections;

public class PlayerGraphics : MonoBehaviour {
    public float move_time = 1;
    public void Player_Move(GameObject player,Direction dir)
    {
        Vector2 end = (Vector2)player.transform.position + (Vector2)Toolkit.DirectiontoVector(dir);
        StartCoroutine(Player_Move_Coroutine(player,end));
    }

    public void Player_Change_Direction(GameObject player,Direction dir)
    {
        if(dir==Direction.Right)
            player.transform.rotation = Quaternion.Euler(new Vector3(0,0,0));
        else if(dir== Direction.Left)
            player.transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
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

        // Move Completed
        
    }

}

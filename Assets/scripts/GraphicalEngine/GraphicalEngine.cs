using UnityEngine;
using System.Collections;
using System.Threading;
using System.Runtime.CompilerServices;
public class GraphicalEngine : MonoBehaviour {

    public Database database { get; set; }
    private UI ui;
    private bool fall = false;
    private int fall_pos;
    private float gravity = 9.8f;
    private float velocity = 5;
    private float fall_distance;
    private Vector2 player_pos;
    private float lean_offset = 0.2f;
    private float top_rotate;
    private PlayerGraphics pl_graphics;
    private APIGraphic api;
    private LogicalEngine engine;
    private float lean_move = 0.2f;
    void Start()
    {
        engine = Starter.GetEngine();
        database = Starter.GetDataBase();
        top_rotate = 75;
        api = engine.apigraphic;
       
    }


    public void Move_Object(GameObject obj,Unit unit, Vector2 pos)
    {
        StartCoroutine(Move_Object_Coroutine(obj,unit,pos));
    }

    private IEnumerator Move_Object_Coroutine(GameObject obj, Unit unit,Vector2 end)
    {
        float remain_distance = ((Vector2)obj.transform.position - end).sqrMagnitude;
        float move_time = 1f;
        while (remain_distance > float.Epsilon)
        {
            remain_distance = ((Vector2)obj.transform.position - end).sqrMagnitude;
            Vector3 new_pos = Vector3.MoveTowards(obj.transform.position, end, Time.deltaTime * 1 / move_time);
            obj.transform.position = new_pos;
            yield return null;
        }
        api.MoveGameObjectFinished(obj,unit);
    }
	// Use this for initialization
    /*public void Refresh()
    {
        for(int i=0; i<database.units.GetLength(0); i++)
        {
            for(int j=0; j < database.units.GetLength(1); j++)
            {
                for(int k=0; k<database.units[i,j].Count; k++)
                {
                    Refresh(database.units[i, j][k]);
                }
            }
        }
    }*/

    /*public void Refresh(Unit u)
    {
        u.obj.transform.position = u.position;
        switch (u.unitType)
        {
            case UnitType.Player: _Player_Change_Ability(); _Player_Change_Direction(); break;
            case UnitType.Container: _Container_Change_Sprite((Container)u); break;
        }
    }*/

    /*public void _blink(Direction dir)
    {
        switch (dir)
        {
            case Direction.Down: database.player.transform.position = Toolkit.VectorSum(database.player.transform.position, new Vector2(0, -2)); break;
            case Direction.Up: database.player.transform.position = Toolkit.VectorSum(database.player.transform.position, new Vector2(0, 2)); break;
            case Direction.Right: database.player.transform.position = Toolkit.VectorSum(database.player.transform.position, new Vector2(2, 0)); break;
            case Direction.Left: database.player.transform.position = Toolkit.VectorSum(database.player.transform.position, new Vector2(-2, 0)); break;
        }
    }
    */
    [MethodImpl(MethodImplOptions.Synchronized)]
    public  void _move(Player player, Vector2 position)
    {
        player.gameObject.GetComponent<PlayerGraphics>().Player_Move(player.gameObject,position);
    }

    /*
    [MethodImpl(MethodImplOptions.Synchronized)]
    public static void MoveObject(GameObject obj, Vector2 position)
    {
        obj.transform.position = position;

    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public bool _Move_Object(GameObject obj, Vector2 pos)
    {
        obj.transform.position = pos;
        return true;
    }

    public void _gravity()
    {
        if (fall)
        {
            Vector2 cal = Time.deltaTime * Toolkit.DirectiontoVector(database.gravity_direction) * velocity ;
            fall_distance += Time.deltaTime * velocity;
            velocity += Time.deltaTime * gravity;
            database.player.transform.position = Toolkit.VectorSum(database.player.transform.position, cal);
            if (fall_distance > fall_pos)
            {
                fall = false;
                database.player.transform.position = Toolkit.VectorSum(player_pos, Toolkit.DirectiontoVector(database.gravity_direction) * fall_pos);
            }
        }
    }

    public void _Player_Blink(Vector2 position)
    {
        player.transform.position = position;
    }

    public void _fall_activate(int pos)
    {
        fall_distance = 0;
        fall = true;
        fall_pos = pos;
        player_pos = database.player.transform.position;
    }


    public void _Smooth_Move(GameObject obj,Direction d)
    {
        Debug.Log("start move");
        Vector2 end = (Vector2)obj.transform.position + Toolkit.DirectiontoVector(d);
        Debug.Log(end);
        StartCoroutine(Smooth_Move(obj,end));
       
    }   

    public void _Smooth_Move(GameObject obj,Vector2 end_position)
    {     
        StartCoroutine(Smooth_Move(obj, end_position));   
    }

    protected IEnumerator Smooth_Move(GameObject obj,Vector2 end)
    {
        float remain_distance = ((Vector2)obj.transform.position - end).sqrMagnitude;
        float move_time = 1f;
        while (remain_distance > float.Epsilon)
        {
            remain_distance = ((Vector2)obj.transform.position - end).sqrMagnitude;
            Vector3 new_pos = Vector3.MoveTowards(obj.transform.position, end, Time.deltaTime * 1 / move_time);
            obj.transform.position = new_pos;
            yield return null;
        }
        //Starter.GetEngine().DoneMoving();
    } 

    public void _Player_Change_Ability()
    {
        string path = @"player\";
        if (player.ability == null)
        {
            path += "player";
        }
        else
            switch (player.ability.abilitytype)
            {
                case AbilityType.Fuel: path += "player-green"; break;
                case AbilityType.Direction: path += "player-red"; break;
            }      
        player.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = (Sprite)Resources.Load(path, typeof(Sprite));
    }


    

    public void _Player_Change_Direction()
    {
        float rot = 0;
        switch (player.move_direction[0])
        {
            case Direction.Left: rot = 180; break;
            case Direction.Right: rot = 0; break;
        }
        player.transform.GetChild(0).rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        player.transform.GetChild(0).rotation = Quaternion.Euler(new Vector3(0, rot, 0));
        

    }

    */
}

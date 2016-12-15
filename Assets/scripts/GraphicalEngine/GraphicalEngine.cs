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
    public Player player { get; set; }

    private float lean_move = 0.2f;
    void Start()
    {

        database = Starter.GetDataBase();
        player = database.player.GetComponent<Player>();
        top_rotate = 75;

        //ui = GameObject.Find("Canvas").GetComponent<UI>();
    }
	// Use this for initialization


    public void Refresh()
    {
        for(int i=0; i<database.units.GetLength(0); i++)
        {
            for(int j=0; j < database.units.GetLength(1); j++)
            {
                for(int k=0; k<database.units[i,j].Count; k++)
                {
                    if (database.units[i, j][k].unitType == UnitType.Wall && (((Wall)database.units[i, j][k]).direction == Direction.Left || ((Wall)database.units[i, j][k]).direction == Direction.Down))
                    {
                        continue;
                    }
                    Refresh(database.units[i, j][k]);
                }
            }
        }
    }

    public void Refresh(Unit u)
    {
        if (u.unitType == UnitType.Wall && (((Wall)u).direction == Direction.Left || ((Wall)u).direction == Direction.Down))
        {
            Wall.print("djncskjdvidfbvifbvidpfbvdifbvidfbvdifbvdfijvdofn bdfonbdofgnbfgonbfognbfgjnbfgijnbfignbifjnbijnbjnbnbnbonbw[oufrb[gbefr['");
            return;
        }
        u.obj.transform.position = u.position;
        switch (u.unitType)
        {
            case UnitType.Player: _Player_Change_Ability(); _Player_Change_Direction(); break;
            case UnitType.Container: _Container_Change_Sprite((Container)u); break;
            case UnitType.Block: _Block_Change_Sprite((Block)u); break;
            case UnitType.Switch: _Switch_Change_Sprite((Switch)u); break;
            case UnitType.Door: Door_Change_Sprite((Door)u); break;
        }
    }

    public void _blink(Direction dir)
    {
        switch (dir)
        {
            case Direction.Down: database.player.transform.position = Toolkit.VectorSum(database.player.transform.position, new Vector2(0, -2)); break;
            case Direction.Up: database.player.transform.position = Toolkit.VectorSum(database.player.transform.position, new Vector2(0, 2)); break;
            case Direction.Right: database.player.transform.position = Toolkit.VectorSum(database.player.transform.position, new Vector2(2, 0)); break;
            case Direction.Left: database.player.transform.position = Toolkit.VectorSum(database.player.transform.position, new Vector2(-2, 0)); break;
        }
    }

    public void _jump(int height)
    {
        switch (database.gravity_direction)
        {
            case Direction.Down: database.player.transform.position = Toolkit.VectorSum(database.player.transform.position, new Vector2(0, height)); break;
            case Direction.Up: database.player.transform.position = Toolkit.VectorSum(database.player.transform.position, new Vector2(0, -height)); break;
            case Direction.Right: database.player.transform.position = Toolkit.VectorSum(database.player.transform.position, new Vector2(-height, 0)); break;
            case Direction.Left: database.player.transform.position = Toolkit.VectorSum(database.player.transform.position, new Vector2(2, height)); break;
        }
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public  void _move(Direction dir)
    {
        
        switch (dir)
        {
            case Direction.Down: database.player.transform.position = Toolkit.VectorSum(database.player.transform.position, new Vector2(0, -1)); break;
            case Direction.Up: database.player.transform.position = Toolkit.VectorSum(database.player.transform.position, new Vector2(0, 1)); break;
            case Direction.Right: database.player.transform.position = Toolkit.VectorSum(database.player.transform.position, new Vector2(1, 0)); break;
            case Direction.Left: database.player.transform.position = Toolkit.VectorSum(database.player.transform.position, new Vector2(-1, 0)); break;
        }
    }

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

    public void _lean_right()
    {

        player.transform.GetChild(0).transform.position = new Vector3(lean_move, 0, 0) + player.transform.GetChild(0).transform.position;
    }

    public void _lean_right_undo()
    {
        player.transform.GetChild(0).transform.position = new Vector3(-lean_move, 0, 0) + player.transform.GetChild(0).transform.position;
    }

    public void _lean_top()
    {
        player.transform.GetChild(0).rotation = Quaternion.Euler(new Vector3(0,0,90));
    }

    public void _lean_top_undo()
    {
        if(player.move_direction[0]==Direction.Right)
            player.transform.GetChild(0).rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        else
            player.transform.GetChild(0).rotation = Quaternion.Euler(new Vector3(0, 180, 0));
    }
    public void _lean_left()
    {
        player.transform.GetChild(0).transform.position = new Vector3(-lean_move, 0, 0) + player.transform.GetChild(0).transform.position;

    }

    public void _lean_left_undo()
    {
        player.transform.GetChild(0).transform.position = new Vector3(lean_move, 0, 0) + player.transform.GetChild(0).transform.position;
    }

    public void _right_light()
    {
        //ui._right_light();
    }

    public void _gravity()
    {
        if (fall)
        {
            Vector2 cal = Time.deltaTime * _direction_to_vector(database.gravity_direction) * velocity ;
            fall_distance += Time.deltaTime * velocity;
            velocity += Time.deltaTime * gravity;
            database.player.transform.position = Toolkit.VectorSum(database.player.transform.position, cal);
            if (fall_distance > fall_pos)
            {
                fall = false;
                database.player.transform.position = Toolkit.VectorSum(player_pos, _direction_to_vector(database.gravity_direction) * fall_pos);
            }
        }
    }

    
    private Vector2 _direction_to_vector(Direction dir)
    {
        if (dir == Direction.Down)
            return new Vector2(0, -1);
        else if (dir == Direction.Left)
            return new Vector2(-1, 0);
        else if (dir == Direction.Right)
            return new Vector2(1, 0);
        else if (dir == Direction.Down)
            return new Vector2(0, 1);
        return new Vector2(0, 0);
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
        Starter.GetEngine().DoneMoving();
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

    public void _Container_Change_Sprite(Container container)
    {
        
        string path = @"blocks\";
        if (container.ability == null)
        {
            path += "white-light-middle";
        }
        
        else
        switch (container.ability.abilitytype)
        {
            case AbilityType.Fuel: path += "green-light-middle"; break;
            case AbilityType.Direction: path += "red-light-middle"; break;
        }
        container.obj.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = (Sprite)Resources.Load(path, typeof(Sprite));
    }

    public void _Block_Change_Sprite(Block block)
    {
        string path = @"blocks\";
        if (block.ability == null)
        {
            path += "block-empty";
        }
        
        else
        switch (block.ability.abilitytype)
        {
            case AbilityType.Fuel: path += "block-green"; break;
            case AbilityType.Direction: path += "block-red"; break;
        }

        block.obj.GetComponent<SpriteRenderer>().sprite = (Sprite)Resources.Load(path,typeof(Sprite));
    }

    public void _Switch_Change_Sprite(Switch lamp)
    {
                
        string path = @"switches\";
        if (lamp.isAutomatic)
            path += "switch-auto";
        else
        {
            if (lamp.disabled)
            {
                path += "switch-gray";
            }
            else if (lamp.isOn)
                path += "switch-green";
            else
                path += "switch-red";
        }
        try
        {
            lamp.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = (Sprite)Resources.Load(path, typeof(Sprite));
        }
        catch { };
        
   
        //Debug.Log(path);
    }



    public void _Internal_Door_Change_Sprite(Door door)
    {
        string path = @"doors\";
        if (door.isOpen)
            path += "open-door";
        else
            path += "close-door";
        door.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = (Sprite)Resources.Load(path, typeof(Sprite));
    }

    private void Door_Change_Sprite(Door door)
    {
        _Internal_Door_Change_Sprite(door);
    } 
}

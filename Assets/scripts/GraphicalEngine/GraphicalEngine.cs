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

    public void Simple_Container(SimpleContainer container)
    {
        string lights = @"Containers\";
        switch (container.abilities.Count)
        {
            case 1: lights += "Lights 1"; break;
            case 2: lights += "Lights 2"; break;
            case 3: lights += "Lights 3"; break;
            case 4: lights += "Lights 4"; break;
            default: lights += "Lights Infinite"; break;
        }
        container.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = (Sprite)Resources.Load(lights, typeof(Sprite));
    }

  
    public void Dynamic_Container(DynamicContainer container)
    {
        int rot = 0;
        switch (container.direction)
        {
            case Direction.Right: rot = 0; break;
            case Direction.Left: rot = 180; break;
            case Direction.Up: rot = 90; break;
            case Direction.Down: rot = 270; break;
        }
        container.transform.GetChild(0).rotation = Quaternion.Euler(new Vector3(0,0,rot));
    }

    
}

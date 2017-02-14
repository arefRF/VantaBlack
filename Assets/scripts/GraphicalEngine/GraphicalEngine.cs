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
    private PlayerGraphics pl_graphics;
    private APIGraphic api;
    private LogicalEngine engine;
    private float lean_move = 0.2f;
    private bool finish_lock;
    private Coroutine object_co;
    void Start()
    {
        engine = Starter.GetEngine();
        database = Starter.GetDataBase();
        api = engine.apigraphic;
       
    }
    public void Move_Object(GameObject obj,Unit unit, Vector2 pos)
    {
        finish_lock = true;
        if(object_co != null)
            StopCoroutine(object_co);
       object_co =  StartCoroutine(Move_Object_Coroutine(obj,unit,pos));
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
            if (remain_distance < 0.01 && finish_lock)
            {
                finish_lock = false;
                
                api.MoveGameObjectFinished(obj,unit);
            }

            yield return null;
        }
    }

    public void Simple_Container(SimpleContainer container)
    {
        if (container.abilities.Count != 0)
        {
            if (container.abilities[0] == AbilityType.Key)
            {
                container.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = (Sprite)Resources.Load("Doors\\Key", typeof(Sprite));
            }
        }
        else
            container.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = (Sprite)Resources.Load("", typeof(Sprite));
        Container_Change_Number(container);
    }

    public void Container_Change_Number(Container container)
    {

        string lights = @"Containers\Numbers\";
        switch (container.abilities.Count)
        {
            case 0: lights += ""; break;
            case 1: lights += "Lights 1"; break;
            case 2: lights += "Lights 2"; break;
            case 3: lights += "Lights 3"; break;
            case 4: lights += "Lights 4"; break;
            default: lights += "Lights Infinite"; break;
        }
        if ( container.abilities.Count != 0 && container.abilities[0] == AbilityType.Key)
            lights = @"Containers\Lights Infinite";
        container.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = (Sprite)Resources.Load(lights, typeof(Sprite));
    }

    public void Gate(Gate gate)
    {
        gate.GetComponent<SpriteRenderer>().sprite = (Sprite)Resources.Load("Doors\\Door 2", typeof(Sprite));
    }
    
    public void Stop_All_Co()
    {
        StopAllCoroutines();
    }
    public void Dynamic_Container(DynamicContainer container)
    {

        // On or Off Sprite
        string toggle = @"Containers\Icons\";
        if (container.on)
        {
            toggle += "ABILITY FUEL ON";
            container.transform.GetChild(3).gameObject.SetActive(true);
        }
        else
        {
            toggle += "ABILITY FUEL OFF";
            container.transform.GetChild(3).gameObject.SetActive(false);
        }
        container.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = (Sprite)Resources.Load(toggle, typeof(Sprite));

        //Change Number of 
        Container_Change_Number(container);


        // Rotation for Abilities with Direction
          
                int rot = 0;
                switch (container.direction)
                {
                    case Direction.Right: rot = 0; break;
                    case Direction.Left: rot = 180; break;
                    case Direction.Up: rot = 90; break;
                    case Direction.Down: rot = 270; break;
                }
                container.transform.GetChild(0).rotation = Quaternion.Euler(new Vector3(0, 0, rot));
            if( container.abilities.Count!= 0 &&  container.abilities[0] == AbilityType.Key)
                container.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = (Sprite)Resources.Load("Doors\\Key", typeof(Sprite));
            

    }

    
}

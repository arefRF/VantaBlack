using UnityEngine;
using System.Collections;
using System.Threading;
using System.Runtime.CompilerServices;
using System.Collections.Generic;

public class GraphicalEngine : MonoBehaviour {

    public Database database { get; set; }
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
    private string[] simple_objects_off = new string[] { "Direction","Glass"};
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
        for(int i = 0; i < container.transform.childCount; i++)
        {
            GameObject obj = container.transform.GetChild(i).gameObject;
            bool off = System.Array.Exists(simple_objects_off,delegate (string s) { return s == obj.name; });
            if (off)
                obj.SetActive(false);
        }
        /*
        if (container.abilities.Count != 0)
        {
            if (container.abilities[0].abilitytype == AbilityType.Key)
            {
                container.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = (Sprite)Resources.Load("Doors\\Key", typeof(Sprite));
            }
            else
                container.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = (Sprite)Resources.Load("", typeof(Sprite));

        }
        else
            container.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = (Sprite)Resources.Load("", typeof(Sprite));
        Container_Change_Number(container);
        try {
            container.transform.GetChild(3).gameObject.SetActive(false);
        }
        catch
        {
            Debug.Log(container.transform.childCount);
            Debug.Log(container.transform.parent);
        }

        //Change Color
        Set_Simple_Color(container);
        */
    }
    private void Set_Icon(Container container)
    {
        if (container.abilities.Count != 0)
        {
            if(container.abilities[0].abilitytype == AbilityType.Fuel)
            {
               SpriteRenderer icon =  container.transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>();
               
            }
        }
        else
        {
            
        }
    }

    private string Icon_Path(AbilityType type)
    {
        string path = @"Containers/Icons";
        if(type == AbilityType.Fuel)
        {
            path += "ABILITY FUEL FULL";
        }
        return path;
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
        if ( container.abilities.Count != 0 && container.abilities[0].abilitytype == AbilityType.Key)
            lights = @"Containers\Lights Infinite";
        container.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = (Sprite)Resources.Load(lights, typeof(Sprite));
    }

    public void Gate(Gate gate)
    {
        if(gate.abilities[0].abilitytype == AbilityType.Key)
        {
            gate.transform.GetChild(6).GetComponent<SpriteRenderer>().sprite = (Sprite)Resources.Load("Doors/door part 6",typeof(Sprite));
        }
    }
    
    public void Stop_All_Co()
    {
        StopAllCoroutines();
    }

    private Vector3 Ability_Color(List<Ability> ability)
    {
        if (ability.Count != 0)
        {
            if (ability[0].abilitytype == AbilityType.Key)
            {
                return new Vector3(1, 1, 1);
            }
            else if (ability[0].abilitytype == AbilityType.Fuel)
            {
                return new Vector3(0, 0.941f, 0.654f);
            }
        }

        // else white
        return new Vector3(1, 1, 1);
    }

    // Dynamic Container Color
    private void Set_Dynamic_Color(DynamicContainer container)
    {
        Vector3 color = Ability_Color(container.abilities);
        for (int i = 0; i < container.transform.childCount; i++)
        {
            container.transform.GetChild(i).GetComponent<SpriteRenderer>().color = new Color(color.x, color.y, color.z, 1);
        }
    }

    private void Set_Simple_Color(SimpleContainer container)
    {
        Vector3 color = Ability_Color(container.abilities);
        for (int i = 0; i < container.transform.childCount; i++)
        {
            container.transform.GetChild(i).GetComponent<SpriteRenderer>().color = new Color(color.x, color.y, color.z, 1);
        }
    }
    public void Dynamic_Container(DynamicContainer container)
    {    /*
        // On or Off Sprite
        string toggle = @"Containers\Icons\";
        if (container.on)
        {
            toggle += "ABILITY FUEL ON";
            container.transform.GetChild(2).gameObject.SetActive(true);
            container.transform.GetChild(3).gameObject.SetActive(true);
            container.GetComponent<Animator>().speed = 4;
        }
        else
        {
            toggle += "ABILITY FUEL OFF";
            container.transform.GetChild(2).gameObject.SetActive(false);
            container.transform.GetChild(3).gameObject.SetActive(false);
            container.GetComponent<Animator>().speed = 1;
        }
        container.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = (Sprite)Resources.Load(toggle, typeof(Sprite));

        //Change Number of 
        Container_Change_Number(container);

        //change Color
        Set_Dynamic_Color(container);

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

        // Direction lights roation
        container.transform.GetChild(2).rotation = Quaternion.Euler(new Vector3(0, 0, rot-90));
        if ( container.abilities.Count!= 0 &&  container.abilities[0].abilitytype == AbilityType.Key)
                container.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = (Sprite)Resources.Load("Doors\\Key", typeof(Sprite));
            
        */
    }

    
}

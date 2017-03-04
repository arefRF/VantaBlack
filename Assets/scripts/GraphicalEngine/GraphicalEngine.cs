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
    private string[] simple_objects_off = new string[] { "Direction","Glass","Switches","Border","Icon Holder","Glow"};
    void Awake()
    {
        Application.targetFrameRate = 240;
    }
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
        Set_Icon(container);
        Container_Change_Number(container);

    }

    private void Set_Dynamic_Special_Icon(DynamicContainer container)
    {
        SpriteRenderer fuel_icon = GetObjectInChild(container.gameObject, "Icon Holder").transform.GetChild(0).GetComponent<SpriteRenderer>(); ;
        if (container.abilities.Count != 0)
        {
            if (container.abilities[0].abilitytype == AbilityType.Fuel)
            {
                fuel_icon.sprite = (Sprite)Resources.Load("Containers\\Icons\\ABILITY FUEL OFF", typeof(Sprite));
            }
        }
        Vector3 color = Ability_Color(container.abilities, ComplimentColor(container));
        fuel_icon.color = new Color(color.x, color.y, color.z, 1);
    }
    private void Set_Icon(Container container)
    {
        SpriteRenderer icon = GetObjectInChild(container.gameObject, "Icon").GetComponent<SpriteRenderer>();
        Vector3 color = Ability_Color(container.abilities, ComplimentColor(container));
        icon.color = new Color(color.x, color.y, color.z, 1);
        if (container.abilities.Count != 0)
        {
            if (container.abilities[0].abilitytype == AbilityType.Fuel)
            {
                if(container is DynamicContainer)
                    icon.color = new Color(color.x, color.y, color.z, 0.1f);
                    
            }
            string path = Icon_Path(container.abilities[0].abilitytype);
            icon.sprite = (Sprite)Resources.Load(path, typeof(Sprite));
        }
        else
        {
            icon.color = new Color(1, 1, 1, 0);
        }
    
        

    }

    private string Icon_Path(AbilityType type)
    {
        string path = @"Containers\Icons\";
        if (type == AbilityType.Fuel)
        {
            path += "ABILITY FUEL FULL";
        }
        else if (type == AbilityType.Key)
            path = @"Doors\Key B";
        return path;
    }
    private GameObject GetObjectInChild(GameObject parent,string name) 
    {
        for(int i = 0; i < parent.transform.childCount; i++)
        {
            if (parent.transform.GetChild(i).name == name)
                return parent.transform.GetChild(i).gameObject;
        }
        return null;
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
        SpriteRenderer number = GetObjectInChild(container.gameObject, "Number").GetComponent<SpriteRenderer>();
        number.sprite = (Sprite)Resources.Load(lights, typeof(Sprite));
        Vector3 color = Ability_Color(container.abilities, ComplimentColor(container));
        number.color = new Color(color.x, color.y, color.z, 1);
    }

    public void Gate(Gate gate)
    {
        if(gate.abilities.Count != 0 && gate.abilities[0].abilitytype == AbilityType.Key)
        {
            gate.transform.GetChild(6).GetComponent<SpriteRenderer>().sprite = (Sprite)Resources.Load("Doors/door part 6",typeof(Sprite));
        }
    }
    
    public void Stop_All_Co()
    {
        StopAllCoroutines();
    }

    private Vector3 Ability_Color(List<Ability> ability,bool compliment)
    {
        if (ability.Count != 0)
        {
            if (ability[0].abilitytype == AbilityType.Key)
            {
                return new Vector3(1, 1, 1);
            }
            else if (ability[0].abilitytype == AbilityType.Fuel)
            {
                if (compliment)
                    return new Vector3(1, 1, 1);
                else
                    return new Vector3(0, 0.941f, 0.654f);
            }
        }

        // else white
        return new Vector3(1, 1, 1);
    }

    private void DynamicRotation(DynamicContainer container)
    {
        //Rotation for Abilities with Direction

        int rot = 0;
        switch (container.direction)
        {
            case Direction.Right: rot = 0; break;
            case Direction.Left: rot = 180; break;
            case Direction.Up: rot = 90; break;
            case Direction.Down: rot = 270; break;
        }
        GetObjectInChild(container.gameObject,"Icon Holder").transform.rotation = Quaternion.Euler(new Vector3(0, 0, rot));
        GameObject direction = GetObjectInChild(container.gameObject, "Direction");
        direction.transform.rotation = Quaternion.Euler(0, 0, rot-90);
        Vector3 color = Ability_Color(container.abilities,false);
        direction.GetComponent<SpriteRenderer>().color = new Color(color.x, color.y, color.z, 1);
    }
    private bool ComplimentColor(Container container)
    {
        if (container is DynamicContainer)
            return ((DynamicContainer)container).on;
        else
            return false;
    }

    // Dynamic Container Color
    public void Dynamic_Container(DynamicContainer container)
    {
        Set_Icon(container);
        Set_Dynamic_Special_Icon(container);
        Container_Change_Number(container);
        DynamicRotation(container);
        DynamicSwitch(container);

    }

    private void DynamicSwitch(DynamicContainer container)
    {
        if(container.on)
        {

            container.GetComponent<Animator>().speed = 4;
            GetObjectInChild(container.gameObject, "Switches").GetComponent<Animator>().SetBool("On", true);
            GetObjectInChild(container.gameObject, "Glass").GetComponent<SpriteRenderer>().sprite =
                (Sprite)Resources.Load("Containers\\Active\\Glass On", typeof(Sprite));
            Vector3 color = Ability_Color(container.abilities, false);
            GetObjectInChild(container.gameObject, "Glass").GetComponent<SpriteRenderer>().color = new Color(color.x, color.y, color.z,1);


        }
        else
        {

            container.GetComponent<Animator>().speed = 1;
            GetObjectInChild(container.gameObject, "Switches").GetComponent<Animator>().SetBool("On", false);
            GetObjectInChild(container.gameObject, "Glass").GetComponent<SpriteRenderer>().sprite =
    (Sprite)Resources.Load("Containers\\Active\\Glass Off", typeof(Sprite));
            Vector3 color = Ability_Color(container.abilities, false);
            GetObjectInChild(container.gameObject, "Glass").GetComponent<SpriteRenderer>().color = new Color(1,1, 1, 1);
        }
    }

    
}

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
    public GameObject beam;
    GameObject beamParent;
    private List<MoveObject> move_objects;
    void Awake()
    {
        Application.targetFrameRate = 240;
    }
    void Start()
    {
        engine = Starter.GetEngine();
        database = Starter.GetDataBase();
        api = engine.apigraphic;
        move_objects = new List<MoveObject>();

    }

    private void makeBeam()
    {
        if (beamParent == null)
            beamParent = new GameObject();
        GameObject beam1 = Instantiate(beam);
        beam1.transform.parent = beamParent.transform;
        beam1.transform.localScale = new Vector3(7, 5, 1);
        beam1.transform.position = new Vector3(15, 5, 0);
    }

    public void Move_Object(GameObject obj,Unit unit, Vector2 pos)
    {
        finish_lock = true;
        StopSameCo(unit);
        MoveObject move = new MoveObject();
        move.code = unit.codeNumber;
        move.co =  StartCoroutine(Move_Object_Coroutine(obj,unit,pos));
        move_objects.Add(move);
       
    }

    private void StopSameCo(Unit unit)
    {
        try {
            for (int i = 0; i < move_objects.Count; i++)
            {
                if (move_objects[i].code == unit.codeNumber)
                {
                    StopCoroutine(move_objects[i].co);
                }
            }
        }
        catch
        {
            Debug.Log("catching exception tu tabe khafane");
        }
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
        Container_Change_Number(container);
        Set_Icon(container);

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
        SpriteRenderer icon; 
        if (container is DynamicContainer)
             icon = GetObjectInChild(container.transform.GetChild(1).gameObject, "Icon").GetComponent<SpriteRenderer>();
        else
            icon  = GetObjectInChild(container.gameObject, "Icon").GetComponent<SpriteRenderer>();
        Vector3 color = Ability_Color(container.abilities, ComplimentColor(container));
        icon.color = new Color(color.x, color.y, color.z, 1);
        if (container.abilities.Count != 0)
        {
            if (container.abilities[0].abilitytype == AbilityType.Fuel)
            {
                if (container is DynamicContainer)
                {
                    if(!((DynamicContainer)container).on)
                        icon.color = new Color(color.x, color.y, color.z, 1);
                    else
                        icon.color = new Color(0, 0, 0, 1);
                }
                    
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
        string path = @"Containers\Icons\New\";
        if (type == AbilityType.Fuel)
        {
            path += "Fuel Off";
        }
        else if (type == AbilityType.Key)
            path += @"Key";
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
        Vector3 color = Ability_Color(container.abilities, false);
        for (int i = 1; i < 4 + 1; i++)
        {
            GetObjectInChild(container.gameObject, "Light Holder").transform.GetChild(i).gameObject.SetActive(false);
        }
        for (int i = 1; i < container.abilities.Count + 1; i++)
        {
            GetObjectInChild(container.gameObject, "Light Holder").transform.GetChild(i).gameObject.SetActive(true);
            GetObjectInChild(container.gameObject, "Light Holder").transform.GetChild(i).GetComponent<SpriteRenderer>().color = new Color(color.x, color.y, color.z, 1);
        }
    }

    public void Gate(Gate gate)
    {
        if(gate.abilities.Count != 0 && gate.abilities[0].abilitytype == AbilityType.Key)
        {
            gate.transform.GetChild(6).GetComponent<SpriteRenderer>().sprite = (Sprite)Resources.Load("Doors/Door 2 ON",typeof(Sprite));
        }
    }
    
    public void Branch(Branch branch)
    {
        if (branch.islocked)
        {
            branch.transform.GetChild(5).GetComponent<SpriteRenderer>().enabled = true;
            if(branch.transform.GetChild(5).GetComponent<SpriteRenderer>().sprite == null)
                branch.transform.GetChild(5).GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Branch/Branch Lock");
        }
        else
            branch.transform.GetChild(5).GetComponent<SpriteRenderer>().enabled = false;
    }

    public void Stop_All_Co()
    {
        StopAllCoroutines();
    }

    public void Fountatin(Fountain fountatin)
    {
        GameObject lights = GetObjectInChild(fountatin.gameObject, "Lights");
        Vector3 color = Ability_Color(fountatin.ability, false);
        for (int i = 0; i < 4; i++)
        {
            lights.transform.GetChild(i).gameObject.SetActive(false);
        }
        for (int i = 0; i < fountatin.count - fountatin.abilities.Count; i++)
        {
            lights.transform.GetChild(i).GetComponent<SpriteRenderer>().color = new Color(color.x,color.y,color.z,1);
            lights.transform.GetChild(i).gameObject.SetActive(true);
        }
    }
    private Vector3 Ability_Color(List<Ability> ability,bool compliment)
    {
        if (ability.Count != 0)
        {
            switch (ability[0].abilitytype)
            {
                case AbilityType.Key: return new Vector3(1, 1, 1);
                case AbilityType.Fuel: if (compliment) return new Vector3(1, 1, 1); else return new Vector3(1, 0.674f, 0.211f);
                case AbilityType.Jump: return new Vector3(0.59f, 0.78f, 1);
                case AbilityType.Teleport: return new Vector3(0.92f,0.36f,0.44f);
                case AbilityType.Gravity: return new Vector3(0.81f,0.60f,0.96f);
                case AbilityType.Rope: return new Vector3(1,0.60f,0.30f);
            }
        }
        // else white
        return new Vector3(1, 1, 1);
    }

    private Vector3 Ability_Color(AbilityType abilitytype, bool compliment)
    {
            switch (abilitytype)
            {
                case AbilityType.Key: return new Vector3(1, 1, 1);
                case AbilityType.Fuel: if (compliment) return new Vector3(1, 1, 1); else return new Vector3(1, 0.674f, 0.211f);
                case AbilityType.Jump: return new Vector3(0.59f, 0.78f, 1);
                case AbilityType.Teleport: return new Vector3(0.92f, 0.36f, 0.44f);
                case AbilityType.Gravity: return new Vector3(0.81f, 0.60f, 0.96f);
                case AbilityType.Rope: return new Vector3(1, 0.60f, 0.30f);
            }
        
        // else white
        return new Vector3(1, 1, 1);
    }

    private void DynamicRotation(DynamicContainer container)
    {
        //Rotation for Abilities with Direction
        string arrow = "Containers\\Version 2\\Blue\\";
        int rot = 0;
        switch (container.direction)
        {
            case Direction.Right: rot = 270; arrow+="Arrow Right"; break;
            case Direction.Left: rot = 90;arrow += "Arrow Left"; break;
            case Direction.Up: rot = 0; arrow += "Arrow Up"; break;
            case Direction.Down: rot =180; arrow += "Arrow Down"; break;
        }
        GetObjectInChild(container.gameObject, "Light Holder").transform.rotation = Quaternion.Euler(new Vector3(0, 0, rot));
        GetObjectInChild(container.gameObject, "Arrow").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(arrow);
        Vector3 color = Ability_Color(container.abilities,false);
   
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
        //container.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Containers\\Version 2\\Body");
        Set_Icon(container);
       // Set_Dynamic_Special_Icon(container);
        Container_Change_Number(container);
        DynamicRotation(container);
        DynamicSwitch(container);

    }


    public void AddLaser(Vector2 pos1,Vector2 pos2,Direction dir)
    {
        makeBeam();
        GameObject myLine = new GameObject();
        myLine.tag = "LaserUI";
        myLine.transform.position = pos1;
        myLine.AddComponent<LineRenderer>();
        LineRenderer render = myLine.GetComponent<LineRenderer>();
        render.material = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));
        render.SetColors(Color.red, Color.red);
        render.SetWidth(0.01f, 0.01f);
        switch (dir)
        {
            case Direction.Up: pos1 = pos1 - new Vector2(0, 0.5f);  pos2 = pos2 - new Vector2(0, 0.5f); break;
            case Direction.Down: pos1 = pos1 + new Vector2(0, 0.5f); pos2 = pos2 + new Vector2(0, 0.5f); break;
            case Direction.Left: pos1 = pos1 + new Vector2(0.5f, 0); pos2 = pos2 + new Vector2(0.5f, 0); break;
            case Direction.Right: pos1 = pos1 - new Vector2(0.5f, 0); pos2 = pos2 - new Vector2(0.5f, 0); break;
        }
        render.SetPosition(0, pos1);
        render.SetPosition(1, pos2);

    }

    public void RemoveLasers()
    {
        try {
            GameObject[] lasers = GameObject.FindGameObjectsWithTag("LaserUI");
            for (int i = 0; i < lasers.Length; i++)
            {
                Destroy(lasers[i]);
            }
        }
        catch
        {
            
        }
    }

    public void StaticContainer(StaticContainer container)
    {
       
    }

    private void DynamicSwitch(DynamicContainer container)
    {

  
        if (container.on)
        {
            GetObjectInChild(container.gameObject, "Light Holder").transform.GetChild(0).gameObject.SetActive(true);
            Vector3 color = Ability_Color(container.abilities, false);
            GetObjectInChild(container.gameObject, "Light Holder").transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(color.x, color.y, color.z,1);
  

        }
        else
        {
            GetObjectInChild(container.gameObject, "Light Holder").transform.GetChild(0).gameObject.SetActive(false);
            Vector3 color = Ability_Color(container.abilities, false);
            GetObjectInChild(container.gameObject, "Light Holder").transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(1,1, 1, 1);
        }
    }

    public void EnterPortalMode(List<Unit> containers,Container container)
    {
        for(int i = 0; i < containers.Count; i++)
        {
            containers[i].GetComponent<SpriteRenderer>().color = new Color(0.3f, 0.5f, 0.8f, 1);
        }
        container.GetComponent<SpriteRenderer>().color = new Color(0.6f, 0.1f, 0.8f, 1);
    }

    public void QuitPortalMode(List<Unit> containers)
    {
        for (int i = 0; i < containers.Count; i++)
        {
            containers[i].GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        }
    }

    // highlight current and make portal color the previous one
    public void PortalHighlighter(Unit current,Unit pre)
    {
        current.GetComponent<SpriteRenderer>().color = new Color(0.6f, 0.1f, 0.8f, 1);
        pre.GetComponent<SpriteRenderer>().color = new Color(0.3f, 0.5f, 0.8f, 1);
    }

    
}

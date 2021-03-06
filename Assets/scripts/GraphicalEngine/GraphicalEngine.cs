using UnityEngine;
using System.Collections;
using System.Threading;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
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

    private Coroutine object_co;
    private string[] simple_objects_off = new string[] { "Direction", "Glass", "Switches", "Border", "Icon Holder", "Glow" };
    private List<MoveObject> move_objects;
    public LaserGraphics lasergraphics { get; set; }
    void Awake()
    {
        Application.targetFrameRate = 240;
        try
        {
            lasergraphics = GetComponent<LaserGraphics>();
        }
        catch { }
    }
    void Start()
    {
        engine = Starter.GetEngine();
        database = Starter.GetDataBase();
        api = engine.apigraphic;
        move_objects = new List<MoveObject>();
        
    }



    public void Move_Object(GameObject obj,Unit unit, Vector2 pos)
    {
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
        bool finish_lock_90percent = true, finish_lock_50percent = true; 
        float remain_distance = ((Vector2)obj.transform.position - end).sqrMagnitude;
        float move_time = 0.6f;
        while (remain_distance > float.Epsilon)
        {
            remain_distance = ((Vector2)obj.transform.position - end).sqrMagnitude;
            Vector3 new_pos = Vector3.MoveTowards(obj.transform.position, end, Time.deltaTime * 1 / move_time);
            obj.transform.position = new_pos;
            if (remain_distance < 0.01 && finish_lock_90percent)
            {
                finish_lock_90percent = false;
                api.MoveGameObjectFinished_90percent(obj,unit);
            }
            if (remain_distance < 0.2 && finish_lock_50percent)
            {
                finish_lock_50percent = false;
                api.MoveGameObjectFinished_50percent(obj, unit);
            }
            yield return null;
        }

    }

    public void Simple_Container(SimpleContainer container)
    {
        Container_Change_Number(container);
        SimpleSetCapacityIcon(container);
        Set_Icon(container);

    }

    private void Set_Dynamic_Special_Icon(DynamicContainer container)
    {
        SpriteRenderer fuel_icon = Toolkit.GetObjectInChild(container.gameObject, "Icon Holder").transform.GetChild(0).GetComponent<SpriteRenderer>(); ;
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
             icon = Toolkit.GetObjectInChild(container.transform.GetChild(1).gameObject, "Icon").GetComponent<SpriteRenderer>();
        else
            icon  = Toolkit.GetObjectInChild(container.gameObject, "Icon").GetComponent<SpriteRenderer>();
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
            string path = Toolkit.Icon_Path(container.abilities[0].abilitytype);
            icon.sprite = (Sprite)Resources.Load(path, typeof(Sprite));
        }
        else
        {
            icon.color = new Color(1, 1, 1, 0);
        }
    
        

    }

    public void Container_Change_Number(Container container)
    {
        Vector3 color = Ability_Color(container.abilities, false);
        for (int i = 1; i < 4 + 1; i++)
        {
            Toolkit.GetObjectInChild(container.gameObject, "Light Holder").transform.GetChild(i).gameObject.SetActive(false);
        }
        for (int i = 1; i < container.abilities.Count + 1; i++)
        {
            Toolkit.GetObjectInChild(container.gameObject, "Light Holder").transform.GetChild(i).gameObject.SetActive(true);
            Toolkit.GetObjectInChild(container.gameObject, "Light Holder").transform.GetChild(i).GetComponent<SpriteRenderer>().color = new Color(color.x, color.y, color.z, 1);
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
            Toolkit.GetObjectInChild(branch.gameObject, "Icon").transform.GetChild(1).gameObject.SetActive(true);
        else
        {
            Toolkit.GetObjectInChild(branch.gameObject, "Icon").transform.GetChild(1).gameObject.SetActive(false);
        }
    }

    public void LaserHit(DynamicContainer container)
    {
        container.transform.GetChild(1).GetChild(6).GetComponent<Opacity>().LaserHit();
    }

    public void LaserUnhit(DynamicContainer container)
    {
        container.transform.GetChild(1).GetChild(6).GetComponent<Opacity>().LaserUnhit();
    }

    public void BranchLight(Branch branch,bool on,Player player)
    {
        GameObject glass;
        Toolkit.GetObjectInChild(branch.gameObject, "Icon").transform.GetChild(0).gameObject.SetActive(on);
        Color color = Toolkit.Ability_Color(player.abilities);
        color = new Color(color.r, color.g, color.b, 1);
        Toolkit.GetObjectInChild(branch.gameObject, "Icon").transform.GetChild(0).GetComponent<SpriteRenderer>().color = color;
        if (on)
            branch.GetComponent<AudioSource>().Play();
    }
    public void Stop_All_Co()
    {
        StopAllCoroutines();
    }


    public void UnlockBranchLaser(Branch branch)
    {
        GameObject icon_holder = Toolkit.GetObjectInChild(branch.gameObject, "Icon");
        Toolkit.GetObjectInChild(icon_holder, "LockIcon").SetActive(false);
        //branch.GetComponent<Animator>().SetTrigger("Unlock");
        branch.BranchUnlockAnimationFinished();
    }
    public void Fountatin(Fountain fountatin)
    {
        GameObject lights = Toolkit.GetObjectInChild(fountatin.gameObject, "Lights");
        Vector3 color = Ability_Color(fountatin.ability, false);
        for (int i = 0; i < 4; i++)
        {
            lights.transform.GetChild(i).gameObject.SetActive(false);
        }
        for (int i = 0; i < fountatin.count - fountatin.abilities.Count; i++)
        {
            lights.transform.GetChild(i).GetComponent<SpriteRenderer>().color = new Color(color.x, color.y, color.z, 1);
            lights.transform.GetChild(i).gameObject.SetActive(true);
        }

        //Change the Color of the Icon
        GameObject Icon = Toolkit.GetObjectInChild(fountatin.gameObject, "Icon");
        Icon.GetComponent<SpriteRenderer>().color = new Color(color.x, color.y, color.z, 1);
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
                case AbilityType.Laser: if (compliment) return new Vector3(1, 1, 1); return new Vector3(0.84f, 0.074f, 0.074f);
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
        string arrow = "Containers\\";
        int rot = 0;
        switch (container.direction)
        {
            case Direction.Right: rot = 270; arrow+="Arrow Right"; break;
            case Direction.Left: rot = 90;arrow += "Arrow Left"; break;
            case Direction.Up: rot = 0; arrow += "Arrow Up"; break;
            case Direction.Down: rot =180; arrow += "Arrow Down"; break;
        }
        arrow += " " + container.capacity;
        Toolkit.GetObjectInChild(container.gameObject, "Light Holder").transform.rotation = Quaternion.Euler(new Vector3(0, 0, rot));
        Toolkit.GetObjectInChild(container.gameObject, "Arrow").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(arrow);
   
    }

    private void SimpleSetCapacityIcon(SimpleContainer container)
    {
        string circle = "Containers\\Circle " + container.capacity;
        Toolkit.GetObjectInChild(container.gameObject, "Number").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(circle);
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
        /*if(pos1.x == pos2.x && pos1.y > pos2.y)
        {
            AddLaser(pos2, pos1, dir);
            return;
        }
        if(pos1.y == pos2.y && pos1.x > pos2.x)
        {
            AddLaser(pos2, pos1, dir);
            return;
        }
        Vector2 temppos = new Vector2(pos1.x, pos1.y);
        if(pos1.x == pos2.x)
            for(; temppos.y <= pos2.y; temppos.y++)
                makeBeam(temppos);
        else
            for (; temppos.x <= pos2.x; temppos.x++)
                makeBeam(temppos);

        GameObject myLine = new GameObject();

        /*GameObject myLine = new GameObject();
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
        */
    }

    

    public void StaticContainer(StaticContainer container)
    {
       
    }

    private void DynamicSwitch(DynamicContainer container)
    {

  
        if (container.on)
        {
            Toolkit.GetObjectInChild(container.gameObject, "Light Holder").transform.GetChild(0).gameObject.SetActive(true);
            Vector3 color = Ability_Color(container.abilities, false);
            Toolkit.GetObjectInChild(container.gameObject, "Light Holder").transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(color.x, color.y, color.z,1);
  

        }
        else
        {
            Toolkit.GetObjectInChild(container.gameObject, "Light Holder").transform.GetChild(0).gameObject.SetActive(false);
            Vector3 color = Ability_Color(container.abilities, false);
            Toolkit.GetObjectInChild(container.gameObject, "Light Holder").transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(1,1, 1, 1);
        }
    }

    public void LaserDieAnimation(Player player)
    {
        GameObject die = Toolkit.GetObjectInChild(GameObject.Find("Special Effects"), "Dying");
        die.transform.position = player.transform.position;
        die.GetComponent<Animator>().SetTrigger("Die");
        player.transform.GetChild(0).gameObject.SetActive(false);
        StartCoroutine(DieLevelReset(1, player));
    }

    private IEnumerator DieLevelReset(float f, Player player)
    {
        yield return new WaitForSeconds(f);
        Starter.GetSceneLoader().Load(SceneManager.GetActiveScene().name, 0.5f);
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

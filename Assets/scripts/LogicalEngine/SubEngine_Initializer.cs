using UnityEngine;
using System.Collections.Generic;

public class SubEngine_Initializer{

    private List<Unit>[,] units;
    int x, y;
    APIUnit api;
    LogicalEngine engine;
    public Sprite[] sprite_Container, sprite_Rock, sprite_Branch;
    public Sprite sprite_BranchJoint;

    public SubEngine_Initializer(int x, int y, LogicalEngine engine)
    {
        this.x = x;
        this.y = y;
        this.engine = engine;
        api = engine.apiunit;

        sprite_Container = new Sprite[16];
        sprite_Rock = new Sprite[16];
        sprite_Branch = new Sprite[6];
        string containerrootpath = "Containers\\Box";
        string rockrootpath = "Rocks\\Rock";
        string branchrootpath = "Branch\\";
        for(int i=0; i<6; i++)
        {
            sprite_Branch[i] = Resources.Load<Sprite>(branchrootpath + (i + 1));
        }
        sprite_BranchJoint = Resources.Load<Sprite>(branchrootpath + "Joint");

        for (int i=1; i < 16; i++)
        {
            string containerpath = containerrootpath + " " + i;
            sprite_Container[i] = Resources.Load<Sprite>(containerpath);
            string rockpath = rockrootpath + "" + i;
            sprite_Rock[i] = Resources.Load<Sprite>(rockpath);
        }
        sprite_Rock[0] = Resources.Load<Sprite>(rockrootpath + "-full");
    }

    public List<Unit>[,] init()
    {
        Unit.Code = 0;
        units = new List<Unit>[x,y];
        for (int i = 0; i < x; i++)
            for (int j = 0; j < y; j++)
                units[i,j] = new List<Unit>();
        GameObject input = GameObject.Find("Units");
        for(int i=0; i<input.transform.childCount; i++)
        {
            GameObject parent = input.transform.GetChild(i).gameObject;
            List<Unit> connectedunits = new List<Unit>();
            for(int j=0; j<parent.transform.childCount; j++)
            {
                GameObject obj = parent.transform.GetChild(j).gameObject;
                switch (obj.tag)
                {
                    case "Box": units[(int)obj.transform.position.x, (int)obj.transform.position.y].Add(obj.GetComponent<Box>()); break;
                    case "Branch": units[(int)obj.transform.position.x, (int)obj.transform.position.y].Add(obj.GetComponent<Branch>()); break;
                    case "Dynamic Container": units[(int)obj.transform.position.x, (int)obj.transform.position.y].Add(obj.GetComponent<DynamicContainer>()); break;
                    case "Static Container": units[(int)obj.transform.position.x, (int)obj.transform.position.y].Add(obj.GetComponent<StaticContainer>()); break;
                    case "Simple Container": units[(int)obj.transform.position.x, (int)obj.transform.position.y].Add(obj.GetComponent<SimpleContainer>()); break;
                    case "Gate": units[(int)obj.transform.position.x, (int)obj.transform.position.y].Add(obj.GetComponent<Gate>()); break;
                    case "Pointer": units[(int)obj.transform.position.x, (int)obj.transform.position.y].Add(obj.GetComponent<PointerContainer>()); break;
                    case "Player": units[(int)obj.transform.position.x, (int)obj.transform.position.y].Add(obj.GetComponent<Player>()); break;
                    case "Ramp": units[(int)obj.transform.position.x, (int)obj.transform.position.y].Add(obj.GetComponent<Ramp>()); break;
                    case "Rock": units[(int)obj.transform.position.x, (int)obj.transform.position.y].Add(obj.GetComponent<Rock>()); break;
                    case "Vision": units[(int)obj.transform.position.x, (int)obj.transform.position.y].Add(obj.GetComponent<Vision>()); break;
                    case "Pipe": units[(int)obj.transform.position.x, (int)obj.transform.position.y].Add(obj.GetComponent<Pipe>()); engine.database.pipes.Add(obj.GetComponent<Pipe>()); break;
                    default: Debug.Log(obj.tag + " Not supported"); break;
                }
                units[(int)obj.transform.position.x, (int)obj.transform.position.y][units[(int)obj.transform.position.x, (int)obj.transform.position.y].Count - 1].ConnectedUnits = new List<Unit>();
                //engine.apigraphic.UnitChangeSprite(units[(int)obj.transform.position.x, (int)obj.transform.position.y][units[(int)obj.transform.position.x, (int)obj.transform.position.y].Count - 1]);
                connectedunits.Add(units[(int)obj.transform.position.x, (int)obj.transform.position.y][units[(int)obj.transform.position.x, (int)obj.transform.position.y].Count - 1]);
                units[(int)obj.transform.position.x, (int)obj.transform.position.y][units[(int)obj.transform.position.x, (int)obj.transform.position.y].Count - 1].codeNumber = Unit.Code;
                units[(int)obj.transform.position.x, (int)obj.transform.position.y][units[(int)obj.transform.position.x, (int)obj.transform.position.y].Count - 1].api = api;
                units[(int)obj.transform.position.x, (int)obj.transform.position.y][units[(int)obj.transform.position.x, (int)obj.transform.position.y].Count - 1].position = units[(int)obj.transform.position.x, (int)obj.transform.position.y][units[(int)obj.transform.position.x, (int)obj.transform.position.y].Count - 1].transform.position;

                Unit.Code++;
            }
            for (int j = 0; j < connectedunits.Count; j++)
                connectedunits[j].SetConnectedUnits(connectedunits);
        }

        return units;
    }
}

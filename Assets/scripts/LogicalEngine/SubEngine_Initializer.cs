using UnityEngine;
using System.Collections.Generic;

public class SubEngine_Initializer{

    private List<Unit>[,] units;
    int x, y;
    APIUnit api;
    LogicalEngine engine;
    public Sprite[] sprite_Container, sprite_Rock, sprite_Ramp,sprite_Branch;

    public SubEngine_Initializer(int x, int y, LogicalEngine engine)
    {
        this.x = x;
        this.y = y;
        this.engine = engine;
        api = engine.apiunit;

        sprite_Container = new Sprite[5];
        sprite_Rock = new Sprite[15];
        sprite_Ramp = new Sprite[4];
        sprite_Branch = new Sprite[4];
        string containerrootpath = "Containers\\Connected";
        string rockrootpath = "Rocks\\Rock";
        string branchrootpath = "Branch\\";
        string ramprootpath = "Ramps\\Rock Half";

        for (int i=0; i < 15; i++)
        {
            string rockpath = rockrootpath + "" + (i + 1);
            sprite_Rock[i] = Resources.Load<Sprite>(rockpath);
        }
        for(int i=0; i < 5; i++)
        {
            string containerpath = containerrootpath + " " + (i + 1);
            sprite_Container[i] = Resources.Load<Sprite>(containerpath);
        }
        for(int i=0; i<4; i++)
        {
            string ramppath = ramprootpath + " " + (i + 1);
            sprite_Ramp[i] = Resources.Load<Sprite>(ramppath);
        }
     
        for (int i =0; i < 4; i++)
        {
            string branch_path = branchrootpath + "Branch " + (i + 1);
            sprite_Branch[i] = Resources.Load<Sprite>(branch_path);
        }
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
                    case "Dynamic Container": units[(int)obj.transform.position.x, (int)obj.transform.position.y].Add(obj.GetComponent<DynamicContainer>());AddFuncList(obj); break;
                    case "Static Container": units[(int)obj.transform.position.x, (int)obj.transform.position.y].Add(obj.GetComponent<StaticContainer>()); AddFuncList(obj); break;
                    case "Simple Container": units[(int)obj.transform.position.x, (int)obj.transform.position.y].Add(obj.GetComponent<SimpleContainer>()); break;
                    case "Gate": units[(int)obj.transform.position.x, (int)obj.transform.position.y].Add(obj.GetComponent<Gate>()); break;
                    case "Pointer": units[(int)obj.transform.position.x, (int)obj.transform.position.y].Add(obj.GetComponent<PointerContainer>()); break;
                    case "Player": units[(int)obj.transform.position.x, (int)obj.transform.position.y].Add(obj.GetComponent<Player>()); break;
                    case "Ramp": units[(int)obj.transform.position.x, (int)obj.transform.position.y].Add(obj.GetComponent<Ramp>()); break;
                    case "Rock": units[(int)obj.transform.position.x, (int)obj.transform.position.y].Add(obj.GetComponent<Rock>()); break;
                    case "Vision": units[(int)obj.transform.position.x, (int)obj.transform.position.y].Add(obj.GetComponent<Vision>()); break;
                    case "Pipe": units[(int)obj.transform.position.x, (int)obj.transform.position.y].Add(obj.GetComponent<Pipe>()); InitPipe(obj.GetComponent<Pipe>()); break;
                    case "Fountain": units[(int)obj.transform.position.x, (int)obj.transform.position.y].Add(obj.GetComponent<Fountain>()); break;
                    case "Drainer": units[(int)obj.transform.position.x, (int)obj.transform.position.y].Add(obj.GetComponent<Drainer>()); engine.database.drainers.Add(obj.GetComponent<Drainer>()); break;
                    case "Laser": units[(int)obj.transform.position.x, (int)obj.transform.position.y].Add(obj.GetComponent<Laser>()); engine.database.lasers.Add(obj.GetComponent<Laser>()); break;
                    case "Leanable" : units[(int)obj.transform.position.x, (int)obj.transform.position.y].Add(obj.GetComponent<Leanable>()); break;
                    case "Mocking Bot": obj.GetComponent<MockingBotPart>().api = api; obj.GetComponent<MockingBotPart>().bot.init(obj.GetComponent<MockingBotPart>(), units); continue;
                    default: Debug.Log(obj + " Not supported"); break;
                }
                units[(int)obj.transform.position.x, (int)obj.transform.position.y][units[(int)obj.transform.position.x, (int)obj.transform.position.y].Count - 1].ConnectedUnits = new List<Unit>();
                //engine.apigraphic.UnitChangeSprite(units[(int)obj.transform.position.x, (int)obj.transform.position.y][units[(int)obj.transform.position.x, (int)obj.transform.position.y].Count - 1]);
                connectedunits.Add(units[(int)obj.transform.position.x, (int)obj.transform.position.y][units[(int)obj.transform.position.x, (int)obj.transform.position.y].Count - 1]);
                units[(int)obj.transform.position.x, (int)obj.transform.position.y][units[(int)obj.transform.position.x, (int)obj.transform.position.y].Count - 1].codeNumber = Unit.Code;
                units[(int)obj.transform.position.x, (int)obj.transform.position.y][units[(int)obj.transform.position.x, (int)obj.transform.position.y].Count - 1].api = api;
                units[(int)obj.transform.position.x, (int)obj.transform.position.y][units[(int)obj.transform.position.x, (int)obj.transform.position.y].Count - 1].position = units[(int)obj.transform.position.x, (int)obj.transform.position.y][units[(int)obj.transform.position.x, (int)obj.transform.position.y].Count - 1].transform.position;

                Unit.Code++;
                if(obj.tag == "Pipe")
                {
                    units[(int)obj.transform.position.x, (int)obj.transform.position.y].Remove(obj.GetComponent<Pipe>());
                }
            }
            for (int j = 0; j < connectedunits.Count; j++)
                connectedunits[j].SetConnectedUnits(connectedunits);
        }

        return units;
    }

    // function to add functional containers to list of database
    private void AddFuncList(GameObject obj)
    {
        Starter.GetDataBase().functionalCon.Add(obj.GetComponent<FunctionalContainer>());
    }
    private void InitPipe(Pipe pipe)
    {
        for (int i = 0; i < pipe.PipedTo.Count; i++)
            if (!(pipe.PipedTo[i] is Pipe))
                throw new System.Exception();
        engine.database.pipes.Add(pipe);
    }
}

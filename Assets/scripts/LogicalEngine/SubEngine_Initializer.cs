using UnityEngine;
using System.Collections.Generic;

public class SubEngine_Initializer{

    private List<Unit>[,] units;
    int x, y;
    APIUnit api;
    LogicalEngine engine;
    public Sprite[] sprite_Container, sprite_Rock;

    public SubEngine_Initializer(int x, int y, LogicalEngine engine)
    {
        this.x = x;
        this.y = y;
        this.engine = engine;
        api = engine.apiunit;

        sprite_Container = new Sprite[16];
        sprite_Rock = new Sprite[16];
        string containerrootpath = "Containers\\Box";
        string rockrootpath = "Rocks\\Rock";

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
                    default: Debug.Log(obj.tag + " Not supported"); break;
                }
                units[(int)obj.transform.position.x, (int)obj.transform.position.y][units[(int)obj.transform.position.x, (int)obj.transform.position.y].Count - 1].ConnectedUnits = new List<Unit>();
                engine.apigraphic.UnitChangeSprite(units[(int)obj.transform.position.x, (int)obj.transform.position.y][units[(int)obj.transform.position.x, (int)obj.transform.position.y].Count - 1]);
                connectedunits.Add(units[(int)obj.transform.position.x, (int)obj.transform.position.y][units[(int)obj.transform.position.x, (int)obj.transform.position.y].Count - 1]);
                units[(int)obj.transform.position.x, (int)obj.transform.position.y][units[(int)obj.transform.position.x, (int)obj.transform.position.y].Count - 1].codeNumber = Unit.Code;
                units[(int)obj.transform.position.x, (int)obj.transform.position.y][units[(int)obj.transform.position.x, (int)obj.transform.position.y].Count - 1].api = api;
                units[(int)obj.transform.position.x, (int)obj.transform.position.y][units[(int)obj.transform.position.x, (int)obj.transform.position.y].Count - 1].position = units[(int)obj.transform.position.x, (int)obj.transform.position.y][units[(int)obj.transform.position.x, (int)obj.transform.position.y].Count - 1].transform.position;
                Unit.Code++;
            }
            for (int j = 0; j < connectedunits.Count; j++)
                connectedunits[j].SetConnectedUnits(connectedunits);
        }

        SetSprites(units);

        return units;
    }

    private void SetSprites(List<Unit>[,] units)
    {
        for(int i=0;i< x; i++)
        {
            for(int j=0; j< y; j++)
            {
                for(int k=0; k<units[i,j].Count; k++)
                {
                    if (units[i, j][k] is Container && !(units[i, j][k] is Gate))
                    {
                        SetContainerSprite(units, units[i, j][k]);
                        engine.apigraphic.UnitChangeSprite(units[i, j][k]);
                    }
                    else if (units[i, j][k] is Rock)
                        SetRockSprite(units, units[i, j][k]);
                    else if (units[i, j][k] is Ramp)
                        SetRampSprite(units,units[i,j][k]);
                }
            }
        }
    }


    private void SetRampSprite(List<Unit>[,] units,Unit unit)
    {
        string ramprootpath = "Ramps\\Ramp-type";
        string ramp_path = "";
        bool[] notconnected = GetConnectedSides(units, unit);
        Ramp ramp = (Ramp)unit;
        if(ramp.type == 1)
        {
            ramp_path = ramprootpath + "1-";
            // bot and left connected
            if (!notconnected[2] && !notconnected[3])
                ramp_path += "2";
            // just bot connected
            else if (!notconnected[2] && notconnected[3])
                ramp_path += "down";
            // just left connected
            else if (notconnected[2] && !notconnected[3])
                ramp_path += "left";
        }
        else if(ramp.type == 2)
        {
            ramp_path = ramprootpath + "2-";
            if (!notconnected[0] && !notconnected[3])
                ramp_path += "2";
            else if (!notconnected[0] && notconnected[3])
                ramp_path += "top";
            else if (notconnected[0] && !notconnected[3])
                ramp_path += "left";
        }
        else if(ramp.type == 3)
        {
            ramp_path = ramprootpath + "3-";
            if (!notconnected[0] && !notconnected[1])
                ramp_path += "2";
            //right connected
            else if (notconnected[0] && !notconnected[1])
                ramp_path += "right";
            else if (!notconnected[0] && notconnected[1])
                ramp_path += "left";
            //not connected to anything
            else
                ramp_path += "0";

        }
        else if(ramp.type == 4)
        {
            ramp_path = ramprootpath + "4-";
            if (!notconnected[1] && !notconnected[2])
                ramp_path += "2";
            else if (!notconnected[1] && notconnected[2])
                ramp_path += "right";
            else if (notconnected[1] && !notconnected[2])
                ramp_path += "down";
        }
        unit.GetComponent<SpriteRenderer>().sprite = (Sprite)Resources.Load(ramp_path,typeof(Sprite));
    }
    private void SetRockSprite(List<Unit>[,] units, Unit unit)
    {
        bool[] notconnected = GetConnectedSides(units, unit);
        if (notconnected[0] && notconnected[1] && notconnected[2] && notconnected[3])
            unit.gameObject.GetComponent<SpriteRenderer>().sprite = sprite_Rock[1];
        else if (notconnected[0] && notconnected[1] && notconnected[2])
            unit.gameObject.GetComponent<SpriteRenderer>().sprite = sprite_Rock[2];
        else if (notconnected[0] && notconnected[2] && notconnected[3])
            unit.gameObject.GetComponent<SpriteRenderer>().sprite = sprite_Rock[3];
        else if (notconnected[0] && notconnected[1] && notconnected[3])
            unit.gameObject.GetComponent<SpriteRenderer>().sprite = sprite_Rock[4];
        else if (notconnected[1] && notconnected[2] && notconnected[3])
            unit.gameObject.GetComponent<SpriteRenderer>().sprite = sprite_Rock[5];
        else if (notconnected[0] && notconnected[2])
            unit.gameObject.GetComponent<SpriteRenderer>().sprite = sprite_Rock[6];
        else if (notconnected[1] && notconnected[3])
            unit.gameObject.GetComponent<SpriteRenderer>().sprite = sprite_Rock[7];
        else if (notconnected[0] && notconnected[1])
            unit.gameObject.GetComponent<SpriteRenderer>().sprite = sprite_Rock[8];
        else if (notconnected[0] && notconnected[3])
            unit.gameObject.GetComponent<SpriteRenderer>().sprite = sprite_Rock[9];
        else if (notconnected[1] && notconnected[2])
            unit.gameObject.GetComponent<SpriteRenderer>().sprite = sprite_Rock[10];
        else if (notconnected[2] && notconnected[3])
            unit.gameObject.GetComponent<SpriteRenderer>().sprite = sprite_Rock[11];
        else if (notconnected[0])
            unit.gameObject.GetComponent<SpriteRenderer>().sprite = sprite_Rock[12];
        else if (notconnected[1])
            unit.gameObject.GetComponent<SpriteRenderer>().sprite = sprite_Rock[13];
        else if (notconnected[2])
            unit.gameObject.GetComponent<SpriteRenderer>().sprite = sprite_Rock[14];
        else if (notconnected[3])
            unit.gameObject.GetComponent<SpriteRenderer>().sprite = sprite_Rock[15];
        else
            unit.gameObject.GetComponent<SpriteRenderer>().sprite = sprite_Rock[0];
    }

    private void SetContainerSprite(List<Unit>[,] units, Unit unit)
    {
        bool[] notconnected = GetConnectedSides(units, unit);
        if (notconnected[0] && notconnected[1] && notconnected[2] && notconnected[3])
            unit.gameObject.GetComponent<SpriteRenderer>().sprite = sprite_Container[1];
        else if (notconnected[0] && notconnected[1] && notconnected[2])
            unit.gameObject.GetComponent<SpriteRenderer>().sprite = sprite_Container[2];
        else if (notconnected[0] && notconnected[2] && notconnected[3])
            unit.gameObject.GetComponent<SpriteRenderer>().sprite = sprite_Container[3];
        else if (notconnected[0] && notconnected[1] && notconnected[3])
            unit.gameObject.GetComponent<SpriteRenderer>().sprite = sprite_Container[4];
        else if (notconnected[1] && notconnected[2] && notconnected[3])
            unit.gameObject.GetComponent<SpriteRenderer>().sprite = sprite_Container[5];
        else if (notconnected[0] && notconnected[2])
            unit.gameObject.GetComponent<SpriteRenderer>().sprite = sprite_Container[6];
        else if (notconnected[0] && notconnected[1])
            unit.gameObject.GetComponent<SpriteRenderer>().sprite = sprite_Container[7];
        else if (notconnected[1] && notconnected[3])
            unit.gameObject.GetComponent<SpriteRenderer>().sprite = sprite_Container[8];
        else if (notconnected[0] && notconnected[3])
            unit.gameObject.GetComponent<SpriteRenderer>().sprite = sprite_Container[9];
        else if (notconnected[2] && notconnected[3])
            unit.gameObject.GetComponent<SpriteRenderer>().sprite = sprite_Container[10];
        else if (notconnected[1] && notconnected[2])
            unit.gameObject.GetComponent<SpriteRenderer>().sprite = sprite_Container[11];
        else if (notconnected[0])
            unit.gameObject.GetComponent<SpriteRenderer>().sprite = sprite_Container[12];
        else if (notconnected[1])
            unit.gameObject.GetComponent<SpriteRenderer>().sprite = sprite_Container[13];
        else if (notconnected[2])
            unit.gameObject.GetComponent<SpriteRenderer>().sprite = sprite_Container[14];
        else if (notconnected[3])
            unit.gameObject.GetComponent<SpriteRenderer>().sprite = sprite_Container[15];
    }



    private bool[] GetConnectedSides(List<Unit>[,] units, Unit unit)
    {
        bool[] result = new bool[4];
        result[0] = !IsConnectedFromPosition(units, unit, Toolkit.VectorSum(unit.position, new Vector2(0, 1)));
        result[1] = !IsConnectedFromPosition(units, unit, Toolkit.VectorSum(unit.position, new Vector2(1, 0)));
        result[2] = !IsConnectedFromPosition(units, unit, Toolkit.VectorSum(unit.position, new Vector2(0, -1)));
        result[3] = !IsConnectedFromPosition(units, unit, Toolkit.VectorSum(unit.position, new Vector2(-1, 0)));
        return result;
    }

    private bool IsConnectedFromPosition(List<Unit>[,] units, Unit unit, Vector2 pos)
    {
        try
        {
            for (int i = 0; i < units[(int)pos.x, (int)pos.y].Count; i++)
            {
                Unit u = units[(int)pos.x, (int)pos.y][i];
                if (u.gameObject.transform.parent == unit.gameObject.transform.parent)
                {
                    return true;
                }
            }
        }
        catch { }
        return false;
    }

}

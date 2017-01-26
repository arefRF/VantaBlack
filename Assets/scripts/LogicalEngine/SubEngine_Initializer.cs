using UnityEngine;
using System.Collections.Generic;

public class SubEngine_Initializer{

    private List<Unit>[,] units;
    int x, y;
    APIUnit api;

    public Sprite[] sprite_Container;

    public SubEngine_Initializer(int x, int y, APIUnit api)
    {
        this.x = x;
        this.y = y;
        this.api = api;

        sprite_Container = new Sprite[16];
        string rootpath = "Containers\\Box ";
        for(int i=1; i < 16; i++)
        {
            string path = rootpath + i;
            sprite_Container[i] = Resources.Load<Sprite>(path);
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
                    if(units[i,j][k] is Container)
                    {
                        Unit unit = units[i, j][k];
                        Debug.Log(unit.gameObject.GetComponent<SpriteRenderer>().sprite);
                        SetContainerSprite(units, unit);
                        Debug.Log(unit.gameObject.GetComponent<SpriteRenderer>().sprite);
                    }
                }
            }
        }
    }

    private void SetContainerSprite(List<Unit>[,] units, Unit unit)
    {
        bool[] notconnected = GetConnectedSides(units, unit);
        if (notconnected[0] && notconnected[1] && notconnected[2] && notconnected[3])
            unit.gameObject.GetComponent<SpriteRenderer>().sprite = sprite_Container[1];
        if (notconnected[0] && notconnected[1] && notconnected[2])
            unit.gameObject.GetComponent<SpriteRenderer>().sprite = sprite_Container[2];
        else if (notconnected[0] && notconnected[2] && notconnected[3])
            unit.gameObject.GetComponent<SpriteRenderer>().sprite = sprite_Container[3];
        else if (notconnected[0] && notconnected[1] && notconnected[3])
            unit.gameObject.GetComponent<SpriteRenderer>().sprite = sprite_Container[4];
        else if (notconnected[1] && notconnected[2] && notconnected[3])
            unit.gameObject.GetComponent<SpriteRenderer>().sprite = sprite_Container[5];
        if (notconnected[0] && notconnected[2])
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
        {
            Debug.Log("hello");
            unit.gameObject.GetComponent<SpriteRenderer>().sprite = sprite_Container[11];
        }
        else if (notconnected[0])
            unit.gameObject.GetComponent<SpriteRenderer>().sprite = sprite_Container[12];
        else if (notconnected[1])
            unit.gameObject.GetComponent<SpriteRenderer>().sprite = sprite_Container[13];
        else if (notconnected[2])
            unit.gameObject.GetComponent<SpriteRenderer>().sprite = sprite_Container[14];
        else if (notconnected[3])
            unit.gameObject.GetComponent<SpriteRenderer>().sprite = sprite_Container[15];
    }

    private bool[] GetConnectedSides(List<Unit>[,] units, Unit container)
    {
        bool[] result = new bool[4];
        result[0] = !IsConnectedFromUp(units, container);
        result[1] = !IsConnectedFromRight(units, container);
        result[2] = !IsConnectedFromDown(units, container);
        result[3] = !IsConnectedFromLeft(units, container);
        return result;
    }

    private bool IsConnectedFromLeft(List<Unit>[,] units, Unit Container)
    {
        Vector2 pos = Toolkit.VectorSum(Container.position, new Vector2(-1, 0));
        for(int i=0; i<units[(int)pos.x, (int)pos.y].Count; i++)
        {
            Unit u = units[(int)pos.x, (int)pos.y][i];
            if (u.gameObject.transform.parent == Container.gameObject.transform.parent)
            {
                return true;
            }
        }
        return false;
    }
    private bool IsConnectedFromUp(List<Unit>[,] units, Unit Container)
    {
        Vector2 pos = Toolkit.VectorSum(Container.position, new Vector2(0, 1));
        for (int i = 0; i < units[(int)pos.x, (int)pos.y].Count; i++)
        {
            Unit u = units[(int)pos.x, (int)pos.y][i];
            if (u.gameObject.transform.parent == Container.gameObject.transform.parent)
            {
                return true;
            }
        }
        return false;
    }
    private bool IsConnectedFromRight(List<Unit>[,] units, Unit Container)
    {
        Vector2 pos = Toolkit.VectorSum(Container.position, new Vector2(1, 0));
        for (int i = 0; i < units[(int)pos.x, (int)pos.y].Count; i++)
        {
            Unit u = units[(int)pos.x, (int)pos.y][i];
            if (u.gameObject.transform.parent == Container.gameObject.transform.parent)
            {
                return true;
            }
        }
        return false;
    }
    private bool IsConnectedFromDown(List<Unit>[,] units, Unit Container)
    {
        Vector2 pos = Toolkit.VectorSum(Container.position, new Vector2(0, -1));
        for (int i = 0; i < units[(int)pos.x, (int)pos.y].Count; i++)
        {
            Unit u = units[(int)pos.x, (int)pos.y][i];
            if (u.gameObject.transform.parent == Container.gameObject.transform.parent)
            {
                return true;
            }
        }
        return false;
    }
}

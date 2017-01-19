using UnityEngine;
using System.Collections.Generic;

public class SubEngine_Initializer{

    private List<Unit>[,] units;
    int x, y;
    APIUnit api;

    public SubEngine_Initializer(int x, int y, APIUnit api)
    {
        this.x = x;
        this.y = y;
        this.api = api;
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
                units[(int)obj.transform.position.x, (int)obj.transform.position.y][units[(int)obj.transform.position.x, (int)obj.transform.position.y].Count - 1].codeNumber = Unit.Code;
                units[(int)obj.transform.position.x, (int)obj.transform.position.y][units[(int)obj.transform.position.x, (int)obj.transform.position.y].Count - 1].api = api;
                units[(int)obj.transform.position.x, (int)obj.transform.position.y][units[(int)obj.transform.position.x, (int)obj.transform.position.y].Count - 1].position = units[(int)obj.transform.position.x, (int)obj.transform.position.y][units[(int)obj.transform.position.x, (int)obj.transform.position.y].Count - 1].transform.position;
                Unit.Code++;
            }
        }
        return units;
    }
}

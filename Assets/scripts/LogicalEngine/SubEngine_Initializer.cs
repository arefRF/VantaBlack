using UnityEngine;
using System.Collections.Generic;

public class SubEngine_Initializer{

    private List<Unit>[,] units;
    int x, y;

    public SubEngine_Initializer(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public void init()
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
                    case "Box": units[i, j].Add(obj.GetComponent<Box>()); break;
                    case "Branch": units[i, j].Add(obj.GetComponent<Branch>()); break;
                    case "Dynamic Container": units[i, j].Add(obj.GetComponent<DynamicContainer>()); break;
                    case "Static Container": units[i, j].Add(obj.GetComponent<StaticContainer>()); break;
                    case "Sipmle Container": units[i, j].Add(obj.GetComponent<SimpleContainer>()); break;
                    case "Gate": units[i, j].Add(obj.GetComponent<Gate>()); break;
                    case "Pointer": units[i, j].Add(obj.GetComponent<PointerContainer>()); break;
                    case "Player": units[i, j].Add(obj.GetComponent<Player>()); break;
                    case "Ramp": units[i, j].Add(obj.GetComponent<Ramp>()); break;
                    case "Rock": units[i, j].Add(obj.GetComponent<Rock>()); break;
                    case "Vision": units[i, j].Add(obj.GetComponent<Vision>()); break;
                    default: Debug.Log(obj.tag + " Not supported"); break;
                }
                units[i, j][units[i, j].Count - 1].codeNumber = Unit.Code;
                Unit.Code++;
            }
        }
    }
}

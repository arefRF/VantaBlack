using UnityEngine;
using System.Collections;

public class Starter  {

    public static void _Set_Everything()
    {
        GameObject.Find("Graphical").GetComponent<GraphicalEngine>().database = Database.database;
        GameObject.Find("Graphical").GetComponent<GraphicalEngine>().player = Database.database.player.GetComponent<Player>();
        Debug.Log(Application.persistentDataPath);
    }
}

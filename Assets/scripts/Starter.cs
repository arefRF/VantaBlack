using UnityEngine;
using System.Collections.Generic;

public class Starter : MonoBehaviour{
    public Database database;
    public LogicalEngine logicalengine;
    public int x, y;
    public Direction Gravity_Directin;
    public List<GameObject> player;
    public static LogicalEngine staticengine;
    void Awake()
    {
        _Set_Everything();
    }
    public void _Set_Everything()
    {
        database = new Database(0);
        database.player = new List<Player>();
        for (int i = 0; i < player.Count; i++)
            database.player.Add(player[i].GetComponent<Player>());
        logicalengine = new LogicalEngine(x, y);
        staticengine = logicalengine;
        database.gravity_direction = Gravity_Directin;
        Database.GravityDirection = database.gravity_direction;
        database.state = State.Busy;
        staticengine = logicalengine;
        GameObject.Find("Graphical").GetComponent<GraphicalEngine>().database = database;
        logicalengine.Run();
    }

    public static Database GetDataBase()
    {
        return GameObject.Find("Starter").GetComponent<Starter>().database;
    }

    public static LogicalEngine GetEngine()
    {
        return staticengine;
    }
    public static Direction GetGravityDirection()
    {
        return staticengine.database.gravity_direction;
    }

}

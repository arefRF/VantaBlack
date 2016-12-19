using UnityEngine;
using System.Collections;

public class Starter : MonoBehaviour{
    public Database database;
    public LogicalEngine logicalengine;
    public int x, y;
    public Direction Gravity_Directin;
    public GameObject player;
    public static LogicalEngine staticengine;
    void Awake()
    {
        _Set_Everything();
    }
    public void _Set_Everything()
    {
        database = new Database(0);
        database.player = player;
        logicalengine = new LogicalEngine(x, y);
        staticengine = logicalengine;
        database.gravity_direction = Gravity_Directin;
        database.state = State.Busy;
        database.logicalengine = logicalengine;
        staticengine = logicalengine;
        GameObject.Find("Graphical").GetComponent<GraphicalEngine>().database = database;
        GameObject.Find("Graphical").GetComponent<GraphicalEngine>().player = database.player.GetComponent<Player>();


        logicalengine.run();
    }

    public static Database GetDataBase()
    {
        return GameObject.Find("Starter").GetComponent<Starter>().database;
    }

    public static LogicalEngine GetEngine()
    {
        return staticengine;
    }
}

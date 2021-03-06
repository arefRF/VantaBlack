using UnityEngine;
using System.Collections.Generic;

public class Starter : MonoBehaviour{
    public Database database;
    public LogicalEngine logicalengine;
    public int x, y;
    public Direction Gravity_Directin;
    public List<GameObject> player;
    public static LogicalEngine staticengine;
    public bool BlocksInvis = false;
    public static bool Blockinvis;
    public static GameManager gamemanager;
    public static SceneLoader sceneloader;
    public LockCombination LockCombination;
    void Awake()
    {
        Blockinvis = BlocksInvis;
        _Set_Everything();
    }
    void Start()
    {
        logicalengine.Applygravity();
    }
    public void _Set_Everything()
    {
        database = new Database(0);
        database.player = new List<Player>();
        for (int i = 0; i < player.Count; i++)
            database.player.Add(player[i].GetComponent<Player>());
        logicalengine = new LogicalEngine(x, y);
        staticengine = logicalengine;
        database.SetGravity(Gravity_Directin);
        Database.GravityDirection = database.gravity_direction;
        database.state = State.Busy;
        staticengine = logicalengine;
        GameObject.Find("Graphical").GetComponent<GraphicalEngine>().database = database;
        Toolkit.database = database;
        sceneloader = GameObject.Find("SceneLoader").GetComponent<SceneLoader>();
        //gamemanager = GameObject.Find("GameManager").GetComponent<GameManager>();
        if (LockCombination != null)
            logicalengine.lockcombination = LockCombination;
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

    public static Direction GetDefaulatGravity()
    {
        return Database.GravityDirection;
    }

    public static GameManager GetGameManager()
    {
        return gamemanager;
    }

    public static SceneLoader GetSceneLoader()
    {
        return sceneloader;
    }
}

using UnityEngine;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class GameManager : MonoBehaviour {
    public static GameManager manager = null;
    Database database;
    FileStream file;
    public LogicalEngine engine;
    void Start()
    {
        database = Starter.GetDataBase();
        engine = Starter.GetEngine();
        if (manager != null && manager != this)
        {
            Destroy(gameObject);
        }
        else
        {
            manager = this;
        }
        DontDestroyOnLoad(gameObject);
        if(File.Exists(Application.persistentDataPath + "save.bin"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            file = File.Open(Application.persistentDataPath + "save.bin", FileMode.Open);
            SaveSerialize temp = bf.Deserialize(file) as SaveSerialize;
            //Toolkit.code
        }
        else
        {

        } 
    }

    public void SetPlayer()
    {

    }
}

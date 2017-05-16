using UnityEngine;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class GameManager : MonoBehaviour {
    public static GameManager manager = null;
    Database database;
    FileStream file;
    public LogicalEngine engine;
    public bool shouldload;
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
        if(File.Exists(/*Application.persistentDataPath + */"save.bin"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            file = File.Open(/*Application.persistentDataPath + */"save.bin", FileMode.Open);
            SaveSerialize temp = bf.Deserialize(file) as SaveSerialize;
            if (temp.scenename == UnityEngine.SceneManagement.SceneManager.GetActiveScene().name && shouldload)
            {
                for (int i = 0; i < temp.branchCodeNumbers.Count; i++)
                {
                    Branch branch = Toolkit.GetUnitByCodeNumber(temp.branchCodeNumbers[i]) as Branch;
                    branch.blocked = true;
                }
                SetPlayer(temp);
                file.Close();
            }
        }
        else
        {

        } 
    }

    public void SetPlayer(SaveSerialize saveserialize)
    {
        Player player = database.player[0];
        engine.apiunit.RemoveFromDatabase(player);
        player.position = new Vector2(saveserialize.posx, saveserialize.posy);
        player.transform.position = player.position;
        player.abilities.Clear();
        for(int i=0; i<saveserialize.abilitycount; i++)
        {
            player.abilities.Add(Ability.GetAbilityInstance(saveserialize.abilitytype));
        }
        player._setability();
    }
}

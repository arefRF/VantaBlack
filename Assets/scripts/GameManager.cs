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
        if (manager != null && manager != this)
        {
            Destroy(gameObject);
        }
        else
        {
            manager = this;
        }
        DontDestroyOnLoad(gameObject);
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name.Equals("Menu V2")  ) 
        {
            if(File.Exists(/*Application.persistentDataPath + */"save.bin"))
                GameObject.Find("Menu").GetComponent<Menu>().Continue();
            return;
        }
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name.Equals("Intro"))
            return;
        database = Starter.GetDataBase();
        engine = Starter.GetEngine();
        if (File.Exists(/*Application.persistentDataPath + */"save.bin"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            file = File.Open(/*Application.persistentDataPath + */"save.bin", FileMode.Open);
            SaveSerialize temp = bf.Deserialize(file) as SaveSerialize;
            if (temp.scenename == UnityEngine.SceneManagement.SceneManager.GetActiveScene().name && manager.shouldload)
            {
                for (int i = 0; i < temp.branchCodeNumbers.Count; i++)
                {
                    Branch branch = Toolkit.GetUnitByCodeNumber(temp.branchCodeNumbers[i]) as Branch;
                    branch.blocked = true;
                }
                SetPlayer(temp);
            }
            file.Close();
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

    public void NewGame()
    {
        Debug.Log("new game");
        manager.shouldload = true;
        UnityEngine.SceneManagement.SceneManager.LoadScene("Part-0");
    }

    public void Continue()
    {
        Debug.Log("continue");
        manager.shouldload = true;
        BinaryFormatter bf = new BinaryFormatter();
        file = File.Open(/*Application.persistentDataPath + */"save.bin", FileMode.Open);
        SaveSerialize temp = bf.Deserialize(file) as SaveSerialize;
        UnityEngine.SceneManagement.SceneManager.LoadScene(temp.scenename);
        file.Close();
    }

    public void LoadLevel(string scenename)
    {
        manager.shouldload = false;
        UnityEngine.SceneManagement.SceneManager.LoadScene(scenename);
    }
}

using UnityEngine;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

[System.Serializable]
public class SaveSerialize {
    public string scenename;
    public int posx, posy, abilitycount, draincount;
    public AbilityType abilitytype;
    public List<long> branchCodeNumbers;

    public SaveSerialize()
    {
        draincount = 0;
        branchCodeNumbers = new List<long>();
    }

    public void serialize(string scenename, Vector2 playerpos, AbilityType abilitytype, int abilitycount)
    {
        this.scenename = scenename;
        posx = (int)playerpos.x;
        posy = (int)playerpos.y;
        this.abilitytype = abilitytype;
        this.abilitycount = abilitycount;
        Debug.Log("serializing");
        IFormatter formatter = new BinaryFormatter();
        Stream stream = new FileStream("save.bin", FileMode.Create, FileAccess.Write, FileShare.None);
        formatter.Serialize(stream, this);
        stream.Close();
    }
}

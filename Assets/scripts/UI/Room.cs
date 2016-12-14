using UnityEngine;
using System.Collections;
[System.Serializable]
public class Room  {
    public string name;
    public bool isVisible;

    public Room(string name)
    {
        this.name = name;
        isVisible = false;
    }
}

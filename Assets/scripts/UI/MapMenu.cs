using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class MapMenu {
    public  Room[] rooms;

    public MapMenu(int n)
    {
        rooms = new Room[n];
        for(int i = 0; i < n; i++)
        {
            rooms[i] = new Room("R"+(i+1).ToString());
            rooms[i].isVisible = false;
        }
    }


    public void _show_room(string name)
    {
        foreach (Room room in rooms){
            if (room.name == name)
            {
                room.isVisible = true;
                break;
            }

        }
    }

    public void _hide_room(string name)
    {
        foreach (Room room in rooms)
        {
            if (room.name == name)
            {
                room.isVisible = false;
                break;
            }

        }
    }

}

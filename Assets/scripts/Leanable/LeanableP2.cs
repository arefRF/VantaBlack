using UnityEngine;
using System.Collections.Generic;

public class LeanableP2 : Leanable{

    private Part0_RobotScript roboscrip;
    private bool on, tempon;
    public override void Run()
    {
        roboscrip = Robot.GetComponent<Part0_RobotScript>();
        abilities = new List<Ability>();
        base.Run();
    }

    void Update()
    {
        if (on == tempon || !roboscrip.moved)
            return;
        Debug.Log("duuuude");
        tempon = on;
        if (roboscrip.moved)
        {
            if (on)
            {
                for (int i = 0; i < ConnectedUnits.Count; i++)
                {
                    api.RemoveFromDatabase(ConnectedUnits[i]);
                    ConnectedUnits[i].position = new Vector2(ConnectedUnits[i].position.x, ConnectedUnits[i].position.y + 1);
                    ConnectedUnits[i].transform.position = position;
                    api.AddToDatabase(ConnectedUnits[i]);
                }
            }
            else
            {
                for (int i = 0; i < ConnectedUnits.Count; i++)
                {
                    Debug.Log(ConnectedUnits[i].position);
                    api.RemoveFromDatabase(ConnectedUnits[i]);
                    ConnectedUnits[i].position = new Vector2(ConnectedUnits[i].position.x, ConnectedUnits[i].position.y - 1);
                    ConnectedUnits[i].transform.position = position;
                    api.AddToDatabase(ConnectedUnits[i]);
                }
            }
        }
    }

    public override void LeanedAction(Player player, Direction direction)
    {
        if (abilities.Count == 0)
            return;
        on = !on;
        Debug.Log(on);
        if (on)
        {
            Robot.GetComponent<Animator>().SetBool("Start", true);
        }
        else
        {
            Robot.GetComponent<Animator>().SetBool("Start", false);
        }
    }
}

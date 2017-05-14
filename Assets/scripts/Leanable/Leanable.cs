using UnityEngine;
using System.Collections.Generic;

public class Leanable : Container {

    public bool canLean = true;
    public GameObject Robot;
    private Part0_RobotScript roboscrip;

    public override void Run()
    {
        roboscrip = Robot.GetComponent<Part0_RobotScript>();
        abilities = new List<Ability>();
        base.Run();
    }

    // Update is called once per frame
    void Update () {
        if (roboscrip.moved)
        {
            api.RemoveFromDatabase(this);
            for (int i = 0; i < ConnectedUnits.Count; i++)
                api.RemoveFromDatabase(ConnectedUnits[i]);
        }
    }

    public override void PlayerAbsorb(Player player)
    {
        
    }

    public override void PlayerAbsorbHold(Player player)
    {
        
    }

    public void LeanedAction(Player player, Direction direction)
    {
        if (abilities.Count == 0)
            return;
        canLean = false;
        api.engine.inputcontroller.LeanUndo(player, Toolkit.ReverseDirection(direction), PlayerState.Idle);
        Robot.GetComponent<Animator>().SetBool("Start", true);
    }

    public void MoveFinished()
    {

    }
    //public void 
}

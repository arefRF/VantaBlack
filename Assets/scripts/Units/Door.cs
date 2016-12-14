using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class Door : Unit
{

    public bool isOpen;
    public Direction direction;
    public Vector2 Camera_Position;
    public float Camera_Zoom;
    public string sceneName;
    public bool save = true;

    // Use this for initialization
    void Awake()
    {
        unitType = UnitType.Door;
        obj = this.gameObject;
        position = gameObject.transform.position;
        movable = false;
        layer = 1;
    }

    public void OpenDoor()
    {
        if (isOpen)
            return;
        isOpen = true;
        Interface.GetEngine().Gengine._Internal_Door_Change_Sprite(this);
    }

    public void CloseDoor()
    {
        if (!isOpen)
            return;
        isOpen = false;
        Interface.GetEngine().Gengine._Internal_Door_Change_Sprite(this);
    }

    public virtual void OpenClose()
    {
        Interface.GetEngine().AddToSnapshot(this);
        if (isOpen)
            CloseDoor();
        else
            OpenDoor();
    }
    public override bool CanMove(UnitType unittype)
    {

        return false;
    }

    public CloneableDoor Clone()
    {
        return CloneableDoor.Clone(this);
    }
}

public class CloneableDoor : CloneableUnit
{
    public bool isOpen;
    public Direction direction;
    public static CloneableDoor Clone(Door door)
    {
        CloneableDoor d = new CloneableDoor();
        CloneableUnit.init(door, d);
        d.isOpen = door.isOpen;
        return d;
    }
}

using UnityEngine;
using System.Collections;

public class Gate : Block {

    public bool isOpen;
    public Direction direction;
    public Vector2 Camera_Position;
    public float Camera_Zoom;
    public string sceneName;
    public bool save = true;

    // Use this for initialization
    void Awake () {
        unitType = UnitType.Door;
        obj = this.gameObject;
        position = gameObject.transform.position;
        movable = false;
        layer = 1;
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OpenDoor()
    {
        if (isOpen)
            return;
        isOpen = true;
        Starter.GetEngine().Gengine._Internal_Door_Change_Sprite(this);
    }

    public void CloseDoor()
    {
        if (!isOpen)
            return;
        isOpen = false;
        Starter.GetEngine().Gengine._Internal_Door_Change_Sprite(this);
    }

    public virtual void OpenClose()
    {
        Starter.GetEngine().AddToSnapshot(this);
        if (isOpen)
            CloseDoor();
        else
            OpenDoor();
    }
}

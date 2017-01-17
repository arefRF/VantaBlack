using UnityEngine;
using System.Collections;

public class Gate : Container {

    public bool isOpen;
    public Direction direction;
    public Vector2 Camera_Position;
    public float Camera_Zoom;
    public string sceneName;
    public bool save = true;



    public override bool PlayerMoveInto(Direction dir)
    {
        return false;
    }
}

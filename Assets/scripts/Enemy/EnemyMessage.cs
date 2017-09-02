using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMessage : MonoBehaviour {
    /// <summary>
    ///  Message Class for Enemy components communation
    ///  position and direction are basic message variables
    /// </summary>
    public Vector2 position;
    public Direction direction;
    public enum MessageType {
        Move,
        KillPlayer,
        FireLaser,

    }
}

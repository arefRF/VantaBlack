using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMessage{
    /// <summary>
    ///  Message Class for Enemy components communation
    ///  position and direction are basic message variables
    /// </summary>
    public Vector2 position;
    public Direction direction;
    public MessageType messagetype;
    public enum MessageType {
        PhysicalMove,
        PhysicalMoveStop,
        MoveAnimation,
        MoveAnimationStop,
        KillPlayer,
        FireLaser,
        OnOffChanged,
        StartPatrol,
        StopPatrol,
    }

    public EnemyMessage(MessageType messagetype)
    {
        this.messagetype = messagetype;
    }
    public EnemyMessage(MessageType messagetype, Vector2 position)
    {
        this.messagetype = messagetype;
        this.position = position;
    }
    public EnemyMessage(MessageType messagetype, Direction direction)
    {
        this.messagetype = messagetype;
        this.direction = direction;
    }
    public EnemyMessage(MessageType messagetype, Direction direction, Vector2 position)
    {
        this.messagetype = messagetype;
        this.direction = direction;
        this.position = position;
    }
}

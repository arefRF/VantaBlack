using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyGraphics : MonoBehaviour {

    private Enemy enemy;
    void Start()
    {
        enemy = GetComponent<Enemy>();
    }
    public void MoveSound()
    {
        
    }

    public void MoveAnimation()
    {
        Debug.Log("move animation");
    }

    public void KillPlayerAnimation()
    {

    }

    public void KillPlayerSound()
    {

    }

    public void GetMessage(EnemyMessage message)
    {
        switch (message.messagetype)
        {
            case EnemyMessage.MessageType.MoveAnimation: MoveAnimation(); break;
        }

    }
}

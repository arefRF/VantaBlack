using System.Collections;
using System.Collections.Generic;
using UnityEngine;

<<<<<<< HEAD
public class EnemyGraphics : MonoBehaviour
{
    void Start()
    {

=======
public class EnemyGraphics : MonoBehaviour {

    private Enemy enemy;
    void Start()
    {
        enemy = GetComponent<Enemy>();
>>>>>>> 58b2ea0947f206daddf4182534b17895815eb317
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

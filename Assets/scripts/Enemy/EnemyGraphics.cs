using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyGraphics : MonoBehaviour {

    private Enemy enemy;
    private AudioSource sound;
    private Animator animator;
    void Start()
    {
        enemy = GetComponent<Enemy>();
        sound = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();

    }

    public void MoveAnimation()
    {
        sound.Play();
        animator.SetInteger("Move", 1);
    }

    public void MoveAnimationStop()
    {
        sound.Stop();
        animator.SetInteger("Move", 0);
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
            case EnemyMessage.MessageType.MoveAnimationStop: MoveAnimationStop(); break;
        }

    }
}

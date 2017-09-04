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

    public void OnOffGraphics()
    {
        if (enemy.IsOn)
        {
            OpenCloseEyes(true);
            Toolkit.GetObjectInChild(gameObject, "Powers").SetActive(true);
        }
        else
        {
            OpenCloseEyes(false);
            Toolkit.GetObjectInChild(gameObject, "Powers").SetActive(false);
        }
    }

    private void OpenCloseEyes(bool open)
    {
        if (open)
        {
            Toolkit.GetObjectInChild(gameObject, "Eyes").SetActive(true);
            Toolkit.GetObjectInChild(gameObject, "Eyes Close").SetActive(false);
        }
        else
        {
            Toolkit.GetObjectInChild(gameObject, "Eyes").SetActive(false);
            Toolkit.GetObjectInChild(gameObject, "Eyes Close").SetActive(true);
        }
    }
    public void GetMessage(EnemyMessage message)
    {
        switch (message.messagetype)
        {
            case EnemyMessage.MessageType.MoveAnimation: MoveAnimation(); break;
            case EnemyMessage.MessageType.MoveAnimationStop: MoveAnimationStop(); break;
            case EnemyMessage.MessageType.OnOffChanged: OnOffGraphics(); break;
        }

    }
}

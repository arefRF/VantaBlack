﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerGraphics : MonoBehaviour {
    public float move_time = 0.5f;
    private APIGraphic api;
    private LogicalEngine engine;
    private Vector2 unmoved_pos;
    private Animator animator;
    private Player player;
    void Start()
    {
        unmoved_pos = transform.position;
        engine = Starter.GetEngine();
        api = engine.apigraphic;
        animator = GetComponent<Animator>();
        player = GetComponent<Player>();
            engine.apigraphic.Absorb(player, null);
    }


    public void Lean_Right()
    {
        //transform.GetChild(0).localPosition +=  new Vector3(1f,0,0);
        animator.SetInteger("Lean", 2);
        animator.SetBool("isLean", true);
    }

    public void Lean_Left()
    {
        //transform.GetChild(0).localPosition += new Vector3(-1f, 0,0);
        animator.SetInteger("Lean", 4);
        animator.SetBool("isLean", true);
    }

    public void Lean_Up()
    {
        
        // transform.GetChild(0).localPosition += new Vector3(0, 1f,0);
        animator.SetInteger("Lean", 1);
        animator.SetBool("isLean", true);
    }

    public void Lean_Down()
    {
        //transform.GetChild(0).localPosition +=  new Vector3( 0, -1f,0);
        animator.SetInteger("Lean", 3);
        animator.SetBool("isLean", true);
    }

    public void FakeLean_Down()
    {
        animator.SetInteger("Lean", 3);
        animator.SetBool("isFakeLean", true);
    }

    public void FakeLean_Right()
    {
        animator.SetInteger("Lean", 2);
        animator.SetBool("isFakeLean", true);
    }
    public void FakeLean_Left()
    {
        animator.SetInteger("Lean", 4);
        animator.SetBool("isFakeLean", true);
    }
    public void FakeLean_Up()
    {
        animator.SetInteger("Lean", 1);
        animator.SetBool("isFakeLean", true);
    }

    public void FakeLean_Finished()
    {
        animator.SetBool("isFakeLean", false);
        animator.SetInteger("Lean", 0);
        transform.GetChild(0).localPosition = new Vector2(0, 0);
        transform.GetChild(1).localPosition = new Vector2(0, 0);
    }

    public void Lean_Finished()
    {
        animator.SetBool("isLean", false);
        animator.SetInteger("Lean", 0);
        transform.GetChild(0).localPosition = new Vector2(0, 0);
        transform.GetChild(1).localPosition = new Vector2(0, 0);
    }

    public void MoveToBranch(Direction dir)
    {
        if (dir == Direction.Up)
        { 
            animator.SetInteger("Branch", 3);
            StartCoroutine(Simple_Move(player.position, 0.9f));
        }
    }

    public void BranchExit(Direction dir)
    {
        if(dir == Direction.Down)
        {
            animator.SetInteger("Branch", -3);
            StartCoroutine(Simple_Move(player.position, 0.4f));
        }
    }
    public void BranchEntered(Direction direction)
    {
        api.MovePlayerFinished(player.gameObject);
        animator.SetInteger("Branch", -5);
    }
    public void BranchExited(Direction dir)
    {
        api.MovePlayerFinished(player.gameObject);
        animator.SetInteger("Branch", 0);
    }


    private IEnumerator Set_Branch_Exited()
    {
        yield return null;
        yield return null;
    
    }
    private IEnumerator Set_Branch_Enterd()
    {
        yield return null;
        yield return null;

    }
    private IEnumerator Set_Pos()
    {
        yield return null;
        player.transform.position = player.position;
        api.MovePlayerFinished(player.gameObject);
    }
    private IEnumerator Simple_Move(Vector2 end, float move_time)
    {
        float remain_distance = ((Vector2)transform.position - end).sqrMagnitude;
        while (remain_distance > float.Epsilon)
        {
            remain_distance = ((Vector2)transform.position - end).sqrMagnitude;
            transform.position = Vector2.MoveTowards(transform.position, end, Time.smoothDeltaTime / move_time);
            api.Camera_AutoMove();
            yield return new WaitForSeconds(0.001f);
        }


        // if it needs Call Finished Move of API
    }
    private IEnumerator Set_Pos_Delay(float delay)
    {
        yield return new WaitForSeconds(delay);
        player.transform.position = player.position;
        api.MovePlayerFinished(player.gameObject);
    }
    private AnimationClip GetAnimationClip(string name)
    {
        if (!animator) return null; // no animator

        foreach (AnimationClip clip in animator.runtimeAnimatorController.animationClips)
        {
            if (clip.name == name)
            {
                return clip;
            }
        }
        return null; // no clip by that name
    }

    public void Move_Animation(Direction dir)
    {
        if (dir == Direction.Right)
            animator.SetInteger("Walk", 1);
        else
            animator.SetInteger("Walk", -1);
    }

    public void Move_Finished()
    {
        animator.SetInteger("Walk", 0);
    }
    public void Player_Change_Direction(Player player,Direction dir)
    { 

        api.PlayerChangeDirectionFinished(gameObject.GetComponent<Player>());

    }  

    private void Change_Direction_Finished()
    {
        api.PlayerChangeDirectionFinished(gameObject.GetComponent<Player>());
    }

    public void ChangeColor()
    {
        float[] color = Ability_Color(player.abilities);
        transform.GetChild(1).GetChild(0).GetComponent<SpriteRenderer>().color = new Color(color[0], color[1], color[2], color[3]);
        ChangeBodyColor();
    }

    private void ChangeBodyColor()
    {
        string path = "Player\\";
        if (player.abilities.Count != 0)
        {
            if (player.abilities[0].abilitytype == AbilityType.Fuel)
                path += "player 1 green";
            else if (player.abilities[0].abilitytype == AbilityType.Key)
                path += "player 1";
            else
                path += "player 1";
        }
        else
            path += "player 1";
        player.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = (Sprite)Resources.Load(path, typeof(Sprite));
    }
    private float[] Ability_Color(List<Ability> ability)
    {
        float[] color = new float[4];
        if (ability.Count != 0)
        {
            if (ability[0].abilitytype == AbilityType.Key)
            {
                color = new float[] { 1, 1, 1, 1 };
            }
            else if (ability[0].abilitytype == AbilityType.Fuel)
            {
                color = new float[] { 0, 0.941f, 0.654f, 1 };

            }
        }
        else
            color = new float[] { 1,1,1,0};
        return color;
    }

}

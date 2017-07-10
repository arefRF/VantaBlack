using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerGraphics : MonoBehaviour {
    public float move_time = 0.5f;
    private APIGraphic api;
    private LogicalEngine engine;
    private Vector2 unmoved_pos;
    private Animator animator;
    private Animator eyeAnimator;
    private Animator bodyAnimator;
    private Player player;
    void Start()
    {
        unmoved_pos = transform.position;
        engine = Starter.GetEngine();
        api = engine.apigraphic;
        animator = transform.GetChild(0).GetComponent<Animator>();
        eyeAnimator = transform.GetChild(0).GetChild(2).GetComponent<Animator>();
        player = GetComponent<Player>();
        engine.apigraphic.Absorb(player, null);
        bodyAnimator = transform.GetChild(0).GetComponent<Animator>();
    }


    public void Lean_Right(bool on_air)
    {
        ResetStates();
        transform.GetChild(0).rotation = Quaternion.Euler(0, 0, 0);
        if (on_air)
            animator.SetInteger("Lean Air", 3);
        else
            animator.SetInteger("Lean", 3);

    }

    public void Lean_Left(bool on_air)
    {
        ResetStates();
        transform.GetChild(0).rotation = Quaternion.Euler(0, 180, 0);
        if (on_air)
            animator.SetInteger("Lean Air", 3);
        else
            animator.SetInteger("Lean", 3);

    }

    public void Lean_Up(bool on_air)
    {
        ResetStates();
        animator.SetInteger("Lean", 1);

    }

    public void Lean_Down(bool on_air)
    {
        ResetStates();
        Debug.Log("Lean Down");
        animator.SetInteger("Lean", 2);
  
    }

    public void FakeLean_Down()
    {
       // animator.SetInteger("Lean", 3);

    }

    public void FakeLean_Right()
    {
     //   animator.SetInteger("Lean", 2);

    }
    public void FakeLean_Left()
    { 
      //  animator.SetInteger("Lean", 4); 

    }
    public void FakeLean_Up()
    { 
       // animator.SetInteger("Lean", 1);

    }

    public void Jump(Direction dir)
    {
        animator.SetBool("Jump", true);
    }

    public void FakeLean_Finished()
    {/*
        animator.SetInteger("Lean", 0);
        animator.SetBool("isFakeLean", false);
        transform.GetChild(0).localPosition = new Vector2(0, 0);
        transform.GetChild(1).localPosition = new Vector2(0, 0);*/
    }

    public void Lean_Finished()
    {
        animator.SetInteger("Lean", 0);
        animator.SetInteger("Lean Air", 0);
        bodyAnimator.SetBool("Lean", false);
    }

    public void MoveToBranch(Direction dir)
    {
        ResetStates();
        StopAllCoroutines();

        if (dir == Direction.Left || dir == Direction.Right)
            animator.SetInteger("Branch", 3);
        else if(dir == Direction.Up)
            StartCoroutine(Simple_Move(player.position, 0.3f));
        else
            StartCoroutine(Simple_Move(player.position, 0.3f));

       

       
    }

    public void DrainFinished()
    {
        player.DrainFinished();
    }

    public void ResetStates()
    {
       // animator.SetBool("Jump", false);
        animator.SetInteger("Walk", 0);
       // animator.SetBool("Transition", false);
      //  animator.SetInteger("Ramp", 0);
       
    }
    public void BranchExit(Direction dir,int ramp_type)
    {
        StopAllCoroutines();
        animator.SetInteger("Branch", 0);
        Vector2 pos = On_Ramp_Pos(ramp_type) + player.position;
        if (dir == Direction.Right)
            transform.GetChild(0).rotation = Quaternion.Euler(0, 0, 0);
        else if(dir == Direction.Left)
            transform.GetChild(0).rotation = Quaternion.Euler(0, 180, 0);

        transform.GetChild(0).localScale = new Vector2(1, 1);

        StartCoroutine(Simple_Move(pos, 0.65f));
    }
    private Vector2 On_Ramp_Pos(int type)
    {
        if (type == 4)
            return new Vector2(-0.22f, 0.2f);
        else if (type == 1)
            return new Vector2(0.19f, 0.25f);

        return new Vector2(0, 0);
    }

    public void Hit()
    {
        animator.SetTrigger("Hit");
        eyeAnimator.SetTrigger("Hit");
    }
    private IEnumerator Simple_Move(Vector2 end, float move_time)
    {
        float remain_distance = ((Vector2)transform.position - end).sqrMagnitude;
        while (remain_distance > float.Epsilon)
        {
           
            remain_distance = ((Vector2)transform.position - end).sqrMagnitude;
            transform.position = Vector2.MoveTowards(transform.position, end, Time.smoothDeltaTime / move_time);
            yield return new WaitForSeconds(0.001f);
        }
        yield return new WaitForSeconds(0.2f);
        player.MoveToBranchFinished();
    }

    public void Ramp_Animation(Direction dir,int type)
    {
        if (dir == Direction.Right)
            transform.GetChild(0).rotation = Quaternion.Euler(0, 0, 0);
        else
            transform.GetChild(0).rotation = Quaternion.Euler(0, 180, 0);
        animator.SetInteger("Walk", 2);
        if (dir == Direction.Right)
            animator.SetInteger("Ramp", type);
        else
        {
            if (type == 1)
                animator.SetInteger("Ramp", 4);
            else
                animator.SetInteger("Ramp", 1);
        }
    }

    public void Drain()
    {
        animator.SetTrigger("Drain");
    }
    public void TransitionAnimation()
    {
        animator.SetBool("Transition", true);
    }
    public void Ramp_Exit()
    {
        animator.SetInteger("Ramp", 0);
    }
    public void Move_Animation(Direction dir)
    {
        //animator.SetInteger("Ramp", 0);
       // animator.SetBool("Transition", false);
        if (dir == Direction.Right)
        {
            transform.GetChild(0).rotation = Quaternion.Euler(0, 0, 0);
            animator.SetInteger("Walk", 1);
        }
        else
        {
            animator.SetInteger("Walk", 1);
            transform.GetChild(0).rotation = Quaternion.Euler(0, 180, 0);

        }
    }

    public void Move_Finished()
    {
        animator.SetInteger("Walk", 0);
    }
    public void Player_Change_Direction(Player player,Direction dir)
    {
        int rot = 0;
        if (player.transform.GetChild(0).rotation.y == 0)
            rot = 180;
        player.transform.GetChild(0).rotation = Quaternion.Euler(player.transform.rotation.x, rot, player.transform.rotation.z);
        api.PlayerChangeDirectionFinished(gameObject.GetComponent<Player>());
    }  

    private void Change_Direction_Finished()
    {
        api.PlayerChangeDirectionFinished(gameObject.GetComponent<Player>());
    }

    public void Teleport(Vector2 pos)
    {
        player.transform.position = pos;
        StartCoroutine(TPFinish());
    }

    private IEnumerator TPFinish()
    {
        yield return new WaitForSeconds(0.1f);
        player.TeleportFinished();
    }
    public void ChangeColor()
    {
        if(bodyAnimator == null)
             bodyAnimator = transform.GetChild(0).GetComponent<Animator>();
        if (player.abilities.Count != 0)
        {
            if (player.abilities[0].abilitytype == AbilityType.Fuel)
            {
                if (player.state == PlayerState.Lean && player.leandirection == Direction.Up)
                {
                    bodyAnimator.SetInteger("Ability", 2);
                    bodyAnimator.SetBool("Lean", true);
                }
                else
                    bodyAnimator.SetInteger("Ability", 1);
            }
            else
            {
                if (player.state == PlayerState.Lean && player.leandirection == Direction.Up)
                {
                    bodyAnimator.SetInteger("Ability", 0);
                    
                }

                else
                    bodyAnimator.SetInteger("Ability", 0);
            }
            
        }
        else
            bodyAnimator.SetInteger("Ability", 0);
    }

    public void ChangeColorFinished()
    {
       
    }

    private float[] Ability_Color(List<Ability> ability)
    {
        float[] color = new float[4];
        if (ability.Count != 0)
        {
            switch (ability[0].abilitytype)
            {
                case AbilityType.Key: color = new float[] { 1, 1, 1, 1 };break;
                case AbilityType.Fuel: color = new float[] { 0, 0.941f, 0.654f, 1 };break;
                case AbilityType.Jump: color = new float[] { 0.59f, 0.78f, 1 ,1};break;
                case AbilityType.Teleport: color = new float[] { 0.92f, 0.36f, 0.44f, 1 };break;
                case AbilityType.Gravity: color = new float[] { 0.81f, 0.60f, 0.96f, 1 };break;
                case AbilityType.Rope: color = new float[] { 1, 0.60f, 0.30f, 1 };break;
            }
        }
        else
            color = new float[] { 1,1,1,0};
        return color;
    }

}

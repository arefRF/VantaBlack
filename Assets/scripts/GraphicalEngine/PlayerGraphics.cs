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
    private Player player;
    void Start()
    {
        unmoved_pos = transform.position;
        engine = Starter.GetEngine();
        api = engine.apigraphic;
        animator = transform.GetChild(0).GetChild(0).GetComponent<Animator>();
        eyeAnimator = transform.GetChild(0).GetChild(0).GetChild(3).GetComponent<Animator>();
        player = GetComponent<Player>();
            engine.apigraphic.Absorb(player, null);
    }


    public void Lean_Right()
    {
        ResetStates();
        transform.GetChild(0).rotation = Quaternion.Euler(0, 0, 0);
        animator.SetInteger("Lean", 2);

    }

    public void Lean_Left()
    {
        ResetStates();
        transform.GetChild(0).rotation = Quaternion.Euler(0, 180, 0);
        animator.SetInteger("Lean", 4);

    }

    public void Lean_Up()
    {
        ResetStates();
        animator.SetInteger("Lean", 1);
    }

    public void Lean_Down()
    {
        ResetStates();
        animator.SetInteger("Lean", 3);
  
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
    public void Shield_Color_Hide()
    {
        
        transform.GetChild(1).GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
    }

    public void Shield_Color_Show()
    {
        float[] color = Ability_Color(player.abilities);
        transform.GetChild(1).GetComponent<SpriteRenderer>().color = new Color(color[0],color[1],color[2],color[3]);
    }
    public void Lean_Finished()
    {
        animator.SetInteger("Lean", 0);
    }

    public void MoveToBranch(Direction dir)
    {
        /*
        if (dir == Direction.Up)
        {
            player.transform.GetChild(1).rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (dir == Direction.Down)
        {
            player.transform.GetChild(1).rotation = Quaternion.Euler(0, 0, 180);
        }
        else if (dir == Direction.Left)
        {
            player.transform.GetChild(1).rotation = Quaternion.Euler(0, 0, 90);
        }
        else
            player.transform.GetChild(1).rotation = Quaternion.Euler(0, 0, 270);
            */
        //ResetStates();
        animator.SetInteger("Walk", 0);
        animator.SetInteger("Branch", 1);
        StopAllCoroutines();

        StartCoroutine(Simple_Move(player.position, 0.3f));
    }

    public void DrainFinished()
    {
        player.DrainFinished();
    }

    public void ResetStates()
    {
        animator.SetBool("Jump", false);
        animator.SetInteger("Walk", 0);
        animator.SetBool("Transition", false);
        animator.SetInteger("Ramp", 0);
       
    }
    public void BranchExit(Direction dir,int ramp_type)
    {/*
        if (dir == Direction.Up)
        {
            player.transform.GetChild(1).rotation = Quaternion.Euler(0, 0, 180);
        }
        else if (dir == Direction.Down)
        {
            player.transform.GetChild(1).rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (dir == Direction.Left)
        {
            player.transform.GetChild(1).rotation = Quaternion.Euler(0, 0, 270);
        }
        else
            player.transform.GetChild(1).rotation = Quaternion.Euler(0, 0, 90);
        ResetStates();
        animator.SetInteger("Branch", -1); */
        StopAllCoroutines();
        animator.SetInteger("Branch", 0);
        Vector2 pos = On_Ramp_Pos(ramp_type) + player.position;
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

    // animation calls this
    public void MoveToBranchAnimationFinished()
    {
       // animator.SetInteger("Branch", 0);
    }

    // animation calss this
    public void BranchExitAnimationFinished()
    {
       // animator.SetInteger("Branch", 0);
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
            api.Camera_AutoMove();
            yield return new WaitForSeconds(0.001f);
        }

        yield return new WaitForSeconds(0.3f);
        api.MovePlayerFinished(player.gameObject);
        //animator.SetInteger("Branch", 0);
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
        animator.SetInteger("Ramp", 0);
        animator.SetBool("Transition", false);
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
    {/*
        float[] color = Ability_Color(player.abilities);
        transform.GetChild(1).GetChild(0).GetComponent<SpriteRenderer>().color = new Color(color[0], color[1], color[2], color[3]);
        ChangeBodyColor(); */
    }

    private void ChangeBodyColor()
    {/*
        string path = "Player\\";
        if (player.abilities.Count != 0)
        {
            if (player.abilities[0].abilitytype == AbilityType.Fuel)
                path += "player 1 green";
            else if (player.abilities[0].abilitytype == AbilityType.Key)
                path += "player 1 white";
            else
                path += "player 1";
        }
        else
            path += "player 1";
        player.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = (Sprite)Resources.Load(path, typeof(Sprite));*/
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

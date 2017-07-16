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
    private int z_rot;
    void Start()
    {
        z_rot = 0;
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
        int zrot = 0, yrot = 0, xrot = 0;
        int num = 3;
        if (player.gravity == Direction.Up)
        {
            zrot = 180;
            yrot = 180;
        }
        else if (player.gravity == Direction.Right)
        {
            zrot = 90;
            if (player.direction == Direction.Down)
                xrot = 180;
            num = 2;
        }
        else if (player.gravity == Direction.Left)
        {
            zrot = 270;
            if (player.direction == Direction.Up)
                xrot = 180;
            num = 1;
        }
        transform.GetChild(0).rotation = Quaternion.Euler(xrot, yrot, zrot);
        if (on_air)
            animator.SetInteger("Lean Air", num);
        else
            animator.SetInteger("Lean", num);

    }

    public void Lean_Left(bool on_air)
    {
        ResetStates();
        int zrot = 0, yrot = 180, xrot = 0;
        int num = 3;
        if (player.gravity == Direction.Up)
        {
            zrot = 180;
            yrot = 0;
        }
        else if (player.gravity == Direction.Right)
        {
            zrot = 270;
            if (player.direction == Direction.Up)
                xrot = 180;
            num = 1;
        }
        else if (player.gravity == Direction.Left)
        {
            zrot = 90;
            if (player.direction == Direction.Down)
                xrot = 180;
            num = 2;
        }
        transform.GetChild(0).rotation = Quaternion.Euler(xrot, yrot, zrot);
        if (on_air)
            animator.SetInteger("Lean Air", num);
        else
            animator.SetInteger("Lean", num);

    }

    public void Lean_Up(bool on_air)
    {
        int num = 1;
        if (player.gravity == Direction.Up)
            num = 2;
        else if (player.gravity == Direction.Right)
            num = 3;
        else if (player.gravity == Direction.Left)
            num = 3;
        ResetStates();
        if (on_air)
            animator.SetInteger("Lean Air", num);
        else
            animator.SetInteger("Lean", num);
    }

    public void Lean_Down(bool on_air)
    {
        int num = 2;
        if (player.gravity == Direction.Up)
            num = 1;
        else if (player.gravity == Direction.Right)
            num = 3;
        else if (player.gravity == Direction.Left)
            num = 3;
        ResetStates();
        Debug.Log(num);
        if (on_air)
            animator.SetInteger("Lean Air", num);
        else
            animator.SetInteger("Lean", num);
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
        if(player.gravity == Direction.Right)
        {
            int num = Toolkit.DirectionToNumber(dir);
            num++;
            if (num == 5)
                num = 1;
            dir = Toolkit.NumberToDirection(num);

        }
        else if (player.gravity == Direction.Left)
        {
            int num = Toolkit.DirectionToNumber(dir);
            num--;
            if (num == 0)
                num = 4;
            dir = Toolkit.NumberToDirection(num);

        }
        ResetStates();
        StopAllCoroutines();
        transform.GetChild(0).GetComponent<AnimationEvents>().call = true;
        if (dir == Direction.Left || dir == Direction.Right)
            animator.SetInteger("Branch", 3);
        else if (dir == Direction.Up)
        {
            StartCoroutine(Simple_Move(player.position, 0.3f, true));
            animator.SetInteger("Branch", 1);
            transform.GetChild(0).GetComponent<AnimationEvents>().call = false;
        }
        else
        {
            StartCoroutine(Simple_Move(player.position, 0.3f, true));
            animator.SetInteger("Branch", 1);
            transform.GetChild(0).GetComponent<AnimationEvents>().call = false;
        }
    }

    public void DrainFinished()
    {
        player.DrainFinished();
    }

    public void ResetStates()
    {
        // Get call stack
       // animator.SetBool("Jump", false);
        animator.SetInteger("Walk", 0);
       // animator.SetBool("Transition", false);
      //  animator.SetInteger("Ramp", 0);
       
    }
    public void BranchExit(Direction dir,int ramp_type)
    {
        if (player.gravity == Direction.Right)
        {
            int num = Toolkit.DirectionToNumber(dir);
            num ++;
            if (num >= 5)
                num = 1;
            dir = Toolkit.NumberToDirection(num);

        }
        else if (player.gravity == Direction.Left)
        {
            int num = Toolkit.DirectionToNumber(dir);
            num--;
            if (num == 0)
                num = 4;
            dir = Toolkit.NumberToDirection(num);

        }
        StopAllCoroutines();
        int zrot = 0;
        if (player.gravity == Direction.Up)
            zrot = 180;
        else if (player.gravity == Direction.Left)
            zrot = 270;
        else if (player.gravity == Direction.Right)
            zrot = 90;
        animator.SetInteger("Branch", 0);
        Vector2 pos = On_Ramp_Pos(ramp_type) + player.position;
        if (dir == Direction.Right)
        {
            if (player.gravity == Direction.Up)
                transform.GetChild(0).rotation = Quaternion.Euler(0, 180, zrot);
            else
                transform.GetChild(0).rotation = Quaternion.Euler(0, 0, zrot);
        }
        else if (dir == Direction.Left)
        {
            if (player.gravity == Direction.Up)
                transform.GetChild(0).rotation = Quaternion.Euler(0, 0, zrot);
            else if(player.gravity == Direction.Left || player.gravity == Direction.Right)
                transform.GetChild(0).rotation = Quaternion.Euler(180, 0, zrot);
            else 
                transform.GetChild(0).rotation = Quaternion.Euler(0, 180, zrot);
        }
        if (dir == Direction.Left || dir == Direction.Right)
            animator.SetInteger("Branch", 4);
        // for later change
        else
            animator.SetInteger("branch", 4);
        StartCoroutine(Branch_Exit(pos, 0.65f));
    }


    private IEnumerator Branch_Exit(Vector2 end, float move_time)
    {
        float remain_distance = ((Vector2)transform.position - end).sqrMagnitude;
        float speed = 1;
        while (remain_distance > float.Epsilon)
        {
            remain_distance = ((Vector2)transform.position - end).sqrMagnitude;
            transform.position = Vector2.MoveTowards(transform.position, end, Time.smoothDeltaTime * speed);
            yield return new WaitForSeconds(0.001f);
        }
        yield return new WaitForSeconds(0.2f);
            player.MoveOutOfBranchFinished();
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
    private IEnumerator Simple_Move(Vector2 end, float move_time,bool enter)
    {
        Debug.Log(end);
        float remain_distance = ((Vector2)transform.position - end).sqrMagnitude;
        while (remain_distance > float.Epsilon)
        {
           
            remain_distance = ((Vector2)transform.position - end).sqrMagnitude;
            transform.position = Vector2.MoveTowards(transform.position, end, Time.smoothDeltaTime / move_time);
            
            yield return new WaitForSeconds(0.001f);
        }
        yield return new WaitForSeconds(0.2f);
        if (enter)
            player.MoveToBranchFinished();
        else
            player.MoveOutOfBranchFinished();
    }

    public void Ramp_Animation(Direction dir,int type)
    {
        animator.SetInteger("Walk", 1);
        int zrot = 0;
        if (player.gravity == Direction.Up)
            zrot = 180;
        if (dir == Direction.Right)
        {
            if (type == 4)
            {
                transform.GetChild(0).rotation= Quaternion.Euler(0,0, zrot);
                // z_rot set to 0 to change it later
                z_rot = 0;
            }
            else
            {
                transform.GetChild(0).rotation = Quaternion.Euler(0, 0, zrot);
                z_rot = 0;
            }
        }
        else
        {
            if(type == 4)
            {
                transform.GetChild(0).rotation = Quaternion.Euler(0, 180, zrot);
                z_rot = 0;
            }
            else
            {
                transform.GetChild(0).rotation = Quaternion.Euler(0,180, zrot);
                z_rot = 0;
            }
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
        int zrot = 0;
        if (player.gravity == Direction.Up)
        {
            zrot = 180;
            dir = Toolkit.ReverseDirection(dir);
        }
        else if(player.gravity == Direction.Right)
        {
            zrot = 270;
            //dir = Toolkit.ReverseDirection(dir);
        }
        else if(player.gravity == Direction.Left)
        {
            zrot = 90;
            dir = Toolkit.ReverseDirection(dir);
        }
        if (dir == Direction.Right)
        {
            transform.GetChild(0).rotation = Quaternion.Euler(0, 0, zrot);
            animator.SetInteger("Walk", 1);
        }
        else if(dir == Direction.Up)
        {
            transform.GetChild(0).rotation = Quaternion.Euler(180,180, zrot);
            animator.SetInteger("Walk", 1);
        }
        else
        {
            animator.SetInteger("Walk", 1);
            transform.GetChild(0).rotation = Quaternion.Euler(0, 180, zrot);

        }
    }

    public void Ramp_To_Block_Animation(Direction dir)
    {
        animator.SetInteger("Walk", 1);
        int y = 0;
        if (dir == Direction.Right)
        {
            transform.GetChild(0).rotation = Quaternion.Euler(0, 0, z_rot);
            y = 0;
        }
        else
        {
            transform.GetChild(0).rotation = Quaternion.Euler(0, 180, z_rot);
            y = 180;
        }

        StartCoroutine(RampToBlockWait(y));
    }
    public void LandAnimation()
    {
        animator.SetBool("Assemble", false);
        animator.SetBool("Jump", false);
        StartCoroutine(WaitForLand());
    }


    IEnumerator WaitForLand()
    {
        yield return new WaitForSeconds(0.1f);
        api.LandFinished(player);
    }

    public void FallAnimation()
    {
        animator.SetBool("Jump", true);
        animator.SetBool("Assemble", true);
    }
    private IEnumerator RampToBlockWait(float y)
    {
        yield return new WaitForSeconds(0.4f);
        int zrot = 0;
        if(player.gravity == Direction.Up)
        {
            zrot = 180;
        }
        transform.GetChild(0).rotation = Quaternion.Euler(0, y, zrot);
        z_rot = 0;
    }
    public void Move_Finished()
    {
        animator.SetInteger("Walk", 0);
    }
    public void Player_Change_Direction(Player player,Direction dir)
    {
        int yrot = 0, zrot = 0; ;
        float xrot = player.transform.rotation.x;
        if (player.transform.GetChild(0).rotation.y == 0)
            yrot = 180;
        if (player.gravity == Direction.Up)
        {
            zrot = 180;
        }
        else if (player.gravity == Direction.Right)
        {
            yrot = 0;
            zrot = 90;
            if (dir == Direction.Down)
                xrot = 180;
        }
        else if(player.gravity == Direction.Left)
        {
            yrot = 0;
            zrot = 270;
            if (dir == Direction.Up)
                xrot = 180;
        }
        player.transform.GetChild(0).rotation = Quaternion.Euler(xrot, yrot, zrot);
        if (dir == Direction.Right)
            animator.SetInteger("Change Direction", 1);
        else
            animator.SetInteger("Change Direction", 2);
       
    }  

    public void Change_Direction_Finished(Direction dir)
    {
        if (dir == player.direction)
        {
            animator.SetInteger("Change Direction", 0);
            api.PlayerChangeDirectionFinished(gameObject.GetComponent<Player>());
        }
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
        float[] color = Ability_Color(player.abilities);
        // Body Color
        transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<SpriteRenderer>().color = new Color(color[0], color[1], color[2], color[3]);
        //Eye BAckground Color
        transform.GetChild(0).GetChild(0).GetChild(0).GetChild(1).GetComponent<SpriteRenderer>().color = new Color(color[0], color[1], color[2], color[3]);

        // Eye Color
        if (player.abilities.Count==0)
            transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().color = new Color(1,1,1);
        else
            transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().color = new Color(0, 0, 0);
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
                case AbilityType.Fuel: color = new float[] { 1, 0.674f, 0.211f, 1 };break;
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

    public void ShowHologram()
    {
        GameObject hologram = Toolkit.GetObjectInChild(gameObject.transform.GetChild(0).GetChild(0).gameObject, "Hologram");
        GameObject lights = Toolkit.GetObjectInChild(hologram, "Lights");
        GameObject Number = Toolkit.GetObjectInChild(hologram, "Number");
        SpriteRenderer IconSpriteRenderer = Toolkit.GetObjectInChild(hologram, "Icon").GetComponent<SpriteRenderer>();
        float[] color = Ability_Color(player.abilities);
        Color abilitycolor  = new Color(color[0], color[1], color[2], color[3]);
        IconSpriteRenderer.sprite = null;
        Number.SetActive(false);
        if (player.abilities.Count != 0)
        {
            Number.SetActive(true);
            string path = Toolkit.Icon_Path(player.abilities[0].abilitytype);
            IconSpriteRenderer.color = abilitycolor;
            IconSpriteRenderer.sprite = (Sprite)Resources.Load(path, typeof(Sprite));
        }
        for (int i = 1; i <= 4; i++)
        {
            GameObject light = Toolkit.GetObjectInChild(lights, "Light " + i);
            light.SetActive(false);
        }
        for (int i=1; i<=player.abilities.Count; i++)
        {
            GameObject light = Toolkit.GetObjectInChild(lights, "Light " + i);
            light.GetComponent<SpriteRenderer>().color = abilitycolor;
            light.SetActive(true);
        }
        hologram.SetActive(true);
    }

    public void HideHologram()
    {
        Toolkit.GetObjectInChild(gameObject.transform.GetChild(0).GetChild(0).gameObject, "Hologram").SetActive(false);
    }
}

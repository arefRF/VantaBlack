using UnityEngine;
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

    public void Lean_Finished()
    {
        animator.SetBool("isLean", false);
        animator.SetInteger("Lean", 0);
        transform.GetChild(0).localPosition = new Vector2(0, 0);
        transform.GetChild(1).localPosition = new Vector2(0, 0);
    }

    private Vector2 Camera_Pos()
    {
        return (Vector2)Camera.main.transform.position + ((Vector2)transform.position - unmoved_pos);
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
        Vector3 color = Ability_Color(player.abilities);
        transform.GetChild(1).GetChild(0).GetComponent<SpriteRenderer>().color = new Color(color.x, color.y, color.z, 1);
        if (player.abilities.Count == 0)
            transform.GetChild(1).GetChild(0).GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
    }

    private Vector3 Ability_Color(List<Ability> ability)
    {
        if (ability.Count != 0)
        {
            if (ability[0].abilitytype == AbilityType.Key)
            {
                return new Vector3(1, 1, 1);
            }
            else if (ability[0].abilitytype == AbilityType.Fuel)
            {
                return new Vector3(0, 0.941f, 0.654f);
            }
        }

        // else white
        return new Vector3(1, 1, 1);
    }

}

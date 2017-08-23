using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class PlayerGraphicsV5 : PlayerGraphics
{
    public override void MoveToBranch(Direction dir)
    {
        StopAllCoroutines();
        StartCoroutine(Simple_Move(player.position, 0.3f, true));
    }

    public override void ChangeColor()
    {
            ChangeSprites();
    }
    private void ChangeOffSprites()
    {

    }

    public override void FallAnimation()
    {
        animator.SetBool("Fall", true);
    }

    public override void LandAnimation()
    {
        animator.SetBool("Fall", false);
        animator.SetTrigger("Land");
    }

    public override void Move_Animation(Direction dir)
    {
        int zrot = 0;
        if (player.gravity == Direction.Up)
        {
            zrot = 180;
            dir = Toolkit.ReverseDirection(dir);
        }
        else if (player.gravity == Direction.Right)
        {
            zrot = 270;
            //dir = Toolkit.ReverseDirection(dir);
        }
        else if (player.gravity == Direction.Left)
        {
            zrot = 90;
            dir = Toolkit.ReverseDirection(dir);
        }
        if (dir == Direction.Right)
        {
            transform.GetChild(0).rotation = Quaternion.Euler(0, 0, zrot);
        }
        else if (dir == Direction.Up)
        {
            transform.GetChild(0).rotation = Quaternion.Euler(180, 180, zrot);
        }
        else
        {
            transform.GetChild(0).rotation = Quaternion.Euler(0, 180, zrot);

        }
    }
    public override void BlockToFallAnimation()
    {
        animator.SetTrigger("BlockToFall");
    }
    private void ChangeSprites()
    {
        Color color = Ability_Color(player.abilities);
        color = new Color(color.r, color.g, color.b, 1);
        SpriteRenderer brain = Toolkit.GetObjectInChild(this.gameObject, "Brain").GetComponent<SpriteRenderer>();
        SpriteRenderer right_hand = Toolkit.GetObjectInChild(this.gameObject, "Arm 2 Right").GetComponent<SpriteRenderer>();
        SpriteRenderer left_hand = Toolkit.GetObjectInChild(this.gameObject, "Arm 2 Left").GetComponent<SpriteRenderer>();
        string base_path =  "Player\\Player Version 5\\";
        string on = "";
        if (player.abilities.Count != 0)
            on = " On";
        brain.sprite = (Sprite)Resources.Load(base_path+"Brain"+on,typeof (Sprite));
        brain.color = color;
        right_hand.sprite = (Sprite)Resources.Load(base_path + "Arm 2 Right" + on, typeof(Sprite));
        right_hand.color = color;
        left_hand.sprite = (Sprite)Resources.Load(base_path+ "Arm 2 Left"+ on,typeof(Sprite));
        left_hand.color = color;
    }
}
